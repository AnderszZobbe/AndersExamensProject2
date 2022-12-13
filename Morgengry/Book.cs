using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgengry
{
    public class Book : Merchandise
    {
        public double Price { get; set; }
        public String Title { get; set; }
        public Book(string itemid) : this(itemid,"")
        { }
        public Book(string itemid, string title) : this(itemid, title, 0)
        { }
        public Book(string itemid, string title, double price) : base(itemid)
        {
            Title = title;
            Price = price;
        }
        public new string ToString()
        {
            return "ItemId: " + ItemId + ", Title: " + Title + ", Price: " + Price;
        }
    }
}
