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

        obterInformacoesPratos(casosTeste);;
        imprimeInformacoes(casosTeste);
        Console.WriteLine("\t\t------SOLUÇÃO DINÂMICA------");
        processaCasoTesteDinamico(casosTeste);
        Console.WriteLine("\t\t------SOLUÇÃO GULOSA------");
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

    public static void processaCasoTesteDinamico(List<CasosTeste> casosTeste)
    {
        for(int c = 0; c < casosTeste.Count; c++){
            casosTeste[c].pratos = casosTeste[c].pratos.OrderByDescending(v => v.custo).ToList();
            string[,] matriz = criarMatrizDinamica(casosTeste[c].orcamento, casosTeste[c].pratos);

            for(int i = 2; i < matriz.GetLength(0); i++){
                for(int j = 1; j < matriz.GetLength(1); j++){
                    if(j - casosTeste[c].pratos[i - 2].custo >= 0){
                        matriz[i, j] = melhorValorEntre(matriz[i - 1, j], matriz[i, j - casosTeste[c].pratos[i - 2].custo], casosTeste[c].pratos[i - 2].lucro, i - 2, casosTeste[c].pratos, casosTeste[c].numDias);
                    }else{
                        matriz[i, j] = matriz[i - 1, j];
                    }
                }
            }
            List<Prato> menu = obtemMenu(matriz[matriz.GetLength(0) - 1, matriz.GetLength(1) - 1], casosTeste[c].pratos);
            double lucro = obtemLucro(matriz[matriz.GetLength(0) - 1, matriz.GetLength(1) - 1]);
            exibirMenuMetodoDinamico(lucro, menu, c);
        }
    }

    static string[,] criarMatrizDinamica(int orcamento, List<Prato> pratos){
        string[,] matriz = new string[pratos.Count + 2, orcamento + 1];
        string celulaBase = geraCelulaBase(pratos);
        for(int i = 0; i < matriz.GetLength(1); i++){
            matriz[0, i] = i.ToString();
        }
        for(int i = 0; i < matriz.GetLength(1); i++){
            matriz[1, i] = celulaBase;
        }
        for(int i = 2; i < matriz.GetLength(0); i++){
            matriz[i, 0] = celulaBase;
        }

        return matriz;
    }

    static string geraCelulaBase(List<Prato> pratos){
        string resultado = "0-";
        for(int i = 0; i < pratos.Count; i++){
            resultado += pratos[i].custo.ToString();
            resultado += "-";
        }
        return resultado;
    }

    static string melhorValorEntre(string primeiraCelula, string segundaCelula, double lucro, int numeroPrato, List<Prato> pratos, int numDias){
        string[] primeiraCelulaDividida = primeiraCelula.Split("-");
        string[] segundaCelulaDividida = segundaCelula.Split("-");

        double[] primeiraCelulaValores = paraDouble(primeiraCelulaDividida);
        double[] segundaCelulaValores = paraDouble(segundaCelulaDividida);
        double[] segundaCelulaComLucroDinamico = lucroDinamico(segundaCelulaValores, numeroPrato, pratos, numDias, lucro);

        if(primeiraCelulaValores[0] < segundaCelulaComLucroDinamico[0]){
            return paraString(segundaCelulaComLucroDinamico);
        }else{
            return primeiraCelula;
        }
    }

    static double[] lucroDinamico(double[] celula, int numeroPrato, List<Prato> pratos, int numDias, double lucroAcumulado){
        Prato prato = pratos[numeroPrato];
        if(parteDecimal(celula[numeroPrato + 1]) == 0 && diasPrevistos(celula) < numDias){
            celula[0] += prato.lucro;
            celula[numeroPrato + 1] = Math.Round(celula[numeroPrato + 1] + 0.1, 1);
        }else if(parteDecimal(celula[numeroPrato + 1]) == 1 && diasPrevistos(celula) < numDias){
            celula[0] += prato.lucro / 2;
            celula[numeroPrato + 1] = Math.Round(celula[numeroPrato + 1] + 0.1, 1);
        }else if(diasPrevistos(celula) < numDias){
            celula[numeroPrato + 1] = Math.Round(celula[numeroPrato + 1] + 0.1, 1);
        }

        return celula;
    }

    static int parteDecimal(double numero)
    {
        string numeroString = numero.ToString(System.Globalization.CultureInfo.InvariantCulture);

        string[] partes = numeroString.Split('.');

        if (partes.Length > 1)
        {
            string parteDecimalString = partes[1];
            if (int.TryParse(parteDecimalString, out int parteDecimal))
            {
                return parteDecimal;
            }
        }

        return 0;
    }


    static int diasPrevistos(double[] celula){
        int dias = 0;

        for(int i = 1; i < celula.Length; i++){
            dias += parteDecimal(celula[i]);
        }

        return dias;
    }

    static double[] paraDouble(string[] vetorStrings){
        double[] resultado = new double[vetorStrings.Length - 1];
        for(int i = 0; i < vetorStrings.Length - 1; i++){
            resultado[i] = double.Parse(vetorStrings[i]);
        }
        return resultado;
    }

    static string paraString(double[] vetorDoubles){
        string resultado = "";
        for(int i = 0; i < vetorDoubles.Length; i++){
            resultado += vetorDoubles[i].ToString();
            resultado += "-";
        }
        return resultado;
    }

    static List<Prato> obtemMenu(string celula, List<Prato> pratos){
        string[] celulaDividida = celula.Split('-');
        double[] celulaValores = paraDouble(celulaDividida);
        List<Prato> resultado = new List<Prato>();

        for(int i = 1; i < celulaValores.Length; i++){
            if(parteDecimal(celulaValores[i]) == 1){
                resultado.Add(pratos[i - 1]);
            }else if(parteDecimal(celulaValores[i]) == 2){
                resultado.Add(pratos[i - 1]);
                resultado.Add(pratos[i - 1]);
            }
            else if(parteDecimal(celulaValores[i]) == 3){
                resultado.Add(pratos[i - 1]);
                resultado.Add(pratos[i - 1]);
                resultado.Add(pratos[i - 1]);
            }
        }

        return resultado;
    }

    static double obtemLucro(string celula){
        string[] celulaDividida = celula.Split('-');
        return double.Parse(celulaDividida[0]);
    }

    static void PrintMatrix(string[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    public static void exibirMenuMetodoDinamico(double lucro, List<Prato> menu, int casoTeste)
    {
        Console.WriteLine("\nSaída: ");
        Console.WriteLine($"CASO DE TESTE {casoTeste + 1}: ");
        Console.WriteLine("Lucro: " + lucro);
        Console.WriteLine("Pratos:");
        imprimePratos(menu);
        Console.WriteLine(" ----------------- ");
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