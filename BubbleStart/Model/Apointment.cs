using BubbleStart.Helpers;
using System;

namespace BubbleStart.Model
{
    public class Apointment : BaseModel
    {
        private DateTime _DateTime;

        public string PersonName
        {
            get
            {
                switch (Person)
                {
                    case SelectedPersonEnum.Gogo:
                        return "Γεωργία";

                    case SelectedPersonEnum.Dimitris:
                        return "Dimitris";

                    case SelectedPersonEnum.Yoga:
                        return "Yoga";

                    case SelectedPersonEnum.Massage:
                        return "Massage";

                    case SelectedPersonEnum.Online:
                        return "Online";

                    case SelectedPersonEnum.Personal:
                        return "Personal";

                    default:
                        return "Error";
                }
            }
        }

        public string Description => GetDescription();

        private string GetDescription()
        {
            if (Person == SelectedPersonEnum.Massage)
            {
                return "Massage";
            }
            if (Person == SelectedPersonEnum.Yoga)
            {
                return "Yoga";
            }
            if (Room==0)
            {
                return "Functional, " + PersonName;
            }
            if (Room == 1)
            {
                return "Reformer, " + PersonName;
            }
            return "Σφάλμα";
        }

        private int _room;

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

        public int Room
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