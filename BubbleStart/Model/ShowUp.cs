using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace BubbleStart.Model
{
    public class ShowUp : BaseModel
    {
        public ShowUp()
        {

        }
        #region Fields

        private bool _Arrive;
        private SolidColorBrush _Color;
        private DateTime _Left;
        private DateTime _Time;





        private bool _Massage;


        public bool Massage
        {
            get
            {
                return _Massage;
            }

            set
            {
                if (_Massage == value)
                {
                    return;
                }

                _Massage = value;
                RaisePropertyChanged();
            }
        }
        #endregion Fields

        #region Properties

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




        private Program _Prog;

        [NotMapped]
        public Program Prog
        {
            get
            {
                return _Prog;
            }

            set
            {
                if (_Prog == value)
                {
                    return;
                }

                _Prog = value;
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
                if (value == null)
                {
                    RaisePropertyChanged();
                    return;
                }

                _Time = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public SolidColorBrush Color
        {
            get
            {
                return _Color;
            }

            set
            {
                if (_Color == value)
                {
                    return;
                }

                _Color = value;
                RaisePropertyChanged();
            }
        }

        public Customer Customer { get; set; }
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

        #endregion Properties

    }
}