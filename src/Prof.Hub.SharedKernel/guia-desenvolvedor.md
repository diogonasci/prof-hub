# 4. Guia para Desenvolvedores

## 4.1 Classe Simulation

### 4.1.1 Método: Calculate

Este é o método principal que coordena toda a simulação financeira.

```csharp
public async Task<Simulation> Calculate(CustomerFinancialPlanning fp, Scenario scenario)
{
    CreateCalculationBasis(fp, scenario,
        out Dictionary<string, List<Contribution>> fpGoals,
        out Dictionary<string, List<Contribution>> fpExpenses,
        out Dictionary<string, List<Contribution>> fpRevenues,
        out Dictionary<string, Contribution> fpContributions,
        out Dictionary<string, List<Contribution>> fpBenefitsPGBL);

    var monthId = 0;
    DateTime startDate = new DateTime(fp.Created.Year, fp.Created.Month, 1);
    DateTime endDate = fp.MainGoal.GetFinalDate(fp);
    Projections = new Dictionary<string, PointInTime>();

    for (var pointInTimeDate = startDate.Date;
         pointInTimeDate.Date <= endDate.Date || pointInTimeDate.Month == endDate.Month;
         pointInTimeDate = pointInTimeDate.AddMonths(1))
    {
        monthId++;
        SetPointInTimeCalculation(fp, scenario, pointInTimeDate, monthId,
            fpGoals, fpExpenses, fpRevenues, fpContributions, fpBenefitsPGBL,
            out List<Contribution> pointInTimeGoals,
            out List<Contribution> pointInTimeExpenses,
            out List<Contribution> pointInTimeRevenues,
            out Contribution pointInTimeContributions,
            out List<Contribution> pointInTimeBenefitsPGBL);

        var pointInTime = new PointInTime(pointInTimeDate, monthId, fp.Personal,
            pointInTimeGoals, pointInTimeExpenses, pointInTimeRevenues,
            pointInTimeContributions, pointInTimeBenefitsPGBL);

        pointInTime.Calculate(this, fp, scenario);
        Projections.Add(pointInTimeDate.ToDateKey(), pointInTime);
    }

    SetBigNumbers(fp, scenario);
    await SetScore(fp, scenario);

    return this;
}
```

### 4.1.2 Método: SetBigNumbers

Calcula as métricas agregadas importantes para a avaliação do plano financeiro.

```csharp
private void SetBigNumbers(CustomerFinancialPlanning fp, Scenario scenario)
{
    var filteredProjections = Projections
        .Where(p => p.Key.FromDateKey() <= scenario.GetRetirementDate(fp))
        .ToList();

    double numYears = (double)filteredProjections.Count / 12.0;
    var totalRevenue = filteredProjections.Sum(p => p.Value.MonthRevenues?.Sum(x => x.Value) ?? 0);
    var totalExpenses = filteredProjections.Sum(p => p.Value.MonthExpenses?.Sum(x => x.Value) ?? 0);
    var totalContributions = filteredProjections.Sum(p => p.Value.MonthContributions?.Value ?? 0);
    var initialPatrimony = fp?.Patrimony?.Onshore?.TotalPatrimony ?? 0;

    scenario.BigNumbers.AverageAnnualRevenue = (totalRevenue) / numYears;
    scenario.BigNumbers.AverageAnnualExpense = (totalExpenses) / numYears;
    scenario.BigNumbers.AverageAnnualInvestmentCapacity = (totalContributions) / numYears;

    // ... Código para calcular outras métricas ...

    scenario.BigNumbers.ActualPatrimony = filteredProjections.LastOrDefault().Value.ActualProjection;
    scenario.BigNumbers.IdealPatrimony = filteredProjections.LastOrDefault().Value.PatrimonyMaintenance;
    scenario.BigNumbers.ConsumptionPatrimony = filteredProjections.LastOrDefault().Value.PatrimonyConsumption;

    scenario.BigNumbers.PatrimonyFromContributions = totalContributions;
    scenario.BigNumbers.PatrimonyFromReturns = scenario.BigNumbers.ActualPatrimony - scenario.BigNumbers.PatrimonyFromContributions - initialPatrimony;

    scenario.BigNumbers.IdealContributionValue = scenario.GetIdealContributionValue(fp) * 12;
    scenario.BigNumbers.IdealContributionValueForConsumption = scenario.GetIdealContributionValueForConsumption(fp.MainGoal, fp) * 12;
}
```

### 4.1.3 Método: SetScore

Avalia o sucesso do plano financeiro e atribui uma pontuação.

