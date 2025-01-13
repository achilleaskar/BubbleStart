using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class SearchCustomer_ViewModel : MyViewModelBase
    {
        #region Constructors

        public SearchCustomer_ViewModel()
        {
        }

        public SearchCustomer_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            CreateNewCustomerCommand = new RelayCommand(CreateNewCustomer);
            SaveCustomerCommand = new RelayCommand(async () => { await SaveCustomer(); }, CanSaveCustomer);
            ShowedUpCommand = new RelayCommand<int>(async (obj) => { await CustomerShowedUp(obj); });
            CustomerLeftCommand = new RelayCommand(async () => { await CustomerLeft(); }, CanDoIt);
            BodyPartSelected = new RelayCommand<string>(BodyPartChanged);
            CustomersPracticing = new ObservableCollection<Customer>();
            DeleteCustomerCommand = new RelayCommand(async () => { await DeleteCustomer(1); });
            MarketingCustomerCommand = new RelayCommand(async () => { await DeleteCustomer(2); });
            BatchCloseCommand = new RelayCommand(async () => { await BatchClose(true); });
            ToggleForcedDisableCommand = new RelayCommand<object>(async (par) => await DeleteCustomer());
            CancelApointmentCommand = new RelayCommand(async () => { await CancelApointment(); });
            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });
            OpenPopupCommand = new RelayCommand(() => { PopupOpen = true; });
            OpenActiveCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedPracticingCustomer); });
            OpenActiveCustomerSideManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedApointment?.Customer); });
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
        }

        private async Task BatchClose(bool v)
        {
            if (!PopupFinishOpen)
            {
                BatchCustomersLeft = true;
                PopupFinishOpen = true;
            }
        }

        private bool CanDoIt()
        {
            return SelectedPracticingCustomer != null && SelectedBodyPart != null;
        }

        #endregion Constructors

        #region Fields

        private bool _CanAdd;
        private ObservableCollection<Customer> _Customers;
        private ICollectionView _CustomersCollectionView;
        private ObservableCollection<Customer> _CustomersPracticing;
        private bool _EnableStartAfterFilter;

        private bool _Is30min;

        private bool _PopupFinishOpen;

        private bool _PopupOpen;

        private string _SearchTerm;

        private ObservableCollection<BodyPartSelection> _SecBodyParts;

        private int _SelectedAciveIndex;

        private Apointment _SelectedApointment;

        private BodyPart? _SelectedBodyPart;

        private Customer _SelectedCustomer;

        private Customer _SelectedPracticingCustomer;

        private int _SelectedProgramModeIndex;

        private Customer _SelectedSideCustomer;

        private DateTime _StartAfterFilter = DateTime.Today;

        private ObservableCollection<Apointment> _TodaysApointments;

        #endregion Fields

        #region Properties

        public BasicDataManager BasicDataManager { get; }

        public RelayCommand<string> BodyPartSelected { get; set; }

        public bool CanAdd
        {
            get
            {
                return _CanAdd;
            }

            set
            {
                if (_CanAdd == value)
                {
                    return;
                }

                _CanAdd = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CancelApointmentCommand { get; set; }

        public RelayCommand CreateNewCustomerCommand { get; set; }

        public RelayCommand CustomerLeftCommand { get; set; }

        public ObservableCollection<Customer> Customers
        {
            get => _Customers;

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                // Customers.CollectionChanged += Customers_CollectionChanged;

                RaisePropertyChanged();
            }
        }

        public ICollectionView CustomersCollectionView
        {
            get => _CustomersCollectionView;

            set
            {
                if (_CustomersCollectionView == value)
                {
                    return;
                }

                _CustomersCollectionView = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Customer> CustomersPracticing
        {
            get => _CustomersPracticing;

            set
            {
                if (_CustomersPracticing == value)
                {
                    return;
                }

                _CustomersPracticing = value;
                CustomersPracticingCollectionView = CollectionViewSource.GetDefaultView(CustomersPracticing);
                CustomersPracticingCollectionView.SortDescriptions.Add(new SortDescription(nameof(Customer.SureName), ListSortDirection.Ascending));
                RaisePropertyChanged();
            }
        }

        public ICollectionView CustomersPracticingCollectionView { get; set; }

        public RelayCommand DeleteCustomerCommand { get; set; }
        public RelayCommand MarketingCustomerCommand { get; set; }

        public bool Enabled => SelectedCustomer != null;

        public bool EnableStartAfterFilter
        {
            get
            {
                return _EnableStartAfterFilter;
            }

            set
            {
                if (_EnableStartAfterFilter == value)
                {
                    return;
                }

                _EnableStartAfterFilter = value;
                CustomersCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public bool Is30min
        {
            get
            {
                return _Is30min;
            }

            set
            {
                if (_Is30min == value)
                {
                    return;
                }

                _Is30min = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand OpenActiveCustomerManagementCommand { get; set; }

        public RelayCommand OpenActiveCustomerSideManagementCommand { get; set; }

        public RelayCommand OpenCustomerManagementCommand { get; set; }

        public RelayCommand OpenPopupCommand { get; set; }
        public RelayCommand BatchCloseCommand { get; set; }

        public bool PopupFinishOpen
        {
            get
            {
                return _PopupFinishOpen;
            }

            set
            {
                if (_PopupFinishOpen == value)
                {
                    return;
                }

                _PopupFinishOpen = value;
                RaisePropertyChanged();
            }
        }




        private ObservableCollection<User> _Gymnasts;


        public ObservableCollection<User> Gymnasts
        {
            get
            {
                return _Gymnasts;
            }

            set
            {
                if (_Gymnasts == value)
                {
                    return;
                }

                _Gymnasts = value;
                RaisePropertyChanged();
            }
        }





        private User _SelectedGymanst;
        private int _SelectedProgramCountIndex;

        public User SelectedGymanst
        {
            get
            {
                return _SelectedGymanst;
            }

            set
            {
                if (_SelectedGymanst == value)
                {
                    return;
                }

                _SelectedGymanst = value;
                RaisePropertyChanged();
            }
        }

        public bool PopupOpen
        {
            get
            {
                return _PopupOpen;
            }

            set
            {
                if (_PopupOpen == value)
                {
                    return;
                }

                _PopupOpen = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCustomerCommand { get; set; }

        public string SearchTerm
        {
            get => _SearchTerm;

            set
            {
                if (_SearchTerm == value)
                {
                    return;
                }
                _SearchTerm = value;
                CustomersCollectionView?.Refresh();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<BodyPartSelection> SecBodyParts
        {
            get
            {
                return _SecBodyParts;
            }

            set
            {
                if (_SecBodyParts == value)
                {
                    return;
                }

                _SecBodyParts = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedAciveIndex
        {
            get
            {
                return _SelectedAciveIndex;
            }

            set
            {
                if (_SelectedAciveIndex == value)
                {
                    return;
                }

                _SelectedAciveIndex = value;
                CustomersCollectionView?.Refresh();
                RaisePropertyChanged();
            }
        }

        public Apointment SelectedApointment
        {
            get => _SelectedApointment;

            set
            {
                if (_SelectedApointment == value)
                {
                    return;
                }

                _SelectedApointment = value;
                RaisePropertyChanged();
            }
        }

        public BodyPart? SelectedBodyPart
        {
            get
            {
                return _SelectedBodyPart;
            }

            set
            {
                if (_SelectedBodyPart == value)
                {
                    return;
                }
                CanAdd = value != BodyPart.Unknown;

                _SelectedBodyPart = value;
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

        public Customer SelectedPracticingCustomer
        {
            get => _SelectedPracticingCustomer;

            set
            {
                if (_SelectedPracticingCustomer == value)
                {
                    return;
                }

                _SelectedPracticingCustomer = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedProgramModeIndex
        {
            get
            {
                return _SelectedProgramModeIndex;
            }

            set
            {
                if (_SelectedProgramModeIndex == value)
                {
                    return;
                }

                _SelectedProgramModeIndex = value;
                CustomersCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }
        public int SelectedProgramCountIndex
        {
            get
            {
                return _SelectedProgramCountIndex;
            }

            set
            {
                if (_SelectedProgramCountIndex == value)
                {
                    return;
                }

                _SelectedProgramCountIndex = value;
                CustomersCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public Customer SelectedSideCustomer
        {
            get => _SelectedSideCustomer;

            set
            {
                if (_SelectedSideCustomer == value)
                {
                    return;
                }

                _SelectedSideCustomer = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<int> ShowedUpCommand { get; set; }

        public RelayCommand ShowedUpMassCommand { get; set; }

        public DateTime StartAfterFilter
        {
            get
            {
                return _StartAfterFilter;
            }

            set
            {
                if (_StartAfterFilter == value)
                {
                    return;
                }

                _StartAfterFilter = value;
                CustomersCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Apointment> TodaysApointments
        {
            get => _TodaysApointments;

            set
            {
                if (_TodaysApointments == value)
                {
                    return;
                }

                _TodaysApointments = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<object> ToggleForcedDisableCommand { get; set; }

        #endregion Properties
        private readonly ILogger logger = Log.ForContext<SearchCustomer_ViewModel>();

        public bool BatchCustomersLeft { get; set; }

        #region Methods

        public async Task CustomerLeft()
        {
            var selectedCustomers = CustomersPracticing.Where(c => c.SelectedPracticing).ToList();
            if (!BatchCustomersLeft)
            {
                selectedCustomers = new List<Customer> { SelectedPracticingCustomer };
            }
            foreach (var cust in selectedCustomers)
            {
                if (cust.LastShowUp != null)
                {
                    cust.LastShowUp.Left = DateTime.Now;
                    cust.LastShowUp.BodyPart = SelectedBodyPart.Value;
                    cust.LastShowUp.SecBodyPartsString = string.Join(",", SecBodyParts.Where(x => x.Selected).Select(t => ((int)t.SecBodyPart)));
                    logger.Information($"Έκλεισε η παρουσία  απο μπροστά στον : {cust.FullName} - {cust.LastShowUp.Description}");
                }
                else
                    logger.Information($"Έκλεισε η παρουσία απο μπροστά στον : {cust.FullName}");

                cust.IsPracticing = false;
                cust.RaisePropertyChanged(nameof(cust.LastPart));
                CustomersPracticing.Remove(cust);
            }
            ResetList();
            await BasicDataManager.SaveAsync();
            PopupFinishOpen = false;
            BatchCustomersLeft = false;

        }

        public async Task CustomerShowedUp(int programMode)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            PopupOpen = false;
            if (SelectedCustomer != null)
            {
                if (programMode > 50)
                {
                    if (await SelectedCustomer.ShowedUp(true, (ProgramMode)(programMode / 10), Is30min, programMode % 10, gymnast: SelectedGymanst))
                        CustomersPracticing.Add(SelectedCustomer);
                }
                else
                {
                    if (await SelectedCustomer.ShowedUp(true, (ProgramMode)programMode, Is30min, gymnast: SelectedGymanst))
                        CustomersPracticing.Add(SelectedCustomer);
                }
                logger.Information($"Προστέθηκε παρουσία απο μπροστά στον : {SelectedCustomer.FullName} - {SelectedCustomer.LastShowUp?.Description ?? ""}");

                SelectedCustomer.SetColors();
                await BasicDataManager.SaveAsync();
                Is30min = false;
            }
            SelectedCustomer.SetColors();
            foreach (var a in TodaysApointments)
                a.RaisePropertyChanged(nameof(a.ShowedUpToday));
            SelectedApointment = null;
            SelectedGymanst = null;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            TodaysApointments = new ObservableCollection<Apointment>();
            CustomersPracticing.Clear();

            IEnumerable<Apointment> apps;
            foreach (var c in BasicDataManager.Customers)
            {
                try
                {
                    //c.PropertyChanged += EntityViewModelPropertyChanged;
                    c.Loaded = true;
                    c.GetRemainingDays();
                    if (c.LastShowUp != null && c.LastShowUp.Arrived.Date == DateTime.Today && c.LastShowUp.Left < c.LastShowUp.Arrived && c.LastShowUp.Left.Year != 1234)
                    {
                        c.IsPracticing = true;
                        CustomersPracticing.Add(c);
                    }
                    c.CalculateRemainingAmount();
                    apps = c.Apointments.Where(a => a.DateTime.Date == DateTime.Today && !a.Waiting);
                    foreach (var app in apps)
                    {
                        c.AppointmentTime = app.DateTime;
                        TodaysApointments.Add(app);
                    }
                }
                catch (Exception ex)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                }
                c.FullyLoaded = true;
            }
            Customers = new ObservableCollection<Customer>(BasicDataManager.Customers.OrderByDescending(c => c.ActiveCustomer).ThenBy(g => g.SureName));
            CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
            CustomersCollectionView.Filter = CustomerFilter;

            CustomersCollectionView.Refresh();
            TodaysApointments = new ObservableCollection<Apointment>(TodaysApointments.OrderBy(ta => ta.DateTime));

            List<BodyPartSelection> tmp = new List<BodyPartSelection>();
            foreach (var part in (SecBodyPart[])Enum.GetValues(typeof(SecBodyPart)))
            {
                var order = part.GetAttribute<EnumOrderAttribute>();
                tmp.Add(new BodyPartSelection { SecBodyPart = part, Order = order.Order });
            }
            SecBodyParts = new ObservableCollection<BodyPartSelection>(tmp.OrderBy(r => r.Order));
            Gymnasts = BasicDataManager.Gymnasts;

            //int counter = 0;
            //int noprog = 0;
            //int freeprogs= 0;

            ////MessageBox.Show(Customers.Where(c => c.ActiveCustomer == true).Count()+"");

            //var gymHours = await BasicDataManager.Context.GetAllAsync<GymnastHour>();

            //foreach (var c in Customers)
            //{
            //    foreach (var a in c.Apointments)
            //    {
            //        if (a.DateTime >= DateTime.Now)
            //        {
            //            continue;
            //        }
            //        if ((counter+1) % 41 == 0)
            //        {
            //            await BasicDataManager.Context.SaveAsync();
            //            counter++;
            //        }
            //        var ui = gymHours.FirstOrDefault(g => g.Datetime == a.DateTime && a.Room == g.Room)?.Gymnast_Id;
            //        if (ui != a.UserId)
            //        {
            //            a.UserId = ui;
            //            counter++;
            //        }

            //        ShowUp showup = null;
            //        var showups = c.ShowUps.Where(s => s.Arrived.Date == a.DateTime.Date).ToList();
            //        if (showups.Count == 1)
            //        {
            //            showup = showups.First();
            //        }
            //        else if (showups.Count > 1)
            //        {
            //            switch (a.Room)
            //            {
            //                case RoomEnum.Functi onal:
            //                case RoomEnum.Functi onalB:
            //                    showup = showups.FirstOrDefault(s => s.Prog?.ProgramTypeO.ProgramMode == ProgramMode.functional || s.Prog?.ProgramMode == ProgramMode.pilatesFunctional);
            //                    break;
            //                case RoomEnum.Pila tes:
            //                    showup = showups.FirstOrDefault(s => s.Prog?.ProgramTypeO.ProgramMode == ProgramMode.pilates || s.Prog?.ProgramMode == ProgramMode.pilatesFunctional);
            //                    break;
            //                case RoomEnum.Ma ssage:
            //                    showup = showups.FirstOrDefault(s => s.Prog?.ProgramTypeO.ProgramMode == ProgramMode.massage);
            //                    break;
            //                case RoomEnum.Outd oor:
            //                    showup = showups.FirstOrDefault(s => s.Prog?.ProgramTypeO.ProgramMode == ProgramMode.outdoor);
            //                    break;
            //                case RoomEnum.Massag eHalf:
            //                    showup = showups.FirstOrDefault(s => s.Prog?.ProgramTypeO.ProgramMode == ProgramMode.massage);
            //                    break;
            //                case RoomEnum.Pers onal:
            //                    showup = showups.FirstOrDefault(s => s.Prog?.ProgramTypeO.ProgramMode == ProgramMode.personal);
            //                    break;
            //                default:
            //                    break;
            //            }
            //            if (showup == null)
            //            {
            //                if (a.Room != RoomEnum.Mas sage)
            //                {
            //                    showup = showups.FirstOrDefault(s => s.Prog?.ProgramMode != ProgramMode.massage);
            //                }
            //            }
            //        }
            //        if (showup != null)
            //        {
            //            if (showup.Prog != null)
            //            {
            //                var cos = decimal.Round( GetAppointmentCost(a, showup),2);

            //                if (a.Cost != cos && a.UserId != null)
            //                {
            //                    a.Cost = cos;
            //                    counter++;
            //                }
            //                else if (cos<1)
            //                {
            //                    freeprogs++;
            //                }
            //            }
            //            else
            //            {
            //                noprog++;
            //            }
            //        }
            //        else
            //        {

            //        }

            //    }
            //}
            //BasicDataManager.Context.Save();
        }

        private decimal GetAppointmentCost(Apointment a, ShowUp showup)
        {
            decimal fictionShowupsNum;
            decimal price = 0;
            if (showup.Prog.Showups == 0 && showup.Prog.StartDay.AddMonths(showup.Prog.Months) <= DateTime.Today)
            {
                price = showup.Prog.Amount / showup.Prog.ShowUpsList.Count;
            }
            else if (showup.Prog.Showups == 0)
            {
                fictionShowupsNum = (decimal)((showup.Prog.StartDay.AddMonths(showup.Prog.Months) - showup.Prog.StartDay).TotalDays * showup.Prog.ShowUpsList.Count() / (DateTime.Today - showup.Prog.StartDay).TotalDays);
                price = showup.Prog.Amount / fictionShowupsNum;
            }
            else if (showup.Prog.Months == 0)
            {
                price = showup.Prog.ShowUpPrice;
            }
            else if (showup.Prog.Showups > 0 && showup.Prog.Months == 0)
            {
                price = showup.Prog.ShowUpPrice;
            }
            if (price > 0)
            {
                return price;
            }
            if (showup.Prog.ShowUpPrice == 0)
            {

            }
            return showup.Prog.ShowUpPrice;
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        //                case 172:
        //                    toReturn += 'T';
        //                    toReturn += 'H';
        //                    break;
        //            }
        //        }
        //    }
        //    return toReturn;
        //}
        public void ResetList()
        {

            SelectedBodyPart = null;
            foreach (var item in SecBodyParts)
            {
                item.Selected = false;
            }
        }

        private void BodyPartChanged(string selectedIndex)
        {
            _SelectedCustomer.Illness.SelectedIllnessPropertyName = selectedIndex;
        }

        private async Task CancelApointment()
        {
            if (SelectedApointment != null)
            {
                BasicDataManager.Delete(SelectedApointment);
                await BasicDataManager.SaveAsync();
                TodaysApointments.Remove(SelectedApointment);
            }
        }

        private bool CanSaveCustomer()
        {
            return SelectedCustomer != null && !string.IsNullOrEmpty(SelectedCustomer.Name) && !string.IsNullOrEmpty(SelectedCustomer.SureName) && !string.IsNullOrEmpty(SelectedCustomer.Tel);
        }

        private bool CanToggleDisable(object arg)
        {
            return SelectedCustomer != null && (int)SelectedCustomer.ForceDisable != Convert.ToInt32(arg);
        }

        private void CreateNewCustomer()
        {
            SelectedCustomer = new Customer(true);
        }

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;

            if ((SelectedAciveIndex == 1 && customer.IsActiveColor.ToString() != "#FF008000") || (SelectedAciveIndex == 2 && customer.IsActiveColor.ToString() == "#FF008000"))
            {
                return false;
            }
            if (SelectedProgramModeIndex > 0 && !customer.HasActiveProgram((ProgramMode)(SelectedProgramModeIndex - 1)))
            {
                return false;
            }
            if (SelectedProgramCountIndex > 0)
            {
                if (SelectedProgramCountIndex == 1)
                {
                    if (customer.ActivePrograms?.Any(p => p.Months > 0) != true)
                    {
                        return false;
                    }
                }
                else if (SelectedProgramCountIndex == 2)
                {
                    if (customer.ActivePrograms?.Any(p => p.Showups > 0) != true)
                    {
                        return false;
                    }
                }

            }
            if (EnableStartAfterFilter && customer.FirstDate < StartAfterFilter)
            {
                return false;
            }

            if (string.IsNullOrEmpty(SearchTerm))
            {
                return true;
            }
            SearchTerm = SearchTerm.ToUpper();
            string tmpTerm = StaticResources.ToGreek(SearchTerm);
            return customer != null && (customer.Name.ToUpper().Contains(tmpTerm) ||
                customer.SureName.ToUpper().Contains(tmpTerm) ||
                StaticResources.ContainsGreekName(tmpTerm, customer.Name) ||
                StaticResources.ContainsGreekName(tmpTerm, customer.SureName) ||
                customer.Name.ToUpper().Contains(SearchTerm) ||
                customer.SureName.ToUpper().Contains(SearchTerm) || 
                customer.Tel.Contains(tmpTerm));
        }

        private async Task DeleteCustomer(int type = 0)
        {
            if (type == 2)
            {
                if (MessageBox.Show("Θελετε σίγουρα να στείλετε τον πελάτη στην λίστα Marketing?", "Προσοχή", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    BasicDataManager.Add(new Change($"Μετατράπηκε σε Marketing o Πελάτης με όνομα {SelectedCustomer.Name} και επίθετο {SelectedCustomer.SureName}", StaticResources.User));
                    SelectedCustomer.Enabled = false;
                    SelectedCustomer.ForceDisable = ForceDisable.marketing;
                    Customers.Remove(SelectedCustomer);
                    await BasicDataManager.SaveAsync();
                }
            }
            else if (type == 1 || MessageBox.Show("Θελετε σίγουρα να διαγράψετε τον πελάτη?", "Προσοχή", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                BasicDataManager.Add(new Change($"Απενεργοποιήθηκε οριστικά Πελάτης με όνομα {SelectedCustomer.Name} και επίθετο {SelectedCustomer.SureName}", StaticResources.User));
                SelectedCustomer.Enabled = false;
                if (type == 1)
                    SelectedCustomer.ForceDisable = ForceDisable.forceDisable;
                Customers.Remove(SelectedCustomer);
                await BasicDataManager.SaveAsync();
            }
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
            SelectedCustomer = null;
        }


        private async Task SaveCustomer()
        {
            if (SelectedCustomer != null)
            {
                if (SelectedCustomer.Id == 0)
                {
                    var existing = await BasicDataManager.Context.Context.Customers.Where(c => c.Tel == SelectedCustomer.Tel).FirstOrDefaultAsync();
                    if (existing != null)
                    {
                        if (existing.ForceDisable == ForceDisable.forceDisable)
                        {
                            MessageBox.Show("Υπαρχει ήδη πελάτης οριστικά διεγραμένος με αυτο το τηλέφωνο. Επικοινωνήστε με τον προγραμματιστή να τον επαναφέρει.");
                        }
                        else if (!existing.Enabled)
                        {
                            MessageBox.Show("Υπαρχει ήδη ανενεργός πελάτης με αυτο το τηλέφωνο.");
                        }
                        else
                        {
                            MessageBox.Show("Υπαρχει ήδη πελάτης με αυτο το τηλέφωνο.");
                        }
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(SelectedCustomer.DistrictText) && BasicDataManager.Districts.All(d => d.Name != SelectedCustomer.DistrictText))
                {
                    var d = new District { Name = SelectedCustomer.DistrictText };
                    BasicDataManager.Add(d);
                    BasicDataManager.Districts.Add(d);
                }
                if (SelectedCustomer.Id == 0)
                {
                    BasicDataManager.Add(SelectedCustomer);
                    BasicDataManager.Customers.Add(SelectedCustomer);
                    Customers.Add(SelectedCustomer);
                    SelectedCustomer.Loaded = true;
                    SelectedCustomer.InitialLoad();
                    SelectedCustomer.UpdateCollections();
                    SelectedCustomer.SetColors();
                    Messenger.Default.Send(new CustomersChangedMessage());
                }
                await BasicDataManager.SaveAsync();
                SelectedCustomer.RaisePropertyChanged(nameof(Customer.BMI));
            }
        }

        private async Task TogleDisable(object to)
        {
            if (SelectedCustomer != null)
            {
                SelectedCustomer.ForceDisable = (ForceDisable)Convert.ToInt32(to);
                SelectedCustomer.IsActiveColor = SelectedCustomer.GetCustomerColor();
            }
            await BasicDataManager.SaveAsync();
        }

        #endregion Methods

        //private void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (Customer customer in e.OldItems)
        //        {
        //            //Removed items
        //            customer.PropertyChanged -= EntityViewModelPropertyChanged;
        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (Customer customer in e.NewItems)
        //        {
        //            customer.PropertyChanged += EntityViewModelPropertyChanged;
        //        }
        //    }
        //}
        //private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    // RaisePropertyChanged(nameof(SelectedCustomer));

        //    //if (e.PropertyName == "IsActiveColor")
        //    //{
        //    //    Customers = new ObservableCollection<Customer>(Customers.OrderByDescending(c => c.ActiveCustomer).ThenBy(g => g.SureName));
        //private string ToEng(string searchTerm)
        //{
        //    string toReturn = "";
        //    foreach (char c in searchTerm.ToUpper())
        //    {
        //        if (c < 134 || c > 255)
        //        {
        //            toReturn += c;
        //        }
        //        else
        //        {
        //            switch ((int)c)
        //            {
        //                case 164:
        //                case 134:
        //                    toReturn += 'A';
        //                    break;

        //                case 165:
        //                    toReturn += 'B';
        //                    break;

        //                case 166:
        //                    toReturn += 'G';
        //                    break;

        //                case 167:
        //                    toReturn += 'D';
        //                    break;

        //                case 168:
        //                case 141:
        //                    toReturn += 'E';
        //                    break;

        //                case 169:
        //                    toReturn += 'Z';
        //                    break;

        //                case 170:
        //                    toReturn += 'I';
        //                    break;
    }
}