using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinMarketData;
//using NoobsMuc.Coinmarketcap.Client;
using Microsoft.Extensions.Configuration;


namespace StoreCoinMarket
{


    public class ManageCoins
    {
        //ICoinmarketcapClient _client;
        private Settings? _settings;

        public ManageCoins()
        {
            _settings = singletonSettings.Instance.GetSettings();

            //_client = new CoinmarketcapClient(_settings.MarketCapCoinAPI);

        }

        public void InsertCoin( string symbol)
        {
            CoinMarketQuote coinMarketQuote = new CoinMarketQuote();

            Coin coin;

            // fetch new coin data
            //Currency currency = _client.GetCurrencyBySymbol(symbol);
            List<Currency> currencies = coinMarketQuote.GetQuotes(symbol, "EUR").Result;

            Currency currency = currencies[0];

            Console.WriteLine(currency.Name);

            // get db
            var db = new CoinDbContext();
            // check if symbol exists
            if (!db.Coins.Any(coin => coin.Symbol == symbol))
            {
                coin = new Coin();
                coin.Symbol = currency.Symbol;
                coin.Name = currency.Name;
                db.Coins.Add(coin);
                db.SaveChanges();
            }
            else
            {
                // fetch BTC
                coin = (from c in db.Coins where c.Symbol == symbol select c).FirstOrDefault();
            }

            // insert symbol values
            CoinHistory hist = new CoinHistory();
            hist.CoinId = coin.CoinId;
            hist.LastUpdate = DateTime.Now;
            hist.Price = Convert.ToDouble(currency.Price);
            hist.MaxSupply = 0;
            hist.MarketCap = Convert.ToDouble(currency.MarketCapUsd);
            hist.PercentChange1h = Convert.ToDouble(currency.PercentChange1h);
            hist.PercentChange24h = Convert.ToDouble(currency.PercentChange24h);
            hist.PercentChange30d = 0;
            hist.PercentChange7d = Convert.ToDouble(currency.PercentChange7d);
            hist.TotalSupply = 0;
            hist.Volume24h = Convert.ToDouble(currency.Volume24hUsd);
            hist.VolumeChange24h = 0;
            db.CoinHistorys.Add(hist);
            db.SaveChanges();

        }
    }


    

}
