using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using winpro.business.Data;
using winpro.business.Implementation;
using winpro.model.Arguments;
using winpro.services.Model;

namespace winpro.business.Repository
{
    public class CoinRepository : ICoin
    {
        private readonly DataContext _context;
        private readonly string urlFiles = @"E:\Winpro\winpro.services";

        public CoinRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<Coin> AddItemFila(Coin coin)
        {
            _context.Entry(coin).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await _context.SaveChangesAsync();
            return coin;
        }

        public Coin GetItemFila(IServiceScopeFactory scopeFactory)
        {
            try
            {
                using (var scope = scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<DataContext>();
                    Coin coin = context.Coins.AsNoTracking().OrderByDescending(x => x.Id).FirstOrDefault();
                    if (coin == null)
                    {
                        throw new Exception("ITEM_NOT_FOUND");
                    }
                    context.Entry(coin).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.SaveChanges();
                    return coin;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<Moeda> ReadFileDadosMoedaAndGetByCoin(Coin coin)
        {
            var values = this.ReadFileByUrl(this.urlFiles  + @"\DadosMoeda.csv");
            List<Moeda> moedas = new List<Moeda>();
            foreach (var value in values)
            {
                moedas.Add(new Moeda
                {
                    IdMoeda = value[0],
                    Value = value[1],
                    Date = this.ConvertStringToDate(value[1])
                });
            }
            return moedas.Where(v => v.Date != null && v.Date >= coin.Data_Inicio && v.Date <= coin.Data_Fim).ToList();
        }


        // 1.2. Com a lista de moedas/datas, buscar todos os valores de cotação (vlr_cotacao) no arquivo
        // DadosCotacao.csv utilizando o de-para descrito no item 4 (Tabela de de-para) para obter as
        //cotações.
        public List<Cotacao> ReadFileDadosCotacao()
        {
            var values = this.ReadFileByUrl(this.urlFiles + @"\DadosCotacao.csv");
            List<Cotacao> Cotacaes = new List<Cotacao>();
            List<CotacaoDePara> listCotacaoDePara = this.GetListCotacaoDePara();
            foreach (var value in values)
            {
                Cotacaes.Add(new Cotacao
                {
                    Valor = this.ConvertStringToDouble(value[0]),
                    Codigo = value[1],
                    Date = this.ConvertStringToDate(value[2]),
                    IdMoeda = listCotacaoDePara.Find(c => c.CodCotacao == value[1])?.IdMoeda
                });
            }
            return Cotacaes;
        }

        private List<string[]> ReadFileByUrl(string url)
        {
            using (var reader = new StreamReader(url))
            {
                List<string[]> list = new List<string[]>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    list.Add(line.Split(';'));
                }
                reader.Close();
                return list;
            }
        }

        public void CreateFileResultado(List<Moeda> listMoedas, List<Cotacao> listCotacoes)
        {
            var csv = new StringBuilder();
            var dateTimeNow = DateTime.UtcNow;
            csv.AppendLine(string.Format("ID_MOEDA;DATA_REF;VL_COTACAO"));
            foreach (var moeda in listMoedas)
            {
                var cotacao = listCotacoes.OrderByDescending(c => c.Date).FirstOrDefault(c => c.IdMoeda == moeda.IdMoeda);
                csv.AppendLine(string.Format("{0};{1};{2}", moeda.IdMoeda, DateTime.Parse(moeda.Date.ToString()).ToString("yyyy-MM-dd"), cotacao?.Valor));
            }
            File.WriteAllText(@"E:\Winpro\winpro.services\"+ dateTimeNow.ToString("yyyyMMdd_HHmmss") + ".csv", csv.ToString());
        }


        private DateTime? ConvertStringToDate(string value)
        {
            try
            {
                return DateTime.Parse(value);
            }
            catch
            {
                return null;
            }
        }

        private Double ConvertStringToDouble(string value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }

        private List<CotacaoDePara> GetListCotacaoDePara()
        {
            var values = this.ReadFileByUrl(this.urlFiles + @"\cotacao-de-para.csv");
            List<CotacaoDePara> CotacaoDePara = new List<CotacaoDePara>();
            foreach (var value in values)
            {
                CotacaoDePara.Add(new CotacaoDePara
                {
                    IdMoeda = value[0],
                    CodCotacao = value[1],
                });
            }
            return CotacaoDePara;
        }

    }
}
