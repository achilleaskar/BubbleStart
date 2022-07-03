using System.ComponentModel.DataAnnotations;

namespace BubbleStart.Model
{
    public class Item : BaseModel
    {
       
        private bool _Shop;

        public bool Shop
        {
            get
            {
                return _Shop;
            }

            set
            {
                if (_Shop == value)
                {
                    return;
                }

                _Shop = value;
                RaisePropertyChanged();
            }
        }

        private string _Name;

        [StringLength(40)]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}