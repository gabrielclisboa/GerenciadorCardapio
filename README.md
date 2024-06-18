# Sistema de Gerenciamento de Cardápio
## Alunos

* Caio Vitor Souza Fernandes
* Gabriel Campos Ferreira Lisboa
* Lucas Ferreira Guedes
* Roberto Eller Paiva
* Roberto Eller Paiva
* Thais Alves Silva

### 1) Como esse problema pode ser modelado para o paradigma guloso? 
A abordagem de um algoritmo guloso baseia-se na tomada de decisões consideradas localmente ótimas em cada etapa, com a esperança de que essas decisões levem a uma solução globalmente ótima.
Desta forma, para resolver o problema proposto utilizou-se a heurística de selecionar sempre o prato com o menor custo disponível, visando adicionar o maior número possível de pratos ao cardápio e assim, maximizar o lucro total. 

Para isso, a lista de pratos foi previamente ordenada em ordem crescente de custo. A abordagem gulosa sempre tenta escolher o primeiro elemento dessa lista, 
pois ele possui o menor custo. Se este elemento tiver sido escolhido por último, o algoritmo tentará selecionar o segundo elemento da lista, já que o primeiro não proporcionará mais 100% do seu lucro. 
Porém se o segundo prato exceder o orçamento e o primeiro ainda estiver um custo dentro do limite de orçamento, o primeiro será selecionado. Não é necessário avaliar os outros elementos da lista, 
pois se o segundo prato exceder o orçamento, em uma lista ordenada pelos custos, os demais pratos também irão ultrapassar.

Para exemplificar esse processo, vamos analisar o caso de teste abaixo:

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/38d7c6b6-b319-4ba0-bd8f-ced01185103b)

Após ordenar os pratos em ordem crescente de custo, a lista ficará na seguinte ordem: 

prato 2 | prato 1 | prato 3. 

Para cada dia, o algoritmo guloso tentará primeiramente selecionar o prato 2, pois ele possui o menor custo. No dia seguinte, como o prato 2 foi o último a ser escolhido, o algoritmo avaliará o prato 1, verificando se seu custo está dentro do orçamento disponível, se estiver, ele adiciona o prato 1 no cardápio.
Caso o custo do prato 1 ultrapasse o orçamento, o algoritmo não precisará validar o prato 3, já que a lista está ordenada e todos os pratos subsequentes também excederão o orçamento. Nesse cenário, o algoritmo continuará tentando selecionar o prato 2 repetidamente, caso o seu custo ainda esteja dentro do orçamento. Ele repetirá esse processo até finalizar a quantidade de dias.
O resultado final será: 

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/a74eded4-7ea8-4d7c-807f-47654a88e747)


### 2) Seu algoritmo guloso apresenta uma solução ótima? Por quê? 

O algoritmo guloso desenvolvido não apresenta uma solução ótima. Isso ocorre devido ao comportamento de tentar escolher sempre de forma intercalada o prato de menor custo e o segundo prato de menor custo. Esta abordagem abre espaço para alguns casos em que a avaliação do lucro seria necessária para se obter a solução ótima, como por exemplo: o lucro de 50% do primeiro prato ser superior ao lucro do segundo prato.

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/fccb29a7-bf33-497a-bbd0-e2d64b17a0e8)

Neste exemplo, seria mais vantajoso e lucrativo fazer a escolha do primeiro prato na segunda iteração do algoritmo, dado que seu lucro mesmo com desconto é superior ao lucro do segundo prato. Entretanto, o algoritmo tenta contornar esse desconto não escolhendo o prato que foi escolhido anteriormente, partindo para o segundo prato do cardápio que possui o menor custo, almejando o seu lucro máximo.

Outro exemplo é o seguinte:

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/45dc5598-c28a-48ab-b807-f7eadcad0e75)

Teríamos a solução ótima caso o algoritmo escolhesse o prato de custo 20, visto que o seu lucro é superior ao do primeiro prato, que possui o menor custo do cardápio. O algoritmo não avalia o lucro de um prato; ele sempre faz a escolha com base no custo, mesmo que haja outros pratos no cardápio que possuam um lucro maior. Como apresentado no exemplo acima, esses pratos não serão escolhidos devido ao seu custo.

### 3) Como esse problema pode ser modelado para o paradigma de programação dinâmica? 
A programação dinâmica possibilita a resolução de problemas que possuem a propriedade de subestrutura ótima, ou seja, a solução ótima pode ser  construída a partir das soluções ótimas dos seus subproblemas. Além disso, uma de suas vantagens é o reaproveitamento do resultado obtido de subproblemas resolvidos anteriormente. Nesse sentido, a modelagem utilizada, para o paradigma de programação dinâmica, faz o uso da abordagem bottom-up com o intuito de armazenar em uma tabela os lucros máximos possíveis, conforme a disponibilidade de pratos por linha e o orçamento das colunas da tabela. Ao final do preenchimento da tabela, é selecionada a última célula preenchida pelo algoritmo, isto é, a célula localizada na última linha e coluna, a qual representa o lucro máximo possível, conforme os dias, pratos e orçamento informados.

Não obstante, para explanar de maneira mais clara o funcionamento da solução implementada, segue abaixo as principais etapas da execução da solução desenvolvida com as seguintes entradas :

**Cenário:**

