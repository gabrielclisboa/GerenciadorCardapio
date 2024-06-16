using GerenciadorCardapio;
using GerenciadorDeCardapio;
using System.IO;

class Program {

	static void Main(string[] args) {
		List<CasosTeste> casosTesteGuloso = new List<CasosTeste>();
		List<CasosTeste> casosTesteDinamico = new List<CasosTeste>();

		Console.WriteLine("\t\t\t\t------BEM VINDO AO SISTEMA DE GERENCIAMENTO DE CARDÁPIO------");
		Console.WriteLine("\nAo informar os casos de teste, insira os dados nessa ordem:");
		Console.WriteLine("\tNúmero de dias - Número de pratos - Orçamento\n");
		Console.WriteLine("Os dados dos pratos devem ser informados nessa ordem:");
		Console.WriteLine("\tCusto do prato - Lucro do prato");

		obterInformacoesPratos(casosTesteGuloso, casosTesteDinamico, 1);

		//imprimeInformacoes(casosTesteDinamico);
		//imprimeInformacoes(casosTesteDinamico);

		Console.WriteLine("\t\t------SOLUÇÃO GULOSA------");
		processaCasoTesteGuloso(casosTesteDinamico);

		Console.WriteLine("\n\t\t------SOLUÇÃO DINÂMICA------\n");
		//processaCasoTesteDinamico(casosTesteDinamico);

	}

	public static void obterInformacoesPratos(List<CasosTeste> casosTesteGuloso, List<CasosTeste> casosTesteDinamico, int numCasoTeste) {
		int numDias = 0, numPratos = 0, orcamento = 0;
		int lucro, custo, qtdPratos;
		string prato;
		List<Prato> pratosGuloso;
		List<Prato> pratosDinamico;

		Console.WriteLine();

		int[] aux = ValidaCasoDeTeste(numCasoTeste);
		numDias = aux[0];
		numPratos = aux[1];
		orcamento = aux[2];

		if (numDias != 0 && numPratos != 0 && orcamento != 0)
		{
			pratosGuloso = new List<Prato>();
			pratosDinamico = new List<Prato>();
			qtdPratos = 0;

			//Console.WriteLine("Pratos:");
			while (qtdPratos < numPratos)
			{
				aux = ValidaPratos(qtdPratos + 1);
				custo = aux[0];
				lucro = aux[1];

				pratosGuloso.Add(new Prato(qtdPratos + 1, custo, lucro));     //inserindo o novo prato na lista
				pratosDinamico.Add(new Prato(qtdPratos + 1, custo, lucro));     //inserindo o novo prato na lista
				qtdPratos++;
			}

			Console.WriteLine("----------------------------------------------------------\t");
			casosTesteGuloso.Add(new CasosTeste(numDias, numPratos, orcamento, pratosGuloso));
			casosTesteDinamico.Add(new CasosTeste(numDias, numPratos, orcamento, pratosDinamico));

			obterInformacoesPratos(casosTesteGuloso, casosTesteDinamico, ++numCasoTeste);

		}
		else
			return;

	}

