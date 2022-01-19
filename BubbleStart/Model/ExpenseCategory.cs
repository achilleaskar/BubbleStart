using System.ComponentModel.DataAnnotations;

namespace BubbleStart.Model
{
    public class ExpenseCategoryClass : BaseModel
    {
        private string _Name;

        [Required]
        [StringLength(50, MinimumLength = 3)]
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

        private ExpenseCategoryClass _Parent;

        public int? ParentId { get; set; }

        public ExpenseCategoryClass Parent
        {
            get
            {
                return _Parent;
            }

            set
            {
                if (_Parent == value)
                {
                    return;
                }

                _Parent = value;
                RaisePropertyChanged();
            }
        }
    }
}