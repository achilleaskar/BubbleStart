using BubbleStart.Helpers;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class EconomicData_ViewModel : MyViewModelBase
    {

        #region Constructors



        private string _ExpensesTextFilter;


        public string ExpensesTextFilter
        {
            get
            {
                return _ExpensesTextFilter;
            }

            set
            {
                if (_ExpensesTextFilter == value)
                {
                    return;
                }

                _ExpensesTextFilter = value;
                if (Expenses!=null)
                {
                    CollectionViewSource.GetDefaultView(Expenses).Refresh();
                }
                RaisePropertyChanged();
            }
        }
        public EconomicData_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            NewExpense = new Expense();
            ShowCashDataCommand = new RelayCommand(async () => { await ShowCashData(); });
            StartDateCash = StartDateExpenses = StartDatePreview = StartDatePayments = DateTime.Today;
            Expenses = new ObservableCollection<Expense>();
            RegisterExpenseCommand = new RelayCommand(async () => { await RegisterExpense(); }, CanRegisterExpense);
            SaveChangesCommand = new RelayCommand(async () => { await SaveChanges(); }, CanSaveChanges);
            ShowExpensesDataCommand = new RelayCommand(async () => { await ShowExpensesData(); });
            ShowPreviewDataCommand = new RelayCommand(async () => { await ShowPreviewData(); });
            ShowPaymentsDataCommand = new RelayCommand(async () => { await ShowPaymentsData(); });
            DeleteExpenseCommand = new RelayCommand(async () => { await DeleteExpense(); });
        }

        public decimal TotalPayments => GetSum();

        private decimal GetSum()
        {
            decimal sum = 0;
            if (PaymentsCV != null)
            {
                foreach (Payment p in PaymentsCV)
                {
                    sum += p.Amount;
                }
            }
            return sum;
        }

        #endregion Constructors

        #region Fields

        private decimal _Cleanse;
        private ObservableCollection<Payment> _DailyPayments;
        private decimal _Ektakta;
        private DateTime _EndDateCash;
        private DateTime _EndDateExpenses;
        private DateTime _EndDatePayments;

        private DateTime _EndDatePreview;

        private ObservableCollection<Expense> _Expenses;

        private decimal _ExpensesSum;

        private decimal _Fainomenika;

        private decimal _Gwgw;

        private decimal _Misthoi;

        private Expense _NewExpense;

        private decimal _Pagia;

        private ObservableCollection<Payment> _Payments;

        private ICollectionView _PaymentsCV;

        private decimal _Pistotika;

        private ObservableCollection<PreviewData> _Preview;

        private int _RecieptIndex;

        private Expense _SelectedExpense;

        private int _SelectedPaymentMethodIndexIndex;

        private decimal _Spitiou;

        private DateTime _StartDateCash;

        private DateTime _StartDateExpenses;

        private DateTime _StartDatePayments;

        private DateTime _StartDatePreview;

        private decimal _Sum;

        private decimal _Timologia;

        #endregion Fields

        #region Properties

        public BasicDataManager BasicDataManager { get; }

        public decimal Cleanse
        {
            get => _Cleanse;

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
            get => _DailyPayments;

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

        public decimal Ektakta
        {
            get
            {
                return _Ektakta;
            }

            set
            {
                if (_Ektakta == value)
                {
                    return;
                }

                _Ektakta = value;
                RaisePropertyChanged();
            }
        }

        public DateTime EndDateCash
        {
            get => _EndDateCash;

            set
            {
                if (_EndDateCash == value)
                {
                    return;
                }

                _EndDateCash = value;
                if (_StartDateCash > _EndDateCash)
                {
                    StartDateCash = EndDateCash;
                }
                RaisePropertyChanged();
            }
        }

        public DateTime EndDateExpenses
        {
            get => _EndDateExpenses;

            set
            {
                if (_EndDateExpenses == value)
                {
                    return;
                }

                _EndDateExpenses = value;
                if (StartDateExpenses > value)
                {
                    StartDateExpenses = value;
                }
                RaisePropertyChanged();
            }
        }

        public DateTime EndDatePayments
        {
            get
            {
                return _EndDatePayments;
            }

            set
            {
                if (_EndDatePayments == value)
                {
                    return;
                }

                _EndDatePayments = value;
                if (value < StartDatePayments)
                {
                    StartDatePayments = value;
                }
                RaisePropertyChanged();
            }
        }
        public DateTime EndDatePreview
        {
            get
            {
                return _EndDatePreview;
            }

            set
            {
                if (_EndDatePreview == value)
                {
                    return;
                }

                _EndDatePreview = value;
                if (StartDatePreview > value)
                {
                    StartDatePreview = value;
                }

                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Expense> Expenses
        {
            get => _Expenses;

            set
            {
                if (_Expenses == value)
                {
                    return;
                }

                _Expenses = value;
                Expenses.CollectionChanged += Expenses_CollectionChanged;
                if (value != null)
                    CollectionViewSource.GetDefaultView(value).Filter = ExpensesFilter;
                RaisePropertyChanged();
            }
        }

        private bool ExpensesFilter(object obj)
        {
            if (string.IsNullOrEmpty(ExpensesTextFilter))
                return true;
            return obj is Expense e && e.Reason.ToUpperInvariant().Contains(ExpensesTextFilter.ToUpperInvariant());
        }

        public decimal ExpensesSum
        {
            get => _ExpensesSum;

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

        public decimal Fainomenika
        {
            get
            {
                return _Fainomenika;
            }

            set
            {
                if (_Fainomenika == value)
                {
                    return;
                }

                _Fainomenika = value;
                RaisePropertyChanged();
            }
        }

        public decimal Gwgw
        {
            get
            {
                return _Gwgw;
            }

            set
            {
                if (_Gwgw == value)
                {
                    return;
                }

                _Gwgw = value;
                RaisePropertyChanged();
            }
        }

        public decimal Misthoi
        {
            get
            {
                return _Misthoi;
            }

            set
            {
                if (_Misthoi == value)
                {
                    return;
                }

                _Misthoi = value;
                RaisePropertyChanged();
            }
        }

        public Expense NewExpense
        {
            get => _NewExpense;

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

        public decimal Pagia
        {
            get
            {
                return _Pagia;
            }

            set
            {
                if (_Pagia == value)
                {
                    return;
                }

                _Pagia = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Payment> Payments
        {
            get
            {
                return _Payments;
            }

            set
            {
                if (_Payments == value)
                {
                    return;
                }

                _Payments = value;
                if (value != null)
                {
                    PaymentsCV = CollectionViewSource.GetDefaultView(value);
                    PaymentsCV.Filter = PaymentsFilter;
                    PaymentsCV.Refresh();
                }
                RaisePropertyChanged();
            }
        }

        public ICollectionView PaymentsCV
        {
            get
            {
                return _PaymentsCV;
            }

            set
            {
                if (_PaymentsCV == value)
                {
                    return;
                }

                _PaymentsCV = value;
                RaisePropertyChanged();
            }
        }

        public decimal Pistotika
        {
            get
            {
                return _Pistotika;
            }

            set
            {
                if (_Pistotika == value)
                {
                    return;
                }

                _Pistotika = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<PreviewData> Preview
        {
            get
            {
                return _Preview;
            }

            set
            {
                if (_Preview == value)
                {
                    return;
                }

                _Preview = value;
                RaisePropertyChanged();
            }
        }

        public int RecieptIndex
        {
            get
            {
                return _RecieptIndex;
            }

            set
            {
                if (_RecieptIndex == value)
                {
                    return;
                }

                _RecieptIndex = value;
                PaymentsCV.Refresh();
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalPayments));
            }
        }

        public RelayCommand RegisterExpenseCommand { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }

        public Expense SelectedExpense
        {
            get => _SelectedExpense;

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

        public int SelectedPaymentMethodIndexIndex
        {
            get
            {
                return _SelectedPaymentMethodIndexIndex;
            }

            set
            {
                if (_SelectedPaymentMethodIndexIndex == value)
                {
                    return;
                }

                _SelectedPaymentMethodIndexIndex = value;
                PaymentsCV.Refresh();
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalPayments));

            }
        }

        public RelayCommand ShowCashDataCommand { get; set; }

        public RelayCommand ShowExpensesDataCommand { get; set; }

        public RelayCommand ShowPaymentsDataCommand { get; set; }

        public RelayCommand ShowPreviewDataCommand { get; set; }

        public decimal Spitiou
        {
            get
            {
                return _Spitiou;
            }

            set
            {
                if (_Spitiou == value)
                {
                    return;
                }

                _Spitiou = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDateCash
        {
            get => _StartDateCash;

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
            get => _StartDateExpenses;

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

        public DateTime StartDatePayments
        {
            get
            {
                return _StartDatePayments;
            }

            set
            {
                if (_StartDatePayments == value)
                {
                    return;
                }

                _StartDatePayments = value;
                if (value > EndDatePayments)
                {
                    EndDatePayments = value;
                }
                RaisePropertyChanged();
            }
        }
        public DateTime StartDatePreview
        {
            get
            {
                return _StartDatePreview;
            }

            set
            {
                if (_StartDatePreview == value)
                {
                    return;
                }

                _StartDatePreview = value;

                if (value > _EndDatePreview)
                {
                    EndDatePreview = value;
                }
                RaisePropertyChanged();
            }
        }

        public decimal Sum
        {
            get => _Sum;

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

        public decimal Timologia
        {
            get
            {
                return _Timologia;
            }

            set
            {
                if (_Timologia == value)
                {
                    return;
                }

                _Timologia = value;
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
            Cleanse = Sum - ExpensesSum + 50;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public void UpdateAmmounts()
        {
            Pagia = Ektakta = Spitiou = Gwgw = Misthoi = Timologia = Pistotika = Fainomenika = 0;
            foreach (var exp in Expenses)
            {
                switch (exp.ExpenseCategory)
                {
                    case ExpenseCategory.pagia:
                        Pagia += exp.Amount;
                        break;

                    case ExpenseCategory.misthoi:
                        Misthoi += exp.Amount;
                        break;

                    case ExpenseCategory.ektakta:
                        Ektakta += exp.Amount;
                        break;

                    case ExpenseCategory.spitiou:
                        Spitiou += exp.Amount;
                        break;

                    case ExpenseCategory.gwgw:
                        Gwgw += exp.Amount;
                        break;

                    case ExpenseCategory.timologia:
                        Timologia += exp.Amount;
                        break;

                    case ExpenseCategory.pistotika:
                        Pistotika += exp.Amount;
                        break;

                    case ExpenseCategory.fainomenika:
                        Fainomenika += exp.Amount;
                        break;

                    default:
                        break;
                }
            }

            CalculateAmounts();
        }

        private bool CanRegisterExpense()
        {
            return NewExpense != null && NewExpense.Amount > 0 && !string.IsNullOrEmpty(NewExpense.Reason);
        }

        private bool CanSaveChanges()
        {
            return BasicDataManager.HasChanges();
        }

        private void DailyPayments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private async Task DeleteExpense()
        {
            BasicDataManager.Add(new Change($"Διαγράφηκε ΕΞΟΔΟ {SelectedExpense.Amount}€ που είχε περαστεί {SelectedExpense.Date.ToString("ddd dd/MM/yy")} απο τον χρήστη {SelectedExpense.User.UserName}", StaticResources.User));
            BasicDataManager.Delete(SelectedExpense);
            await BasicDataManager.SaveAsync();
            Expenses.Remove(SelectedExpense);
            UpdateAmmounts();
        }

        private void Expenses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private bool PaymentsFilter(object obj)
        {
            return obj is Payment p &&
                (SelectedPaymentMethodIndexIndex == 0 ||
                    (SelectedPaymentMethodIndexIndex == 1 && p.PaymentType == PaymentType.Cash) ||
                    (SelectedPaymentMethodIndexIndex == 2 && p.PaymentType == PaymentType.Visa) ||
                    (SelectedPaymentMethodIndexIndex == 3 && p.PaymentType == PaymentType.Bank))
                && (RecieptIndex == 0 || (RecieptIndex == 1 && p.Reciept) || (RecieptIndex == 2 && !p.Reciept));
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
            UpdateAmmounts();
        }

        private async Task SaveChanges()
        {
            await BasicDataManager.SaveAsync();
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

            UpdateAmmounts();
        }

        private async Task ShowPaymentsData()
        {
            Payments = new ObservableCollection<Payment>(await BasicDataManager.Context.GetAllPaymentsAsync(StartDatePayments, EndDatePayments, false));
            RaisePropertyChanged(nameof(TotalPayments));
        }

        private async Task ShowPreviewData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<PreviewData> previewData = new List<PreviewData>();
            List<Expense> expenses = await BasicDataManager.Context.GetAllExpensesAsync(e => e.Date >= StartDatePreview && e.Date <= EndDatePreview, false);
            List<Payment> payments = (await BasicDataManager.Context.GetAllPaymentsAsync(StartDatePreview, EndDatePreview, false)).ToList();
            List<Program> programs = (await BasicDataManager.Context.GetAllAsync<Program>(p => p.DayOfIssue >= StartDatePreview && p.DayOfIssue <= EndDatePreview)).ToList();

            PreviewData tmpMonth;
            foreach (var expense in expenses)
            {
                tmpMonth = previewData.FirstOrDefault(m => m.Date.Month == expense.Date.Month && m.Date.Year == expense.Date.Year);
                if (tmpMonth != null)
                    tmpMonth.Expenses += expense.Amount;
                else
                {
                    previewData.Add(new PreviewData { Date = new DateTime(expense.Date.Year, expense.Date.Month, 1), Expenses = expense.Amount });
                }
            }

            foreach (var payment in payments)
            {
                tmpMonth = previewData.FirstOrDefault(m => m.Date.Month == payment.Date.Month && m.Date.Year == payment.Date.Year);
                if (tmpMonth != null)
                    tmpMonth.Recieved += payment.Amount;
                else
                {
                    previewData.Add(new PreviewData { Date = new DateTime(payment.Date.Year, payment.Date.Month, 1), Expenses = payment.Amount });
                }
            }

            foreach (var program in programs)
            {
                tmpMonth = previewData.FirstOrDefault(m => m.Date.Month == program.DayOfIssue.Month && m.Date.Year == program.DayOfIssue.Year);
                if (tmpMonth != null)
                    tmpMonth.Sold += program.Amount;
                else
                {
                    previewData.Add(new PreviewData { Date = new DateTime(program.DayOfIssue.Year, program.DayOfIssue.Month, 1), Expenses = program.Amount });
                }
            }

            Preview = new ObservableCollection<PreviewData>(previewData.OrderBy(r => r.Date));

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }

    public class PreviewData : BaseModel
    {
        #region Fields

        private decimal _Expenses;
        private DateTime _Month;

        private decimal _Recieved;

        private decimal _Sold;

        #endregion Fields

        #region Properties

        public DateTime Date
        {
            get
            {
                return _Month;
            }

            set
            {
                if (_Month == value)
                {
                    return;
                }

                _Month = value;
                RaisePropertyChanged();
            }
        }
        public decimal Expenses
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

        public decimal Profit => Recieved - Expenses;

        public decimal Recieved
        {
            get
            {
                return _Recieved;
            }

            set
            {
                if (_Recieved == value)
                {
                    return;
                }

                _Recieved = value;
                RaisePropertyChanged();
            }
        }
        public decimal Sold
        {
            get
            {
                return _Sold;
            }

            set
            {
                if (_Sold == value)
                {
                    return;
                }

                _Sold = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}