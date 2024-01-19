using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCoinMarket
{
    public sealed class Settings
    {
        public required string MarketCapCoinAPI { get; set; }
        public required string MariaDbHost { get; set; }
        public required string MariaDbDatabase { get; set; }

        public required string MariaDbUser { get; set; }
        public required string MariaDbDPassword { get; set; }


    }
}
