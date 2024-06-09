using GerenciadorCardapio;
using GerenciadorDeCardapio;

class Program
{
    static void Main(string[] args)
    {
        List<CasosTeste> casosTeste = new List<CasosTeste>();

        Console.WriteLine("\t\t------BEM VINDO AO SISTEMA DE GERENCIAMENTO DE CARDÁPIO------");
        Console.WriteLine("\nAo informar os casos de teste, insira os dados nessa ordem:");
        Console.WriteLine("\tNúmero de dias - Número de pratos - Orçamento\n");
        Console.WriteLine("Os dados dos pratos devem ser informados nessa ordem:");
        Console.WriteLine("\tCusto do prato - Lucro do prato");

        obterInformacoesPratos(casosTeste);
        imprimeInformacoes(casosTeste);

        processaCasoTeste(casosTeste);
    }


    public static void obterInformacoesPratos(List<CasosTeste> casosTeste)
    {
        int numDias, numPratos, orcamento;
        int lucro, custo, qtdPratos;
        string casoTeste, prato;
        List<Prato> pratos;

        Console.WriteLine("\nCaso de teste: ");
        casoTeste = Console.ReadLine();

        numDias = int.Parse(casoTeste.Split(' ')[0]);
        numPratos = int.Parse(casoTeste.Split(' ')[1]);
        orcamento = int.Parse(casoTeste.Split(' ')[2]);

        while (numDias != 0 && numPratos != 0 && orcamento != 0)
        {
            pratos = new List<Prato>();
            qtdPratos = 0;

            Console.WriteLine("Pratos:");
            while (qtdPratos < numPratos)
            {
                prato = Console.ReadLine();

                custo = int.Parse(prato.Split(' ')[0]); //armazenando informações do custo
                lucro = int.Parse(prato.Split(' ')[1]); //armazenando informações do lucro

                pratos.Add(new Prato(qtdPratos + 1, custo, lucro));     //inserindo o novo prato na lista
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

    public static void imprimeInformacoes(List<CasosTeste> casosTeste)
    {
        Console.WriteLine("\n-----INFORMAÇÕES DOS CASOS DE TESTE----\n");
        foreach (CasosTeste casoTeste in casosTeste)
        {
            Console.WriteLine("QtdDias\tQtdPratos\tOrçamento");
            Console.Write(casoTeste.numDias);
            Console.Write("\t " + casoTeste.numPratos);
            Console.Write("\t\t " + casoTeste.orcamento);
            Console.WriteLine();

            Console.WriteLine("Pratos:");
            Console.WriteLine("Indice\tCusto\tLucro");
            foreach (var prato in casoTeste.pratos)
            {
                Console.Write(prato.indice);
                Console.Write("\t " + prato.custo);
                Console.Write("\t " + prato.lucro);
                Console.WriteLine();
            }

            Console.WriteLine("\n");
        }
    }

    public static void processaCasoTeste(List<CasosTeste> casosTeste)
    {
        List<Prato> pratosEscolhidos = new List<Prato>();
        Console.WriteLine("\nSaída: ");
        for (int i = 0; i < casosTeste.Count(); i++)
        {
            pratosEscolhidos = new List<Prato>();
            casosTeste[i].pratos = casosTeste[i].pratos.OrderBy(c => c.custo).ToList();

            int qtdDias = 0;
            while (qtdDias < casosTeste[i].numDias)
            {

                if (casosTeste[i].pratos.Count == 1)
                {
                    pratosEscolhidos.Add(new Prato(casosTeste[i].pratos[0].indice, casosTeste[i].pratos[0].custo, calculaNovoLucro(casosTeste[i].pratos[0], pratosEscolhidos.Count())));
                }
                else
                {
                    procuraMelhorPrato(casosTeste[i].pratos, pratosEscolhidos);

                }
                qtdDias++;

            }

            double orcamento = pratosEscolhidos.Sum(x => x.custo);
            Console.WriteLine($"CASO DE TESTE {(i + 1)}: ");

            if (casosTeste[i].orcamento < orcamento)
            {
                Console.WriteLine("0.0");
            }
            else
            {
                Console.WriteLine("Lucro: " + pratosEscolhidos.Sum(x => x.lucro));
                Console.WriteLine("Pratos:");
                imprimePratos(pratosEscolhidos);

            }
            Console.WriteLine(" ----------------- ");
        }

        //while (casosTeste.Count > 0) {
        //	//ordenação pelo custo - pratos
        //	//criar lista de pratos que ja escolheu
        //	//olha o ultimo que foi pego
        //	//se o meu atual é igual ao ultimo adc na lista, passo para o prox
        //	//pega o prox
        //	// denovo repete os passos acima

        //	//Validações
        //	// se passou do orçamento retorna 0

        //}
    }



    public static void procuraMelhorPrato(List<Prato> pratos, List<Prato> pratosEscolhidos)
    {
        int i = 0;
        for (i = 0; i < pratos.Count; i++)
        {
            if (pratosEscolhidos.Count() > 0)
            {
                if ((pratos[i].indice != pratosEscolhidos.Last()?.indice))
                {
                    pratosEscolhidos.Add(pratos[i]);

                }
                else
                {
                    pratosEscolhidos.Add(pratos[i + 1]);
                }
                return;

            }
            else
            {
                pratosEscolhidos.Add(pratos[i]);
            }

            break;

        }

    }

    public static double calculaNovoLucro(Prato prato, int qdtPratosEscolhidos)
    {
        if (qdtPratosEscolhidos == 0)
        {
            return prato.lucro;
        }
        else if (qdtPratosEscolhidos == 1)
        {
            return prato.lucro / 2;
        }
        else
        {
            return 0;
        }
    }

    public static void imprimePratos(List<Prato> pratos)
    {
        foreach (var prato in pratos)
        {
            Console.Write(" " + prato.indice);
        }
        Console.WriteLine();
    }


}