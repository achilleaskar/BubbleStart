using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleStart.Model
{
    [Table("BubbleDayWorkingShifts")]
    public class DayWorkingShift : BaseModel
    {
        private int _NumOfDay;

        public int NumOfDay
        {
            get
            {
                return _NumOfDay;
            }

            set
            {
                if (_NumOfDay == value)
                {
                    return;
                }

                _NumOfDay = value;
                RaisePropertyChanged();
            }
        }

        private Shift _Shift;

        public WorkingRule WorkingRule { get; set; }
        public int? WorkingRule_Id { get; set; }
        public Shift Shift
        {
            get
            {
                return _Shift;
            }

            set
            {
                if (_Shift == value)
                {
                    return;
                }

                _Shift = value;
                RaisePropertyChanged();
            }
        }
    }
}