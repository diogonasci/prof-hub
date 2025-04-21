### 5.7.2 Projeção de Manutenção do Patrimônio (PatrimonyMaintenanceProjectionCalculate)

**Conceito**: Esta projeção representa a evolução ideal do patrimônio para que o cliente consiga viver apenas dos rendimentos na aposentadoria, sem consumir o capital principal. É representada pela linha verde nas visualizações gráficas do sistema e serve como referência para o objetivo mais conservador.

**Importância**: A projeção de manutenção estabelece uma meta clara e segura para o cliente. Se a projeção real (linha preta) estiver acima da linha verde na data da aposentadoria, o cliente terá a tranquilidade de saber que poderá viver apenas dos rendimentos, preservando seu capital e eventualmente deixando herança.

**Como funciona**:
1. O sistema divide o cálculo em duas fases: antes e depois da aposentadoria
2. Durante a fase de acumulação, considera o patrimônio atual, seu rendimento e o aporte ideal mensal
3. Durante a fase de aposentadoria, considera apenas os rendimentos, sem consumo de capital
4. A projeção é atualizada mês a mês até o fim da expectativa de vida

**Fórmula**:
Durante a fase de acumulação:
```
patrimonyMaintenance = patrimonyMaintenance + monthlyDividend + idealContributionValue
```

Durante a fase de aposentadoria (com rentabilidade real):
```
patrimonyMaintenance = patrimonyMaintenance + monthlyDividend
```

**Parâmetros**:
- `patrimonyMaintenance`: Patrimônio acumulado na estratégia de manutenção até o mês anterior
- `monthlyDividend`: Rendimento mensal do patrimônio
- `idealContributionValue`: Valor ideal mensal para atingir o objetivo (calculado na seção 5.3.1)

**Exemplo prático**:
Fase de acumulação (antes da aposentadoria):
Suponha que:
- Patrimônio acumulado na estratégia de manutenção: R$ 500.000,00
- Rentabilidade mensal: 0.946% (11.92% ao ano)
- Aporte mensal ideal: R$ 875,83 (calculado na seção 5.3.1)

```
monthlyDividend = 500,000 × 0.00946 = R$ 4,730.00

patrimonyMaintenance = 500,000 + 4,730 + 875.83
patrimonyMaintenance = R$ 505,605.83
```

Fase de aposentadoria:
Suponha que:
- Patrimônio acumulado na aposentadoria: R$ 1.733.101,39
- Rentabilidade real mensal: 0.572% (7.10% ao ano)

```
monthlyDividend = 1,733,101.39 × 0.00572 = R$ 9,913.34

patrimonyMaintenance = 1,733,101.39 + 9,913.34
patrimonyMaintenance = R$ 1,743,014.73
```

Na aposentadoria, observamos que o rendimento mensal (R$ 9.913,34) é praticamente igual ao valor desejado de R$ 10.000,00, confirmando a precisão do cálculo do patrimônio necessário. O pequeno crescimento patrimonial ocorre porque o rendimento é um pouco menor que o valor desejado, mas com o passar dos meses esta diferença diminui devido ao crescimento do patrimônio.

**No código**: Este cálculo é implementado no método `PatrimonyMaintenanceProjectionCalculate()` da classe `PointInTime` e é executado para cada mês da projeção, formando a linha verde nas visualizações do sistema.## 5.3 Cálculo de Aporte Mensal Ideal

### 5.3.1 Para Estratégia de Manutenção

**Conceito**: Este cálculo determina o valor mensal que o cliente precisa investir regularmente durante a fase de acumulação para atingir o patrimônio necessário para a estratégia de manutenção. Este é um dos "números mágicos" mais importantes que o sistema oferece ao cliente.

**Importância**: Definir o valor de aporte mensal ideal é crucial para o planejamento financeiro, pois orienta o cliente sobre quanto ele deve economizar e investir mensalmente para atingir seus objetivos de aposentadoria. Este valor serve como referência para ajustes no orçamento e tomada de decisões financeiras.

**Como funciona**:
1. O sistema identifica o montante total necessário para a aposentadoria (conforme calculado na seção 5.2.1)
2. Considera o patrimônio atual do cliente
3. Calcula quanto este patrimônio atual crescerá até a aposentadoria considerando os rendimentos
4. Determina o valor mensal que, investido regularmente, preencherá a diferença entre o patrimônio projetado e o necessário

**Fórmula**:
```
idealContribution = (retirementAmount - initialPatrimony × growthFactor) / ((growthFactor - 1) / monthlyProfitabilityRate)
```

**Parâmetros**:
- `retirementAmount`: Patrimônio necessário para aposentadoria na estratégia de manutenção
- `initialPatrimony`: Patrimônio atual do cliente
- `growthFactor`: Fator de crescimento, calculado como (1 + monthlyProfitabilityRate)^timeInMonths
- `monthlyProfitabilityRate`: Taxa de rentabilidade mensal em formato decimal
- `timeInMonths`: Número de meses até a aposentadoria

**Exemplo prático**:
Suponha que:
- O patrimônio necessário para aposentadoria seja R$ 1.733.101,39 (calculado anteriormente)
- O patrimônio atual do cliente seja R$ 100.000,00
- Faltam 240 meses (20 anos) para a aposentadoria
- A rentabilidade nominal mensal seja de 0.946% (11.92% ao ano)

Primeiro, calculamos o fator de crescimento:
```
growthFactor = (1 + 0.00946)^240
growthFactor = 1.00946^240
growthFactor = 9.48
```

Agora calculamos o aporte mensal ideal:
```
idealContribution = (1,733,101.39 - 100,000 × 9.48) / ((9.48 - 1) / 0.00946)
idealContribution = (1,733,101.39 - 948,000) / (8.48 / 0.00946)
idealContribution = 785,101.39 / 896.41
idealContribution = R$ 875.83
```

Este cálculo mostra que o cliente precisaria investir R$ 875,83 por mês durante os próximos 20 anos para acumular o patrimônio necessário para viver apenas dos rendimentos na aposentadoria, considerando que já possui R$ 100.000,00 investidos.

**No código**: Este cálculo é implementado no método `GetIdealContributionValue()` da classe `Scenario` e é um dos principais outputs do sistema para orientar o cliente.# 5. Fórmulas e Fundamentos Matemáticos

