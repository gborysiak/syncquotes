using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CoinMarketData;

namespace StoreCoinMarket
{
    internal class CoinMarketQuote
    {
        private Settings? _settings;

        public CoinMarketQuote()
        {
            _settings = singletonSettings.Instance.GetSettings();

        }

        public async Task<List<Currency>> GetQuotes(string symbol, string currency)
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
            string url = $"https://pro-api.coinmarketcap.com/v2/cryptocurrency/quotes/latest?symbol={symbol}&convert={currency}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-CMC_PRO_API_KEY", _settings.MarketCapCoinAPI);
            request.Headers.Add("Accept", " application/json");
            request.Headers.Add("Accept-Encoding", "deflate, gzip");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            List<Currency> currencyList = new List<Currency>();
            GetSymbolData(currency, symbol, content, currencyList);


            //return await response.Content.ReadAsStringAsync();
            return currencyList;
        }

        private static void GetSymbolData(string convert, string symbol,
            string content, List<Currency> currencyList)
        {
            string replaceString = $"\"{symbol}\":[";
            content = content.Replace(replaceString, "\"CurrenyData\":[");

            string replaceString1 = $"\"quote\":{{\"{convert}\":";
            content = content.Replace(replaceString1, "\"quote\":{\"CurrenyPriceInfo\":");

            content = content.Replace("data", "dataItem");

            CoinmarketcapDataSymbol result1 = JsonConvert.DeserializeObject<CoinmarketcapDataSymbol>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            foreach (CurrenyData data in result1.dataItem.CurrenyData)
            {
                Currency item = new Currency
                {
                    Id = data.id.ToString(),
                    Name = data.name,
                    Symbol = data.symbol,
                    Rank = data.cmc_rank.ToString(),
                    Price = data.quote.CurrenyPriceInfo.price ?? 0,
                    Volume24hUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    MarketCapUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    PercentChange1h = data.quote.CurrenyPriceInfo.percent_change_1h ?? 0,
                    PercentChange24h = data.quote.CurrenyPriceInfo.percent_change_24h ?? 0,
                    PercentChange7d = data.quote.CurrenyPriceInfo.percent_change_7d ?? 0,
                    LastUpdated = data.quote.CurrenyPriceInfo.last_updated,
                    MarketCapConvert = data.quote.CurrenyPriceInfo.market_cap ?? 0,
                    ConvertCurrency = convert
                };

                currencyList.Add(item);
            }
        }

        private static string GetNonSymbolData(string convert, bool oneItemonly, string content, List<Currency> currencyList)
        {
            content = content.Replace(convert, "CurrenyPriceInfo");
            if (oneItemonly)
                content = content.Replace("data", "dataItem");

            CoinmarketcapItemData result = JsonConvert.DeserializeObject<CoinmarketcapItemData>(content);

            foreach (ItemData data in result.DataList)
            {
                Currency item = new Currency
                {
                    Id = data.id.ToString(),
                    Name = data.name,
                    Symbol = data.symbol,
                    Rank = data.cmc_rank.ToString(),
                    Price = data.quote.CurrenyPriceInfo.price ?? 0,
                    Volume24hUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    MarketCapUsd = data.quote.CurrenyPriceInfo.volume_24h ?? 0,
                    PercentChange1h = data.quote.CurrenyPriceInfo.percent_change_1h ?? 0,
                    PercentChange24h = data.quote.CurrenyPriceInfo.percent_change_24h ?? 0,
                    PercentChange7d = data.quote.CurrenyPriceInfo.percent_change_7d ?? 0,
                    LastUpdated = data.quote.CurrenyPriceInfo.last_updated,
                    MarketCapConvert = data.quote.CurrenyPriceInfo.market_cap ?? 0,
                    ConvertCurrency = convert
                };

                currencyList.Add(item);
            }

            return content;
        }

    }
}
