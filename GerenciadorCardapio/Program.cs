using GerenciadorCardapio;
using GerenciadorDeCardapio;
using System.IO;

/*
Projeto: Sistema de Gerenciamento de Cardápio
Disciplina: Fundamentos de Projeto e Análise de Algoritmos - Sistemas de Informação
Professora: Amália Soares Vieira de Vasconcelos
Autores:   
	- Caio Vitor Souza Fernandes        (1216303@sga.pucminas.br)
	- Gabriel Campos Ferreira Lisboa    (1362353@sga.pucminas.br)
	- Lucas Ferreira Guedes                (1405925@sga.pucminas.br)
	- Roberto Eller Paiva                (1395158@sga.pucminas.br)
	- Thais Alves Silva                    (1390032@sga.pucminas.br)
Versão: 1.3.0
Data de Entrega: 17/06/2024
Última Modificação: 17/06/2024
Descrição: 
    Este sistema ajuda Alfred a planejar seu cardápio para os próximos dias, maximizando o lucro obtido com a preparação dos pratos enquanto respeita o orçamento disponível. 
    O problema foi solucionado utilizando uma abordagem gulosa e programação dinâmica. 
Histórico de Versões:
	[Versão 1.0.0]: Criação das classes de dominio do problema.
	[Versão 1.1.0]: Criação dos métodos para leitura e armazenamento das informações do usuário
	[Versão 1.2.0]: Implementação da soução utilizando abordagem gulosa.
	[Versão 1.2.1]: Implementação da soução utilizando abordagem dinâmica.
	[Versão 1.3.0]: Correção de bugs e melhorias nas saídas para o usuário.
*/

class Program {

	static void Main(string[] args) {
		List<CasosTeste> casosTesteGuloso = new List<CasosTeste>();
		List<CasosTeste> casosTesteDinamico = new List<CasosTeste>();

		Console.WriteLine("\t\t\t\t------BEM VINDO AO SISTEMA DE GERENCIAMENTO DE CARDÁPIO------");
		Console.WriteLine("\nCasos de teste, insira os dados nessa ordem:");
		Console.WriteLine("\tNúmero de dias  Número de pratos  Orçamento\n");
		Console.WriteLine("\tNúmero de dias: (valores entre 1 e 21)");
		Console.WriteLine("\tNúmero de pratos: (valores entre 1 e 50)");
		Console.WriteLine("\tOrçamento: (valores entre 0 e 100)");
		
		Console.WriteLine("\nDados dos pratos devem ser informados nessa ordem:");
		Console.WriteLine("\tCusto do prato  Lucro do prato");
		Console.WriteLine("\n\tCusto: (valores entre 1 e 50)");
		Console.WriteLine("\tLucro: (valores entre 1 e 10000)");

		obterInformacoesPratos(casosTesteGuloso, casosTesteDinamico, 1);

		//imprimeInformacoes(casosTesteDinamico);
		//imprimeInformacoes(casosTesteDinamico);

		Console.WriteLine("\t\t------SOLUÇÃO GULOSA------");
		AlgoritmoGuloso.processaCasoTesteGuloso(casosTesteDinamico);

		Console.WriteLine("\n\t\t------SOLUÇÃO DINÂMICA------\n");
		AlgoritmoDinamico.processaCasoTesteDinamico(casosTesteDinamico);

	}

	//Função para ler e armazenar as infomarções dos casos de teste e dos pratos
	public static void obterInformacoesPratos(List<CasosTeste> casosTesteGuloso, List<CasosTeste> casosTesteDinamico, int numCasoTeste) {
		int numDias, numPratos, orcamento;
		int lucro, custo, qtdPratos;
		string prato;
		List<Prato> pratosGuloso;
		List<Prato> pratosDinamico;

		Console.WriteLine();

		int[] infosEntrada = ValidaCasoDeTeste(numCasoTeste);

		numDias = infosEntrada[0];			//armazenando o numero de dias informado pelo usuário
		numPratos = infosEntrada[1];        //armazenando o numero de pratos informado pelo usuário
		orcamento = infosEntrada[2];        //armazenando o orcamento disponível informado pelo usuário

		if (numDias != 0 && numPratos != 0 && orcamento != 0)
		{
			pratosGuloso = new List<Prato>();
			pratosDinamico = new List<Prato>();
			qtdPratos = 0;

			while (qtdPratos < numPratos)
			{
				infosEntrada = ValidaPratos(qtdPratos + 1);
				custo = infosEntrada[0];			//armazenando o custo do prato 
				lucro = infosEntrada[1];            //armazenando o lucro do prato 

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

	// Função responsável por ler e validar os casos de teste
	public static int[] ValidaCasoDeTeste(int numCasoTeste) 
	{
		string casoTeste;
		int[] infoCasoTeste = new int[3];

		//vai executar enquanto o usuário não informar todos os valores válidos
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

			//Validadando se o número de dias infomado está no intervalo permitido
			if (infoCasoTeste[0] < 0 || infoCasoTeste[0] > 21) // Valida 
			{
				Console.WriteLine("  *Informe valores válidos.\n");
				continue;
			}
			//Validadando se o número de pratos informado está no intervalo permitido
			else if (infoCasoTeste[1] < 0 || infoCasoTeste[1] > 50)
			{
				Console.WriteLine("  *Informe valores válidos.\n");
				continue;
			}
			//Validadando se o orcamento está no intervalo permitido
			else if (infoCasoTeste[2] < 0 || infoCasoTeste[2] > 100)
			{
				Console.WriteLine("  *Informe valores válidos.\n");
				continue;
			}
			return infoCasoTeste;
		}

	}

	// Função responsável por ler e validar os dados dos pratos
	public static int[] ValidaPratos(int numPrato) 
    {
		string prato;
		int[] infoPrato = new int[2];

		//vai executar enquanto o usuário não informar todos os valores dos pratos válidos
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

			//Verificando se o valor do custo informado está no intervalo permitido
			if (infoPrato[0] < 1 || infoPrato[0] > 50)
			{
				Console.WriteLine("  *Informe valores válidos para o custo e lucro.\n");
				continue;
			}

			//Verificando se o valor do lucro informado está no intervalo permitido
			else if (infoPrato[1] < 1 || infoPrato[1] > 10000)
			{
				Console.WriteLine("  *Informe valores válidos para o custo e lucro.\n");
				continue;
			}

			return infoPrato;
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