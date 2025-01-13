using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Messages
{
    public class ErrorMessage
    {

        public ErrorMessage(string error)
        {
            Error = error;
        }

        public string Error { get; set; }
    }
}