## 5.1 Cálculo de Rentabilidade

### 5.1.1 Rentabilidade Nominal

**Conceito**: A rentabilidade nominal é o retorno bruto de um investimento antes de considerar os efeitos da inflação. No sistema Financial Planning, calculamos a rentabilidade nominal como uma função da taxa CDI (Certificado de Depósito Interbancário), ajustada por um percentual escolhido e descontada do imposto de renda.

**Importância**: Este cálculo é fundamental porque os investimentos em renda fixa no Brasil são frequentemente indexados como um percentual do CDI. O desempenho de um investimento é comumente avaliado pela sua capacidade de entregar um determinado percentual do CDI.

**Como funciona**:
1. O usuário define o percentual do CDI que deseja considerar (por exemplo, 110% do CDI)
2. Este percentual é multiplicado pela taxa CDI anual atual
3. O resultado é então ajustado pelo imposto de renda, que reduz o ganho efetivo

**Fórmula**:
```
nominalProfitability = (CDIPercentage / 100 × AnnualCDI) × (1 - IR / 100)
```

**Parâmetros**:
- `CDIPercentage`: Percentual do CDI alvo para o investimento (ex: 110%)
- `AnnualCDI`: Taxa anual do CDI em percentual (ex: 12.75%)
- `IR`: Alíquota do imposto de renda em percentual (ex: 15% para investimentos com prazo superior a 2 anos)

**Exemplo prático**:
Considere um investimento que rende 110% do CDI, com CDI anual de 12.75% e alíquota de IR de 15%:

```
nominalProfitability = (110 / 100 × 12.75) × (1 - 15 / 100)
nominalProfitability = 14.025 × 0.85
nominalProfitability = 11.92%
```

Assim, este investimento teria uma rentabilidade nominal anual de 11.92%, que representa o ganho efetivo antes de considerar os efeitos da inflação.

**No código**: Este cálculo é realizado no método `GetNominalProfitability()` da classe `Strategy`, que retorna a taxa de rentabilidade nominal anual utilizada em diversas projeções financeiras no sistema.

### 5.1.2 Rentabilidade Real

**Conceito**: A rentabilidade real representa o ganho efetivo de poder de compra proporcionado por um investimento após descontar os efeitos da inflação. Enquanto a rentabilidade nominal indica o aumento numérico do capital, a rentabilidade real mostra o quanto esse capital aumentou em termos de poder aquisitivo.

**Importância**: Em planejamentos financeiros de longo prazo, especialmente para aposentadoria, o que realmente importa é a rentabilidade real, pois ela garante que o capital investido manterá ou aumentará seu poder de compra ao longo do tempo. Em períodos de inflação alta, um investimento pode ter rentabilidade nominal positiva, mas rentabilidade real negativa (perda de poder aquisitivo).

**Como funciona**:
1. O sistema utiliza a rentabilidade nominal previamente calculada
2. Ajusta esta rentabilidade com base na taxa de inflação anual
3. O resultado é a taxa real de crescimento do poder aquisitivo do capital

**Fórmula**:
```
realProfitability = ((1 + nominalProfitability) / (1 + annualInflation)) - 1
```

**Parâmetros**:
- `nominalProfitability`: Rentabilidade nominal em formato decimal (ex: 0.1192 para 11.92%)
- `annualInflation`: Taxa de inflação anual em formato decimal (ex: 0.0450 para 4.50%)

**Exemplo prático**:
Considerando a rentabilidade nominal de 11.92% calculada no exemplo anterior e uma inflação anual de 4.50%:

```
realProfitability = ((1 + 0.1192) / (1 + 0.0450)) - 1
realProfitability = (1.1192 / 1.0450) - 1
realProfitability = 1.0710 - 1
realProfitability = 0.0710 = 7.10%
```

Neste exemplo, embora o investimento tenha uma rentabilidade nominal de 11.92%, o ganho efetivo em poder de compra (rentabilidade real) é de 7.10%. Isto significa que, apesar da inflação corroer parte do ganho nominal, o investidor ainda aumenta seu poder aquisitivo em 7.10% ao ano.

**No código**: Este cálculo é implementado no método `GetRealProfitability()` da classe `Strategy` e é fundamental para todas as projeções de longo prazo que visam manter o poder de compra do cliente durante a aposentadoria.

### 5.1.3 Taxa de Inflação Mensal

**Conceito**: A taxa de inflação mensal representa a variação dos preços de produtos e serviços em um período de um mês. No Financial Planning, convertemos a taxa de inflação anual em mensal para realizar cálculos com precisão em bases mensais, permitindo projeções mais granulares.

**Importância**: Como o sistema processa as projeções financeiras mês a mês, é necessário trabalhar com taxas mensais para garantir que o efeito da inflação seja aplicado corretamente em cada período. Utilizar simplesmente a taxa anual dividida por 12 seria matematicamente incorreto devido à natureza dos juros compostos.

**Como funciona**:
1. O sistema recebe a taxa de inflação anual (geralmente fornecida como um dado macroeconômico)
2. A taxa anual é convertida para uma taxa mensal equivalente usando a fórmula de juros compostos
3. Esta taxa mensal é então utilizada para calcular o impacto da inflação em cada período do planejamento

**Fórmula**:
```
monthlyInflation = (1 + annualInflation)^(1/12) - 1
```

**Parâmetros**:
- `annualInflation`: Taxa de inflação anual em formato decimal (ex: 0.0450 para 4.50%)
- `monthlyInflation`: Taxa resultante de inflação mensal em formato decimal

**Exemplo prático**:
Considerando uma inflação anual de 4.50%, calculamos a taxa mensal equivalente:

```
monthlyInflation = (1 + 0.0450)^(1/12) - 1
monthlyInflation = (1.0450)^0.0833 - 1
monthlyInflation = 1.00367 - 1
monthlyInflation = 0.00367 = 0.367%
```

Isto significa que um produto que custa R$ 100,00 hoje custará aproximadamente R$ 100,37 após um mês, considerando a inflação anual de 4.50%.

**No código**: Este cálculo é implementado no método `GetMonthlyInflationRate()` da classe `Strategy` e é usado extensivamente em diversos pontos do sistema para ajustar valores futuros pela inflação.

