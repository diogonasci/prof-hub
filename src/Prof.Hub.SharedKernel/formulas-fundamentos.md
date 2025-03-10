# 5. Fórmulas e Fundamentos Matemáticos

## 5.1 Cálculo de Rentabilidade

### 5.1.1 Rentabilidade Nominal

Calcula o retorno nominal considerando a porcentagem do CDI e o imposto.

```
nominalProfitability = (CDIPercentage / 100 × AnnualCDI) × (1 - IR / 100)
```

Onde:

- `CDIPercentage`: Percentual do CDI alvo para o investimento
- `AnnualCDI`: Taxa anual do CDI (em percentual)
- `IR`: Alíquota do imposto de renda (em percentual)

### 5.1.2 Rentabilidade Real

Calcula o retorno real descontando a inflação da rentabilidade nominal.

```
realProfitability = ((1 + nominalProfitability) / (1 + annualInflation)) - 1
```

Onde:

- `nominalProfitability`: Rentabilidade nominal em formato decimal
- `annualInflation`: Taxa de inflação anual em formato decimal

### 5.1.3 Taxa de Inflação Mensal

Converte a taxa de inflação anual para mensal utilizando juros compostos.

```
monthlyInflation = (1 + annualInflation)^(1/12) - 1
```

Onde:

- `annualInflation`: Taxa de inflação anual em formato decimal

## 5.2 Cálculo de Patrimônio Necessário

### 5.2.1 Estratégia de Manutenção

Calcula o patrimônio necessário para viver apenas dos rendimentos.

```
retirementAmount = goalValue / monthlyRealProfitability
```

Onde:

- `goalValue`: Valor mensal desejado na aposentadoria
- `monthlyRealProfitability`: Taxa de rentabilidade real mensal em formato decimal

### 5.2.2 Estratégia de Consumo

Calcula o patrimônio necessário para consumo gradual durante a aposentadoria.

```
retirementAmountForConsumption = salary × (1 - (1 + monthlyRealProfitability)^(-monthsFromRetirementUntilLifeExpectancy)) / monthlyRealProfitability
```

Onde:

- `salary`: Valor mensal desejado na aposentadoria
- `monthlyRealProfitability`: Taxa de rentabilidade real mensal em formato decimal
- `monthsFromRetirementUntilLifeExpectancy`: Número de meses da aposentadoria até o fim da expectativa de vida

## 5.3 Cálculo de Aporte Mensal Ideal

### 5.3.1 Para Estratégia de Manutenção

```
idealContribution = (retirementAmount - initialPatrimony × growthFactor) / ((growthFactor - 1) / monthlyProfitabilityRate)
```

Onde:

- `retirementAmount`: Patrimônio necessário para aposentadoria
- `initialPatrimony`: Patrimônio atual do cliente
- `growthFactor`: Fator de crescimento (1 + monthlyProfitabilityRate)^timeInMonths
- `monthlyProfitabilityRate`: Taxa de rentabilidade mensal em formato decimal
- `timeInMonths`: Tempo em meses até a aposentadoria

### 5.3.2 Para Estratégia de Consumo

Similar à fórmula anterior, mas utilizando o montante calculado para consumo:

```
idealContribution = (retirementAmountConsumption - initialPatrimony × growthFactor) / ((growthFactor - 1) / monthlyProfitability)
```

## 5.4 Cálculo de Volatilidade do Portfólio

A volatilidade total da carteira é calculada considerando os pesos de cada classe de ativo e suas covariâncias:

```
Portfolio Volatility = √[∑(Weight_i × Covariance_i,j × Weight_j)]
```

Onde:

- `Weight_i`: Peso percentual do ativo i na carteira
- `Covariance_i,j`: Covariância entre os ativos i e j
- `Weight_j`: Peso percentual do ativo j na carteira

## 5.5 Cálculo de Retorno Esperado

O retorno nominal esperado do portfólio é calculado pela média ponderada dos retornos esperados de cada classe:

```
Expected Nominal Return = ∑(Percentage/100 × Expected Nominal Return of Class) × 100
```

## 5.6 Cálculo do CDI Return Premium

Representa o prêmio de retorno em relação ao CDI:

```
CDI Return Premium = (((1 + expectedReturn / 100)) / (1 + cdi / 100)) - 1) × 100
```

Onde:

- `expectedReturn`: Retorno esperado em percentual
- `cdi`: Taxa CDI em percentual