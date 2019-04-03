using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Messages
{
    public class ChangeVisibilityMessage
    {
        public bool Visible { get; private set; }

        public ChangeVisibilityMessage(bool visible)
        {
            Visible = visible;
        }
    }
}