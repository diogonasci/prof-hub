# 8. Glossário

## 8.1 Termos Financeiros

- **Aporte**: Valor investido periodicamente para acumular patrimônio
- **CDI (Certificado de Depósito Interbancário)**: Taxa de referência para muitos investimentos de renda fixa no Brasil
- **ContributionType**: Tipo de contribuição financeira (Receita, Despesa, Aporte, etc.)
- **Covariância**: Medida de como duas variáveis se movem em conjunto
- **Fase de Acumulação**: Período entre o início do planejamento e a aposentadoria
- **Fase de Aposentadoria**: Período após a aposentadoria até o fim da expectativa de vida
- **Inflação**: Aumento geral no nível de preços de bens e serviços
- **PGBL (Plano Gerador de Benefício Livre)**: Tipo de previdência privada com benefícios fiscais
- **Rentabilidade Nominal**: Retorno bruto dos investimentos, sem descontar inflação
- **Rentabilidade Real**: Retorno líquido após descontar a inflação
- **Score**: Pontuação atribuída ao plano financeiro, indicando sua viabilidade (0-10)
- **Volatilidade**: Medida da dispersão dos retornos de um investimento, indicando seu risco

## 8.2 Termos do Sistema

- **ActualProjection**: Projeção realista do patrimônio baseada nos dados fornecidos
- **AllocationClassInfo**: Informações sobre uma classe de alocação específica
- **BigNumbers**: Métricas agregadas importantes para avaliação do plano
- **Contribution**: Objeto que representa uma receita, despesa ou aporte
- **CustomerFinancialPlanning**: Dados do planejamento financeiro do cliente
- **DateTimeExtensions**: Utilitários para manipulação de datas
- **InternalClassesAllocationDto**: Estrutura para armazenar dados de alocação de classes
- **PatrimonyConsumption**: Projeção do patrimônio na estratégia de consumo (linha vermelha)
- **PatrimonyMaintenance**: Projeção do patrimônio na estratégia de manutenção (linha verde)
- **PointInTime**: Representa um ponto específico (mês) na simulação
- **ProjectionType**: Tipo de projeção (Nominal ou Real)
- **RecurrencyType**: Tipo de recorrência (Mensal, Anual, etc.)

## 8.3 Classes de Ativos

- **AssetSubClass.CashPosition**: Posição em caixa (liquidez imediata)
- **AssetSubClass.CreditHighGrade**: Crédito de alta qualidade
- **AssetSubClass.CreditHighYield**: Crédito de alto rendimento
- **AssetSubClass.ShortInflation**: Inflação de curto prazo
- **AssetSubClass.LongInflation**: Inflação de longo prazo
- **AssetSubClass.InflationCredit**: Crédito atrelado à inflação
- **Domain.Enums.AssetClass.Multimarket**: Fundos multimercado
- **Domain.Enums.AssetClass.VariableIncome**: Renda variável
- **Domain.Enums.AssetClass.GlobalVariableIncome**: Renda variável global
- **Domain.Enums.AssetClass.GlobalFixedIncome**: Renda fixa global
- **Domain.Enums.AssetClass.ListedFunds**: Fundos listados
- **Domain.Enums.AssetClass.Alternative**: Investimentos alternativos
