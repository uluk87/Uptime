using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UptimeTest.Amazon;
using UptimeTest.Amazon.ECS;

namespace UptimeTest.Models
{
    public class SearchModel
    {
        String searchKeyword;
        List<AmazonItem> amazonItem;
        String searchMoreUrl;

        public List<AmazonItem> AmazonItem
        {
            get { return amazonItem; }
            set { amazonItem = value; }
        }

        public String SearchKeyword
        {
            get { return searchKeyword; }
            set { searchKeyword = value; }
        }

        public int PagesCount
        {
            get { 
                if (amazonItem.Count > 0)
                    return Convert.ToInt32(Math.Ceiling((double)amazonItem.Count / (double)Settings.SEARCH_RESULTS_PER_PAGE));
                else
                    return 0;
            }
        }

        public String SearchMoreUrl
        {
            get { return searchMoreUrl; }
            set { searchMoreUrl = value; }
        }
    }
}