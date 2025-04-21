# 9. Apêndices

## 9.1 FAQ (Perguntas Frequentes)

### 9.1.1 Aspectos Técnicos

**P: Como o sistema lida com mudanças nos parâmetros econômicos ao longo do tempo?**

R: Os parâmetros econômicos são gerenciados pelo time de Research da XP através de uma interface específica no Product Setup. Quando há necessidade de ajustes nestes parâmetros (como taxas de juros, inflação ou expectativas de retorno), o time de Research realiza a atualização, garantindo que as simulações estejam sempre alinhadas com as perspectivas econômicas mais recentes da XP.

**P: Como o sistema trata impostos sobre investimentos?**

R: O sistema considera o imposto de renda na rentabilidade nominal através do parâmetro IR. Impostos específicos por tipo de investimento precisam ser refletidos nos retornos esperados informados.

**P: É possível simular eventos específicos durante a projeção, como a venda de um imóvel?**

R: Sim, eventos pontuais podem ser modelados como contribuições com data específica. O sistema irá considerar o impacto desse evento na projeção patrimonial a partir daquela data.

**P: Como são calculados os benefícios fiscais do PGBL?**

R: O sistema considera a dedução do imposto de renda durante a fase de acumulação e a tributação durante a fase de desacumulação. A economia fiscal é projetada com base na alíquota de IR do cliente.

### 9.1.2 Aspectos de Negócio

**P: Qual a diferença prática entre as estratégias de manutenção e consumo?**

R: A estratégia de manutenção busca preservar o capital principal, utilizando apenas os rendimentos. Ela requer maior acumulação, mas proporciona mais segurança. A estratégia de consumo planeja esgotar o patrimônio até o fim da expectativa de vida, exigindo menor acumulação, mas com mais riscos de longevidade.

**P: Como interpretar o score do plano financeiro?**

R: O score varia de 0 a 10:

- 0-7: O plano não atinge nem o objetivo de consumo do patrimônio
- 7-10: O plano atinge parcial ou totalmente o objetivo de manutenção do patrimônio Quanto mais próximo de 10, mais robusto é o plano.

**P: Como calcular o patrimônio ideal para a aposentadoria?**

R: Para a estratégia de manutenção, divida a despesa mensal desejada pela rentabilidade real mensal. Para a estratégia de consumo, use a fórmula de valor presente de uma série de pagamentos considerando a expectativa de vida.

**P: O que fazer quando o cliente tem múltiplos objetivos além da aposentadoria?**

R: O sistema prioriza o objetivo principal de aposentadoria, mas permite modelar outros objetivos como despesas ou aportes programados em datas específicas, refletindo seu impacto na projeção patrimonial.
