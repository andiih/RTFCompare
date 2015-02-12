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
            tool.Process(folder);
            var res = tool.DuplicateFiles();

            foreach (var line in res)
            {
                var fileset= string.Join(",", line);
                Console.WriteLine(fileset);

            }
            Console.ReadLine();
        }
    }
}
