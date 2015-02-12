using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using File = CompareModel.File;

namespace CompareTool
{
    public class Tool
    {
        private List<File> _files;
        private MD5 _md5;

        public Tool()
        {
            _md5 = System.Security.Cryptography.MD5.Create();

        }

        private string MakeMD5(string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = _md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }

        public void Process(string sDir)
        {
            _files = ReadDir(sDir);//Read in the list of files
            var extractor = new RTFExtractor();
            foreach (var file in _files)
            {
                var plainContent = extractor.Extract(file);
                file.CheckSum = MakeMD5(plainContent);
            }
        }


        private List<File> ReadDir(string sDir)
        {
            var res = new List<File>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    res.Add(new File(f));
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    res.AddRange(ReadDir(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return res;
        }

        public List<List<string>> DuplicateFiles()
        {
            var grps = _files.GroupBy(f => f.CheckSum).Where(g=>g.Count()>1);
            var res = grps.Select(grp => grp.Select(file => file.Path).ToList()).ToList();
            return res;
        } 
    }
}
