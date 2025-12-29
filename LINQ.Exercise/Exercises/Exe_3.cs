using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /*
         * 22. Write a program in C# Sharp to find the strings for a specific minimum length.
         */
        public int ReturnMinimumLengthOfStringArray(string[] str)
        {
            var result = str.OrderByDescending(s => s.Length).Take(1).Select(s=>s.Length)
                .FirstOrDefault(); // to convert IEnumerable to int
            var i = str.Min(s => s.Length);
            var y = str.Max(s => s.Length);
            var x = str.MaxBy(s => s.Length);

            var result1 = (from s in str
                          orderby s.Length descending
                          select s.Length ).First();

            return result;
        }
        /*
         * 23. Write a program in C# Sharp to generate a cartesian product of two sets.
         */
        public List<(string, string)> DisplayCartesianProduct(List<char> characterList, List<int> NumberList)
        {
            var result = from c in characterList
                         from n in NumberList
                         select (Character: c.ToString(), Number: n.ToString());
            //           select new
            //             {
            //                 Charcter = c.ToString(),
            //                 Number = n.ToString()
            //             };
            //($"Charcter: {c}", $"Number: {n}");

            var result1 = characterList.SelectMany(n => NumberList,
                (characterList, n) => new {
                    Chacter = characterList,
                    NumberList = n });

            //(List<(string, string)>)result;
            return result.ToList();
        }
        /*
         * 24. Write a program in C# Sharp to generate a cartesian product of three sets.
         */
        public List<(string,string,string)> DisplayCartesianProduct(List<char> characterList, List<int> NumberList,List<string> colour)
        {
            var result = from c in characterList
                         from n in NumberList
                         from col in colour
                         select (Character: c.ToString(), Number: n.ToString(),col.ToString());

           var result1 = characterList.SelectMany(c=> NumberList,(c,n)=> new  {c,n})
                                      .SelectMany(cn=>colour,(cn,c)=> new {Character =cn.c.ToString(),
                                           NumberList=cn.n.ToString(),
                                           Colour = c.ToString()});

            return result.ToList();
        }
    }
}