### 5.1.4 Aplicação da Inflação a Valores (ApplyMonthlyInflationRate)

**Conceito**: Este cálculo aplica o efeito cumulativo da inflação a um valor ao longo de um determinado período. É essencial para projetar valores futuros considerando a perda de poder aquisitivo da moeda ao longo do tempo.

**Importância**: No planejamento financeiro de longo prazo, é fundamental considerar como a inflação afetará os valores futuros. Por exemplo, se um cliente deseja ter uma renda mensal de R$ 10.000 na aposentadoria daqui a 20 anos, esse valor precisará ser ajustado pela inflação acumulada no período para manter o mesmo poder de compra.

**Como funciona**:
1. O sistema recebe um valor atual (em moeda de hoje)
2. Projeta esse valor para uma data futura aplicando a taxa de inflação composta 
3. O resultado é o valor futuro equivalente em termos de poder de compra

**Fórmula**:
```
adjustedValue = initialValue × (1 + monthlyInflationRate)^numberOfMonths
```

**Parâmetros**:
- `initialValue`: Valor inicial (atual), sem considerar inflação
- `monthlyInflationRate`: Taxa mensal de inflação em formato decimal (calculada na seção 5.1.3)
- `numberOfMonths`: Número de meses para projeção
- `adjustedValue`: Valor ajustado pela inflação

**Exemplo prático**:
Considere que o cliente deseja uma renda mensal de R$ 10.000 na aposentadoria, que ocorrerá daqui a 240 meses (20 anos). Com uma taxa de inflação mensal de 0.367% (derivada de uma inflação anual de 4.50%):

```
adjustedValue = 10,000 × (1 + 0.00367)^240
adjustedValue = 10,000 × (1.00367)^240
adjustedValue = 10,000 × 2.4033
adjustedValue = R$ 24,033.00
```

Este cálculo mostra que, para manter o poder de compra equivalente a R$ 10.000 de hoje, o cliente precisará de R$ 24.033,00 mensais daqui a 20 anos, considerando a inflação projetada.

**No código**: Este cálculo é implementado no método `ApplyMonthlyInflationRate()` da classe `Strategy` e é aplicado em diversos pontos do sistema onde valores futuros precisam ser ajustados pela inflação, principalmente na projeção de necessidades financeiras na aposentadoria.

### 5.1.5 Cálculo de Rentabilidade Mensal (GetMonthlyProfitabilityRate)

**Conceito**: Similar à conversão da inflação anual para mensal, este cálculo converte a taxa de rentabilidade anual para uma taxa equivalente mensal. Isso permite que o sistema calcule com precisão os rendimentos mensais sobre o patrimônio acumulado.

**Importância**: Como o sistema Financial Planning simula a evolução patrimonial mês a mês, precisamos calcular os rendimentos mensais com precisão. A simples divisão da taxa anual por 12 não seria matematicamente correta devido ao efeito dos juros compostos.

**Como funciona**:
1. O sistema utiliza a taxa de rentabilidade anual (que pode ser nominal ou real, dependendo da configuração)
2. Converte esta taxa anual para uma taxa mensal equivalente usando a fórmula de juros compostos
3. Essa taxa mensal é usada para calcular os rendimentos a cada mês nas projeções financeiras

**Fórmula**:
```
monthlyProfitability = (1 + annualProfitability)^(1/12) - 1
```

**Parâmetros**:
- `annualProfitability`: Taxa anual de rentabilidade em formato decimal (ex: 0.0710 para 7.10%)
- `monthlyProfitability`: Taxa mensal equivalente de rentabilidade em formato decimal

**Exemplo prático**:
Considerando uma rentabilidade anual real de 7.10% (conforme calculado anteriormente):

```
monthlyProfitability = (1 + 0.0710)^(1/12) - 1
monthlyProfitability = (1.0710)^0.0833 - 1
monthlyProfitability = 1.00572 - 1
monthlyProfitability = 0.00572 = 0.572%
```

Isto significa que um patrimônio de R$ 1.000.000,00 geraria aproximadamente R$ 5.720,00 de rendimento real em um mês, considerando a taxa anual de 7.10%.

**No código**: Este cálculo é implementado no método `GetMonthlyProfitabilityRate()` da classe `Scenario` e é fundamental para todas as projeções de evolução patrimonial, sendo usado para calcular os rendimentos mensais tanto na fase de acumulação quanto na fase de aposentadoria.

### 5.1.6 Cálculo de Rentabilidade Real Mensal (GetRealMonthlyProfitabilityRate)

**Conceito**: Este cálculo determina a taxa efetiva mensal de aumento do poder aquisitivo proporcionado pelos investimentos, após descontar os efeitos da inflação. Combina os conceitos de rentabilidade mensal e inflação mensal para obter uma métrica precisa do ganho real em base mensal.

**Importância**: Na projeção patrimonial de longo prazo, especialmente durante a fase de aposentadoria, é essencial trabalhar com a rentabilidade real para garantir que o poder de compra seja mantido. Este cálculo permite que o sistema simule com precisão quanto o patrimônio realmente crescerá em termos de poder aquisitivo a cada mês.

**Como funciona**:
1. O sistema calcula a taxa de rentabilidade nominal mensal
2. Calcula também a taxa de inflação mensal
3. Ajusta a rentabilidade nominal pela inflação para obter a rentabilidade real
4. O resultado é usado para projetar o crescimento efetivo do poder aquisitivo do patrimônio

**Fórmula**:
```
realMonthlyProfitability = ((1 + nominalMonthlyProfitability) / (1 + monthlyInflation)) - 1
```

**Parâmetros**:
- `nominalMonthlyProfitability`: Taxa mensal de rentabilidade nominal
- `monthlyInflation`: Taxa mensal de inflação
- `realMonthlyProfitability`: Taxa real mensal de rentabilidade (ganho efetivo de poder aquisitivo)

**Exemplo prático**:
Considerando uma rentabilidade nominal mensal de 0.946% (equivalente a 11.92% ao ano) e uma inflação mensal de 0.367% (equivalente a 4.50% ao ano):

```
realMonthlyProfitability = ((1 + 0.00946) / (1 + 0.00367)) - 1
realMonthlyProfitability = (1.00946 / 1.00367) - 1
realMonthlyProfitability = 1.00577 - 1
realMonthlyProfitability = 0.00577 = 0.577%
```

