using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgengry
{
    public class Amulet : Merchandise
    {

        public String Design { get; set; }
        public Level Quality { get; set; }
        public Amulet(string itemid) : this(itemid, Level.medium)
        { }
        public Amulet(string itemid,Level quality) : this(itemid, quality,"")
        { }
        public Amulet(string itemid,Level quality,string design) : base (itemid)
        {
            Quality = quality;
            Design = design;
        }
        public new string ToString()
        {
            return "ItemId: " + ItemId + ", Quality: " + Quality + ", Design: " + Design;
        }
    }
}
