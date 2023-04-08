using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Model
{
    public class ProgramChange : BaseModel
    {
        public Guid InstanceGuid { get; set; }

        public DateTime Date { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
