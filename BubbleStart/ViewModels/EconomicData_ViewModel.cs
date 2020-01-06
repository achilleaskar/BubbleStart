using BubbleStart.Helpers;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public class EconomicData_ViewModel : MyViewModelBase
    {
        #region Constructors

        public EconomicData_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            NewExpense = new Expense();
            ShowCashDataCommand = new RelayCommand(async () => { await ShowCashData(); });
            StartDateCash = StartDateExpenses = DateTime.Today;
            Expenses = new ObservableCollection<Expense>();
            RegisterExpenseCommand = new RelayCommand(async () => { await RegisterExpense(); }, CanRegisterExpense);
            ShowExpensesDataCommand = new RelayCommand(async () => { await ShowExpensesData(); });
            DeleteExpenseCommand = new RelayCommand(async () => { await DeleteExpense(); });

        }

        #endregion Constructors

        #region Fields

        private decimal _Cleanse;

        private ObservableCollection<Payment> _DailyPayments;

        private DateTime _EndDateCash;

        private DateTime _EndDateExpenses;

        private ObservableCollection<Expense> _Expenses;

        private decimal _ExpensesSum;

        private Expense _NewExpense;

        private Expense _SelectedExpense;

        private DateTime _StartDateCash;

        private DateTime _StartDateExpenses;

        private decimal _Sum;

        #endregion Fields

        #region Properties

        public BasicDataManager BasicDataManager { get; }

        public decimal Cleanse
        {
            get
            {
                return _Cleanse;
            }

            set
            {
                if (_Cleanse == value)
                {
                    return;
                }

                _Cleanse = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Payment> DailyPayments
        {
            get
            {
                return _DailyPayments;
            }

            set
            {
                if (_DailyPayments == value)
                {
                    return;
                }

                _DailyPayments = value;
                DailyPayments.CollectionChanged += DailyPayments_CollectionChanged;

                RaisePropertyChanged();
            }
        }

        public RelayCommand DeleteExpenseCommand { get; set; }

        public DateTime EndDateCash
        {
            get
            {
                return _EndDateCash;
            }

            set
            {
                if (_EndDateCash == value)
                {
                    return;
                }

                _EndDateCash = value;
                if (_StartDateCash > _EndDateCash)
                {
                    _StartDateCash = EndDateCash;
                }
                RaisePropertyChanged();
            }
        }

        public DateTime EndDateExpenses
        {
            get
            {
                return _EndDateExpenses;
            }

            set
            {
                if (_EndDateExpenses == value)
                {
                    return;
                }

                _EndDateExpenses = value;
                if (StartDateExpenses > value)
                {
                    _StartDateExpenses = value;
                }
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Expense> Expenses
        {
            get
            {
                return _Expenses;
            }

            set
            {
                if (_Expenses == value)
                {
                    return;
                }

                _Expenses = value;
                Expenses.CollectionChanged += Expenses_CollectionChanged;

                RaisePropertyChanged();
            }
        }

        public decimal ExpensesSum
        {
            get
            {
                return _ExpensesSum;
            }

            set
            {
                if (_ExpensesSum == value)
                {
                    return;
                }

                _ExpensesSum = value;
                RaisePropertyChanged();
            }
        }

        public Expense NewExpense
        {
            get
            {
                return _NewExpense;
            }

            set
            {
                if (_NewExpense == value)
                {
                    return;
                }

                _NewExpense = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand RegisterExpenseCommand { get; set; }

        public Expense SelectedExpense
        {
            get
            {
                return _SelectedExpense;
            }

            set
            {
                if (_SelectedExpense == value)
                {
                    return;
                }

                _SelectedExpense = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowCashDataCommand { get; set; }

        public RelayCommand ShowExpensesDataCommand { get; set; }

        public DateTime StartDateCash
        {
            get
            {
                return _StartDateCash;
            }

            set
            {
                if (_StartDateCash == value)
                {
                    return;
                }

                _StartDateCash = value;

                if (_StartDateCash > _EndDateCash)
                {
                    EndDateCash = _StartDateCash;
                }
                RaisePropertyChanged();
            }
        }

        public DateTime StartDateExpenses
        {
            get
            {
                return _StartDateExpenses;
            }

            set
            {
                if (_StartDateExpenses == value)
                {
                    return;
                }

                _StartDateExpenses = value;
                if (EndDateExpenses < value)
                {
                    _EndDateExpenses = value;
                }
                RaisePropertyChanged();
            }
        }

        public decimal Sum
        {
            get
            {
                return _Sum;
            }

            set
            {
                if (_Sum == value)
                {
                    return;
                }

                _Sum = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public void CalculateAmounts()
        {
            Sum = 0;
            if (DailyPayments != null)
            {
                foreach (var item in DailyPayments)
                {
                    Sum += item.Amount;
                }
            }

            ExpensesSum = 0;
            if (Expenses != null)
            {
                foreach (var item in Expenses)
                {
                    ExpensesSum += item.Amount;
                }
            }
            Cleanse = Sum - ExpensesSum+50;
        }

     

        private bool CanRegisterExpense()
        {
            return NewExpense != null && NewExpense.Amount > 0 && !string.IsNullOrEmpty(NewExpense.Reason);
        }

        private void DailyPayments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private async Task DeleteExpense()
        {
            BasicDataManager.Add(new Change($"Διαγράφηκε ΕΞΟΔΟ {SelectedExpense.Amount}€ που είχε περαστεί {SelectedExpense.Date.ToString("ddd dd/MM/yy")} απο τον χρήστη {SelectedExpense.User.UserName}", StaticResources.User));
            BasicDataManager.Delete(SelectedExpense);
            await BasicDataManager.SaveAsync();
            Expenses.Remove(SelectedExpense);
        }

        private void Expenses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private async Task RegisterExpense()
        {
            NewExpense.User = StaticResources.User;
            BasicDataManager.Add(NewExpense);
            await BasicDataManager.SaveAsync();
            if (NewExpense.Date >= StartDateExpenses && NewExpense.Date < EndDateExpenses.AddDays(1))
            {
                Expenses.Add(NewExpense);
            }
            NewExpense = new Expense();
        }

        private async Task ShowCashData()
        {
            DailyPayments = new ObservableCollection<Payment>((await BasicDataManager.Context.GetAllPaymentsAsync(StartDateCash, EndDateCash)).OrderBy(a => a.Date));
            CalculateAmounts();
        }

        private async Task ShowExpensesData()
        {
            DateTime enddate = EndDateExpenses.AddDays(1);
            Expenses = new ObservableCollection<Expense>((await BasicDataManager.Context.GetAllExpensesAsync(e => e.Date >= StartDateExpenses && e.Date < enddate)).OrderBy(a => a.Date));
            NewExpense = new Expense();
            CalculateAmounts();
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
      
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}