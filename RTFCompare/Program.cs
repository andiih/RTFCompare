using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompareTool;

namespace RTFCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            var folder = args[0];
            var tool = new Tool();

            var res = tool.Process(folder).FindDuplicateFiles();

            foreach (var fileset in res.Select(line => string.Join(",", line)))
            {
                Console.WriteLine(fileset);
            }
            Console.ReadLine();
        }
    }
}
