using System;

namespace BubbleStart.Model
{
    public class Change : BaseModel
    {
        #region Constructors
        public Change()
        {

        }
        public Change(string description,User user)
        {
            Date = DateTime.Now;
            Description = description;
            User = user;
        }

        #endregion Constructors

        #region Fields

        private Customer _Customer;
        private DateTime _Date;
        private string _Description;

        private User _User;

        #endregion Fields

        #region Properties

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

        public DateTime Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date == value)
                {
                    return;
                }

                _Date = value;
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description == value)
                {
                    return;
                }

                _Description = value;
                RaisePropertyChanged();
            }
        }

        public User User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}