using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CompareModel;

namespace CompareTool
{
    public class RTFExtractor
    {
        private RichTextBox _rtBox;

        public RTFExtractor()
        {
            _rtBox = new System.Windows.Forms.RichTextBox();
        }

        public byte[] ReadBinary(File file)
        {
            return file.BinaryContent();
        }

        public string ExtractText(File file)
        {
            try
            {
                if (file.IsRTF())
                {
                    var rtfText = file.TextContent();
                    _rtBox.Rtf = rtfText;
                    return _rtBox.Text;

                }
                if (file.IsTXT())
                {
                    return file.TextContent();
                }
                file.Skipped = true;
                return "";
            }
            catch
            {
                file.Skipped = true;
                return "";
            }
        }
    }
}
