using winpro.business.Data;
using winpro.business.Implementation;
using winpro.model.Arguments;

namespace winpro.services.BackGroundService
{
    public class BackGround : winpro.services.BackGroundService.BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private ICoin _coinRepository;
        private DataContext _context;
        public BackGround(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                this._coinRepository = scope.ServiceProvider.GetRequiredService<ICoin>();
                this._context = scope.ServiceProvider.GetService<DataContext>();
            }
            while (!stoppingToken.IsCancellationRequested)
            {

                Console.WriteLine("TESTE");
                this.GetItemFila();

                await Task.Delay(120000, stoppingToken);//3.600.000 milisegundos equivalem a 1 hora.. 60 000 = 1 min
            }

        }

        private void GetItemFila()
        {
            try
            {
                // 1.Acesse o método GetItemFila da api desenvolvida no Item anterior.Caso o método retorne um
                // objeto, obter todas as moedas e períodos correspondentes

                var item = this._coinRepository.GetItemFila(this.scopeFactory);
                if (item != null)
                {

                    // 1.1.Para cada moeda/ período retornada da api, acessar o arquivo DadosMoeda.csv(mesmo
                    // diretório de execução) e trazer todas as moedas / datas que estejam dentro do período
                    // (Inclusive) da moeda que está sendo tratada.
                    List<Moeda> listMoedas = this._coinRepository.ReadFileDadosMoedaAndGetByCoin(item);

                    // 1.2.Com a lista de moedas/ datas, buscar todos os valores de cotação(vlr_cotacao) no arquivo
                    // DadosCotacao.csv utilizando o de-para descrito no item 4(Tabela de de - para) para obter as
                    // cotações.
                    List<Cotacao> listCotacoes = this._coinRepository.ReadFileDadosCotacao();


                    // 1.3.Gerar um arquivo de resultado(apenas com as moedas / datas consultadas) com o nome
                    // Resultado_aaaammdd_HHmmss.csv no mesmo formato do arquivo DadosMoeda.csv.
                    // Porém com uma coluna a mais(VL_COTACAO) contendo o valor de cotação correspondente
                    // (obtido do arquivo DadosCotacao.csv) para cada moeda / data consultada.
                    this._coinRepository.CreateFileResultado(listMoedas, listCotacoes);

                }
            }
            catch
            {

            }
        }

}
}
