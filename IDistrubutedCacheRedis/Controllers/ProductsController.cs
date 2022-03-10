using IDistrubutedCacheRedis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
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

        public async Task<IActionResult> Index()
        {


            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(2);
            Product product = new Product
            {
                Id = 1,
                ProductName = "Kalem",
                Price = 100,
            };
            string jsonProduct = JsonConvert.SerializeObject(product);
            await _distrubutedCacheRedis.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            string jsonProduct = await _distrubutedCacheRedis.GetStringAsync("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.product = p;

            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await _distrubutedCacheRedis.RemoveAsync("Product");

            return View();
        }
    }
}
