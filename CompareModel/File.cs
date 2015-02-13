using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareModel
{
    public class File
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string CheckSum { get; set; }
        public bool Skipped { get; set; }

        public File(string path)
        {
            this.Path = path;
            Skipped = false;
        }

        public bool IsRTF()
        {
            return (this.Path ?? "").ToLower().EndsWith(".rtf");
        }
        public bool IsTXT()
        {
            return (this.Path ?? "").ToLower().EndsWith(".txt");
        }

        public bool TreatAsBinary()
        {
            
            return !IsRTF() && !IsTXT();
        }

        public string TextContent()
        {
            try
            {
                return System.IO.File.ReadAllText(Path);
            }
            catch
            {
                Skipped = true;
                return "";
            }
        }

        public byte[] BinaryContent()
        {
            try{
            return System.IO.File.ReadAllBytes(Path);
            }
            catch
            {
                Skipped = true;
                return null;
            }        
        }
    }
}
