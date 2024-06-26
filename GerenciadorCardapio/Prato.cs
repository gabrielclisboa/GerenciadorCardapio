﻿namespace GerenciadorCardapio
{
    public class Prato
    {
        public bool isReduzido = false;
        public int indice { get; set; }
        public int custo { get; set; }   // custo >= 1  e custo <= 50
        public double lucro { get; set; }   // lucro >= 1  e lucro <= 10000

        public Prato() { }
        public Prato(int indice, int custo, double lucro)
        {
            this.indice = indice;
            this.custo = custo;
            this.lucro = lucro;
        }

		public Prato(Prato original) {
			this.indice = original.indice;
			this.custo = original.custo;
			this.lucro = original.lucro;
		}
	}
}