	public static int[] ValidaCasoDeTeste(int numCasoTeste) // Função responsável por ler e validar os casos de teste
	{
		string casoTeste;
		int[] infoCasoTeste = new int[3];

		while (true)
		{
			Console.WriteLine($"Caso de teste {numCasoTeste}: ");
			casoTeste = Console.ReadLine();

			try
			{
				infoCasoTeste[0] = int.Parse(casoTeste.Split(' ')[0]);  //número de dias
				infoCasoTeste[1] = int.Parse(casoTeste.Split(' ')[1]);  //número de pratos
				infoCasoTeste[2] = int.Parse(casoTeste.Split(' ')[2]);  //orcamento
			}
			catch (Exception e)
			{

				Console.WriteLine("  \n*Por favor, informe 3 números e que sejam inteiros");
				continue;
			}

			if (infoCasoTeste[0] < 0 || infoCasoTeste[0] > 21) // Valida 
			{
				Console.WriteLine("  *Informe valores válidos.\n");
				continue;
			}
			else if (infoCasoTeste[1] < 0 || infoCasoTeste[1] > 50)
			{
				Console.WriteLine("  *Informe valores válidos.\n");
				continue;
			}
			else if (infoCasoTeste[2] < 0 || infoCasoTeste[2] > 100)
			{
				Console.WriteLine("  *Informe valores válidos.\n");
				continue;
			}
			return infoCasoTeste;
		}

	}
	public static int[] ValidaPratos(int numPrato) // Função responsável por ler e validar os pratos
    {
		string prato;
		int[] infoPrato = new int[2];

		while (true)
		{
			Console.WriteLine($"Prato {numPrato}:");
			prato = Console.ReadLine();

			try
			{
				infoPrato[0] = int.Parse(prato.Split(' ')[0]); //armazenando informações do custo
				infoPrato[1] = int.Parse(prato.Split(' ')[1]); //armazenando informações do lucro
			}
			catch (Exception e)
			{
				Console.WriteLine("  *Por favor, informe apenas números\n");
				continue;
			}

			if (infoPrato[0] < 1 || infoPrato[0] > 50)
			{
				Console.WriteLine("  *Informe valores válidos para o custo e lucro.\n");
				continue;
			}
			else if (infoPrato[1] < 1 || infoPrato[1] > 10000)
			{
				Console.WriteLine("  *Informe valores válidos para o custo e lucro.\n");
				continue;
			}
			return infoPrato;
		}

	}

	public static void processaCasoTesteDinamico(List<CasosTeste> casosTeste) {
		double maiorLucroDia = 0;
		List<CasosTeste> casosTesteDinamico = casosTeste;

		Prato pratoLucroDia = new Prato();
		List<Prato> menu = new List<Prato>();
		double lucroTotal = 0;

		for (int i = 0; i < casosTeste.Count; i++) {
			casosTeste[i].pratos = casosTeste[i].pratos.OrderBy(c => c.custo).ToList();

			for (int j = 0; j < casosTeste[i].numDias; j++) {
				double[,] matrizDinamica = criarTabelaDinamica((casosTeste[i].numPratos + 2), (casosTeste[i].numPratos + 1), casosTeste[i].pratos, casosTeste[i].orcamento);

				for (int k = 2; k < matrizDinamica.GetLength(0); k++) {
					for (int l = 1; l < matrizDinamica.GetLength(1); l++) {
						try {
							if (matrizDinamica[k - 1, l] > matrizDinamica[k - 1, (int)matrizDinamica[0, l] - casosTeste[i].pratos[k - 2].custo] + casosTeste[i].pratos[k - 2].lucro) {
								matrizDinamica[k, l] = matrizDinamica[k - 1, l];
							} else {
								matrizDinamica[k, l] = (matrizDinamica[k - 1, (int)matrizDinamica[0, l] - casosTeste[i].pratos[k - 2].custo] + casosTeste[i].pratos[k - 2].lucro);
							}

						} catch (Exception err) {
							matrizDinamica[k, l] = matrizDinamica[k - 1, l];

						}

					}
				}


				PrintMatrix(matrizDinamica);
				maiorLucroDia = (matrizDinamica[matrizDinamica.GetLength(0) - 1, matrizDinamica.GetLength(1) - 1]);
				Console.WriteLine($"\nMAIOR LUCRO: {maiorLucroDia}");


				//Console.WriteLine("---------------------------");

				lucroTotal += maiorLucroDia;

				Console.WriteLine($"LUCRO TOTAL: {lucroTotal}");


				if (maiorLucroDia != 0) {
					casosTeste[i].pratos.ForEach(p => {
						if (p.lucro.Equals(maiorLucroDia)) {
							menu.Add(p);
							pratoLucroDia = p;
							if (p.isReduzido) {
								p.lucro = 0;

							} else {
								p.lucro = (p.lucro / 2);
								p.isReduzido = true;
							}
						}
					});


					casosTeste[i].orcamento = casosTeste[i].orcamento - pratoLucroDia.custo;

					Console.WriteLine("-------LUCRO DOS PRATOS-------");
					casosTeste[i].pratos.ForEach(p => { Console.WriteLine(p.lucro + " \t"); });

				}
			}

			exibirMenuMetodoDinamico(lucroTotal, menu, i);
		}
	}

