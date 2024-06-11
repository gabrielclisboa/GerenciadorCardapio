using GerenciadorCardapio;

namespace GerenciadorDeCardapio {
	public class CasosTeste {
		public int numDias { get; set; }
		public int numPratos { get; set; }
		public int orcamento { get; set; }

		public List<Prato> pratos = new List<Prato>();

        public CasosTeste(int numDias, int numPratos, int orcamento, List<Prato> pratos)
        {
			this.numDias = numDias;
			this.numPratos = numPratos;
			this.orcamento = orcamento;
			this.pratos = pratos;
		}


    }
}
