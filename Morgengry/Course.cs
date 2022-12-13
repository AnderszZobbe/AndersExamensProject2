using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgengry
{
    public class Course : IValuable
    {
        public String Name { get; set; }
        public double CourseHourValue { get; set; } = 875.00;

        public int DurationInMinutes { get; set; }
        public Course(string name):this(name, 0) { }
        public Course(string name, int duration)
        {
            Name = name;
            DurationInMinutes = duration;
        }
        public new string ToString()
        {
            return "Name: " + Name + ", Duration in Minutes: " + DurationInMinutes;
        } 
        public double GetValue()
        {
            

            int k = 0;
            for (int i = 0; i < DurationInMinutes; i += 60)
            {
                k++;
            }
            return k * CourseHourValue;
        }
    }
}
