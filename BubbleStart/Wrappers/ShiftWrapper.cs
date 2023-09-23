using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Wrappers
{
    public class ShiftWrapper : ModelWrapper<Shift>
    {
        public ShiftWrapper() : base(new Shift())
        {
            Title = "Το ωράριο";
        }

        public ShiftWrapper(Shift model) : base(model)
        {
            Title = "Το ωράριο";
        }

        public DateTime? From
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public DateTime? FromB
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public bool Parted
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public DateTime? To
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public DateTime? ToB
        {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public override string ToString()
        {
            if (!From.HasValue || !To.HasValue)
            {
                return "ERROR";
            }
            if (Id == 3)
            {
                return string.Empty;
            }
            return $"{From.Value:HH:mm}-{To.Value:HH:mm}" + (Parted && FromB.HasValue && ToB.HasValue ? $" & {FromB.Value:HH:mm}-{ToB.Value:HH:mm}" : "");
        }
    }
}