```csharp
private async Task SetScore(CustomerFinancialPlanning fp, Scenario scenario)
{
    if (Projections?.Count == 0 || !fp.HasMainGoal) return;

    var gotDate = scenario.GetRetirementDate(fp);
    var retirementProjection = Projections[gotDate.ToDateKey()];

    double score = 0.0;

    if (retirementProjection.ActualProjection < retirementProjection.PatrimonyConsumption)
        score = retirementProjection.ActualProjection / retirementProjection.PatrimonyConsumption * 7;
    else
    {
        var surplusValue = retirementProjection.ActualProjection - retirementProjection.PatrimonyConsumption;
        var differenceKeepingConsuming = retirementProjection.PatrimonyMaintenance - retirementProjection.PatrimonyConsumption;

        score = surplusValue / differenceKeepingConsuming * 3 + 7;
        if (score > 10)
            score = 10;
    }

    score = Math.Round(score * 10, 0);
    Score = Convert.ToInt32(score);
}
```

## 4.2 Classe Scenario

### 4.2.1 Método: GetRetirementDate

Calcula a data em que o cliente atingirá a aposentadoria.

```csharp
public DateTime GetRetirementDate(CustomerFinancialPlanning fp)
{
    if (fp?.Personal?.BirthDate is null)
        return default;

    DateTime retirementDate = fp.Personal.BirthDate.Value.AddYears(RetirementAge.Value);

    if (retirementDate <= fp.Created)
        retirementDate = fp.Created;

    retirementDate = new DateTime(retirementDate.Year, retirementDate.Month, 1);
    return retirementDate;
}
```

### 4.2.2 Método: GetIdealContributionValue

Calcula o valor mensal que o cliente precisa poupar para atingir o objetivo de manutenção do patrimônio.

```csharp
public double GetIdealContributionValue(CustomerFinancialPlanning fp)
{
    int timeInMonths = DateTimeExtensions.CountMonthsBetween(fp.Created, GetRetirementDate(fp));

    if (timeInMonths <= 0)
        return 0;

    double retirementAmount = GetRetirementAmountMaintenance(fp);
    double initialPatrimony = fp?.Patrimony?.Onshore?.TotalPatrimony ?? 0;
    double monthlyProfitabilityRate = GetMonthlyProfitabilityRate();

    if (monthlyProfitabilityRate == 0)
    {
        return (retirementAmount - initialPatrimony) / timeInMonths;
    }

    var growthFactor = Math.Pow(1 + monthlyProfitabilityRate, timeInMonths);
    double idealContribution = (retirementAmount - initialPatrimony * growthFactor) / ((growthFactor - 1) / monthlyProfitabilityRate);

    return idealContribution;
}
```

### 4.2.3 Método: GetRetirementAmountMaintenance

Calcula o patrimônio necessário para viver dos rendimentos na aposentadoria.

```csharp
public double GetRetirementAmountMaintenance(CustomerFinancialPlanning fp)
{
    double retirementAmount = 0;
    double goalValue = fp.MainGoal.Value ?? 0;

    double monthlyRealProfitability = GetMonthlyProfitabilityRate();

    if (fp.Strategy.ProjectionType == ProjectionType.NominalProfitability)
    {
        monthlyRealProfitability = GetRealMonthlyProfitabilityRate(fp);

        var retirementDate = fp.Personal.GetBirthdayByAgeForReports(RetirementAge.Value);
        int timeInMonths = DateTimeExtensions.CountMonthsBetween(fp.Created, retirementDate);

        goalValue = fp.Strategy.ApplyMonthlyInflationRate(fp.MainGoal.Value ?? 0, timeInMonths);
    }

    monthlyRealProfitability = monthlyRealProfitability == 0 ? 1 : monthlyRealProfitability;
    retirementAmount = goalValue / monthlyRealProfitability;

    return retirementAmount;
}
```

## 4.3 Classe Strategy

### 4.3.1 Método: GetMonthlyInflationRate

Converte a taxa de inflação anual para mensal.

```csharp
public double GetMonthlyInflationRate()
{
    double annualInflation = GetAnnualInflationRate();
    double percentageOfAMonthYear = 1.0 / 12.0;
    double monthlyInflation = Math.Pow(1 + annualInflation, percentageOfAMonthYear) - 1;
    return monthlyInflation;
}
```

### 4.3.2 Método: ApplyMonthlyInflationRate

Aplica a inflação mensal a um valor por um período específico.

