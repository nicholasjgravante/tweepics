using System;
using System.Collections.Generic;
using System.Text;

namespace Tweepics.Core.Models
{
    public class Name
    {
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }
        public string FirstLast { get; set; }
        public string Full { get; set; }

        public Name(string first, string middle, string last, string suffix)
        {
            First = first;
            Middle = middle;
            Last = last;
            Suffix = suffix;

            if (suffix == string.Empty)
            {
                FirstLast = first + " " + last;
                Full = first + " " + middle + " " + last;
            }
            else
            {
                FirstLast = first + " " + last + " " + suffix;
                Full = first + " " + middle + " " + last + " " + suffix;
            }
        }
    }
}
