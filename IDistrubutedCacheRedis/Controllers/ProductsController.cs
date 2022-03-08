using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDistrubutedCacheRedis.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distrubutedCacheRedis;

        public ProductsController(IDistributedCache distrubutedCacheRedis)
        {
            _distrubutedCacheRedis = distrubutedCacheRedis;
        }

        public  async Task< IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);
              await _distrubutedCacheRedis.SetStringAsync("Product", "Elma", cacheEntryOptions);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            string name = await _distrubutedCacheRedis.GetStringAsync("Product");
            ViewBag.name = name;

            return View();
        }
        public async Task<IActionResult> Remove()
        {
           await _distrubutedCacheRedis.RemoveAsync("Product");

            return View();
        }
    }
}