Dias: 3      |           Orçamento: 7          |            Pratos:3

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/9851bc17-b2d3-4757-ac53-9fda4c20c451)

Etapa 1) Construção da Tabela
Nessa etapa, ocorre o pré-preenchimento da matriz. Esse processo atribui valor para os cabeçalho, o qual recebe valores de 0 até n em incrementos de 1 em 1, sendo n o orçamento máximo. Após isso, é preenchida a primeira coluna da matriz com o lucro total 0. Por fim, a primeira linha, desconsiderando o cabeçalho, é preenchida com lucro total 0, pois nessa linha é representado os possíveis lucros considerando um conjunto vazio de possíveis pratos. 

É utilizado a seguinte estrutura de célula para armazenar as informações:

**3**-10-3,1-1- : Representa o lucro total da célula.

3-**10-3**,1-**1**- : Representa o custo de cada prato que foi informado pelo usuário.

3-10-3,**1**-1-:  Quantas vezes um determinado prato foi utilizado. 3,1 significa que o prato de custo 3 foi utilizado 1 vez. 

Quando o valor, que representa o custo individual de cada prato, estiver sem nenhuma “,” quer dizer que esse prato não foi escolhido nenhuma vez, exemplo :  3-**10**-3,1-**1**- .

Dessa forma, a célula 0-10-3-1- indica que o lucro total é 0 e que os pratos que possuem custo 10,3 e 1 estão sendo utilizados 0 vezes.

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/a91577a7-fd3a-48fc-a28f-c93e15799cb2)

Durante as explicações a seguir vou me referir a uma célula com “Célula[1][2]”, sendo o [1] a linha(desconsiderando o cabeçalho) e o [2] a coluna.

Etapa 2) Preenchimento das linhas

Após a construção da matriz, ocorre o preenchimento das demais células restantes. Esse processo ocorre de forma sequencial, isto é, da coluna 1 até a coluna 7 começando pela linha 2 até a linha 4. Cada linha possui um conjunto de possíveis pratos para serem utilizados. A definição de qual prato vai ser acrescentado a cada nova linha é definido por uma lista de pratos ordenada de forma decrescente de acordo com o lucro. Sendo assim, os pratos de maior lucro vão ser acrescentados primeiro. Logo, as combinações feitas ao longo do preenchimento da matriz vão tender a gerar lucros maiores quanto a cada iteração.

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/7a084160-d8c4-401a-b033-fb484eaee1ed)

Utilizando como exemplo o preenchimento da célula[2][1] , é realizado os seguintes passos:

1) É possível obter um lucro maior do que o lucro obtido na célula[2][-8] (não existe)  selecionando o prato 2 somado com o valor da célula[2][1] ? Não, pois a célula[2][-8] não existe. Sendo assim é selecionado o caso base.


2) O orçamento está sendo extrapolado ? Não, pois o caso base possui o valor de lucro total igual a 0, que é inferior a 1(limite de lucro da coluna 1).	

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/05265f12-4da3-4ec2-8fa0-0227f3ce08e6)

Esse exemplo demonstra outra característica da solução desenvolvida, que é sempre se atentar ao valor do orçamento e o custo do prato a cada iteração. Avançando algumas execuções, temos outro cenário:

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/c5e34ff2-abe2-4e25-9c43-cf9c8f14e9ae)


Para calcular o valor da célula[4][4], temos novamente os seguintes passos:

1) É possível obter um lucro maior do que o lucro obtido na célula[3][4] (7,5)  selecionando o prato 1 somado com o valor da célula[4][1] ? Sim, pois 8 (lucro da célula[4][1] + prato 1) é maior do que 7,5.

3) O orçamento está sendo extrapolado? Não, a combinação de pratos selecionados na comparação do passo anterior (pratos 1 e 3 que somados resultam no lucro 8), geram o custo total igual a 4, o qual é compatível com o custo máximo da coluna 4, que é igual a 4.

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/71a7529e-4909-4919-950f-a934f2f7aca4)

Etapa 3) Definindo o menu

Ao final da execução da etapa 2, é possível observar o seguinte resultado : 

![image](https://github.com/gabrielclisboa/GerenciadorCardapio/assets/72041841/7a1df521-4965-4a03-b92b-532b6f30b042)

Percebe-se que o lucro total da célula[4][7], última célula, é igual a 13. Conforme o supracitado, a última célula representa o lucro máximo que é passível de se obter considerando todo orçamento informado pelo usuário. Além disso, por conta da estruturação da célula adotada, também é visível os pratos selecionados. A ordem mais eficiente é definida por uma função interna que recebe pratos, da última célula, e retorna a ordem a qual eles geram o maior lucro possível. O retorno dessa função é exibido para o usuário. Dessa forma, o programa finaliza.

### 4) Discuta a subestrutura ótima e a sobreposição dos problemas. 

A subestrutura ótima permite decompor um problema grande em subproblemas menores e combina as soluções ótimas desses subproblemas para construir a solução ótima do problema original. A sobreposição de subproblemas permite a reutilização eficiente dos resultados dos subproblemas já resolvidos, evitando cálculos redundantes. Essas duas propriedades são fundamentais para a aplicação eficaz da programação dinâmica em uma ampla gama de problemas computacionais.

### 5) Algum algoritmo clássico foi adaptado para resolver o problema? Se sim, qual foi ele? 

Não.






  
