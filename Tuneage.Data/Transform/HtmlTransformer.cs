using System;
using System.Collections.Generic;
using System.Text;

namespace Tuneage.Data.Transform
{
    public class HtmlTransformer
    {
        public static string StringToHtmlString(string pureString)
        {
            return pureString
                .Replace("&", "&amp;")
                .Replace("'", "&#x27;");
        }
    }
}
