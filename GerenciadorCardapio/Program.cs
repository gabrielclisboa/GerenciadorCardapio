using GerenciadorCardapio;
using GerenciadorDeCardapio;
using System;
using System.Collections;
using System.Collections.Generic; // Adicionando o namespace necessário
using System.IO;

class Program {
	static void Main(string[] args) {
		List<CasosTeste> casosTeste = new List<CasosTeste>();

		Console.WriteLine("\t\t------BEM VINDO AO SISTEMA DE GERENCIAMENTO DE CARDÁPIO------");
		Console.WriteLine("\nAo informar os casos de teste, insira os dados nessa ordem:");
		Console.WriteLine("\tNúmero de dias - Número de pratos - Orçamento\n");
		Console.WriteLine("Os dados dos pratos devem ser informados nessa ordem:");
		Console.WriteLine("\tCusto do prato - Lucro do prato");

		obterInformacoesPratos(casosTeste);
        imprimeInformacoes(casosTeste);
	}


	public static void obterInformacoesPratos(List<CasosTeste> casosTeste) {
		int numDias, numPratos, orcamento;
		int lucro, custo, qtdPratos;
		string casoTeste, prato;
		List<Prato> pratos;

		Console.WriteLine("\nCaso de teste: ");
		casoTeste = Console.ReadLine();
		
		numDias = int.Parse(casoTeste.Split(' ')[0]);
		numPratos = int.Parse(casoTeste.Split(' ')[1]);
		orcamento = int.Parse(casoTeste.Split(' ')[2]);

		while (numDias != 0 && numPratos != 0 && orcamento != 0) {	
			pratos = new List<Prato>();
			qtdPratos = 0;

			Console.WriteLine("Pratos:");
			while (qtdPratos < numPratos) {
				prato = Console.ReadLine();

				custo = int.Parse(prato.Split(' ')[0]);	//armazenando informações do custo
				lucro = int.Parse(prato.Split(' ')[1]); //armazenando informações do lucro

				pratos.Add(new Prato(qtdPratos + 1, custo, lucro));		//inserindo o novo prato na lista
				qtdPratos++;
			}

            Console.WriteLine("\t ----------------------------------------------------------\t\n");
            casosTeste.Add(new CasosTeste(numDias, numPratos, orcamento, pratos));		

			Console.WriteLine("Caso de teste: ");
			casoTeste = Console.ReadLine();

			numDias = int.Parse(casoTeste.Split(' ')[0]);
			numPratos = int.Parse(casoTeste.Split(' ')[1]);
			orcamento = int.Parse(casoTeste.Split(' ')[2]);
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
}
