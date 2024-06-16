using GerenciadorDeCardapio;

namespace GerenciadorCardapio {
	public static class AlgoritmoDinamico{
        public static void processaCasoTesteDinamico(List<CasosTeste> casosTeste)
        {
            for(int c = 0; c < casosTeste.Count; c++){
                casosTeste[c].pratos = casosTeste[c].pratos.OrderByDescending(v => v.custo).ToList(); // ordenando os pratos em ordem decrescente de custo
                string[,] matriz = criarMatrizDinamica(casosTeste[c].orcamento, casosTeste[c].pratos);

                for(int i = 2; i < matriz.GetLength(0); i++){
                    for(int j = 1; j < matriz.GetLength(1); j++){
                        if(j <= 2 && i == 2){           

                        }
                        if(j < matriz.GetLength(1) - 1){
                            if(j - casosTeste[c].pratos[i - 2].custo >= 0){
                                matriz[i, j] = melhorValorEntre(matriz[i - 1, j], matriz[i, j - casosTeste[c].pratos[i - 2].custo], casosTeste[c].pratos[i - 2].lucro, i - 2, casosTeste[c].pratos, casosTeste[c].numDias, false);
                            }else{
                                matriz[i, j] = matriz[i - 1, j];
                            }
                        }else{
                            if(j - casosTeste[c].pratos[i - 2].custo >= 0){
                                matriz[i, j] = melhorValorEntre(matriz[i - 1, j], matriz[i, j - casosTeste[c].pratos[i - 2].custo], casosTeste[c].pratos[i - 2].lucro, i - 2, casosTeste[c].pratos, casosTeste[c].numDias, true);
                            }else{
                                matriz[i, j] = matriz[i - 1, j];
                            }
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

        static string melhorValorEntre(string primeiraCelula, string segundaCelula, double lucro, int numeroPrato, List<Prato> pratos, int numDias, bool ultima){
            string[] primeiraCelulaDividida = primeiraCelula.Split("-");
            string[] segundaCelulaDividida = segundaCelula.Split("-");

            double[] primeiraCelulaValores = paraDouble(primeiraCelulaDividida);
            double[] segundaCelulaValores = paraDouble(segundaCelulaDividida);
            double[] segundaCelulaComLucroDinamico = lucroDinamico(segundaCelulaValores, numeroPrato, pratos, numDias, lucro);

            if(ultima == true && diasPrevistos(primeiraCelulaValores) == numDias && diasPrevistos(segundaCelulaComLucroDinamico) == numDias){
                if(primeiraCelulaValores[0] < segundaCelulaComLucroDinamico[0]){
                    return paraString(segundaCelulaComLucroDinamico);
                }else{
                   return primeiraCelula; 
                }
            }else if(ultima == true && diasPrevistos(primeiraCelulaValores) == numDias){
                return primeiraCelula;
            }else if(ultima == true && diasPrevistos(segundaCelulaComLucroDinamico) == numDias){
                return paraString(segundaCelulaComLucroDinamico);
            }else if(ultima == true){
                return geraCelulaBase(pratos);
            }else if(primeiraCelulaValores[0] < segundaCelulaComLucroDinamico[0]){
                return paraString(segundaCelulaComLucroDinamico);
            }else{
                return primeiraCelula;
            }
        }

        static string paraString(double[] vetorDoubles){
            string resultado = "";
            for(int i = 0; i < vetorDoubles.Length; i++){
                resultado += vetorDoubles[i].ToString();
                resultado += "-";
            }
            return resultado;
        }
        
        static double[] lucroDinamico(double[] celula, int numeroPrato, List<Prato> pratos, int numDias, double lucroAcumulado){
            Prato prato = pratos[numeroPrato];

            if(diasPrevistos(celula) < numDias){
                List<Prato> pratosUsados = celulaParaLista(paraString(celula), pratos);
                pratosUsados.Add(prato);
                List<Prato> listaComOPrato = IntercalarPratos(pratosUsados);
                double lucro = calculaLucroDaSequencia(listaComOPrato);
                celula[0] = lucro;
                celula[numeroPrato + 1] = Math.Round(celula[numeroPrato + 1] + 0.1, 1);
            }

            return celula;
        }

        public static double calculaLucroDaSequencia(List<Prato> pratos){
            if(pratos.Count == 1){
                return pratos[0].lucro;
            }
            double resultado = pratos[0].lucro;
            resultado += pratos[1].custo == pratos[0].custo ? pratos[1].lucro / 2 : pratos[1].lucro;
            for(int i = 2; i < pratos.Count; i++){
                if(pratos[i].custo == pratos[i - 1].custo && pratos[i].custo == pratos[i - 2].custo){
                    resultado += 0;
                }else if(pratos[i].custo == pratos[i - 1].custo){
                    resultado += pratos[i].lucro / 2;
                }else{
                    resultado += pratos[i].lucro; 
                }
            }
            return resultado;
        }

        public static List<Prato> celulaParaLista(string celula, List<Prato> pratos)
        {
            var resultado = new List<Prato>();
            var pares = celula.Split('-');

            // O primeiro elemento é o total e pode ser ignorado
            for (int i = 1; i < pares.Length; i++)
            {
                var y_qy = pares[i].Split(',');

                if (y_qy.Length == 2)
                {
                    int custo = int.Parse(y_qy[0]);
                    int quantidade = int.Parse(y_qy[1]);

                    var prato = pratos.FirstOrDefault(p => p.custo == custo);

                    if (prato != null)
                    {
                        for (int j = 0; j < quantidade; j++)
                        {
                            resultado.Add(prato);
                        }
                    }
                }
            }

            return resultado;
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

        static List<Prato> obtemMenu(string celula, List<Prato> pratos){
            string[] celulaDividida = celula.Split('-');
            double[] celulaValores = paraDouble(celulaDividida);
            List<Prato> resultado = new List<Prato>();

            for(int i = 1; i < celulaValores.Length; i++){
                for(int j = 0; j < (int)parteDecimal(celulaValores[i]); j++){
                    resultado.Add(pratos[i - 1]);
                }
            }

            return IntercalarPratos(resultado);
        }

        public static List<Prato> IntercalarPratos(List<Prato> pratos)
        {
            // Ordena os pratos pelo custo
            var pratosOrdenados = pratos.OrderBy(p => p.custo).ToList();

            // Cria uma lista para armazenar o resultado final
            var resultado = new List<Prato>();

            // Agrupa os pratos pelo custo
            var grupos = pratosOrdenados.GroupBy(p => p.custo)
                                        .OrderByDescending(g => g.Count())
                                        .ToList();

            // Usa uma fila para intercalar os pratos de diferentes custos
            var fila = new Queue<Prato>();

            foreach (var grupo in grupos)
            {
                foreach (var prato in grupo)
                {
                    fila.Enqueue(prato);
                }
            }

            // Intercala os pratos na ordem desejada
            while (fila.Any())
            {
                var currentPrato = fila.Dequeue();
                resultado.Add(currentPrato);

                // Se o próximo prato tem o mesmo custo, pula ele para a próxima iteração
                if (fila.Any() && fila.Peek().custo == currentPrato.custo)
                {
                    var temp = fila.Dequeue();
                    if (fila.Any())
                    {
                        fila.Enqueue(temp);
                    }
                    else
                    {
                        resultado.Add(temp);
                    }
                }
            }

            return resultado;
        }

        static double obtemLucro(string celula){
            string[] celulaDividida = celula.Split('-');
            return double.Parse(celulaDividida[0]);
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

        public static void imprimePratos(List<Prato> pratos)
        {
            foreach (var prato in pratos)
            {
                Console.Write(" " + prato.indice);
            }
            Console.WriteLine();
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
            Console.WriteLine();
        }
    }
}
