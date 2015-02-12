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

        public string Extract(File file)
        {
            if (file.IsRTF())
            {
                var rtfText = file.Content();
                _rtBox.Rtf = rtfText;
                return _rtBox.Text;

            }
            if (file.IsTXT())
            {
                return file.Content();
            }
            throw new Exception("File type not implemented");
        }
    }
}