Este resultado mostra que, embora o patrimônio cresça nominalmente 0.946% ao mês, o ganho efetivo de poder aquisitivo é de 0.577% mensal, o que é consistente com a taxa anual real de 7.10% calculada anteriormente.

**No código**: Este cálculo é implementado no método `GetRealMonthlyProfitabilityRate()` na classe `Scenario` e é especialmente importante para as projeções que utilizam a opção `ProjectionType.NominalProfitability`, onde os valores são ajustados pela inflação.

## 5.2 Cálculo de Patrimônio Necessário

### 5.2.1 Estratégia de Manutenção

**Conceito**: Este cálculo determina o montante total de patrimônio necessário para que o cliente possa viver apenas dos rendimentos, sem consumir o capital principal. É a base da chamada "linha verde" nas projeções do sistema.

**Importância**: A estratégia de manutenção do patrimônio é altamente conservadora e segura, pois garante que o cliente nunca esgotará seu capital, mesmo se viver além da expectativa de vida. Esta abordagem também proporciona maior tranquilidade e possibilita deixar herança, sendo especialmente valorizada por clientes mais conservadores.

**Como funciona**:
1. O sistema identifica o valor mensal desejado pelo cliente na aposentadoria
2. Calcula o montante de capital necessário para que os rendimentos mensais cubram exatamente esse valor desejado
3. O objetivo é que o rendimento mensal seja igual ao valor necessário para manter o padrão de vida na aposentadoria

**Fórmula**:
```
retirementAmount = goalValue / monthlyRealProfitability
```

**Parâmetros**:
- `goalValue`: Valor mensal desejado na aposentadoria (em R$)
- `monthlyRealProfitability`: Taxa de rentabilidade real mensal em formato decimal
- `retirementAmount`: Montante total necessário para atingir o objetivo

**Exemplo prático**:
Suponha que o cliente deseje uma renda mensal de R$ 10.000 na aposentadoria e o sistema trabalhe com uma rentabilidade real mensal de 0.577% (equivalente aos 7.10% anuais calculados anteriormente):

```
retirementAmount = 10,000 / 0.00577
retirementAmount = R$ 1,733,101.39
```

Este cálculo mostra que o cliente precisará acumular aproximadamente R$ 1,73 milhão até a aposentadoria para que, considerando apenas os rendimentos reais (acima da inflação), seja possível extrair R$ 10.000 mensais sem consumir o capital.

**No código**: Este cálculo é implementado no método `GetRetirementAmountMaintenance()` da classe `Scenario` e é fundamental para determinar o objetivo financeiro a ser atingido na estratégia de manutenção do patrimônio.

### 5.2.2 Estratégia de Consumo

**Conceito**: Este cálculo determina o montante de patrimônio necessário para que o cliente possa manter seu padrão de vida na aposentadoria consumindo gradualmente o capital até o fim da expectativa de vida. É a base da chamada "linha vermelha" nas projeções do sistema.

**Importância**: A estratégia de consumo do patrimônio exige um capital inicial menor comparado à estratégia de manutenção, tornando os objetivos financeiros mais acessíveis. Esta abordagem é mais adequada para clientes que priorizam qualidade de vida durante a aposentadoria e não têm como objetivo principal deixar herança.

**Como funciona**:
1. O sistema identifica o valor mensal desejado pelo cliente na aposentadoria
2. Considera o número de meses entre a aposentadoria e o fim da expectativa de vida
3. Calcula o montante necessário para que, considerando os rendimentos e o consumo gradual do capital, seja possível manter o padrão de vida desejado pelo período estimado

**Fórmula**:
```
retirementAmountForConsumption = salary × (1 - (1 + monthlyRealProfitability)^(-monthsFromRetirementUntilLifeExpectancy)) / monthlyRealProfitability
```

**Parâmetros**:
- `salary`: Valor mensal desejado na aposentadoria (em R$)
- `monthlyRealProfitability`: Taxa de rentabilidade real mensal em formato decimal
- `monthsFromRetirementUntilLifeExpectancy`: Número de meses entre a aposentadoria e o fim da expectativa de vida
- `retirementAmountForConsumption`: Montante necessário para a estratégia de consumo

**Exemplo prático**:
Suponha que:
- O cliente deseje uma renda mensal de R$ 10.000 na aposentadoria
- A expectativa de vida após a aposentadoria seja de 300 meses (25 anos)
- A rentabilidade real mensal seja de 0.577% (7.10% ao ano)

```
retirementAmountForConsumption = 10,000 × (1 - (1 + 0.00577)^(-300)) / 0.00577
retirementAmountForConsumption = 10,000 × (1 - (1.00577)^(-300)) / 0.00577
retirementAmountForConsumption = 10,000 × (1 - 0.1784) / 0.00577
retirementAmountForConsumption = 10,000 × 0.8216 / 0.00577
retirementAmountForConsumption = R$ 1,424,264.13
```

Este cálculo mostra que o cliente precisaria acumular aproximadamente R$ 1,42 milhão até a aposentadoria para poder extrair R$ 10.000 mensais até o fim da expectativa de vida, consumindo gradualmente o capital. Note que este valor é cerca de 18% menor que o montante necessário para a estratégia de manutenção (R$ 1,73 milhão).

**No código**: Este cálculo é implementado no método `GetRetirementAmountConsumption()` da classe `Scenario` e é utilizado para determinar o objetivo financeiro na estratégia de consumo do patrimônio.

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

**Conceito**: Este cálculo determina o valor mensal que o cliente precisa investir regularmente durante a fase de acumulação para atingir o patrimônio necessário para a estratégia de consumo. Similar ao cálculo para a estratégia de manutenção, mas com um objetivo de patrimônio menor.

**Importância**: O aporte mensal ideal para a estratégia de consumo geralmente é mais acessível, tornando-se uma opção mais viável para muitos clientes. Este valor permite que o cliente avalie se seus objetivos financeiros são realistas dentro de sua capacidade atual de investimento.

