using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FreeBird.Infrastructure.Http
{
    public class FormStringControl
    {
        public FormStringControl()
        {
            _values = new NameValueCollection();
        }

        public FormStringControl(string queryValue)
        {
            _values = HttpUtility.ParseQueryString(queryValue);
        }

        private NameValueCollection _values;

        public NameValueCollection Values
        {
            get
            {
                return _values;
            }
        }

        public void AddValue(string name, string value, bool isCover = true)
        {
            if (ContainParamName(name))
            {
                if (isCover)
                {
                    _values.Remove(name);
                }
                else
                {
                    return;
                }
            }
            _values.Add(name, value);
        }

        private string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        private string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        public bool ContainParamName(string paramName)
        {
            return _values.AllKeys.Any(t => t.Equals(paramName, StringComparison.OrdinalIgnoreCase));
        }

        public string GetParamValue(string paramName)
        {
            if (!ContainParamName(paramName))
                return string.Empty;
            var value = _values[paramName];
            return HttpUtility.UrlDecode(value);
        }

        public override string ToString()
        {
            string str2 = string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (string str3 in _values.AllKeys)
            {
                builder.Append(str2);
                builder.Append(UrlEncode(str3));
                builder.Append("=");
                builder.Append(UrlEncode(_values[str3]));
                str2 = "&";
            }
            return builder.ToString();
        }

    }
}