```csharp
public double ApplyMonthlyInflationRate(double initialValue, int numberOfMonths)
{
    var monthlyInflationRate = GetMonthlyInflationRate();
    var adjustedValue = initialValue * Math.Pow(1 + monthlyInflationRate, numberOfMonths);
    return adjustedValue;
}
```

### 4.3.3 Método: GetRealProfitability

Calcula a rentabilidade real considerando imposto e inflação.

```csharp
private double GetRealProfitability()
{
    double nominalProfitability = GetNominalProfitabilityFraction();
    double annualInflation = AnnualInflation / 100;
    double realProfitability = (1 + nominalProfitability) / (1 + annualInflation) - 1;
    return realProfitability * 100;
}
```

## 4.4 Classe PointInTime

### 4.4.1 Método: Calculate

Coordena os cálculos para um ponto específico no tempo.

```csharp
public void Calculate(Simulation simulation, CustomerFinancialPlanning fp, Scenario scenario)
{
    PatrimonyRealProjectionCalculate(simulation, fp, scenario);
    PatrimonyMaintenanceProjectionCalculate(simulation, fp, scenario);
    PatrimonyConsumptionProjectionCalculate(simulation, fp, scenario);
    PGBLBenefitProjectionCalculate(simulation, fp, scenario);
}
```

### 4.4.2 Método: PatrimonyRealProjectionCalculate

Calcula a projeção real do patrimônio.

```csharp
private void PatrimonyRealProjectionCalculate(Simulation simulation, CustomerFinancialPlanning fp, Scenario scenario)
{
    double goalRetirementSum = MonthGoals?.Where(x => x.Type == ContributionType.Retirement).Sum(x => x.Value) ?? 0;
    double goalExpenseSum = MonthGoals?.Where(x => x.Type == ContributionType.Expense).Sum(x => x.Value) ?? 0;

    double contribution = MonthContributions?.Value ?? 0;
    double totalExpenses = MonthExpenses?.Sum(x => x.Value) ?? 0;
    double totalRevenues = MonthRevenues?.Sum(x => x.Value) ?? 0;

    double monthlyProfitabilityRate = scenario.GetMonthlyProfitabilityRate();
    double monthlyDividend = simulation.ActualPatrimony * monthlyProfitabilityRate;

    double actualProjection = simulation.ActualPatrimony + monthlyDividend;

    if (goalRetirementSum > goalExpenseSum + totalExpenses)
        actualProjection += totalRevenues - goalRetirementSum;
    else
        actualProjection += contribution - goalExpenseSum;

    actualProjection = actualProjection <= 0 ? 0 : actualProjection;

    if (Date == scenario.GetRetirementDate(fp))
        simulation.RetirementAmount = actualProjection;

    ActualProjection = actualProjection;
    simulation.ActualPatrimony = ActualProjection;
}
```

## 4.5 Classe CalculateIndicators

### 4.5.1 Método: CalculatePortfolioVolatility

Calcula a volatilidade do portfólio com base na alocação de ativos.

```csharp
public static decimal CalculatePortfolioVolatility(InternalClassesAllocationDto internalAllocationClasses)
{
    decimal[] vetorPesos = new decimal[13];
    decimal[] vetorResultado = new decimal[vetorPesos.Length];

    foreach (var allocationClass in internalAllocationClasses.AllocationClassInfo)
    {
        if (allocationClass.Name == AssetSubClass.CashPosition.GetDescription()) vetorPesos[0] = allocationClass.Percentage;
        if (allocationClass.Name == AssetSubClass.CreditHighGrade.GetDescription()) vetorPesos[1] = allocationClass.Percentage;
        if (allocationClass.Name == AssetSubClass.CreditHighYield.GetDescription()) vetorPesos[2] = allocationClass.Percentage;
        // ... Código para preencher outros índices ...
    }

    // Cálculo de covariância e volatilidade
    // ...

    var volatilidadeCarteira = Math.Sqrt((double)resultado);
    return (decimal)volatilidadeCarteira * 100;
}
```

### 4.5.2 Método: CalculateExpectedNominalReturn

Calcula o retorno nominal esperado do portfólio.

```csharp
private static decimal CalculateExpectedNominalReturn(InternalClassesAllocationDto internalAllocationClasses)
{
    decimal expectedNominalReturn = 0;

    foreach (var allocationClass in internalAllocationClasses.AllocationClassInfo)
    {
        expectedNominalReturn += (allocationClass.Percentage / 100) * allocationClass.ExpectedNominalReturn;
    }

    return expectedNominalReturn * 100;
}
```
