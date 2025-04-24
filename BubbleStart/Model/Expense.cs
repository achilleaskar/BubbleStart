using BubbleStart.Helpers;
using BubbleStart.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows;

namespace BubbleStart.Model
{
    public class Expense : BaseModel
    {
        #region Constructors

        public Expense()
        {
            Date = From = To = DateTime.Today;
        }

        #endregion Constructors

        #region Fields

        [NotMapped]
        public EconomicData_ViewModel parent { get; set; }

        private decimal _Amount;
        private DateTime _Date;
        private DateTime _From;
        private bool _Income;
        private ExpenseCategoryClass _MainCategory;
        private string _Reason;
        private ExpenseCategoryClass _SecondaryCategory;
        private DateTime _To;
        private User _User;

        #endregion Fields

        #region Properties

        public decimal Amount
        {
            get => _Amount;

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public ObservableCollection<ExpenseCategoryClass> SecondaryCategories => parent != null && MainCategory != null ? (Income ?
            new ObservableCollection<ExpenseCategoryClass>(parent.BasicDataManager.ExpenseCategoryClasses.Where(p => p.ParentId == 20 || p.Id == -1).OrderBy(r => r.Name)) :
            new ObservableCollection<ExpenseCategoryClass>(parent.BasicDataManager.ExpenseCategoryClasses.Where(p => p.ParentId == MainCategory.Id || p.Id == -1).OrderBy(r => r.Name)))
            : new ObservableCollection<ExpenseCategoryClass>();

        public DateTime Date
        {
            get => _Date;

            set
            {
                if (_Date == value)
                {
                    return;
                }

                if (From == To && To == _Date)
                {
                    From = To = value;
                }
                _Date = value;

                RaisePropertyChanged();
            }
        }

        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                if (To <= value)
                {
                    To = value;
                }
                RaisePropertyChanged();
            }
        }

        public bool Income
        {
            get
            {
                return _Income;
            }

            set
            {
                if (_Income == value)
                {
                    return;
                }

                _Income = value;
                RaisePropertyChanged();
            }
        }

        public ExpenseCategoryClass MainCategory
        {
            get
            {
                return _MainCategory;
            }

            set
            {
                if (_MainCategory == value)
                {
                    return;
                }

                _MainCategory = value;
                if (value?.Id == -1)
                {
                    _MainCategory = null;
                }
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SecondaryCategories));
            }
        }

        public int? MainCategoryId { get; set; }




        private decimal _Bank;


        public decimal Bank
        {
            get
            {
                return _Bank;
            }

            set
            {
                if (_Bank == value)
                {
                    return;
                }

                _Bank = value;
                RaisePropertyChanged();
            }
        }


        public Visibility ShowBankCash => MainCategory?.Id == 3 ? Visibility.Visible : Visibility.Collapsed;

        private decimal _Cash;


        public decimal Cash
        {
            get
            {
                return _Cash;
            }

            set
            {
                if (_Cash == value)
                {
                    return;
                }

                _Cash = value;

                if (MainCategory?.Id == 3||MainCategoryId==3)
                {
                    Amount = Bank + Cash;
                }

                RaisePropertyChanged();
            }
        }

        public string Reason
        {
            get => _Reason;

            set
            {
                if (_Reason == value)
                {
                    return;
                }

                _Reason = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        public ExpenseCategoryClass SecondaryCategory
        {
            get
            {
                return _SecondaryCategory;
            }

            set
            {
                if (_SecondaryCategory == value)
                {
                    return;
                }

                _SecondaryCategory = value;
                if (value?.Id == -1)
                {
                    _SecondaryCategory = null;
                }
                RaisePropertyChanged();
            }
        }

        public int? SecondaryCategoryId { get; set; }

        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                if (value < From)
                {
                    From = value;
                }
                RaisePropertyChanged();
            }
        }


        private bool? _Reciept;


        public bool? Reciept
        {
            get
            {
                return _Reciept;
            }

            set
            {
                if (_Reciept == value)
                {
                    return;
                }

                _Reciept = value;
                RaisePropertyChanged();
            }
        }


        private Stores _SelectedStore;


        public Stores SelectedStore
        {
            get
            {
                return _SelectedStore;
            }

            set
            {
                if (_SelectedStore == value)
                {
                    return;
                }

                _SelectedStore = value;
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

        #endregion Properties
    }
}