**Como funciona**:
1. O sistema identifica o montante total necessário para a aposentadoria na estratégia de consumo (conforme calculado na seção 5.2.2)
2. Considera o patrimônio atual do cliente e sua projeção até a aposentadoria
3. Calcula o valor mensal necessário para complementar a diferença entre o patrimônio projetado e o necessário
4. A fórmula é essencialmente a mesma da estratégia de manutenção, apenas usando um valor-alvo diferente

**Fórmula**:
```
idealContribution = (retirementAmountConsumption - initialPatrimony × growthFactor) / ((growthFactor - 1) / monthlyProfitability)
```

**Parâmetros**:
- `retirementAmountConsumption`: Patrimônio necessário para aposentadoria na estratégia de consumo
- `initialPatrimony`: Patrimônio atual do cliente
- `growthFactor`: Fator de crescimento, calculado como (1 + monthlyProfitabilityRate)^timeInMonths
- `monthlyProfitability`: Taxa de rentabilidade mensal em formato decimal
- `timeInMonths`: Número de meses até a aposentadoria

**Exemplo prático**:
Suponha que:
- O patrimônio necessário para a estratégia de consumo seja R$ 1.424.264,13 (calculado anteriormente)
- O patrimônio atual do cliente seja R$ 100.000,00
- Faltam 240 meses (20 anos) para a aposentadoria
- A rentabilidade nominal mensal seja de 0.946% (11.92% ao ano)

Já calculamos o fator de crescimento:
```
growthFactor = 9.48
```

Agora calculamos o aporte mensal ideal para a estratégia de consumo:
```
idealContribution = (1,424,264.13 - 100,000 × 9.48) / ((9.48 - 1) / 0.00946)
idealContribution = (1,424,264.13 - 948,000) / (8.48 / 0.00946)
idealContribution = 476,264.13 / 896.41
idealContribution = R$ 531.31
```

Este cálculo mostra que, para a estratégia de consumo, o cliente precisaria investir R$ 531,31 por mês durante 20 anos - um valor significativamente menor que os R$ 875,83 necessários para a estratégia de manutenção. Isso demonstra como a escolha da estratégia pode tornar os objetivos de aposentadoria mais acessíveis.

**No código**: Este cálculo é implementado no método `GetIdealContributionValueForConsumption()` da classe `Scenario` e é apresentado ao cliente como uma alternativa mais acessível à estratégia de manutenção.

## 5.4 Cálculo de Volatilidade do Portfólio

**Conceito**: A volatilidade do portfólio é uma medida do risco total da carteira de investimentos, considerando não apenas o risco individual de cada ativo, mas também como eles se comportam em conjunto. Este cálculo é fundamental para avaliar se a alocação de ativos está adequada ao perfil de risco do cliente.

**Importância**: O equilíbrio entre risco e retorno é crucial no planejamento financeiro. A volatilidade do portfólio ajuda a determinar se a carteira de investimentos está alinhada com a tolerância a risco do cliente e com seus objetivos de longo prazo. Uma volatilidade muito alta pode causar ansiedade e levar a decisões emocionais prejudiciais em momentos de crise.

**Como funciona**:
1. O sistema identifica o peso (percentual) de cada classe de ativo na carteira
2. Utiliza uma matriz de covariâncias que representa como cada par de classes de ativos se correlaciona
3. Aplica a fórmula da volatilidade do portfólio, que considera tanto os pesos quanto as covariâncias
4. O resultado é uma medida percentual da variação esperada dos retornos da carteira

**Fórmula**:
```
Portfolio Volatility = √[∑(Weight_i × Covariance_i,j × Weight_j)]
```

**Parâmetros**:
- `Weight_i`: Peso percentual do ativo i na carteira (em formato decimal)
- `Covariance_i,j`: Covariância entre os ativos i e j
- `Weight_j`: Peso percentual do ativo j na carteira (em formato decimal)

**Exemplo prático**:
Considere uma carteira com três classes de ativos:
- Renda Fixa: 50% da carteira, volatilidade anual de 3%
- Renda Variável Nacional: 30% da carteira, volatilidade anual de 18%
- Renda Variável Internacional: 20% da carteira, volatilidade anual de 15%

E a seguinte matriz de correlação:
- Correlação Renda Fixa & Renda Variável Nacional: 0.3
- Correlação Renda Fixa & Renda Variável Internacional: 0.2
- Correlação Renda Variável Nacional & Renda Variável Internacional: 0.7

Primeiro, calculamos as covariâncias:
```
Cov(RF, RVN) = 0.3 × 3% × 18% = 0.162%
Cov(RF, RVI) = 0.2 × 3% × 15% = 0.09%
Cov(RVN, RVI) = 0.7 × 18% × 15% = 1.89%
```

Agora, calculamos a variância do portfólio:
```
Var(P) = (0.5)² × (3%)² + (0.3)² × (18%)² + (0.2)² × (15%)² + 
         2 × 0.5 × 0.3 × 0.162% + 2 × 0.5 × 0.2 × 0.09% + 2 × 0.3 × 0.2 × 1.89%
         
Var(P) = 0.25 × 0.0009 + 0.09 × 0.0324 + 0.04 × 0.0225 + 
         2 × 0.15 × 0.00162 + 2 × 0.1 × 0.0009 + 2 × 0.06 × 0.0189
         
Var(P) = 0.000225 + 0.002916 + 0.0009 + 
         0.000486 + 0.00018 + 0.002268
         
Var(P) = 0.006975
```

Finalmente, calculamos a volatilidade do portfólio (desvio padrão):
```
Portfolio Volatility = √0.006975 = 0.0835 = 8.35%
```

Este cálculo mostra que, embora a carteira contenha ativos de alta volatilidade (renda variável), a diversificação e as correlações entre os ativos resultam em uma volatilidade total de 8.35%, significativamente menor que a volatilidade individual dos ativos de maior risco.

**No código**: Este cálculo é implementado no método `CalculatePortfolioVolatility()` da classe `CalculateIndicators` e é utilizado para avaliar o risco total da carteira do cliente, permitindo ajustes para adequá-la ao seu perfil de risco.

## 5.5 Cálculo de Retorno Esperado

**Conceito**: O cálculo do retorno esperado do portfólio determina a rentabilidade projetada da carteira como um todo, considerando o retorno individual esperado de cada classe de ativo e sua respectiva participação no portfólio. Este é um dos parâmetros mais importantes para as projeções patrimoniais.

