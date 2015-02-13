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
    /// <summary>
    /// MD5 Based comparison tool
    /// </summary>
    public class Tool
    {
        private List<File> _files;
        private MD5 _md5;

        /// <summary>
        /// Initialize a new Tool
        /// </summary>
        public Tool()
        {
            _md5 = System.Security.Cryptography.MD5.Create();
        }


        /// <summary>
        /// Recursively process a directory.
        /// This (recursively) reads the files in that directory, adds them into the internal _files structure, and checksums them
        /// </summary>
        /// <param name="sDir">Directory to read</param>
        /// <returns>'this' for chaining</returns>
        public Tool Process(string sDir)
        {
            string[] supportedTypes =  {"txt", "rtf"};
            _files = ReadDir(sDir,null);//Read in the list of files
            var extractor = new RTFExtractor();
            int cnt = 0;
            foreach (var file in _files)
            {
                if (file.TreatAsBinary())
                {
                    var binaryContent = extractor.ReadBinary(file);
                    file.CheckSum = MakeMd5(binaryContent);
                    if (string.IsNullOrWhiteSpace(file.CheckSum)) file.Skipped = true;

                }
                else
                {
                    var plainContent = extractor.ExtractText(file);
                    file.CheckSum = MakeMd5(plainContent);
                }
                cnt++;
                if (cnt%100==0) Console.Write(".");
            }
            Console.WriteLine();
            return this;
        }

        /// <summary>
        /// Group files according to checksum
        /// </summary>
        /// <returns>Returns the paths of duplicate files in groups.</returns>
        public List<List<string>> FindDuplicateFiles()
        {
            if (_files==null) throw new Exception("Call ProcessFiles before calling FindDuplicates");
            var grps = _files.Where(f=>!f.Skipped).GroupBy(f => f.CheckSum).Where(g => g.Count() > 1);
            var res = grps.Select(grp => grp.Select(file => file.Path).ToList()).ToList();
            return res;
        } 



        private List<File> ReadDir(string sDir, string[] extensions=null)
        {
            var res = new List<File>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    var ext = f.Split('.').LastOrDefault().ToLower();
                    if (extensions == null||extensions.Contains(ext)) res.Add(new File(f));
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    res.AddRange(ReadDir(d,extensions));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return res;
        }

        private string MakeMd5(string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            return MakeMd5(inputBytes);
        }

        private string MakeMd5(byte[] inputBytes)
        {
            try
            {
                byte[] hash = _md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
            catch
            {
                return null;
            }
        }



    }
}
