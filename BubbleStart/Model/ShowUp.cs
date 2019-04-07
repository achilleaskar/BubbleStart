using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Model
{
    public class ShowUp : BaseModel
    {



        private DateTime _Time;




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




    

        private DateTime _Left;


        public DateTime Left
        {
            get
            {
                return _Left;
            }

            set
            {
                if (_Left == value)
                {
                    return;
                }

                _Left = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Arrived
        {
            get
            {
                return _Time;
            }

            set
            {
                if (_Time == value)
                {
                    return;
                }

                _Time = value;
                RaisePropertyChanged();
            }
        }




        private bool _Arrive;


        public bool Arrive
        {
            get
            {
                return _Arrive;
            }

            set
            {
                if (_Arrive == value)
                {
                    return;
                }

                _Arrive = value;
                RaisePropertyChanged();
            }
        }
    }
}
