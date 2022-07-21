using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winpro.services.Model;

namespace winpro.business.Implementation
{
    public interface ICoin
    {
        Task<Coin> AddItemFila(Coin coin);
        Task<Coin> GetItemFila();
    }
}
