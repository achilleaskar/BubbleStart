using BubbleStart.Helpers;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;
using static BubbleStart.Model.Program;

namespace BubbleStart.Model
{
    public class ShowUp : BaseModel
    {
        private bool _IsSelected;

        private bool _Is30min;

        private bool _Present;

        public bool Present
        {
            get
            {
                return _Present;
            }

            set
            {
                if (_Present == value)
                {
                    return;
                }

                _Present = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(RealColor));
            }
        }

        public bool Is30min
        {
            get
            {
                return _Is30min;
            }

            set
            {
                if (_Is30min == value)
                {
                    return;
                }

                _Is30min = value;
                RaisePropertyChanged();
            }
        }

        public IList<Change> Changes { get; set; }

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

        public SolidColorBrush RealColor => Present ? new SolidColorBrush(Colors.LightPink) : Real ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.OrangeRed);

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

        public string Description => GetDescription();

        private string GetDescription()
        {
            var res = string.Empty;
            res += GetProogramName();
            if (Is30min)
            {
                res += ", 30'";
            }

            if (Present)
            {
                res += ", Δώρο";
            }

            if (!Real)
            {
                res += ", Δεν ήρθε";
            }

            return res;
        }

        private string GetProogramName()
        {
            if (Prog != null)
            {
                return ((ProgramTypes)Prog.ProgramType).AsString(EnumFormat.Description);
            }
            switch (ProgramMode)
            {
                case ProgramMode.normal:
                    return "Γυμναστική";

                case ProgramMode.massage:
                    return "Massage";

                case ProgramMode.online:
                    return "Online";

                case ProgramMode.outdoor:
                    return "Outdoor";

                default:
                    return "Σφάλμα";
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