using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Models;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeApi.Web.Controllers
{
    public class SortedController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        private string ListKey = "SortedSetNames";
        public SortedController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(3);
        }
  
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();
            
            if (_db.KeyExists(ListKey))
            {
                _db.SortedSetScan(ListKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            return View(list);

        }
        [HttpPost]
        public IActionResult Add(string name,int score)
        {
            _db.SortedSetAdd(ListKey, name, score);

            _db.KeyExpire(ListKey, DateTime.Now.AddHours(1));
            return RedirectToAction("Index");
        }
        public IActionResult Delete(string name,int score)
        {
            string key = name.Split(':')[0];
            _db.SortedSetRemove(ListKey, key);
            return RedirectToAction("Index");





        }   
    }
}
