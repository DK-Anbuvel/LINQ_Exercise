using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    public  class L1
    {

        public void Selectlinq()
        {
            string[] name = { "Anbu", "Vel", "Siva" };

            var res = from names in name where names.Contains("a") select names;
             
            Console.WriteLine(res);
        }
    }
}
