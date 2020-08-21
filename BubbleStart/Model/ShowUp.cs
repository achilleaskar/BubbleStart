using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;
using static BubbleStart.Helpers.Enums;

namespace BubbleStart.Model
{
    public class ShowUp : BaseModel
    {
        private bool _IsSelected;

        [NotMapped]
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }

            set
            {
                if (_IsSelected == value)
                {
                    return;
                }

                _IsSelected = value;
                RaisePropertyChanged();
            }
        }
        public string Type => GetShowUpType();

        private string GetShowUpType()
        {
            if (ProgramMode == ProgramMode.online)
            {
                return "Onl";
            }
            else if (ProgramMode == ProgramMode.outdoor)
            {
                return "Out";
            }
            else
                return "";
        }

        public SolidColorBrush RealColor => Real ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.OrangeRed);

        private bool _Real = true;

        public bool Real
        {
            get => _Real;

            set
            {
                if (_Real == value)
                {
                    return;
                }

                _Real = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RealColor));
            }
        }

        private int _Count;

        [NotMapped]
        public int Count
        {
            get => _Count;

            set
            {
                if (_Count == value)
                {
                    return;
                }

                _Count = value;
                RaisePropertyChanged();
            }
        }

        #region Fields

        private bool _Arrive;
        private SolidColorBrush _Color;
        private DateTime _Left;
        private DateTime _Time;

        private bool _Massage;

        public bool Massage
        {
            get => _Massage;

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








        private ProgramMode _ProgramMode;


        public ProgramMode ProgramMode
        {
            get
            {
                return _ProgramMode;
            }

            set
            {
                if (_ProgramMode == value)
                {
                    return;
                }

                _ProgramMode = value;
                RaisePropertyChanged();
            }
        }

        #endregion Fields

        #region Properties

        public bool Arrive
        {
            get => _Arrive;

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
            get => _Prog;

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
            get => _Time;

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
            get => _Color;

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
            get => _Left;

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