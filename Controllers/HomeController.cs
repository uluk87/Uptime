using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UptimeTest.Amazon;
using UptimeTest.Amazon.ECS;
using UptimeTest.Models;

namespace UptimeTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SearchModel searchModel = new SearchModel();
            if (Request.Params["search"] != null)
            {
                String searchKeyword = Request.Params["search"];
                searchModel.SearchKeyword = searchKeyword;
                AmazonService amazoneService = new AmazonService();
                String searchMoreFormAmazoneUrl;
                List<AmazonItem> items = amazoneService.searchFormAmazone(searchKeyword, out searchMoreFormAmazoneUrl);
                searchModel.AmazonItem = items;
                searchModel.SearchMoreUrl = searchMoreFormAmazoneUrl;
            }

            return View(searchModel);
        }
    }
}
