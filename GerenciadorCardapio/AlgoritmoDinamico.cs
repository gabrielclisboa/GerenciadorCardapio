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
            if(parteDecimal(celula[numeroPrato + 1]) == 0 && diasPrevistos(celula) < numDias){
                celula[0] += prato.lucro;
                celula[numeroPrato + 1] = Math.Round(celula[numeroPrato + 1] + 0.1, 1);
            }else if(parteDecimal(celula[numeroPrato + 1]) == 1 && diasPrevistos(celula) < numDias && 2 <= (int)numDias/2 + 1){
                celula[0] += prato.lucro;
                celula[numeroPrato + 1] = Math.Round(celula[numeroPrato + 1] + 0.1, 1);
            }else if(parteDecimal(celula[numeroPrato + 1]) == 1 && diasPrevistos(celula) < numDias){
                celula[0] += prato.lucro / 2;
                celula[numeroPrato + 1] = Math.Round(celula[numeroPrato + 1] + 0.1, 1);
            }else if(parteDecimal(celula[numeroPrato + 1]) == 2 && diasPrevistos(celula) < numDias && 3 <= (int)numDias/2 + 1){
                celula[0] += prato.lucro;
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
            // Lista resultante
            List<Prato> resultado = new List<Prato>();
            
            // Dicionário para contar a frequência de cada índice
            var contagemindices = new Dictionary<int, int>();
            foreach (var prato in pratos)
            {
                if (!contagemindices.ContainsKey(prato.indice))
                {
                    contagemindices[prato.indice] = 0;
                }
                contagemindices[prato.indice]++;
            }
            
            // Lista de pares (indice, frequência) ordenada pela frequência decrescente
            var listaContagem = contagemindices.OrderByDescending(kv => kv.Value).ToList();
            
            // Alternar os pratos
            while (pratos.Count > 0)
            {
                bool inserido = false;
                foreach (var item in listaContagem)
                {
                    int indice = item.Key;
                    if (contagemindices[indice] > 0 && (resultado.Count == 0 || resultado.Last().indice != indice))
                    {
                        var prato = pratos.First(p => p.indice == indice);
                        resultado.Add(prato);
                        pratos.Remove(prato);
                        contagemindices[indice]--;
                        inserido = true;
                        break;
                    }
                }
                
                // Se não foi possível inserir nenhum prato de forma intercalada, inserir o próximo disponível
                if (!inserido)
                {
                    var prato = pratos[0];
                    resultado.Add(prato);
                    pratos.RemoveAt(0);
                    contagemindices[prato.indice]--;
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

    }
}
