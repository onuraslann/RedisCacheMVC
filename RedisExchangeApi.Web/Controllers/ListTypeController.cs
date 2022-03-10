using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _db;
        private string ListKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            _db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> nameList = new List<string>();
            if (_db.KeyExists(ListKey))
            {
                _db.ListRange(ListKey).ToList().ForEach(x=>
                {

                    nameList.Add(x.ToString());
                
                });
            }
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            _db.ListRightPush(ListKey,name);


            return RedirectToAction("Index");
        }

     
        public IActionResult Delete(string name)
        {
            _db.ListRemove(ListKey, name);


            return RedirectToAction("Index");
        }
    }
}
