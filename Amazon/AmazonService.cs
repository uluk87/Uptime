using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using UptimeTest.Amazon;
using UptimeTest.Amazon.ECS;
using UptimeTest.Amazon.ECS.Addons;

namespace UptimeTest.Amazon
{
    public class AmazonService
    {
        public const int AMAZON_REQUST_ITEM_LIMIT = 10;
        public const int AMAZON_REQUST_PAGE_LIMIT = 10;
        public const int AMAZON_MAX_BATCH_REQUEST = 2;

        BasicHttpBinding binding;
        AWSECommerceServicePortTypeClient client;

        public AmazonService()
        {
            binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;

            client = new AWSECommerceServicePortTypeClient(binding,
                                new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));
            
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(Settings.AMAZON_ACCESS_KEY_ID, Settings.AMAZON_SECRET_KEY));
        }

        public List<AmazonItem> searchFormAmazone(String keyword, out string searchMoreUrl)
        {
            searchMoreUrl = String.Empty;
            List<AmazonItem> amazoneItems = new List<AmazonItem>();
            // Arvutan välja mitu lehekülge on vaja.
            int searchRequestPageCounts = 1;
            if (Settings.SEARCH_RESULTS_PER_PAGE > AMAZON_REQUST_ITEM_LIMIT)
            {
                double neededItems = Convert.ToDouble(Settings.SEARCH_RESULTS_PER_PAGE * Settings.PRELOAD_PAGE_COUNT);
                searchRequestPageCounts = Convert.ToInt32(Math.Ceiling((double)(neededItems / AMAZON_REQUST_ITEM_LIMIT)));
                if (searchRequestPageCounts > AMAZON_REQUST_PAGE_LIMIT)
                    searchRequestPageCounts = AMAZON_REQUST_PAGE_LIMIT;
            }

            // Arvutan mitu päringut ma Amazon'i serveri teen
            int differentRequests = Convert.ToInt32(Math.Ceiling((double)searchRequestPageCounts / (double)AMAZON_MAX_BATCH_REQUEST));
            for (int i = 0; i < differentRequests; i++)
            {
                int startPage = i * AMAZON_MAX_BATCH_REQUEST + 1;
                int endPage = i * AMAZON_MAX_BATCH_REQUEST + AMAZON_MAX_BATCH_REQUEST;
                if (endPage > searchRequestPageCounts)
                {
                    endPage = endPage - (endPage%searchRequestPageCounts);
                }
                Items[] searchedItems = searchAmazonItems(keyword, startPage, endPage);
                if (searchedItems != null)
                {
                    for (int j = 0; j < searchedItems.Length; j++)
                    {
                        foreach (Item result in searchedItems[j].Item)
                        {
                            ItemAttributes itemAttributes = result.ItemAttributes;
                            Decimal amount = Decimal.MinValue;
                            String currencyCode = "";
                            if (itemAttributes.ListPrice != null)
                            {
                                Decimal.TryParse(itemAttributes.ListPrice.Amount, out amount);
                                currencyCode = itemAttributes.ListPrice.CurrencyCode;
                                if (amount != Decimal.MinValue)
                                    amount = amount / 100;
                            }

                            AmazonItem amazoneItem = new AmazonItem(itemAttributes.Title, amount, currencyCode);
                            amazoneItem.Index = amazoneItems.Count + 1;
                            amazoneItem.PageNumber = Convert.ToInt32(Math.Ceiling((double)amazoneItem.Index / (double)Settings.SEARCH_RESULTS_PER_PAGE));
                            amazoneItems.Add(amazoneItem);
                            
                        }
                        searchMoreUrl = searchedItems[j].MoreSearchResultsUrl;
                    }
                }
            }

            return amazoneItems;
        }

        private Items[] searchAmazonItems(String keyword, int startPage, int endPage)
        {
            List<ItemSearchRequest> itemSearchRequests = new List<ItemSearchRequest>();
            
            ItemSearch itemSearch = new ItemSearch();
            itemSearch.AWSAccessKeyId = Settings.AMAZON_ACCESS_KEY_ID;
            itemSearch.AssociateTag = Settings.AMAZON_ASSOCATE_TAG;
            
            // Valmistan otsingute päringud ette
            for (int i = startPage; i <= endPage; i++)
            {
                itemSearchRequests.Add(getItemSearchRequest(keyword, i));
            }

            itemSearch.Request = itemSearchRequests.ToArray();

            // Otsin esemeid
            ItemSearchResponse response = client.ItemSearch(itemSearch);
            return response.Items;
        }

        private ItemSearchRequest getItemSearchRequest(String keyword, int pageNumber)
        {
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = "All";
            request.Keywords = keyword;
            request.ItemPage = pageNumber.ToString();
            request.ResponseGroup = new string[] { "Medium" };
            return request;
        }
    }
}