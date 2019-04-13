using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Model
{
    public class Program : BaseModel
    {



        private DateTime _DayOfIssue;


        public DateTime DayOfIssue
        {
            get
            {
                return _DayOfIssue;
            }

            set
            {
                if (_DayOfIssue == value)
                {
                    return;
                }

                _DayOfIssue = value;
                RaisePropertyChanged();
            }
        }




        private int _Duration;


        public int Duration
        {
            get
            {
                return _Duration;
            }

            set
            {
                if (_Duration == value)
                {
                    return;
                }

                _Duration = value;
                RaisePropertyChanged();
            }
        }




        private int _Amount;


        public int Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }

    }
}
