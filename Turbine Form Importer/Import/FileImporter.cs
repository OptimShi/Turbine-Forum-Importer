using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turbine_Form_Importer.Import
{
    public class FileImporter
    {
        public string Template;
        public FileImporter(string filename){
            string text = File.ReadAllText(filename);
            Template = getTemplate(text);
        }

        private string getTemplate(string text)
        {

            //Each forum page has a template in a comment at the top of the file, after the DOCTYPE but before the <html> declaration.

            string matchStr = "<!-- BEGIN TEMPLATE: ";
            var start = text.IndexOf(matchStr);
            var htmlStart = text.IndexOf("<html");
        

            if(start != -1 && htmlStart > start)
            {
                start = start + matchStr.Length;
                var end = text.IndexOf("-->");
                string template = text.Substring(start, end - start);
                return template;

            }

            return "";
        }
    }
}