**Importância**: O retorno esperado é fundamental para todas as projeções de longo prazo e para determinar o aporte ideal. Uma estimativa realista de retorno, baseada na alocação atual e em premissas de mercado, permite planejamentos mais confiáveis e ajuda a evitar tanto o excesso de otimismo quanto o pessimismo exagerado.

**Como funciona**:
1. O sistema identifica o peso (percentual) de cada classe de ativo na carteira
2. Atribui uma expectativa de retorno nominal para cada classe, com base em dados históricos e projeções de mercado
3. Calcula a média ponderada dos retornos esperados, considerando a participação de cada classe
4. O resultado é o retorno nominal esperado da carteira como um todo

**Fórmula**:
```
Expected Nominal Return = ∑(Percentage/100 × Expected Nominal Return of Class) × 100
```

**Parâmetros**:
- `Percentage`: Percentual de cada classe de ativo no portfólio
- `Expected Nominal Return of Class`: Retorno nominal esperado para cada classe (em decimal)

**Exemplo prático**:
Considere a mesma carteira do exemplo anterior:
- Renda Fixa: 50% da carteira, retorno nominal esperado de 10% a.a.
- Renda Variável Nacional: 30% da carteira, retorno nominal esperado de 14% a.a.
- Renda Variável Internacional: 20% da carteira, retorno nominal esperado de 12% a.a.

Calculamos o retorno esperado do portfólio:
```
Expected Nominal Return = (50% × 10% + 30% × 14% + 20% × 12%) × 100
Expected Nominal Return = (0.5 × 0.10 + 0.3 × 0.14 + 0.2 × 0.12) × 100
Expected Nominal Return = (0.05 + 0.042 + 0.024) × 100
Expected Nominal Return = 0.116 × 100
Expected Nominal Return = 11.6%
```

Este cálculo mostra que, com a alocação atual, o portfólio tem uma expectativa de retorno nominal de 11.6% ao ano. Este valor seria então utilizado como base para as projeções patrimoniais e cálculos de aporte ideal.

**No código**: Este cálculo é implementado no método `CalculateExpectedNominalReturn()` da classe `CalculateIndicators` e é utilizado como parâmetro fundamental para todas as projeções financeiras do sistema.

## 5.6 Cálculo do CDI Return Premium

**Conceito**: O CDI Return Premium (ou prêmio sobre o CDI) representa o retorno adicional que o portfólio oferece acima da taxa CDI. Este conceito é amplamente utilizado no mercado financeiro brasileiro, onde o CDI é uma referência importante para investimentos.

**Importância**: O prêmio sobre o CDI ajuda a avaliar o desempenho relativo da carteira em comparação com um investimento conservador em renda fixa. É uma métrica valiosa para entender se o risco adicional assumido na alocação está sendo devidamente recompensado por um retorno superior.

**Como funciona**:
1. O sistema compara o retorno esperado total do portfólio com a taxa CDI atual
2. Calcula a diferença relativa entre esses retornos, considerando o efeito dos juros compostos
3. O resultado é apresentado como um percentual que representa o prêmio de retorno sobre o CDI

**Fórmula**:
```
CDI Return Premium = (((1 + expectedReturn / 100)) / (1 + cdi / 100)) - 1) × 100
```

**Parâmetros**:
- `expectedReturn`: Retorno esperado do portfólio em percentual (ex: 11.6%)
- `cdi`: Taxa CDI anual em percentual (ex: 12.75%)

**Exemplo prático**:
Considerando um retorno esperado de 11.6% para o portfólio e uma taxa CDI de 10.0%:

```
CDI Return Premium = (((1 + 11.6 / 100)) / (1 + 10.0 / 100)) - 1) × 100
CDI Return Premium = ((1.116 / 1.10) - 1) × 100
CDI Return Premium = (1.0145 - 1) × 100
CDI Return Premium = 0.0145 × 100
CDI Return Premium = 1.45%
```

Este cálculo mostra que o portfólio tem um prêmio de retorno de 1.45% sobre o CDI. Em outras palavras, espera-se que o portfólio renda 1.45% a mais que um investimento atrelado a 100% do CDI.

Se o resultado fosse negativo, indicaria que o portfólio tem uma expectativa de rendimento inferior ao CDI, o que poderia levar a uma reavaliação da estratégia de investimentos.

**No código**: Este cálculo é implementado no método `CalculateCDIReturnPremium()` da classe `CalculateIndicators` e é utilizado para avaliar o desempenho relativo esperado da carteira em comparação com a taxa de referência do mercado.

## 5.7 Cálculo de Projeções Patrimoniais

### 5.7.1 Projeção Real do Patrimônio (PatrimonyRealProjectionCalculate)

**Conceito**: Esta projeção representa a evolução esperada do patrimônio do cliente ao longo do tempo, considerando seu patrimônio atual, aportes mensais, rendimentos e objetivos financeiros. É a linha preta nas visualizações gráficas do sistema e representa a trajetória mais provável do patrimônio.

**Importância**: A projeção real é o coração do planejamento financeiro, pois mostra ao cliente como seu patrimônio evoluirá com base em suas decisões atuais e premissas econômicas. Ela permite identificar se o cliente está no caminho certo para atingir seus objetivos de aposentadoria e serve como base para recomendações de ajustes.

**Como funciona**:
1. Para cada mês da projeção, o sistema calcula o rendimento do patrimônio acumulado
2. Adiciona as receitas e subtrai as despesas do mês
3. Considera os aportes realizados ou a capacidade de investimento
4. Atualiza o valor do patrimônio para o próximo mês da projeção
5. O processo se repete até o fim da expectativa de vida

**Fórmula**:
```
actualProjection = simulation.ActualPatrimony + monthlyDividend + (totalRevenues - goalRetirementSum ou contribution - goalExpenseSum)
```

**Parâmetros**:
- `simulation.ActualPatrimony`: Patrimônio acumulado até o mês anterior
- `monthlyDividend`: Rendimento mensal do patrimônio (ActualPatrimony * monthlyProfitabilityRate)
- `totalRevenues`: Soma de todas as receitas do mês
- `goalRetirementSum`: Soma dos valores de objetivos de aposentadoria
- `goalExpenseSum`: Soma dos valores de despesas de objetivos
- `contribution`: Valor de contribuição mensal (capacidade de investimento)

