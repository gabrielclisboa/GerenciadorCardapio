using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorCardapio {
	public class Prato {
		public int indice  { get; set; }
		public int custo   { get; set; }   // custo >= 1  e custo <= 50
		public int lucro   { get; set; }   // lucro >= 1  e lucro <= 10000

		public Prato(int indice, int custo, int lucro)
        {
            this.indice = indice;
            this.custo = custo;
            this.lucro = lucro;
        }
    }
}
