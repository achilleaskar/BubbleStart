using BubbleStart.Helpers;
using System;
using System.Linq;
using System.Windows.Media;

namespace BubbleStart.Model
{
    public class Apointment : BaseModel
    {
        private DateTime _DateTime;
        public string ApColor => GetApColor(false);
        public string GymColor => GetApColor(true);

        public string GetApColor(bool gym)
        {
            if (Customer != null)
            {
               
                if (DateTime < DateTime.Now && !Customer.ShowUps.Any(s => s.Arrived.Date == DateTime.Date))
                    return Colors.Red.ToString();
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
            if (Room == RoomEnum.Functional)
            {
                return "Functional, " + PersonName;
            }
            if (Room == RoomEnum.Pilates)
            {
                return "Reformer, " + PersonName;
            }
            if (Room == RoomEnum.Massage)
            {
                return "Massage, " + PersonName;
            }
            if (Room == RoomEnum.Outdoor)
            {
                return "Outdoor, " + PersonName;
            }
            return "Σφάλμα";
        }

        private RoomEnum _room;

        private SelectedPersonEnum _person;

        public SelectedPersonEnum Person
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
    }
}