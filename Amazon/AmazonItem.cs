using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UptimeTest.Amazon
{
    public class AmazonItem
    {
        private int index;
        private int pageNumber;
        private string title;
        private Decimal amount;
        private String currency;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = value; }
        }

        public string Title
        {
            get { return title; }
        }

        public Decimal Amount
        {
            get { return amount; }
        }

        public String FormatedAmount
        {
            get {
                if (amount.Equals(Decimal.MinValue))
                    return "";
                else
                    return amount.ToString("F").Replace(',', '.');
            }
        }
        
        public String Currency
        {
            get { return currency; }
        }

        public AmazonItem(String title, Decimal amount, String currency)
        {
            this.title = title;
            this.amount = amount;
            this.currency = currency;
        }
    }
}