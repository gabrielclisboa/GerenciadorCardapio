using GerenciadorDeCardapio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCardapio {
	public static class AlgoritmoGuloso {

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

						double novoLucro = 0;

						//Validando se não vai ultrapassar o orçamento
						if((gastoTotal + casosTeste[i].pratos[0].custo) > casosTeste[i].orcamento) {
							gastoTotal = 0;
							break;		//Finaliza o caso de teste
						}

						//Validando se o prato já foi escolhido alguma vez para calcular o novo lucro dele
						if (pratosEscolhidos.Count() > 0) {
							if (pratosEscolhidos.Last().lucro > 0)  //validando se o ultimo prato inserido ja não possui custo 0 (foi adicionado mais de 3 vezes)
								novoLucro = pratosEscolhidos.Last().lucro - (casosTeste[i].pratos[0].lucro / 2);        //último custo do prato inserido na lista - custo inical dele / 2
						} else {
							novoLucro = casosTeste[i].pratos[0].lucro; //O Lucro do prato será de 100%
						}

						pratosEscolhidos.Add(new Prato(casosTeste[i].pratos[0].indice, casosTeste[i].pratos[0].custo, novoLucro));  //Vai adicionar sempre o mesmo prato na lista, mas com novo lucro
						gastoTotal += pratosEscolhidos.Last().custo;
					}
					qtdDias++;

				}

				//double gastoTotal = pratosEscolhidos.Sum(x => x.custo);     //Calculando o gasto total para fazer o cardapio escolhido

				Console.WriteLine($"CASO DE TESTE {(i + 1)}: ");

				//Validando se o gasto total não é maior que o orçamento disponível
				if (gastoTotal == 0) {
					Console.WriteLine("O cardápio informado ultrapassou o orçamento.");
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
		public static double procuraMelhorPrato(List<Prato> pratos, List<Prato> pratosEscolhidos, int orcamentoTotal) {
			int i = 0;
			double orcamentoDisponivel = orcamentoTotal - pratosEscolhidos.Sum(x => x.custo);

			//verificando se algum prato já foi escolhido
			if (pratosEscolhidos.Count() > 0) {

				//Verificando se o primeiro prato não foi o ultimo a ser escolhido
				//e se o seu custo não é maior que o orcamento ainda disponivel
				if (pratos[i].indice != pratosEscolhidos.Last()?.indice && pratos[i].custo <= orcamentoDisponivel) {
					pratosEscolhidos.Add(pratos[i]);    //adicionando o primeiro prato na lista

				}

				//Verificando se é possivel inserir o segundo prato na lista
				else if (pratos[i + 1].custo <= orcamentoDisponivel) {
					pratosEscolhidos.Add(pratos[i + 1]);    //adicionando o segundo prato na lista
				}

				//Como não foi possível inserir o segundo prato na lista, vai tentar inserir o primeiro novamente
				else if (pratos[i].custo <= orcamentoDisponivel) {
					double novoLucro = 0;

					//validando se o ultimo prato inserido ja não possui custo 0 (foi adicionado mais de 3 vezes)
					if (pratosEscolhidos.Last().lucro > 0)
						novoLucro = pratosEscolhidos.Last().lucro - (pratos[i].lucro / 2); //último custo do prato inserido na lista - custo inical dele / 2

					pratosEscolhidos.Add(new Prato(indice: pratos[i].indice, custo: pratos[i].custo, lucro: novoLucro));    //adicionando o primeiro prato na lista novamente
				} else {
					// Nenhum prato pode ser mais adicionado na lista, ultrapassou o orçamento
					return 0;
				}

			}
			//Caso nenhum prato tenha sido escolhido, vai tentar inserir o primeiro, pois ele possui o menor custo
			else if (pratos[i].custo < orcamentoDisponivel) {
				pratosEscolhidos.Add(pratos[i]);

			} else {
				// O primeiro prato não possa ser escolhido: siginifca que o cardápio ultrapassou o orçamento
				return 0;
			}

			return pratosEscolhidos.Last().custo;   //Retornando a soma dos custos dos pratos escohidos
		}

		//Função para imprimir as informações dos pratos
		public static void imprimePratos(List<Prato> pratos) {
			foreach (var prato in pratos) {
				Console.Write(" " + prato.indice);
			}
			Console.WriteLine();
		}

	}
}