**Exemplo prático**:
Suponha que em determinado mês temos:
- Patrimônio acumulado: R$ 500.000,00
- Rentabilidade mensal: 0.946% (11.92% ao ano)
- Receitas totais: R$ 15.000,00
- Despesas totais: R$ 10.000,00
- Não há objetivos específicos neste mês

```
monthlyDividend = 500,000 × 0.00946 = R$ 4,730.00

// Como não há objetivos específicos, usamos a diferença entre receitas e despesas
contribution = 15,000 - 10,000 = R$ 5,000.00

actualProjection = 500,000 + 4,730 + 5,000
actualProjection = R$ 509,730.00
```

Este cálculo mostra que, neste mês específico, o patrimônio cresceria de R$ 500.000,00 para R$ 509.730,00, um aumento de R$ 9.730,00 resultante de R$ 4.730,00 de rendimentos e R$ 5.000,00 de novo aporte.

**No código**: Este cálculo é implementado no método `PatrimonyRealProjectionCalculate()` da classe `PointInTime` e é executado para cada mês da projeção, formando a linha preta que representa a evolução esperada do patrimônio.

### 5.7.2 Projeção de Manutenção do Patrimônio (PatrimonyMaintenanceProjectionCalculate)

Calcula a projeção ideal para manutenção do patrimônio (linha verde).

Durante a fase de acumulação:
```
patrimonyMaintenance = patrimonyMaintenance + monthlyDividend + idealContributionValue
```

Durante a fase de aposentadoria (com rentabilidade real):
```
patrimonyMaintenance = patrimonyMaintenance + monthlyDividend
```

Onde:
- `patrimonyMaintenance`: Patrimônio acumulado na estratégia de manutenção
- `monthlyDividend`: Rendimento mensal (patrimonyMaintenance * monthlyProfitabilityRate/monthlyRealProfitability)
- `idealContributionValue`: Valor ideal mensal para atingir o objetivo

### 5.7.3 Projeção de Consumo do Patrimônio (PatrimonyConsumptionProjectionCalculate)

**Conceito**: Esta projeção representa a evolução ideal do patrimônio para a estratégia onde o cliente consome gradualmente o capital durante a aposentadoria, esgotando-o ao final da expectativa de vida. É representada pela linha vermelha nas visualizações gráficas do sistema e serve como referência para um objetivo mais acessível.

**Importância**: A projeção de consumo oferece uma alternativa mais viável para muitos clientes, exigindo um patrimônio inicial menor. Se a projeção real (linha preta) estiver acima da linha vermelha na data da aposentadoria, o cliente poderá manter seu padrão de vida desejado durante toda a aposentadoria, mesmo que o capital seja gradualmente consumido.

**Como funciona**:
1. Similar à projeção de manutenção, o sistema divide o cálculo em duas fases
2. Durante a fase de acumulação, considera o patrimônio atual, rendimentos e o aporte ideal para esta estratégia
3. Durante a fase de aposentadoria, considera tanto os rendimentos quanto o consumo parcial do capital
4. A projeção é feita para que o patrimônio se esgote precisamente ao final da expectativa de vida

**Fórmula**:
Durante a fase de acumulação:
```
patrimonyConsumption = patrimonyConsumption + monthlyDividend + idealContribution
```

Durante a fase de aposentadoria:
```
patrimonyConsumption = patrimonyConsumption + monthlyDividend - goalValueAdjusted
```

**Parâmetros**:
- `patrimonyConsumption`: Patrimônio acumulado na estratégia de consumo até o mês anterior
- `monthlyDividend`: Rendimento mensal do patrimônio
- `idealContribution`: Valor de aporte ideal para estratégia de consumo
- `goalValueAdjusted`: Valor ajustado do objetivo (despesa mensal na aposentadoria)

**Exemplo prático**:
Fase de acumulação (antes da aposentadoria):
Suponha que:
- Patrimônio acumulado na estratégia de consumo: R$ 500.000,00
- Rentabilidade mensal: 0.946% (11.92% ao ano)
- Aporte mensal ideal para consumo: R$ 531,31 (calculado na seção 5.3.2)

```
monthlyDividend = 500,000 × 0.00946 = R$ 4,730.00

patrimonyConsumption = 500,000 + 4,730 + 531.31
patrimonyConsumption = R$ 505,261.31
```

Fase de aposentadoria:
Suponha que:
- Patrimônio acumulado na aposentadoria: R$ 1.424.264,13
- Rentabilidade real mensal: 0.572% (7.10% ao ano)
- Valor mensal desejado: R$ 10.000,00

```
monthlyDividend = 1,424,264.13 × 0.00572 = R$ 8,146.79

patrimonyConsumption = 1,424,264.13 + 8,146.79 - 10,000
patrimonyConsumption = R$ 1,422,410.92
```

Neste exemplo, observamos que durante a aposentadoria o patrimônio diminui ligeiramente a cada mês (R$ 1.424.264,13 para R$ 1.422.410,92), pois o rendimento mensal (R$ 8.146,79) é insuficiente para cobrir o valor desejado de R$ 10.000,00. A diferença de R$ 1.853,21 é retirada do capital principal. Com o passar dos meses, à medida que o capital diminui, os rendimentos também diminuem, acelerando o consumo do patrimônio até seu esgotamento ao final da expectativa de vida.

**No código**: Este cálculo é implementado no método `PatrimonyConsumptionProjectionCalculate()` da classe `PointInTime` e é executado para cada mês da projeção, formando a linha vermelha nas visualizações do sistema.

## 5.8 Métricas de Avaliação do Plano

### 5.8.1 Cálculo do Score (SetScore)

**Conceito**: O score é uma pontuação que avalia a viabilidade do plano financeiro, considerando o patrimônio projetado na data de aposentadoria em relação aos patrimônios necessários para as diferentes estratégias. É uma métrica resumida que facilita a comunicação da qualidade do plano para o cliente.

**Importância**: Uma métrica clara e objetiva ajuda os clientes a entenderem rapidamente se seu plano financeiro está no caminho certo. O score traduz cálculos financeiros complexos em uma escala intuitiva de 0 a 10, facilitando a tomada de decisão e permitindo comparações entre diferentes cenários.

