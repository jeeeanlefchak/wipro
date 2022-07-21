using System;
using Microsoft.EntityFrameworkCore;
using winpro.business.Data;
using winpro.business.Implementation;
using winpro.services.Model;

namespace winpro.business.Repository
{
    public class CoinRepository: ICoin
    {
        private readonly DataContext _context;
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

        public async Task<Coin> GetItemFila()
        {
            Coin coin = await _context.Coins.AsNoTracking().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            if (coin == null)
            {
                throw new Exception("ITEM_NOT_FOUND");
            }
            _context.Entry(coin).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _context.SaveChangesAsync();
            return coin;
        }

    }
}
