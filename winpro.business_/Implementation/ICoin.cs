using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winpro.business.Data;
using winpro.model.Arguments;
using winpro.services.Model;

namespace winpro.business.Implementation
{
    public interface ICoin
    {
        Task<Coin> AddItemFila(Coin coin);
        Coin GetItemFila(IServiceScopeFactory scopeFactory);
        List<Moeda> ReadFileDadosMoedaAndGetByCoin(Coin coin);
        List<Cotacao> ReadFileDadosCotacao();

        void CreateFileResultado(List<Moeda> listMoedas, List<Cotacao> listCotacoes);
    }
}
