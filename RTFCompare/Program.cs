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
            if (args.Count()!=3)
            {
                Console.WriteLine("Usage : RTFCompare folder outputfile outputfile2");
                return;
            }
            var folder = args[0];
            var outputFile = args[1];
            var outputFile2 = args[2];
            var tool = new Tool();

            var res = tool.Process(folder).FindDuplicateFiles();

            var lines = res.Select(line => string.Join(",", line));
            using (System.IO.StreamWriter ofile = new System.IO.StreamWriter(outputFile))
            {
                foreach (string line in lines)
                {
                        ofile.WriteLine(line);
                        Console.Write("+");
                }
            }


            using (System.IO.StreamWriter ofile = new System.IO.StreamWriter(outputFile2))
            {
                foreach (var dupelist in res)
                {
                    foreach (var line in dupelist.Skip(1))
                    ofile.WriteLine(line);
                    Console.Write("*");
                }
            }

//            foreach (var fileset in res.Select(line => string.Join(",", line)))
//            {
//                Console.WriteLine(fileset);
//            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
