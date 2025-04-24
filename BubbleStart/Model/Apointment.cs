using BubbleStart.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Media;

namespace BubbleStart.Model
{
    public class Apointment : BaseModel
    {
        private DateTime _DateTime;
        public string ApColor => GetApColor(false);
        public string GymColor => GetApColor(true);

        public int? UserId { get; set; }

        public decimal Cost { get; set; }

        private bool _Waiting;

        public bool Waiting
        {
            get
            {
                return _Waiting;
            }

            set
            {
                if (_Waiting == value)
                {
                    return;
                }

                _Waiting = value;
                RaisePropertyChanged();
            }
        }

        private bool _Canceled;

        public bool Canceled
        {
            get
            {
                return _Canceled;
            }

            set
            {
                if (_Canceled == value)
                {
                    return;
                }

                _Canceled = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public bool ShowedUpToday
        {
            get
            {
                if (Room == RoomEnum.Massage || Room == RoomEnum.Massage2 || Room == RoomEnum.MassageHalf)
                    return Customer.ShowUps.Any(s => s.Arrived.Date == DateTime.Today && s.ProgramModeNew == ProgramMode.massage);
                return Customer.ShowUps.Any(s => s.Arrived.Date == DateTime.Today && !s.Massage);
            }
        }

        public string GetApColor(bool gym)
        {
            if (Customer != null)
            {
                if (Waiting)
                {
                    return Colors.Silver.ToString();
                }
                if (DateTime < DateTime.Now)
                {
                    if (Room == RoomEnum.Massage || Room == RoomEnum.Massage2 || Room == RoomEnum.MassageHalf)
                    {
                        if (!Customer.ShowUps.Exists(s => s.Arrived.Date == DateTime.Date && s.ProgramModeNew == ProgramMode.massage))
                            return Colors.Red.ToString();
                    }
                    else if (!Customer.ShowUps.Exists(s => s.Arrived.Date == DateTime.Date && s.ProgramModeNew != ProgramMode.massage))
                        return Colors.Red.ToString();
                }
            }
            if (!gym)
            {
                if (Person == SelectedPersonEnum.Gogo)
                    return Colors.LimeGreen.ToString();
                if (Person == SelectedPersonEnum.Functional)
                    return Colors.Orange.ToString();
                if (Person == SelectedPersonEnum.Yoga)
                    return Colors.LightBlue.ToString();
                if (Person == SelectedPersonEnum.Massage)
                    return Colors.HotPink.ToString();
                if (Person == SelectedPersonEnum.Online)
                    return Colors.Yellow.ToString();
                if (Person == SelectedPersonEnum.Personal)
                    return Colors.Cyan.ToString();
                if (Person == SelectedPersonEnum.PilatesMat)
                    return Colors.Pink.ToString();
                if (Person == SelectedPersonEnum.Seminars)
                    return "#3b804d";
            }
            else
            {
                if (Gymnast != null && !string.IsNullOrWhiteSpace(Gymnast.ColorHash))
                    return Gymnast.ColorHash.ToString();
            }
            return Colors.Transparent.ToString();
        }

        public string CustomerWithGym => (Gymnast != null ? Gymnast.Name.Substring(0, 3) + " | " : "") + Customer.ToString();

        public int? GymnastId { get; set; }

        private User _Gymnast;

        public User Gymnast
        {
            get
            {
                return _Gymnast;
            }

            set
            {
                if (_Gymnast == value)
                {
                    return;
                }

                _Gymnast = value;
                RaisePropertyChanged();
            }
        }

        public string PersonName
        {
            get
            {
                switch (Person)
                {
                    case SelectedPersonEnum.Gogo:
                        return "Reformer" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    case SelectedPersonEnum.Functional:
                        return "Functional" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    case SelectedPersonEnum.Yoga:
                        return "Yoga" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    case SelectedPersonEnum.Massage:
                        return "Massage" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    case SelectedPersonEnum.Online:
                        return "Online" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    case SelectedPersonEnum.Personal:
                        return "Personal" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    case SelectedPersonEnum.PilatesMat:
                        return "Pilates Mat" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    case SelectedPersonEnum.Seminars:
                        return "Seminars" + (Gymnast != null ? " - " + Gymnast.Name : "");

                    default:
                        return "Error" + (Gymnast != null ? " - " + Gymnast.Name : "");
                }
            }
        }

        public string Description => GetDescription();

        private string GetDescription()
        {
            if (Person == SelectedPersonEnum.Yoga)
            {
                return "Yoga";
            }
            if (Room == RoomEnum.Fitness || Room == RoomEnum.Strength)
            {
                return "Functional, " + PersonName;
            }

            if (Room == RoomEnum.Functional)
            {
                if (DateTime.Year >= 2025)
                {
                    return "Yoga, " + PersonName;
                }
                return "Functional, " + PersonName;
            }
            if (Room == RoomEnum.FunctionalB)
            {
                if (DateTime.Year >= 2025)
                {
                    return "Mat, " + PersonName;
                }
                return "Functional, " + PersonName;
            }
            if (Room == RoomEnum.Pilates || Room == RoomEnum.Personal2)
            {
                return "Reformer, " + PersonName;
            }
            if (Room == RoomEnum.Massage || Room == RoomEnum.Massage2)
            {
                return "Massage, " + PersonName;
            }
            if (Room == RoomEnum.Outdoor)
            {
                return "Outdoor, " + PersonName;
            }
            if (Room == RoomEnum.MassageHalf)
            {
                return "Massage 30, " + PersonName;
            }
            if (Room == RoomEnum.Personal || Room == RoomEnum.FreeSpace)
            {
                return "Personal, " + PersonName;
            }
            return "Σφάλμα";
        }



        private RoomEnum _room;

        private SelectedPersonEnum? _person;

        public SelectedPersonEnum? Person
        {
            get => _person;

            set
            {
                if (_person == value)
                {
                    return;
                }

                _person = value;
                RaisePropertyChanged();
            }
        }

        public RoomEnum Room
        {
            get => _room;

            set
            {
                if (_room == value)
                {
                    return;
                }

                _room = value;
                RaisePropertyChanged();
            }
        }





        private string _DayString;

        [NotMapped]
        public string DayString
        {
            get
            {
                return _DayString;
            }

            set
            {
                if (_DayString == value)
                {
                    return;
                }

                _DayString = value;
                RaisePropertyChanged();
            }
        }

        private string _TimeString;

        [NotMapped]
        public string TimeString
        {
            get
            {
                return _TimeString;
            }

            set
            {
                if (_TimeString == value)
                {
                    return;
                }

                _TimeString = value;
                RaisePropertyChanged();
            }
        }

        public DateTime DateTime
        {
            get => _DateTime;

            set
            {
                if (_DateTime == value)
                {
                    return;
                }

                _DateTime = value;
                RaisePropertyChanged();
            }
        }

        //public int? CustomerId { get; set; }

        private Customer _Customer;

        public Customer Customer
        {
            get => _Customer;

            set
            {
                if (_Customer == value)
                {
                    return;
                }

                _Customer = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Modified { get; internal set; }
    }
}