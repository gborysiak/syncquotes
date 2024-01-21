using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.EntityFrameworkCore;
//using NetTopologySuite.Index.HPRtree;
using CoinMarketData;

namespace CoinMarketOdata.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;

    //[Route("odata/Coin")]
    public class CoinController : ODataController
    {
        private readonly CoinDbContext _context;
        private readonly ILogger<CoinController> _logger;

        public CoinController(CoinDbContext context, ILogger<CoinController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into CoinController");
        }



        [EnableQuery]
        //[Authorize]
        public ActionResult<IEnumerable<Coin>> Get()
        {
            _logger.LogInformation("Get coins");
            var items = _context.Coins.Include(i => i.History).AsQueryable();
            return Ok(items);
            //return Ok(_context.Coins.AsQueryable());
        }


        [EnableQuery]
        //[Authorize]
        public ActionResult<Coin> Get([FromRoute] int key)
        {
            _logger.LogInformation("Get Coin(" + key + ")");

            var item = _context.Coins.Include(i => i.History).FirstOrDefault(d => d.CoinId.Equals(key));
            //var item = _context.Coins.SingleOrDefault(d => d.CoinId.Equals(key));

            if (item == null)
            {
                return NotFound();
            }


            return Ok(item);
        }
    }
}
