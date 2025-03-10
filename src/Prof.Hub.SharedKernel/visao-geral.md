# 1. Visão Geral

## 1.1 Propósito do Sistema

O Financial Planning foi desenvolvido para auxiliar na criação e análise de simulações financeiras de longo prazo, com foco especial em planejamento para aposentadoria. Ele permite modelar a evolução patrimonial de um cliente ao longo do tempo, considerando diversas variáveis como inflação, retorno de investimentos, impostos e diferentes estratégias de acumulação de capital.

## 1.2 Principais Funcionalidades

O sistema oferece as seguintes capacidades principais:

- **Projeção patrimonial**: Simula a evolução do patrimônio ao longo do tempo, desde o momento atual até o fim da expectativa de vida
- **Cálculo de aportes ideais**: Determina o valor mensal que o cliente precisa investir para atingir seus objetivos financeiros
- **Análise de estratégias**: Compara diferentes abordagens para aposentadoria (manutenção vs. consumo de capital)
- **Gestão de riscos**: Calcula a volatilidade da carteira com base na alocação dos ativos
- **Simulação fiscal**: Considera benefícios fiscais (como PGBL) e impacto de impostos no retorno
- **Cenários "e se"**: Permite testar diferentes cenários de inflação, rentabilidade e idade de aposentadoria
- **Pontuação do plano**: Avalia a viabilidade e probabilidade de sucesso do plano financeiro

## 1.3 Público-alvo

O sistema foi projetado especificamente para os assessores que atendem pessoas físicas no ecossistema XP:

- **Assessores diretos da XP**: Profissionais contratados diretamente pela XP Investimentos que utilizam o sistema para criar e analisar planos financeiros personalizados para seus clientes pessoas físicas
- **Assessores de escritórios parceiros (B2B)**: Profissionais vinculados aos escritórios parceiros da XP que, embora não sejam diretamente contratados pela XP, utilizam a plataforma para atender também clientes pessoas físicas

O Financial Planning é uma ferramenta exclusiva disponibilizada para todo o ecossistema de assessoria da XP, permitindo que todos os assessores, sejam diretos ou de escritórios parceiros, ofereçam um serviço de valor agregado com análises sofisticadas e personalizadas de planejamento para aposentadoria.

## 1.4 Fluxo Básico de Operação

O sistema funciona seguindo esta sequência principal:

1. **Entrada de dados do cliente**: Informações pessoais, patrimônio atual, receitas, despesas e objetivos
2. **Definição de cenário**: Configuração de parâmetros como inflação, retorno esperado e idade de aposentadoria
3. **Processamento mês a mês**: Cálculo da evolução patrimonial para cada mês até o fim da expectativa de vida
4. **Geração de projeções**: Criação das linhas de projeção real, manutenção e consumo
5. **Cálculo de métricas**: Determinação de valores como aporte ideal, montante necessário e score do plano
6. **Apresentação dos resultados**: Visualização das projeções e recomendações
