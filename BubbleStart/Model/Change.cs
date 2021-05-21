using System;

namespace BubbleStart.Model
{
    public class Change : BaseModel
    {
        #region Constructors
        public Change()
        {

        }
        public Change(string description, User user)
        {
            Date = DateTime.Now;
            Description = description;
            User = user;
        }

        public int? Program_Id { get; set; } 
        public int? Payment_Id { get; set; } 
        public int? ShowUp_Id { get; set; } 
        public string Type => GetTypeDesc();

        private string GetTypeDesc()
        {
            if (ShowUp != null)
            {
                return "Παρουσία";
            }
            else if (Program != null)
            {
                return "Πακέτο";
            }
            else if (Payment != null)
            {
                return "Πληρωμή";
            }
            else
            {
                return "";
            }
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

        public DateTime Date
        {
            get => _Date;

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
            get => _Description;

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
            get => _User;

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

        public Program Program { get; set; }
        public Payment Payment { get; set; }
        public ShowUp ShowUp { get; set; }

        #endregion Properties
    }
}