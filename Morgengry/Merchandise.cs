using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgengry
{
    public class Merchandise: IValuable
    {
        public String ItemId { get; set; }

        public double GetValue()
        {
            Merchandise merchandise = this;
            double priceOfLowQuality = 12.5;
            double priceOfMediumQuality = 20.0;
            double priceOfHighQuality = 27.5;
            Exception notValidMerchandise = new Exception();
            Exception notValidQuality = new Exception();
            Book book = new Book("");
            Amulet amulet = new Amulet("");
            if (merchandise.GetType() == book.GetType())
            {
                book = (Book)merchandise;
                return book.Price;
            }
            else if (merchandise.GetType() == amulet.GetType())
            {
                amulet = (Amulet)merchandise;
                if (amulet.Quality == Level.high)
                {
                    return priceOfHighQuality;
                }
                else if (amulet.Quality == Level.medium)
                {
                    return priceOfMediumQuality;
                }
                else if (amulet.Quality == Level.low)
                {
                    return priceOfLowQuality;
                }
                else
                {
                    throw notValidQuality;
                }
            }
            else
            {
                throw notValidMerchandise;

            }
        }
        public Merchandise(string itemid)
        {
            ItemId = itemid;
        }
        public new string ToString()
        {
            return "ItemId: " + ItemId;
        } 
    }
}
