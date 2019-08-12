using BubbleStart.Database;
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

        public EconomicData_ViewModel(GenericRepository context)
        {
            NewExpense = new Expense();
            Context = context;
            ShowCashDataCommand = new RelayCommand(async () => { await ShowCashData(); });
            StartDateCash = StartDateExpenses = DateTime.Today;
            Expenses = new ObservableCollection<Expense>();
            RegisterExpenseCommand = new RelayCommand(async () => { await RegisterExpense(); }, CanRegisterExpense);
            ShowExpensesDataCommand = new RelayCommand(async () => { await ShowExpensesData(); });
            DeleteExpenseCommand = new RelayCommand(async () => { await DeleteExpense(); });
        }

        private void DailyPayments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateAmounts();

        }

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
            Cleanse = Sum - ExpensesSum;
        }

        private void Expenses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateAmounts();
        }

        

        private Expense _SelectedExpense;


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
        private async Task DeleteExpense()
        {
            Context.Add(new Change($"Διαγράφηκε ΕΞΟΔΟ {SelectedExpense.Amount}€ που είχε περαστεί {SelectedExpense.Date.ToString("ddd dd/MM/yy")} απο τον χρήστη {SelectedExpense.User.UserName}", Context.GetById<User>(Helpers.StaticResources.User.Id)));
            Context.Delete(SelectedExpense);
            await Context.SaveAsync();
            Expenses.Remove(SelectedExpense);
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Payment> _DailyPayments;
        private DateTime _EndDateCash;
        private DateTime _EndDateExpenses;
        private ObservableCollection<Expense> _Expenses;
        private Expense _NewExpense;
        private DateTime _StartDateCash;
        private DateTime _StartDateExpenses;
        private decimal _Sum;

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; }

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

        public RelayCommand DeleteExpenseCommand { get; set; }

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




        private decimal _Cleanse;


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


        private decimal _ExpensesSum;


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

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            await Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private bool CanRegisterExpense()
        {
            return NewExpense != null && NewExpense.Amount > 0 && !string.IsNullOrEmpty(NewExpense.Reason);
        }

        private async Task RegisterExpense()
        {
            NewExpense.User = Context.GetById<User>(Helpers.StaticResources.User.Id);
            Context.Add(NewExpense);
            await Context.SaveAsync();
            if (NewExpense.Date >= StartDateExpenses && NewExpense.Date < EndDateExpenses.AddDays(1))
            {
                Expenses.Add(NewExpense);
            }
            NewExpense = new Expense();

        }

        private async Task ShowCashData()
        {
           
            DailyPayments = new ObservableCollection<Payment>((await Context.GetAllPaymentsAsync(StartDateCash, EndDateCash)).OrderBy(a=>a.Date));
            CalculateAmounts();

        }

        private async Task ShowExpensesData()
        {
            DateTime enddate = EndDateExpenses.AddDays(1);
            Expenses = new ObservableCollection<Expense>((await Context.GetAllExpensesAsync(e => e.Date >= StartDateExpenses && e.Date < enddate)).OrderBy(a => a.Date));
            NewExpense = new Expense();
            CalculateAmounts();

        }

        #endregion Methods
    }
}