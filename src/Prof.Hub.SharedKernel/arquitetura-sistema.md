# 3. Arquitetura do Sistema

## 3.1 Principais Classes

O sistema é organizado em várias classes principais que trabalham em conjunto:

### 3.1.1 Simulation

Classe central que coordena toda a simulação financeira. Responsabilidades:

- Iniciar o processo de cálculo para todos os pontos no tempo
- Manter estado atual dos cálculos (patrimônio atual, projeções, etc.)
- Coordenar o fluxo de dados entre as diferentes partes do sistema
- Calcular métricas agregadas e pontuação do plano

### 3.1.2 Scenario

Define os parâmetros de cenário para a simulação. Responsabilidades:

- Determinar idade de aposentadoria e outras datas-chave
- Calcular valores ideais de contribuição mensal
- Calcular montantes necessários para diferentes estratégias
- Gerenciar métricas relevantes para o cenário (BigNumbers)

### 3.1.3 Strategy

Gerencia cálculos relacionados a estratégia financeira, especialmente inflação e rentabilidade. Responsabilidades:

- Calcular taxas de inflação mensal e anual
- Aplicar inflação a valores ao longo do tempo
- Calcular rentabilidade nominal e real
- Determinar taxas de rentabilidade mensal

### 3.1.4 PointInTime

Representa um momento específico (mês) na simulação e calcula valores para este ponto. Responsabilidades:

- Calcular projeção real do patrimônio
- Calcular projeção de manutenção do patrimônio
- Calcular projeção de consumo do patrimônio
- Gerenciar benefícios fiscais (PGBL)

### 3.1.5 CalculateIndicators

Calcula indicadores para o portfólio de investimentos. Responsabilidades:

- Calcular retorno nominal esperado
- Calcular volatilidade do portfólio
- Calcular covariâncias entre classes de ativos
- Mapear classes de alocação e suas subdivisões

## 3.2 Diagrama Conceitual de Classes

```
┌───────────────┐     ┌───────────────┐     ┌───────────────┐
│  Simulation   │────▶│   Scenario    │────▶│   Strategy    │
└───────┬───────┘     └───────┬───────┘     └───────────────┘
        │                     │
        ▼                     ▼
┌───────────────┐     ┌───────────────┐
│  PointInTime  │     │CalculateIndica│
└───────────────┘     │     tors      │
                      └───────────────┘
```

## 3.3 Fluxo de Dados

O sistema processa as informações seguindo este fluxo básico:

1. Recebe dados do cliente (CustomerFinancialPlanning)
2. Cria timelines de receitas, despesas e contribuições
3. Para cada mês da simulação: 
   a. Calcula receitas e despesas do mês 
   b. Aplica inflação quando necessário 
   c. Calcula rendimentos do patrimônio 
   d. Atualiza projeções (real, manutenção, consumo)
4. Ao final, calcula métricas agregadas (BigNumbers)
5. Avalia o plano e atribui uma pontuação (score)

## 3.4 Principais Estruturas de Dados

- **Dictionary<string, PointInTime>**: Armazena as projeções mensais
- **Contribution**: Representa uma entrada ou saída financeira
- **Dictionary<string, List<Contribution>>**: Organiza contribuições por data
- **InternalClassesAllocationDto**: Estrutura para alocações de classes de ativos
