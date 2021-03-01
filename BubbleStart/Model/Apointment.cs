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
                    case 0:
                        return "Γεωργία";
                    case 1:
                        return "Dimitris";
                    case 2:
                        return "Yoga"; 
                    case 3:
                        return "Massage";
                    case 4:
                        return "Online";
                    default:
                        return "";
                }
            }
        }


        private int _room;




        private int _person;


        public int Person
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