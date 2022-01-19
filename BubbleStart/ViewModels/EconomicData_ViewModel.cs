using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using DocumentFormat.OpenXml.Wordprocessing;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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
            ShowOwningCustomersCommand = new RelayCommand(ShowOwningCustomers);
            StartDateCash = StartDateExpenses = StartDatePreview = StartDatePayments = DateTime.Today;
            Expenses = new ObservableCollection<Expense>();
            RegisterExpenseCommand = new RelayCommand(async () => { await RegisterExpense(); }, CanRegisterExpense);
            SaveChangesCommand = new RelayCommand(async () => { await SaveChanges(); }, CanSaveChanges);
            ShowExpensesDataCommand = new RelayCommand(async () => { await ShowExpensesData(); });
            ShowPreviewDataCommand = new RelayCommand(async () => { await ShowPreviewData(); });
            ShowPaymentsDataCommand = new RelayCommand(async () => { await ShowPaymentsData(); });
            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });

            DeleteExpenseCommand = new RelayCommand(async () => { await DeleteExpense(); });
            Messenger.Default.Register<UpdateExpenseCategoriesMessage>(this, msg => Load());
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());

            Load();
        }

        #endregion Constructors

        #region Fields

        private decimal _Cleanse;

        private ObservableCollection<Customer> _CustomersWhoOwn;

        private ObservableCollection<Payment> _DailyPayments;

        private decimal _Ektakta;

        private DateTime _EndDateCash;

        private DateTime _EndDateExpenses;

        private DateTime _EndDatePayments;

        private DateTime _EndDatePreview;

        private ObservableCollection<Expense> _Expenses;

        private decimal _ExpensesSum;

        private string _ExpensesTextFilter;

        private ObservableCollection<ExpenseCheck> _ExpenseTypes;

        private decimal _Fainomenika;

        private decimal _Gwgw;

        private ObservableCollection<ExpenseCategoryClass> _MainCategories;

        private decimal _Misthoi;

        private Expense _NewExpense;

        private decimal _Pagia;

        private ObservableCollection<Payment> _Payments;

        private ICollectionView _PaymentsCV;

        private decimal _Pistotika;

        private ObservableCollection<PreviewData> _Preview;

        private ObservableCollection<PreviewData> _RealPreview;

        private int _RecieptIndex;

        private ObservableCollection<ExpenseCategoryClass> _SecondaryCategories;

        private Customer _SelectedCustomer;

        private Expense _SelectedExpense;

        private int _SelectedPaymentMethodIndexIndex;

        private decimal _Spitiou;

        private DateTime _StartDateCash;

        private DateTime _StartDateExpenses;

        private DateTime _StartDatePayments;

        private DateTime _StartDatePreview;

        private decimal _Sum;

        private decimal _Timologia;

        private decimal _TotalRemaining;

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

        public ObservableCollection<Customer> CustomersWhoOwn
        {
            get
            {
                return _CustomersWhoOwn;
            }

            set
            {
                if (_CustomersWhoOwn == value)
                {
                    return;
                }

                _CustomersWhoOwn = value;
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
                if (Expenses != null)
                {
                    CollectionViewSource.GetDefaultView(Expenses).Refresh();
                    UpdateAmmounts();
                }
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ExpenseCheck> ExpenseTypes
        {
            get
            {
                return _ExpenseTypes;
            }

            set
            {
                if (_ExpenseTypes == value)
                {
                    return;
                }

                _ExpenseTypes = value;
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

        public ObservableCollection<ExpenseCategoryClass> MainCategories
        {
            get
            {
                return _MainCategories;
            }

            set
            {
                if (_MainCategories == value)
                {
                    return;
                }

                _MainCategories = value;
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

        public RelayCommand OpenCustomerManagementCommand { get; set; }

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

        public ObservableCollection<PreviewData> RealPreview
        {
            get
            {
                return _RealPreview;
            }

            set
            {
                if (_RealPreview == value)
                {
                    return;
                }

                _RealPreview = value;
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

        public ObservableCollection<ExpenseCategoryClass> SecondaryCategories
        {
            get
            {
                return _SecondaryCategories;
            }

            set
            {
                if (_SecondaryCategories == value)
                {
                    return;
                }

                _SecondaryCategories = value;
                RaisePropertyChanged();
            }
        }

        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;

            set
            {
                if (_SelectedCustomer == value)
                {
                    return;
                }
                if (_SelectedCustomer != null)
                {
                    _SelectedCustomer.IsSelected = false;
                }
                _SelectedCustomer = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Enabled));
            }
        }

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

        public RelayCommand ShowOwningCustomersCommand { get; set; }

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
                    EndDateExpenses = value;
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

        public decimal TotalPayments => GetSum();

        public decimal TotalRemaining
        {
            get
            {
                return _TotalRemaining;
            }

            set
            {
                if (_TotalRemaining == value)
                {
                    return;
                }

                _TotalRemaining = value;
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
                foreach (Expense item in CollectionViewSource.GetDefaultView(Expenses))
                {
                    ExpensesSum += item.Amount;
                }
            }
            Cleanse = Sum - ExpensesSum + 50;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            if (BasicDataManager.ExpenseCategoryClasses != null)
            {
                MainCategories = new ObservableCollection<ExpenseCategoryClass>(BasicDataManager.ExpenseCategoryClasses.Where(e => e.Id > 1 && (e.ParentId == 1 || e.Parent == null)));
                SecondaryCategories = new ObservableCollection<ExpenseCategoryClass>(BasicDataManager.ExpenseCategoryClasses.Where(e => e.ParentId > 1));
                ExpenseTypes = new ObservableCollection<ExpenseCheck>(MainCategories.Select(e => new ExpenseCheck { ExpenseCategory = e, IsChecked = true }));
                foreach (var e in ExpenseTypes)
                {
                    e.PropertyChanged += E_PropertyChanged;
                }
            }
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public void UpdateAmmounts()
        {
            Pagia = Ektakta = Spitiou = Gwgw = Misthoi = Timologia = Pistotika = Fainomenika = 0;
            foreach (Expense exp in CollectionViewSource.GetDefaultView(Expenses))
            {
                switch (exp.MainCategory?.Id)
                {
                    case 2:
                        Pagia += exp.Amount;
                        break;

                    case 3:
                        Misthoi += exp.Amount;
                        break;

                    case 4:
                        Ektakta += exp.Amount;
                        break;

                    case 19:
                        Spitiou += exp.Amount;
                        break;

                    case 17:
                        Gwgw += exp.Amount;
                        break;

                    case 6:
                        Timologia += exp.Amount;
                        break;

                    case 7:
                        Pistotika += exp.Amount;
                        break;

                    case 8:
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
            UpdateAmmounts();
        }

        private async Task DeleteExpense()
        {
            BasicDataManager.Add(new Change($"Διαγράφηκε ΕΞΟΔΟ {SelectedExpense.Amount}€ που είχε περαστεί {SelectedExpense.Date.ToString("ddd dd/MM/yy")} απο τον χρήστη {SelectedExpense.User.UserName}", StaticResources.User));
            BasicDataManager.Delete(SelectedExpense);
            await BasicDataManager.SaveAsync();
            Expenses.Remove(SelectedExpense);
            UpdateAmmounts();
        }

        private void E_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ExpenseCheck.IsChecked))
            {
                CollectionViewSource.GetDefaultView(Expenses).Refresh();
                UpdateAmmounts();
            }
        }

        private void Expenses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateAmmounts();
        }

        private bool ExpensesFilter(object obj)
        {
            if (!(obj is Expense e) || !ExpenseTypes.Any(ex => ex.ExpenseCategory == e.MainCategory && ex.IsChecked))
            {
                return false;
            }
            if (string.IsNullOrEmpty(ExpensesTextFilter))
                return true;
            return (e.Reason.Contains(ExpensesTextFilter.ToUpperInvariant()) || e.Reason.Contains(StaticResources.ToGreek(ExpensesTextFilter.ToUpperInvariant())));
        }

        private decimal GetAmountInMonth(PreviewData mon, Expense expense)
        {
            if (expense == null || mon == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            if (expense.From.Date == expense.To.Date && expense.From.Month == mon.Date.Month)
            {
                return expense.Amount;
            }
            var addedMonth = mon.Date.AddMonths(1);
            if (expense.From.Date >= mon.Date && expense.To.Date < addedMonth)
            {
                return expense.Amount;
            }
            if (expense.From.Date >= mon.Date && expense.From.Date < addedMonth && expense.To.Date >= addedMonth)
            {
                var x = expense.Amount / ((expense.To.Date - expense.From.Date).Days + 1) * (addedMonth - expense.From.Date).Days;
                return x;
            }
            if (expense.To.Date >= mon.Date && expense.To.Date < addedMonth && expense.From.Date < mon.Date)
            {
                return expense.Amount / ((expense.To.Date - expense.From.Date).Days + 1) * ((expense.To.Date - mon.Date).Days + 1);
            }
            throw new InvalidOperationException(nameof(expense));
        }

        private decimal GetProgramAmountInMonth(PreviewData mon, Program program)
        {
            if (program == null || mon == null)
            {
                throw new ArgumentNullException(nameof(program));
            }
            return program.ShowUpPrice * program.ShowUpsList.Where(s => s.Arrived >= mon.Date && s.Arrived < mon.Date.AddMonths(1)).Count();

            throw new InvalidOperationException(nameof(program));
        }

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

        private void OpenCustomerManagement(Customer c)
        {
            if (c != null)
            {
                c.EditedInCustomerManagement = true;
                c.BasicDataManager = BasicDataManager;
                c.UpdateCollections();
                Window window = new CustomerManagement
                {
                    DataContext = c
                };
                Messenger.Default.Send(new OpenChildWindowCommand(window));
            }
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
            Mouse.OverrideCursor = Cursors.Wait;

            NewExpense.User = StaticResources.User;
            if ((await BasicDataManager.Context.Context.Expenses.AnyAsync(e => e.MainCategoryId == NewExpense.MainCategoryId && e.Amount == NewExpense.Amount &&
            e.From.Day == NewExpense.From.Day && e.From.Month == NewExpense.From.Month && e.From.Year == NewExpense.From.Year &&
            e.To.Day == NewExpense.To.Day && e.To.Month == NewExpense.To.Month && e.To.Year == NewExpense.To.Year)))
            {
                if (MessageBox.Show("Υπάρχει ήδη καταχώρηση με ίδιες ημερομηνίες, κατηγορία και ποσό. Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή!", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    return;
                }
            }
            BasicDataManager.Add(NewExpense);
            await BasicDataManager.SaveAsync();
            if (NewExpense.Date >= StartDateExpenses && NewExpense.Date < EndDateExpenses.AddDays(1))
            {
                Expenses.Add(NewExpense);
            }
            NewExpense = new Expense();
            UpdateAmmounts();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task SaveChanges()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ShowCashData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DailyPayments = new ObservableCollection<Payment>((await BasicDataManager.Context.GetAllPaymentsAsync(StartDateCash, EndDateCash)).OrderBy(a => a.Date));
            UpdateAmmounts();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ShowExpensesData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DateTime enddate = EndDateExpenses.AddDays(1);
            Expenses = new ObservableCollection<Expense>((await BasicDataManager.Context.GetAllExpensesAsync(e => e.Date >= StartDateExpenses && e.Date < enddate)).OrderBy(a => a.Date));
            NewExpense = new Expense();

            UpdateAmmounts();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ShowOwningCustomers()
        {
            CustomersWhoOwn = new ObservableCollection<Customer>(BasicDataManager.Customers.Where(c => c.RemainingAmount > 0));
            TotalRemaining = CustomersWhoOwn.Sum(t => t.RemainingAmount);
        }

        private async Task ShowPaymentsData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Payments = new ObservableCollection<Payment>(await BasicDataManager.Context.GetAllPaymentsAsync(StartDatePayments, EndDatePayments, false));
            RaisePropertyChanged(nameof(TotalPayments));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ShowPreviewData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<PreviewData> previewData = new List<PreviewData>();
            List<Expense> expenses = await BasicDataManager.Context.GetAllExpensesAsync(e => e.Date >= StartDatePreview && e.Date <= EndDatePreview, false, ExpenseTypes.Where(e => e.IsChecked).Select(e => e.ExpenseCategory).ToList());
            List<Payment> payments = (await BasicDataManager.Context.GetAllPaymentsAsync(StartDatePreview, EndDatePreview, false)).ToList();
            List<Program> programs = (await BasicDataManager.Context.GetAllAsync<Program>(p => p.DayOfIssue >= StartDatePreview && p.DayOfIssue <= EndDatePreview)).ToList();

            List<PreviewData> realpreviewData = new List<PreviewData>();
            List<Expense> expensesPreview = await BasicDataManager.Context.GetAllExpensesAsync(e => (e.To >= StartDatePreview && e.To <= EndDatePreview) || (e.From >= StartDatePreview && e.From <= EndDatePreview), false, ExpenseTypes.Where(e => e.IsChecked).Select(e => e.ExpenseCategory).ToList());
            List<Program> programsPreview = (await BasicDataManager.Context.GetProgramsFullAsync(p => p.ShowUpsList.Any(s => s.Arrived >= StartDatePreview && s.Arrived <= EndDatePreview))).ToList();

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

            //real
            var firstmonth = new DateTime(StartDatePreview.Year, StartDatePreview.Month, 1);
            var lastMonth = new DateTime(EndDatePreview.Year, EndDatePreview.Month, 1);
            while (firstmonth <= lastMonth)
            {
                realpreviewData.Add(new PreviewData { Date = firstmonth });
                firstmonth = firstmonth.AddMonths(1);
            }
            IEnumerable<PreviewData> months = new List<PreviewData>();
            foreach (var expense in expensesPreview)
            {
                months = realpreviewData.Where(m => (expense.To >= m.Date && expense.To < m.Date.AddMonths(1)) || (expense.From >= m.Date && expense.From < m.Date.AddMonths(1)));
                if (months?.Count() > 0)
                {
                    foreach (var mon in months)
                    {
                        mon.Expenses += GetAmountInMonth(mon, expense);
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            months = new List<PreviewData>();
            DateTime firstProgMonth;
            DateTime LastProgMonth;
            foreach (var program in programsPreview)
            {
                program.ShowUpsList = program.ShowUpsList.OrderBy(s => s.Arrived).ToList();
                if (!program.ShowUpsList.Any())
                {
                    continue;
                }
                firstProgMonth = new DateTime(program.ShowUpsList.First().Arrived.Year, program.ShowUpsList.First().Arrived.Month, 1);
                LastProgMonth = new DateTime(program.ShowUpsList.Last().Arrived.Year, program.ShowUpsList.Last().Arrived.Month, 1);
                months = realpreviewData.Where(m => m.Date >= firstProgMonth && m.Date <= LastProgMonth);
                if (months.Count() > 0)
                    foreach (var mon in months)
                    {
                        mon.Recieved += GetProgramAmountInMonth(mon, program);
                    }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            Preview = new ObservableCollection<PreviewData>(previewData.OrderBy(r => r.Date));
            RealPreview = new ObservableCollection<PreviewData>(realpreviewData.OrderBy(r => r.Date));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }

    public class ExpenseCheck : ObservableObject
    {
        #region Fields

        private ExpenseCategoryClass _ExpenseCategory;
        private bool _IsChecked;

        #endregion Fields

        #region Properties

        public ExpenseCategoryClass ExpenseCategory
        {
            get
            {
                return _ExpenseCategory;
            }

            set
            {
                if (_ExpenseCategory == value)
                {
                    return;
                }

                _ExpenseCategory = value;
                RaisePropertyChanged();
            }
        }

        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }

            set
            {
                if (_IsChecked == value)
                {
                    return;
                }

                _IsChecked = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
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