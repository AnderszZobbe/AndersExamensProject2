using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgengry
{
    public class ValuableRepository
    {
        List<IValuable> valuables = new List<IValuable>(0);

        public void AddValuable(IValuable item)
        {
            valuables.Add(item);
        }
        public  IValuable GetValuable(string id)
        {
            Exception invalidId = new Exception();   
            foreach(IValuable i in valuables)
            {
                Course course = new Course("");
                Merchandise merchandise = new Merchandise("");
                if (i.GetType() == merchandise.GetType())
                {
                    merchandise = (Merchandise)i;
                    if (id == merchandise.ItemId)
                    {
                        return i;
                    }
                }
                else if (i.GetType() == course.GetType())
                {
                    course = (Course)i;
                    if(course.Name == id)
                    {
                        return i;
                    }
                }
                
            }
            throw invalidId;
        }
        public double GetTotalValue()
        {
            double value = 0;
            foreach(IValuable i in valuables)
            {
                value += i.GetValue();
            }
            return value;
        }
        public int Count()
        {
            return valuables.Count;
        }
    }
}
