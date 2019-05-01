using BubbleStart.Database;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
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
        private float _Sum;

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

        public float Sum
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
            Context.Add(NewExpense);
            await Context.SaveAsync();
        }

        private async Task ShowCashData()
        {
            Sum = 0;
            DailyPayments = new ObservableCollection<Payment>(await Context.GetAllPaymentsAsync(StartDateCash, EndDateCash));
            foreach (var p in DailyPayments)
            {
                Sum += p.Amount;
            }
        }

        private async Task ShowExpensesData()
        {
            DateTime enddate = EndDateExpenses.AddDays(1);
            Expenses = new ObservableCollection<Expense>(await Context.GetAllAsync<Expense>(e => e.Date >= StartDateExpenses && e.Date < enddate));
            NewExpense = new Expense();
        }

        #endregion Methods
    }
}