**Como funciona**:
1. O sistema analisa o patrimônio projetado (linha preta) na data da aposentadoria
2. Compara este valor com os patrimônios necessários para as estratégias de consumo (linha vermelha) e manutenção (linha verde)
3. Calcula o score em uma escala de 0 a 10, onde valores acima de 7 indicam que pelo menos a estratégia de consumo é viável
4. Valores entre 7 e 10 representam o grau de aproximação à estratégia de manutenção

**Fórmula**:
Se o patrimônio projetado estiver abaixo do necessário para consumo:
```
score = (actualProjection / patrimonyConsumption) * 7
```

Se o patrimônio projetado ultrapassar o necessário para consumo:
```
score = ((actualProjection - patrimonyConsumption) / (patrimonyMaintenance - patrimonyConsumption)) * 3 + 7
```

**Parâmetros**:
- `actualProjection`: Patrimônio projetado na data de aposentadoria
- `patrimonyConsumption`: Patrimônio necessário para estratégia de consumo
- `patrimonyMaintenance`: Patrimônio necessário para estratégia de manutenção

**Exemplo prático**:
Cenário 1 - Abaixo da estratégia de consumo:
Suponha que:
- Patrimônio projetado na aposentadoria: R$ 1.000.000,00
- Patrimônio necessário para consumo: R$ 1.424.264,13
- Patrimônio necessário para manutenção: R$ 1.733.101,39

```
score = (1,000,000 / 1,424,264.13) * 7
score = 0.7020 * 7
score = 4.91
```

Neste cenário, o score de 4.91 indica que o plano atingirá apenas cerca de 70% do objetivo mínimo (estratégia de consumo), sinalizando que ajustes são necessários.

Cenário 2 - Entre as estratégias de consumo e manutenção:
Suponha que:
- Patrimônio projetado na aposentadoria: R$ 1.600.000,00
- Patrimônio necessário para consumo: R$ 1.424.264,13
- Patrimônio necessário para manutenção: R$ 1.733.101,39

```
score = ((1,600,000 - 1,424,264.13) / (1,733,101.39 - 1,424,264.13)) * 3 + 7
score = (175,735.87 / 308,837.26) * 3 + 7
score = 0.5690 * 3 + 7
score = 1.71 + 7
score = 8.71
```

Este score de 8.71 indica que o plano não apenas atinge o objetivo mínimo (estratégia de consumo), mas está significativamente próximo do objetivo ideal (estratégia de manutenção).

**No código**: Este cálculo é implementado no método `SetScore()` da classe `Simulation` e o resultado é apresentado visualmente como um indicador de qualidade do plano financeiro.

### 5.8.2 Cálculo de Big Numbers (SetBigNumbers)

**Conceito**: Os "Big Numbers" são métricas agregadas que resumem os principais indicadores do plano financeiro. Eles capturam valores médios anuais, totais acumulados e outras estatísticas relevantes para uma visão condensada da situação financeira e das projeções.

**Importância**: Estas métricas simplificam a comunicação com o cliente, destacando os números mais importantes do planejamento financeiro. Elas ajudam a contextualizar os valores, facilitam comparações entre diferentes cenários e permitem uma avaliação rápida da viabilidade do plano.

**Como funciona**:
1. O sistema analisa todas as projeções mensais até a data da aposentadoria
2. Calcula médias anuais de receitas, despesas e capacidade de investimento
3. Identifica anos com valores de destaque (maior receita, maior despesa, etc.)
4. Compara o patrimônio projetado com os patrimônios necessários para as diferentes estratégias
5. Calcula a contribuição dos aportes e dos rendimentos para o patrimônio final

**Principais métricas calculadas**:
- `AverageAnnualRevenue`: Receita anual média até a aposentadoria
- `AverageAnnualExpense`: Despesa anual média até a aposentadoria
- `AverageAnnualInvestmentCapacity`: Capacidade média anual de investimento
- `YearWithHighestRevenue`: Ano com maior receita projetada
- `YearWithHighestExpense`: Ano com maior despesa projetada
- `ActualPatrimony`: Patrimônio projetado na data da aposentadoria
- `IdealPatrimony`: Patrimônio necessário para a estratégia de manutenção
- `ConsumptionPatrimony`: Patrimônio necessário para a estratégia de consumo
- `PatrimonyFromContributions`: Parcela do patrimônio final resultante de aportes
- `PatrimonyFromReturns`: Parcela do patrimônio final resultante de rendimentos
- `IdealContributionValue`: Valor anual de aporte ideal para a estratégia de manutenção
- `IdealContributionValueForConsumption`: Valor anual de aporte ideal para a estratégia de consumo

**Exemplo prático**:
Suponha as seguintes projeções até a aposentadoria (20 anos):
- Total de receitas: R$ 3.600.000,00 (R$ 15.000,00 mensais por 240 meses)
- Total de despesas: R$ 2.400.000,00 (R$ 10.000,00 mensais por 240 meses)
- Total de contribuições: R$ 1.200.000,00 (R$ 5.000,00 mensais por 240 meses)
- Patrimônio inicial: R$ 100.000,00
- Patrimônio projetado na aposentadoria: R$ 1.600.000,00

```
AverageAnnualRevenue = 3,600,000 / 20 = R$ 180,000.00
AverageAnnualExpense = 2,400,000 / 20 = R$ 120,000.00
AverageAnnualInvestmentCapacity = 1,200,000 / 20 = R$ 60,000.00

PatrimonyFromContributions = 1,200,000
PatrimonyFromReturns = 1,600,000 - 1,200,000 - 100,000 = R$ 300,000.00
```

Estas métricas mostram que:
- O cliente tem uma receita anual média de R$ 180.000,00
- Suas despesas anuais médias são de R$ 120.000,00
- Sua capacidade anual de investimento é de R$ 60.000,00
- Do patrimônio final de R$ 1.600.000,00, R$ 1.200.000,00 (75%) vêm de aportes diretos, R$ 300.000,00 (18,75%) vêm de rendimentos, e R$ 100.000,00 (6,25%) é o patrimônio inicial

**No código**: Este cálculo é implementado no método `SetBigNumbers()` da classe `Simulation` e os resultados são apresentados em uma seção específica do relatório para o cliente.