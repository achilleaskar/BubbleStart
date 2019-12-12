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
                    default:
                        return "";
                }
            }
        }


        private int _room;




        private int _person;


        public int Person
        {
            get
            {
                return _person;
            }

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
            get
            {
                return _room;
            }

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
            get
            {
                return _DateTime;
            }

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
            get
            {
                return _Customer;
            }

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