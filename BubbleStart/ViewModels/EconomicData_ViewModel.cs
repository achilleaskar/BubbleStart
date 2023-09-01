using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
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

        private decimal _Total;

        public decimal Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged();
            }
        }

        private int _Days;

        public int Days
        {
            get
            {
                return _Days;
            }

            set
            {
                if (_Days == value)
                {
                    return;
                }

                _Days = value;
                RaisePropertyChanged();
            }
        }

        public EconomicData_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            NewExpense = new Expense();
            Bars = new ObservableCollection<Bar>();

            NewIncome = new Expense { Income = true, MainCategoryId = 20 };
            ShowCashDataCommand = new RelayCommand(async () => { await ShowCashData(); });//
            ShowExpensesGroupedDataCommand = new RelayCommand(async () => { await ShowExpensesGroupedData(); });//
            ShowExpireShowUpsCommand = new RelayCommand(ShowExpireShowUps);//
            ShowExpireDaysCommand = new RelayCommand(async () => { await ShowExpireDays(); });//
            ShowOwningCustomersCommand = new RelayCommand(ShowOwningCustomers);
            StartDateCash = StartDateExpenses = StartDatePreview = StartDatePayments = DateTime.Today;
            Expenses = new ObservableCollection<Expense>();
            Incomes = new ObservableCollection<Expense>();
            RegisterExpenseCommand = new RelayCommand(async () => { await RegisterExpense(); }, CanRegisterExpense);
            RegisterIncomeCommand = new RelayCommand(async () => { await RegisterIncome(); }, CanRegisterIncome);
            SaveChangesCommand = new RelayCommand(async () => { await SaveChanges(); }, CanSaveChanges);
            SelectAllCommand = new RelayCommand(SelectAll);
            SelectAllIncomeCommand = new RelayCommand(SelectAllIncome);
            ShowEconomicDetailsCommand = new RelayCommand<ExpenseCheck>(ShowEconomicDetails);
            ShowExpensesDataCommand = new RelayCommand(async () => { await ShowExpensesData(); });//
            FindAndReplaceCommand = new RelayCommand(async () => { await FindAndReplace(); });
            ShowIncomesDataCommand = new RelayCommand(async () => { await ShowIncomesData(); });//
            ShowPreviewDataCommand = new RelayCommand(async () => { await ShowPreviewData(); });
            ShowPaymentsDataCommand = new RelayCommand(async () => { await ShowPaymentsData(); });
            FullyLoadCustomerCommand = new RelayCommand<Customer>(async (c) => { await FullyLoadCustomer(c); });
            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });

            ShowSalesDataCommand = new RelayCommand(async () => { await ShowSalesData(); });
            CustomersExpire = new ObservableCollection<Customer>();
            DeleteExpenseCommand = new RelayCommand(async () => { await DeleteExpense(); });
            DeleteIncomeCommand = new RelayCommand(async () => { await DeleteIncome(); });
            Messenger.Default.Register<UpdateExpenseCategoriesMessage>(this, msg => Load());
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
            Sales = new ObservableCollection<Program>();
            StartDateSales = DateTime.Today;
            Load();
        }

        private void ShowExpireShowUps()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var date = DateTime.Today.AddDays(-Days);
            CustomersExpireInShowUps = new ObservableCollection<Customer>(BasicDataManager.Customers.Where(c => c.Enabled && c.RemainingDaysTotal < ShowUps));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ShowExpireDays()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var date = DateTime.Today.AddDays(-Days);
            CustomersExpire = new ObservableCollection<Customer>(await BasicDataManager.Context.Context.Customers.Where(c => c.Enabled && !c.ShowUps.Any(s => s.Arrived >= date)).Include(r => r.ShowUps).ToListAsync());
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private ObservableCollection<IGrouping<ExpenseCategoryClass, Expense>> _ExpensesGrouped;

        public ObservableCollection<IGrouping<ExpenseCategoryClass, Expense>> ExpensesGrouped
        {
            get
            {
                return _ExpensesGrouped;
            }

            set
            {
                if (_ExpensesGrouped == value)
                {
                    return;
                }

                _ExpensesGrouped = value;
                RaisePropertyChanged();
            }
        }

        private int _ShowUps;

        public int ShowUps
        {
            get
            {
                return _ShowUps;
            }

            set
            {
                if (_ShowUps == value)
                {
                    return;
                }

                _ShowUps = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Customer> _CustomersExpireInShowUps;

        public ObservableCollection<Customer> CustomersExpireInShowUps
        {
            get
            {
                return _CustomersExpireInShowUps;
            }

            set
            {
                if (_CustomersExpireInShowUps == value)
                {
                    return;
                }

                _CustomersExpireInShowUps = value;
                RaisePropertyChanged();
            }
        }

        private async Task ShowExpensesGroupedData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DateTime enddate = EndDateExpenses.AddDays(1);
            var t = (await BasicDataManager.Context.GetAllExpensesAsync(e => !e.Income && e.Date >= StartDateExpenses && e.Date < enddate,
                false,
                expensetypes: ExpenseTypes.Any(e => !e.IsChecked) ? ExpenseTypes.Where(e => e.IsChecked).Select(e => e.ExpenseCategory.Id).ToList() : null,
                MainCategory?.Id ?? -1,
                NewExpense?.SecondaryCategory?.Id ?? -1));
            foreach (var item in t)
            {
                item.parent = this;
            }

            ExpensesGrouped = new ObservableCollection<IGrouping<ExpenseCategoryClass, Expense>>(t.
                OrderBy(a => a.Date).GroupBy(e => e.MainCategory)
                .OrderByDescending(e => e.Count()));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        internal async Task FullyLoadCustomer(Customer customer)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var c = await BasicDataManager.Context.GetFullCustomerByIdAsync(customer.Id);
            c.BasicDataManager = BasicDataManager;
            c.InitialLoad();
            c.Loaded = true;
            c.GetRemainingDays();
            c.CalculateRemainingAmount();
            Mouse.OverrideCursor = Cursors.Arrow;
            OpenCustomerManagement(c);
        }

        private async Task ShowSalesData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Sales.Clear();
            var enddate = EndDateSales.AddDays(1);
            int dealid = SelectedDealIndex > 0 ? Deals[SelectedDealIndex - 1].Id : 0;
            var t = await BasicDataManager.Context.Context.Programs
                 .Where(p => (SelectedDealIndex == 0 || p.DealId == dealid) &&
                 (SelectedProgramTypeIndex == 0 || p.ProgramTypeO.ProgramMode == (ProgramMode)(SelectedProgramTypeIndex - 1)) &&
                 ((p.DayOfIssue >= StartDateSales && p.DayOfIssue < enddate) || p.Payments.Any(r => r.Date >= StartDateSales && r.Date < enddate)))
                 .Include(p => p.Customer)
                 .Include(p => p.Payments)
                 .ToListAsync();

            Sales = new ObservableCollection<Program>(t.Where(r => r.FirstPaidDate >= StartDateSales && r.FirstPaidDate < enddate)
                .OrderBy(r => r.FirstPaidDate));
            foreach (var p in Sales)
            {
                p.CalculateRemainingAmount();
            }
            TotalSales = Sales.Sum(s => s.Amount);
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private string _Find;

        public string Find
        {
            get
            {
                return _Find;
            }

            set
            {
                if (_Find == value)
                {
                    return;
                }

                _Find = value;
                RaisePropertyChanged();
            }
        }

        private string _Replace;

        public string Replace
        {
            get
            {
                return _Replace;
            }

            set
            {
                if (_Replace == value)
                {
                    return;
                }

                _Replace = value;
                RaisePropertyChanged();
            }
        }

        private async Task FindAndReplace()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Find = Find.ToUpper().Trim();
            Replace = Replace.Trim();
            foreach (Expense e in CollectionViewSource.GetDefaultView(Expenses))
            {
                if (e.Reason.Trim().ToUpper() == Find)
                    e.Reason = Replace;
                //e.Reason = Regex.Replace(e.Reason, Find, Replace, RegexOptions.IgnoreCase);
            }

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public bool HasBars => Bars.Count > 0;

        internal void CreatePlot()
        {
            Bars = new ObservableCollection<Bar>();

            if (RealPreview == null || RealPreview.Count == 0)
            {
                return;
            }

            foreach (var b in RealPreview)
            {
                Bars.Add(new Bar { Value = decimal.Round(b.Profit, 2), Label = b.Date.ToString("MMM yyyy") });
            }

            int baseHeight = 550;
            var max = Bars.Max(t => Math.Abs(t.Value));
            var maxt = Bars.Max(t => t.Value);
            var mint = Bars.Min(t => t.Value);
            if (mint >= 0 || maxt <= 0)
            {
                BarsHeight = baseHeight;
            }
            else if (maxt >= Math.Abs(mint))
            {
                BarsHeight = (int)(100 * maxt / (maxt + mint * -1)) * baseHeight / 100;
            }
            else
            {
                BarsHeight = ((int)(100 * mint * -1 / (maxt + mint * -1))) * baseHeight / 100;
            }

            foreach (var b in Bars)
            {
                b.Height = (int)(b.Value * BarsHeight / max) + 10;
            }
            RaisePropertyChanged(nameof(HasBars));
        }

        private int _BarsHeight;

        public int BarsHeight
        {
            get
            {
                return _BarsHeight;
            }

            set
            {
                if (_BarsHeight == value)
                {
                    return;
                }

                _BarsHeight = value;
                RaisePropertyChanged();
            }
        }

        private void ShowEconomicDetails(ExpenseCheck obj)
        {
            EconomicDetails = new ObservableCollection<EconomicDetail>(BasicDataManager.ExpenseCategoryClasses.Where(e => e.ParentId == obj.ExpenseCategory.Id).Select(a => new EconomicDetail
            {
                Amount = Expenses.Where(t => t.SecondaryCategory?.Id == a.Id).Sum(w => w.Amount),
                Category = a
            }).Where(o => o.Amount > 0));
            RaisePropertyChanged(nameof(AnyDetails));
        }

        private void ShowIncomeDetails()
        {
            IncomeDetails = new ObservableCollection<EconomicDetail>();
            foreach (var id in IncomeTypes)
            {
                if (id.IsChecked)
                {
                    IncomeDetails.Add(new EconomicDetail
                    {
                        Amount = Incomes.Where(t => t.SecondaryCategory?.Id == id.ExpenseCategory.Id).Sum(w => w.Amount),
                        Category = id.ExpenseCategory
                    });
                }
            }
        }

        private ObservableCollection<EconomicDetail> _EconomicDetails;

        private ObservableCollection<Deal> _Deals;

        public ObservableCollection<Deal> Deals
        {
            get
            {
                return _Deals;
            }

            set
            {
                if (_Deals == value)
                {
                    return;
                }

                _Deals = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<EconomicDetail> _IncomeDetails;

        public ObservableCollection<EconomicDetail> IncomeDetails
        {
            get
            {
                return _IncomeDetails;
            }

            set
            {
                if (_IncomeDetails == value)
                {
                    return;
                }

                _IncomeDetails = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<EconomicDetail> EconomicDetails
        {
            get
            {
                return _EconomicDetails;
            }

            set
            {
                if (_EconomicDetails == value)
                {
                    return;
                }

                _EconomicDetails = value;
                RaisePropertyChanged();
            }
        }

        private void SelectAll()
        {
            bool any = ExpenseTypes.Any(r => !r.IsChecked);
            foreach (var item in ExpenseTypes)
            {
                item.IsChecked = any;
            }
        }

        private void SelectAllIncome()
        {
            bool any = IncomeTypes.Any(r => !r.IsChecked);
            foreach (var item in IncomeTypes)
            {
                item.IsChecked = any;
            }
        }

        private async Task ShowIncomesData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DateTime enddate = EndDateExpenses.AddDays(1);
            Incomes = new ObservableCollection<Expense>((await BasicDataManager.Context.GetAllIncomesAsync(e => e.Income && e.Date >= StartDateExpenses && e.Date < enddate,
                false,
                expensetypes: IncomeTypes.Any(e => !e.IsChecked) ? IncomeTypes.Where(e => e.IsChecked).Select(e => e.ExpenseCategory.Id).ToList() : null,
                NewIncome?.SecondaryCategory?.Id ?? -1))
                .OrderBy(a => a.Date));
            foreach (var item in Incomes)
            {
                item.parent = this;
            }
            NewIncome = new Expense { Income = true, MainCategoryId = 20 };

            UpdateAmmounts();
            ShowIncomeDetails();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private Expense _NewIncome;

        public Expense NewIncome
        {
            get
            {
                return _NewIncome;
            }

            set
            {
                if (_NewIncome == value)
                {
                    return;
                }

                _NewIncome = value;
                RaisePropertyChanged();
            }
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

        private ObservableCollection<Customer> _CustomersWithGun;

        public ObservableCollection<Customer> CustomersWithGun
        {
            get
            {
                return _CustomersWithGun;
            }

            set
            {
                if (_CustomersWithGun == value)
                {
                    return;
                }

                _CustomersWithGun = value;
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
        public RelayCommand DeleteIncomeCommand { get; set; }

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

        private decimal _IncomesTotal;

        public decimal IncomesTotal
        {
            get
            {
                return _IncomesTotal;
            }

            set
            {
                if (_IncomesTotal == value)
                {
                    return;
                }

                _IncomesTotal = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Expense> _Incomes;

        public ObservableCollection<Expense> Incomes
        {
            get
            {
                return _Incomes;
            }

            set
            {
                if (_Incomes == value)
                {
                    return;
                }

                _Incomes = value;
                if (value != null)
                {
                    IncomesCV = CollectionViewSource.GetDefaultView(value);
                    IncomesCV.Filter = IncomesFilter;
                }
                RaisePropertyChanged();
            }
        }

        private ICollectionView _IncomesCV;

        public ICollectionView IncomesCV
        {
            get
            {
                return _IncomesCV;
            }

            set
            {
                if (_IncomesCV == value)
                {
                    return;
                }

                _IncomesCV = value;
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

        private string _IncomesTextFilter;

        public string IncomesTextFilter
        {
            get
            {
                return _IncomesTextFilter;
            }

            set
            {
                if (_IncomesTextFilter == value)
                {
                    return;
                }

                _IncomesTextFilter = value;
                if (Incomes != null)
                {
                    IncomesCV.Refresh();
                    UpdateAmmounts();
                }
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

        private ExpenseCategoryClass _MainCategory;

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
                NewExpense.MainCategory = value;
                UpdateSecondaryCategories();
                RaisePropertyChanged();
            }
        }

        private void UpdateSecondaryCategories()
        {
            if (NewExpense == null || NewExpense.MainCategory == null || !BasicDataManager.ExpenseCategoryClasses.Any(r => r.Parent?.Id == NewExpense.MainCategory.Id))
                SecondaryCategories = new ObservableCollection<ExpenseCategoryClass>();
            else
            {
                var t = BasicDataManager.ExpenseCategoryClasses.Where(r => !r.Disabled && r.ParentId == NewExpense.MainCategory.Id || r.Id == -1);

                SecondaryCategories = new ObservableCollection<ExpenseCategoryClass>(t);
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

        public ObservableCollection<ExpenseCheck> IncomeTypes
        {
            get
            {
                return _incomeTypes;
            }

            set
            {
                if (_incomeTypes == value)
                {
                    return;
                }

                _incomeTypes = value;
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
                if (value != null)
                {
                    value.parent = this;
                    MainCategory = null;
                }
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
        public RelayCommand RegisterIncomeCommand { get; set; }

        public RelayCommand SaveChangesCommand { get; set; }
        public RelayCommand ShowExpireShowUpsCommand { get; set; }
        public RelayCommand SelectAllCommand { get; set; }
        public RelayCommand SelectAllIncomeCommand { get; set; }
        public RelayCommand<ExpenseCheck> ShowEconomicDetailsCommand { get; set; }

        private ObservableCollection<ExpenseCategoryClass> _SecondaryIncomeCategories;

        public ObservableCollection<ExpenseCategoryClass> SecondaryIncomeCategories
        {
            get
            {
                return _SecondaryIncomeCategories;
            }

            set
            {
                if (_SecondaryIncomeCategories == value)
                {
                    return;
                }

                _SecondaryIncomeCategories = value;
                RaisePropertyChanged();
            }
        }

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
                value?.RaisePropertyChanged(nameof(Customer.Enabled));
            }
        }

        private Expense _SelectedIncome;

        public Expense SelectedIncome
        {
            get
            {
                return _SelectedIncome;
            }

            set
            {
                if (_SelectedIncome == value)
                {
                    return;
                }

                _SelectedIncome = value;
                RaisePropertyChanged();
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

        private ObservableCollection<Program> _Sales;

        public ObservableCollection<Program> Sales
        {
            get
            {
                return _Sales;
            }

            set
            {
                if (_Sales == value)
                {
                    return;
                }

                _Sales = value;
                RaisePropertyChanged();
            }
        }

        private decimal _TotalSales;

        public decimal TotalSales
        {
            get
            {
                return _TotalSales;
            }

            set
            {
                if (_TotalSales == value)
                {
                    return;
                }

                _TotalSales = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Customer> _CustomersExpire;

        public ObservableCollection<Customer> CustomersExpire
        {
            get
            {
                return _CustomersExpire;
            }

            set
            {
                if (_CustomersExpire == value)
                {
                    return;
                }

                _CustomersExpire = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowCashDataCommand { get; set; }
        public RelayCommand ShowExpireDaysCommand { get; set; }
        public RelayCommand ShowExpensesGroupedDataCommand { get; set; }

        public RelayCommand ShowExpensesDataCommand { get; set; }
        public RelayCommand ShowSalesDataCommand { get; set; }
        public RelayCommand FindAndReplaceCommand { get; set; }

        public RelayCommand ShowIncomesDataCommand { get; set; }

        public RelayCommand ShowOwningCustomersCommand { get; set; }

        public RelayCommand ShowPaymentsDataCommand { get; set; }
        public RelayCommand<Customer> FullyLoadCustomerCommand { get; }
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

        private decimal _TotalRemainingGun;

        public decimal TotalRemainingGun
        {
            get
            {
                return _TotalRemainingGun;
            }

            set
            {
                if (_TotalRemainingGun == value)
                {
                    return;
                }

                _TotalRemainingGun = value;
                RaisePropertyChanged();
            }
        }

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
            IncomesTotal = 0;
            if (Incomes != null)
            {
                foreach (Expense e in IncomesCV)
                {
                    IncomesTotal += e.Amount;
                }
            }
            Cleanse = Sum - ExpensesSum + 50;
            Total = Sum - ExpensesSum + IncomesTotal;
        }

        public bool AnyDetails => EconomicDetails.Any();

        private DateTime _StartDateSales;

        public DateTime StartDateSales
        {
            get
            {
                return _StartDateSales;
            }

            set
            {
                if (_StartDateSales == value)
                {
                    return;
                }
                if (value > EndDateSales)
                {
                    EndDateSales = value;
                }
                _StartDateSales = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _EndDateSales;

        private int _SelectedProgramTypeIndex;

        private int _SelectedDealIndex;

        public int SelectedDealIndex
        {
            get
            {
                return _SelectedDealIndex;
            }

            set
            {
                if (_SelectedDealIndex == value)
                {
                    return;
                }

                _SelectedDealIndex = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedProgramTypeIndex
        {
            get
            {
                return _SelectedProgramTypeIndex;
            }

            set
            {
                if (_SelectedProgramTypeIndex == value)
                {
                    return;
                }

                _SelectedProgramTypeIndex = value;
                RaisePropertyChanged();
            }
        }

        public DateTime EndDateSales
        {
            get
            {
                return _EndDateSales;
            }

            set
            {
                if (_EndDateSales == value)
                {
                    return;
                }
                if (value < StartDateSales)
                {
                    StartDateSales = value;
                }
                _EndDateSales = value;
                RaisePropertyChanged();
            }
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            EconomicDetails = new ObservableCollection<EconomicDetail>();
            Expenses.Clear();
            Incomes.Clear();
            Deals = new ObservableCollection<Deal>(BasicDataManager.Deals.Where(d => d.Id > 0));
            if (BasicDataManager.ExpenseCategoryClasses != null)
            {
                MainCategories = new ObservableCollection<ExpenseCategoryClass>(BasicDataManager.ExpenseCategoryClasses.Where(e => (e.Id > 1 || e.Id == -1) && e.Id != 20 && (e.ParentId == 1 || e.Parent == null)));

                SecondaryIncomeCategories = new ObservableCollection<ExpenseCategoryClass>(BasicDataManager.ExpenseCategoryClasses.Where(e => e.ParentId == 20));
                UpdateSecondaryCategories();
                ExpenseTypes = new ObservableCollection<ExpenseCheck>(MainCategories.Where(m => m.Id >= 0).Select(e => new ExpenseCheck { ExpenseCategory = e, IsChecked = true }));
                IncomeTypes = new ObservableCollection<ExpenseCheck>(SecondaryIncomeCategories.Where(m => m.Id >= 0).Select(e => new ExpenseCheck { ExpenseCategory = e, IsChecked = true }));
                foreach (var e in ExpenseTypes)
                {
                    e.PropertyChanged += E_PropertyChanged;
                }
                foreach (var e in IncomeTypes)
                {
                    e.PropertyChanged += I_PropertyChanged;
                }
            }
        }

        private void I_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ExpenseCheck.IsChecked))
            {
                CollectionViewSource.GetDefaultView(Incomes).Refresh();
                ShowIncomeDetails();
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

        private bool CanRegisterIncome()
        {
            return NewIncome != null && NewIncome.Amount > 0 && !string.IsNullOrEmpty(NewIncome.Reason);
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

        private async Task DeleteIncome()
        {
            BasicDataManager.Add(new Change($"Διαγράφηκε ΕΣΟΔΟ {SelectedIncome.Amount}€ που είχε περαστεί {SelectedIncome.Date.ToString("ddd dd/MM/yy")} απο τον χρήστη {SelectedIncome.User.UserName}", StaticResources.User));
            BasicDataManager.Delete(SelectedIncome);
            await BasicDataManager.SaveAsync();
            Incomes.Remove(SelectedIncome);
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
            if (!(obj is Expense e) || e.Income || (ExpenseTypes.Any(ex => !ex.IsChecked) && !ExpenseTypes.Any(ex => ex.ExpenseCategory == e.MainCategory && ex.IsChecked)))
            {
                return false;
            }
            if (string.IsNullOrEmpty(ExpensesTextFilter))
                return true;
            return e.Reason.Contains(ExpensesTextFilter.ToUpperInvariant()) || e.Reason.Contains(StaticResources.ToGreek(ExpensesTextFilter.ToUpperInvariant()))
                || e.MainCategory?.Name?.ToUpperInvariant().Contains(ExpensesTextFilter.ToUpperInvariant()) == true
                || e.SecondaryCategory?.Name?.ToUpperInvariant().Contains(ExpensesTextFilter.ToUpperInvariant()) == true;
        }

        private bool IncomesFilter(object obj)
        {
            if (!(obj is Expense e) || !e.Income || (IncomeTypes.Any(ex => !ex.IsChecked) && !IncomeTypes.Any(ex => ex.ExpenseCategory == e.SecondaryCategory && ex.IsChecked)))
            {
                return false;
            }
            if (string.IsNullOrEmpty(IncomesTextFilter))
                return true;
            return (e.Reason.Contains(IncomesTextFilter.ToUpperInvariant()) || e.Reason.Contains(StaticResources.ToGreek(IncomesTextFilter.ToUpperInvariant()))
                || e.SecondaryCategory?.Name?.Contains(IncomesTextFilter.ToUpperInvariant()) == true);
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

        private ObservableCollection<Bar> _Bars;
        private ObservableCollection<ExpenseCheck> _incomeTypes;

        public ObservableCollection<Bar> Bars
        {
            get
            {
                return _Bars;
            }

            set
            {
                if (_Bars == value)
                {
                    return;
                }

                _Bars = value;
                RaisePropertyChanged();
            }
        }

        private decimal GetProgramAmountInMonth(PreviewData mon, Program program)
        {
            double toses;
            decimal fictionShowupsNum;

            if (program == null || mon == null)
            {
                throw new ArgumentNullException(nameof(program));
            }
            if (program.Showups == 0)
            {
                if (program.StartDay.AddMonths(program.Months) <= DateTime.Today)
                {
                    return program.Amount / program.ShowUpsList.Count() * program.ShowUpsList.Where(s => s.Arrived >= mon.Date && s.Arrived < mon.Date.AddMonths(1)).Count();
                }
                else
                {
                    toses = (DateTime.Today - program.StartDay).TotalDays + 1;
                    fictionShowupsNum = (decimal)((program.StartDay.AddMonths(program.Months) - program.StartDay).TotalDays * program.ShowUpsList.Count() / (DateTime.Today - program.StartDay).TotalDays);
                    return (program.Amount / fictionShowupsNum * program.ShowUpsList.Where(s => s.Arrived >= mon.Date && s.Arrived < mon.Date.AddMonths(1)).Count());
                }
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
                c.FillDefaultProframs();

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
            if ((await BasicDataManager.Context.Context.Expenses.AnyAsync(e => !e.Income && e.MainCategoryId == NewExpense.MainCategoryId && e.Amount == NewExpense.Amount &&
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

        private async Task RegisterIncome()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            NewIncome.User = StaticResources.User;
            if ((await BasicDataManager.Context.Context.Expenses.AnyAsync(e => e.Income && e.MainCategoryId == NewIncome.MainCategoryId && e.Amount == NewIncome.Amount &&
            e.From.Day == NewIncome.From.Day && e.From.Month == NewIncome.From.Month && e.From.Year == NewIncome.From.Year &&
            e.To.Day == NewIncome.To.Day && e.To.Month == NewIncome.To.Month && e.To.Year == NewIncome.To.Year)))
            {
                if (MessageBox.Show("Υπάρχει ήδη καταχώρηση με ίδιες ημερομηνίες, κατηγορία και ποσό. Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή!", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    return;
                }
            }
            NewIncome.parent = this;
            BasicDataManager.Add(NewIncome);
            await BasicDataManager.SaveAsync();
            if (NewIncome.Date >= StartDateExpenses && NewIncome.Date < EndDateExpenses.AddDays(1))
            {
                Incomes.Add(NewIncome);
            }
            NewIncome = new Expense { Income = true, MainCategoryId = 20 };
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
            Expenses = new ObservableCollection<Expense>((await BasicDataManager.Context.GetAllExpensesAsync(e => !e.Income && e.Date >= StartDateExpenses && e.Date < enddate,
                false,
                expensetypes: ExpenseTypes.Any(e => !e.IsChecked) ? ExpenseTypes.Where(e => e.IsChecked).Select(e => e.ExpenseCategory.Id).ToList() : null,
                MainCategory?.Id ?? -1,
                NewExpense?.SecondaryCategory?.Id ?? -1)).OrderBy(a => a.Date));
            foreach (var item in Expenses)
            {
                item.parent = this;
            }

            UpdateAmmounts();
            EconomicDetails = new ObservableCollection<EconomicDetail>();
            RaisePropertyChanged(nameof(AnyDetails));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ShowOwningCustomers()
        {
            var own = new List<Customer>();
            var gun = new List<Customer>();
            foreach (var customer in BasicDataManager.Customers.Where(c => c.RemainingAmount > 0))
            {
                if (customer.RemainingAmount == customer.Programs.Where(p => p.Gun).Sum(a => a.Amount))
                    gun.Add(customer);
                else
                    own.Add(customer);
            }

            CustomersWhoOwn = new ObservableCollection<Customer>(own);
            CustomersWithGun = new ObservableCollection<Customer>(gun);
            TotalRemaining = own.Sum(t => t.RemainingAmount);
            TotalRemainingGun = gun.Sum(t => t.RemainingAmount);
        }

        private async Task ShowPaymentsData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Payments = new ObservableCollection<Payment>(await BasicDataManager.Context.GetAllPaymentsAsync(StartDatePayments, EndDatePayments, false));
            RaisePropertyChanged(nameof(TotalPayments));

            CustomerBuys = new ObservableCollection<CustomerBuy>(Payments.GroupBy(c => c.Customer).ToList().Select(g => new CustomerBuy
            {
                Customer = g.Key,
                Total = g.Sum(r => r.Amount),
                BestCategory = g.GroupBy(p => p.Program?.ProgramTypeO).OrderByDescending(grp => grp.Count())
                .Select(grp => grp.Key).First().ToString()
            }).OrderByDescending(b => b.Total));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private ObservableCollection<CustomerBuy> _CustomerBuys;

        public ObservableCollection<CustomerBuy> CustomerBuys
        {
            get
            {
                return _CustomerBuys;
            }

            set
            {
                if (_CustomerBuys == value)
                {
                    return;
                }

                _CustomerBuys = value;
                RaisePropertyChanged();
            }
        }

        private async Task ShowPreviewData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<PreviewData> previewData = new List<PreviewData>();
            List<Expense> expenses = await BasicDataManager.Context.GetAllExpensesAsync(e => e.Date >= StartDatePreview && e.Date <= EndDatePreview, false);
            List<Payment> payments = (await BasicDataManager.Context.GetAllPaymentsAsync(StartDatePreview, EndDatePreview, false)).ToList();
            List<Program> programs = (await BasicDataManager.Context.GetAllAsync<Program>(p => p.DayOfIssue >= StartDatePreview && p.DayOfIssue <= EndDatePreview)).ToList();

            List<PreviewData> realpreviewData = new List<PreviewData>();
            List<Expense> expensesPreview = await BasicDataManager.Context.GetAllExpensesAsync(e => (e.To >= StartDatePreview && e.To <= EndDatePreview) || (e.From >= StartDatePreview && e.From <= EndDatePreview), false);
            List<Program> programsPreview = (await BasicDataManager.Context.GetProgramsFullAsync(p => p.ShowUpsList.Any(s => s.Arrived >= StartDatePreview && s.Arrived <= EndDatePreview))).ToList();
            var t = programs.Where(x => !programsPreview.Any(y => y.Id == x.Id)).ToList();
            PreviewData tmpMonth;
            foreach (var expense in expenses.Where(e => !e.Income))
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

            foreach (var incom in expenses.Where(e => e.Income))
            {
                tmpMonth = previewData.FirstOrDefault(m => m.Date.Month == incom.Date.Month && m.Date.Year == incom.Date.Year);
                if (tmpMonth != null)
                    tmpMonth.Recieved += incom.Amount;
                else
                {
                    previewData.Add(new PreviewData { Date = new DateTime(incom.Date.Year, incom.Date.Month, 1), Expenses = incom.Amount });
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
            foreach (var expense in expensesPreview.Where(e => !e.Income))
            {
                months = realpreviewData.Where(m => (expense.To >= m.Date && expense.To < m.Date.AddMonths(1)) || (expense.From >= m.Date && expense.From < m.Date.AddMonths(1)));
                if (months?.Count() > 0)
                {
                    foreach (var mon in months)
                    {
                        mon.Expenses += decimal.Round(GetAmountInMonth(mon, expense), 2);
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            foreach (var expense in expensesPreview.Where(e => e.Income))
            {
                months = realpreviewData.Where(m => (expense.To >= m.Date && expense.To < m.Date.AddMonths(1)) || (expense.From >= m.Date && expense.From < m.Date.AddMonths(1)));
                if (months?.Count() > 0)
                {
                    foreach (var mon in months)
                    {
                        mon.Recieved += GetAmountInMonth(mon, expense);
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
                        mon.Recieved += decimal.Round(GetProgramAmountInMonth(mon, program), 2);
                    }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            Preview = new ObservableCollection<PreviewData>(previewData.OrderBy(r => r.Date));

            RealPreview = new ObservableCollection<PreviewData>(realpreviewData.OrderBy(r => r.Date));
            CreatePlot();
            RecievedTot = Preview.Sum(r => r.Recieved);
            PackTot = Preview.Sum(r => r.Sold);
            IncomesTot = Preview.Sum(r => r.Recieved);
            ExpenseTot = Preview.Sum(r => r.Expenses);
            DiffTot = Preview.Sum(r => r.Profit);
            IncomesTotR = RealPreview.Sum(r => r.Recieved);
            ExpenseTotR = RealPreview.Sum(r => r.Expenses);
            DiffTotR = decimal.Round(RealPreview.Sum(r => r.Profit), 2);
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods

        private decimal _RecievedTot;

        public decimal RecievedTot
        {
            get
            {
                return _RecievedTot;
            }

            set
            {
                if (_RecievedTot == value)
                {
                    return;
                }

                _RecievedTot = value;
                RaisePropertyChanged();
            }
        }

        private decimal _IncomesTot;

        public decimal IncomesTot
        {
            get
            {
                return _IncomesTot;
            }

            set
            {
                if (_IncomesTot == value)
                {
                    return;
                }

                _IncomesTot = value;
                RaisePropertyChanged();
            }
        }

        private decimal _ExpenseTot;

        public decimal ExpenseTot
        {
            get
            {
                return _ExpenseTot;
            }

            set
            {
                if (_ExpenseTot == value)
                {
                    return;
                }

                _ExpenseTot = value;
                RaisePropertyChanged();
            }
        }

        private decimal _IncomesTotR;

        public decimal IncomesTotR
        {
            get
            {
                return _IncomesTotR;
            }

            set
            {
                if (_IncomesTotR == value)
                {
                    return;
                }

                _IncomesTotR = value;
                RaisePropertyChanged();
            }
        }

        private decimal _ExpenseTotR;

        public decimal ExpenseTotR
        {
            get
            {
                return _ExpenseTotR;
            }

            set
            {
                if (_ExpenseTotR == value)
                {
                    return;
                }

                _ExpenseTotR = value;
                RaisePropertyChanged();
            }
        }

        private decimal _DiffTotR;

        public decimal DiffTotR
        {
            get
            {
                return _DiffTotR;
            }

            set
            {
                if (_DiffTotR == value)
                {
                    return;
                }

                _DiffTotR = value;
                RaisePropertyChanged();
            }
        }

        private decimal _DiffTot;

        public decimal DiffTot
        {
            get
            {
                return _DiffTot;
            }

            set
            {
                if (_DiffTot == value)
                {
                    return;
                }

                _DiffTot = value;
                RaisePropertyChanged();
            }
        }

        private decimal _PackTot;

        public decimal PackTot
        {
            get
            {
                return _PackTot;
            }

            set
            {
                if (_PackTot == value)
                {
                    return;
                }

                _PackTot = value;
                RaisePropertyChanged();
            }
        }
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

    public class EconomicDetail : BaseModel
    {
        private ExpenseCategoryClass _Category;

        public ExpenseCategoryClass Category
        {
            get
            {
                return _Category;
            }

            set
            {
                if (_Category == value)
                {
                    return;
                }

                _Category = value;
                RaisePropertyChanged();
            }
        }

        private decimal _Amount;

        public decimal Amount
        {
            get
            {
                return _Amount;
            }

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
    }

    public class Bar : BaseModel
    {
        private int _Height;

        public int Height
        {
            get
            {
                return _Height;
            }

            set
            {
                if (_Height == value)
                {
                    return;
                }

                _Height = value;
                RaisePropertyChanged();
            }
        }

        public int HeightNeg => Height * -1;

        private string _Label;

        public string Label
        {
            get
            {
                return _Label;
            }

            set
            {
                if (_Label == value)
                {
                    return;
                }

                _Label = value;
                RaisePropertyChanged();
            }
        }

        private decimal _Value;

        public decimal Value
        {
            get
            {
                return _Value;
            }

            set
            {
                if (_Value == value)
                {
                    return;
                }

                _Value = value;
                RaisePropertyChanged();
            }
        }
    }

    public class CustomerBuy : BaseModel
    {
        private Customer _Customer;

        public Customer Customer
        {
            get
            {
                return _Customer;
            }

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

        private decimal _Total;

        public decimal Total
        {
            get
            {
                return _Total;
            }

            set
            {
                if (_Total == value)
                {
                    return;
                }

                _Total = value;
                RaisePropertyChanged();
            }
        }

        private string _BestCategory;

        public string BestCategory
        {
            get
            {
                return _BestCategory;
            }

            set
            {
                if (_BestCategory == value)
                {
                    return;
                }

                _BestCategory = value;
                RaisePropertyChanged();
            }
        }
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