	static void PrintMatrix(double[,] matrix) {
		int rows = matrix.GetLength(0);
		int columns = matrix.GetLength(1);

		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				Console.Write(matrix[i, j] + "\t");
			}
			Console.WriteLine();
		}
	}
	public static void exibirMenuMetodoDinamico(double lucro, List<Prato> menu, int casoTeste) {
		Console.WriteLine("\nSaída: ");
		Console.WriteLine($"CASO DE TESTE {casoTeste + 1}: ");
		Console.WriteLine("  Lucro máximo: " + lucro);
		Console.WriteLine("  Pratos a ser cozinhados:");
		Console.Write("  ");
		imprimePratos(menu);
		Console.WriteLine(" ----------------- ");
	}

	public static double[,] criarTabelaDinamica(int linhas, int colunas, List<Prato> pratos, double orcamento) {
		double[,] tabelaPreenchida = new double[linhas, colunas];

		tabelaPreenchida[0, 0] = 0;

		for (int i = 1; i < colunas; i++) {
			if (pratos[i - 1].custo <= orcamento)
				tabelaPreenchida[0, i] = pratos[i - 1].custo;
			else if (i > 1)
				tabelaPreenchida[0, i] = tabelaPreenchida[0, i - 1];
			else
				tabelaPreenchida[0, i] = 0;
		}

		for (int i = 0; i < colunas; i++) {
			tabelaPreenchida[1, i] = 0;
		}

		for (int i = 2; i < linhas; i++) {
			tabelaPreenchida[i, 0] = 0;
		}

		return tabelaPreenchida;
	}
	public static void processaCasoTesteGuloso(List<CasosTeste> casosTeste) {
		List<Prato> pratosEscolhidos = new List<Prato>();
		double gastoTotal = 0;

		Console.WriteLine("\nSaída: \n");
		for (int i = 0; i < casosTeste.Count(); i++) {
			pratosEscolhidos = new List<Prato>();
			casosTeste[i].pratos = casosTeste[i].pratos.OrderBy(c => c.custo).ToList(); //Ordenando a lista de pratos em ordem crescente do custo dos pratos

			int qtdDias = 0;
			gastoTotal = 0;
			while (qtdDias < casosTeste[i].numDias) {

				//verificando se o caso de teste possui mais de 1 prato, pois desta forma será possivel escolher um prato diferente daquele escolhido anteriormente
				if (casosTeste[i].pratos.Count > 1) {
					gastoTotal = procuraMelhorPrato(casosTeste[i].pratos, pratosEscolhidos, casosTeste[i].orcamento);
					
				} else {
					//Vai calcular o novo lucro de acordo com a quantidade de vezes seguidas que o prato já foi escolhido
					double novoLucro = calculaNovoLucro(casosTeste[i].pratos[0], pratosEscolhidos.Count());
					pratosEscolhidos.Add(new Prato(casosTeste[i].pratos[0].indice, casosTeste[i].pratos[0].custo, novoLucro));  //Vai adicionar sempre o mesmo prato na lista, mas com novo lucro

				}
				qtdDias++;

			}

			//double gastoTotal = pratosEscolhidos.Sum(x => x.custo);     //Calculando o gasto total para fazer o cardapio escolhido

			Console.WriteLine($"CASO DE TESTE {(i + 1)}: ");

			//Validando se o gasto total não é maior que o orçamento disponível
			if (gastoTotal == 0) {
                Console.WriteLine("Cardápio informado ultrapassou o orçamento");
                Console.WriteLine("0.0");
			} else {
				Console.WriteLine("  Lucro máximo: " + pratosEscolhidos.Sum(x => x.lucro));
				Console.WriteLine("  Pratos a ser cozinhados:");
				Console.Write("  ");
				imprimePratos(pratosEscolhidos);        //Imprimindo os pratos escolhidos para o cardápio

			}
			Console.WriteLine(" ----------------- ");
		}
	}

	//Função para encontrar o proximo prato a ser escolhido
	public static double procuraMelhorPrato(List<Prato> pratos, List<Prato> pratosEscolhidos, int orcamento)
	{
		int i = 0;

		//verificando se algum prato já foi escolhido
		if (pratosEscolhidos.Count() > 0)
		{

			//verificando se o primeiro prato não foi o ultimo a ser escolhido
			if (pratos[i].indice != pratosEscolhidos.Last()?.indice && (pratosEscolhidos.Sum(x => x.custo) + pratos[i].custo) <= orcamento)
			{
				pratosEscolhidos.Add(pratos[i]);    //adicionando o primeiro prato na lista

			}
			else if ((pratosEscolhidos.Sum(x => x.custo) + pratos[i + 1].custo) <= orcamento)
			{
				pratosEscolhidos.Add(pratos[i + 1]);    //adicionando o segundo prato na lista
			}
			else if ((pratosEscolhidos.Sum(x => x.custo) + pratos[i].custo) <= orcamento)
			{
				double novoLucro = 0;
                if (pratosEscolhidos.Last().lucro > 0)
				     novoLucro = pratosEscolhidos.Last().lucro - (pratos[i].lucro / 2); 

				pratosEscolhidos.Add(new Prato(indice: pratos[i].indice, custo: pratos[i].custo, lucro: novoLucro));    //adicionando o primeiro prato na lista novamente
			}
			else
			{
                // Caso o primeiro prato ou o segundo, não possa ser escolhido, siginifca que o cardápio ultrapassou o orçamento
                return 0;
			}

		}
		else if (pratos[i].custo > orcamento)
		{
			// Caso o primeiro prato não possa ser escolhido, siginifca que o cardápio ultrapassou o orçamento
            return 0;

        }
		else
		{
            //Caso nenhum prato tenha sido escolhido, o primeiro prato vai ser escolhido, pois ele possui o menor custo
            pratosEscolhidos.Add(pratos[i]);
        }
        return pratosEscolhidos.Sum(x => x.custo);
    }

	//Função para calcular o novo lucro de pratos escolhidos 2 vezes ou mais seguidas
	public static double calculaNovoLucro(Prato prato, int qdtPratosEscolhidos) {
		if (qdtPratosEscolhidos == 0) {
			return prato.lucro;		//retorna 100% do lucro	
		} else if (qdtPratosEscolhidos == 1) {
			return prato.lucro / 2;		//retorna 50% do lucro
		} else {
			return 0;						
		}
	}


	public static void imprimeInformacoes(List<CasosTeste> casosTeste) {
		Console.WriteLine("\n-----INFORMAÇÕES DOS CASOS DE TESTE----\n");
		foreach (CasosTeste casoTeste in casosTeste) {
			Console.WriteLine("QtdDias\tQtdPratos\tOrçamento");
			Console.Write(casoTeste.numDias);
			Console.Write("\t " + casoTeste.numPratos);
			Console.Write("\t\t " + casoTeste.orcamento);
			Console.WriteLine();

			Console.WriteLine("Pratos:");
			Console.WriteLine("Indice\tCusto\tLucro");
			foreach (var prato in casoTeste.pratos) {
				Console.Write(prato.indice);
				Console.Write("\t " + prato.custo);
				Console.Write("\t " + prato.lucro);
				Console.WriteLine();
			}

			Console.WriteLine("\n");
		}
	}

	public static void imprimePratos(List<Prato> pratos) {
		foreach (var prato in pratos) {
			Console.Write(" " + prato.indice);
		}
		Console.WriteLine();
	}


}