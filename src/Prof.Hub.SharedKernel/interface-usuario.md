# 6. Interface e Experiência do Usuário

## 6.1 Visão Geral da Interface

O sistema Financial Planning possui uma interface moderna e intuitiva, organizada em seções que seguem o fluxo natural do processo de planejamento financeiro.

### 6.1.1 Dashboard Inicial

- Apresenta um resumo dos dados do cliente
- Código do cliente
- Patrimônio total
- Posição consolidada
- Política de investidor

### 6.1.2 Formulários de Input

- **Informações Pessoais**: Dados básicos do cliente
- **Objetivos**: Definição de metas financeiras, especialmente aposentadoria
- **Receitas e Despesas**: Cadastro do fluxo financeiro mensal e pontual
- **Patrimônio**: Registro detalhado dos ativos do cliente, organizados por categoria

### 6.1.3 Visualizações de Output

- **Projeção Financeira**: Gráficos mostrando as três curvas principais (projeção atual, manutenção do patrimônio e consumo do patrimônio)
- **Balanço Financeiro**: Visão tabular e gráfica das receitas e despesas
- **Cenários**: Comparativo entre diferentes estratégias e seus resultados esperados

### 6.1.4 Relatório Final

O sistema gera automaticamente um relatório PDF personalizado com todas as análises e recomendações.

## 6.2 Fluxo de Interação do Usuário

O processo de criação de um planejamento financeiro segue estas etapas na interface:

1. **Cadastro do Cliente**: Inserção dos dados pessoais básicos
2. **Definição de Objetivos**: Especificação da renda mensal desejada na aposentadoria, idade de aposentadoria e expectativa de vida
3. **Registro Financeiro**: Cadastro detalhado de receitas e despesas recorrentes e pontuais
4. **Inventário Patrimonial**: Registro de todos os ativos do cliente, com categorização e percentuais de alocação
5. **Definição de Cenários**: Configuração dos parâmetros de rentabilidade, inflação e estratégia de aposentadoria
6. **Análise de Resultados**: Visualização das projeções geradas e do score de viabilidade
7. **Geração de Relatório**: Criação de um documento PDF personalizado para o cliente

## 6.3 Elementos Visuais Principais

### 6.3.1 Gráfico de Projeção Financeira

Este é o elemento visual central do sistema, contendo três curvas principais:

- **Linha Preta (Projeção Atual)**: Representa a evolução real esperada do patrimônio do cliente com base nos dados fornecidos
- **Linha Verde (Manutenção do Patrimônio)**: Representa o patrimônio necessário para implementar a estratégia de viver apenas dos rendimentos
- **Linha Vermelha (Consumo do Patrimônio)**: Representa o patrimônio necessário para implementar a estratégia de consumo gradual do capital

O ponto onde as linhas se cruzam com a marca da idade de aposentadoria (representada por um ícone de aposentadoria) é crítico para a análise.

### 6.3.2 Indicador de Probabilidade de Sucesso

Este elemento visual circular mostra, em uma escala de 0% a 100%, a probabilidade do plano financeiro atingir os objetivos estabelecidos:

- **0-30%**: Baixa probabilidade de sucesso, representada em vermelho escuro
- **31-60%**: Probabilidade média, representada em vermelho/amarelo
- **61-100%**: Alta probabilidade, representada em verde

O valor é calculado com base no score estabelecido na classe Simulation, convertido para uma escala percentual.
