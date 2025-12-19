using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_Exercises.Exercises
{
    public class Exe_3
    {
        /* 21.Write a program in C# Sharp to create a list of
           numbers and display numbers greater than 80.
         */

        public List<int> DisplayGreaterThen(List<int> list)
        {
            var result = from s in list
                         where s > 80
                         select s;
            var result1 = list.Where(e=> e > 80).ToList();

            return result.ToList();
        }
    }
}
