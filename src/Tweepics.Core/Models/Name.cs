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
        public string FirstLast { get; set; }
        public string Full { get; set; }

        public Name() { }

        public Name(string first, string middle, string last)
        {
            First = first;
            Middle = middle;
            Last = last;
            FirstLast = first + " " + last;
            Full = first + " " + middle + " " + last;
        }
    }


}
