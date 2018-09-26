using System;
using System.Collections.Generic;
using System.Text;

namespace Tweepics.Core.Models
{
    public class Office
    {
        public string Government { get; set; }
        public string Branch { get; set; }
        public string State { get; set; }
        public string Title { get; set; }

        public Office (string government, string branch, string state, string title)
        {
            Government = government;
            Branch = branch;
            State = state;
            Title = title;
        }
    }
}
