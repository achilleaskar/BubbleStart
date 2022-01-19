using BubbleStart.Wrappers;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace BubbleStart.Model
{
    [Table("BubbleUsers")]
    public class User : BaseModel, INamed
    {
        private ObservableCollection<WorkingRule> _WorkingRules;
        #region Constructors

        public User()
        {
            HashedPassword = new byte[0];
            Surename = string.Empty;
            WorkingRules = new ObservableCollection<WorkingRule>();
        }

        #endregion Constructors

        #region Properties

        public byte[] HashedPassword { get; set; }

        public int Level { get; set; }

        [Required(ErrorMessage = "Το όνομα απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το όνομα μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Το επίθετο απαιτείται!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Το Επίθετο μπορεί να είναι απο 3 έως 20 χαρακτήρες.")]
        public string Surename { get; set; }

        [Required(ErrorMessage = "Το Τηλέφωνο απαιτείται!")]
        [StringLength(18, MinimumLength = 3, ErrorMessage = "Το τηλέφωνο πρέπει να είναι τουλάχιστον 10 χαρακτήρες.")]
        [Phone(ErrorMessage = "Το τηλέφωνο δν έχει τη σωστή μορφή")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Το username απαιτείται!")]
        [StringLength(12, MinimumLength = 3, ErrorMessage = "Το username μπορεί να είναι απο 3 έως 12 χαρακτήρες.")]
        public string UserName { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
        public ObservableCollection<WorkingRule> WorkingRules
        {
            get
            {
                return _WorkingRules;
            }

            set
            {
                if (_WorkingRules == value)
                {
                    return;
                }

                _WorkingRules = value;
                RaisePropertyChanged();
            }
        }


        [StringLength(10)]
        public string ColorHash { get; set; }
    }
}