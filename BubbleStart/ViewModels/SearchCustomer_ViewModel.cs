using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BubbleStart.ViewModels
{
    public class SearchCustomer_ViewModel : MyViewModelBase
    {
        #region Constructors





        private bool _EnableStartAfterFilter;


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


        private DateTime _StartAfterFilter = DateTime.Today;


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

        public SearchCustomer_ViewModel()
        {
        }

        private int _SelectedProgramModeIndex;

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

        public SearchCustomer_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            CreateNewCustomerCommand = new RelayCommand(CreateNewCustomer);
            SaveCustomerCommand = new RelayCommand(async () => { await SaveCustomer(); }, CanSaveCustomer);
            ShowedUpCommand = new RelayCommand<int>(async (obj) => { await CustomerShowedUp(obj); });
            CustomerLeftCommand = new RelayCommand(async () => { await CustomerLeft(); });
            BodyPartSelected = new RelayCommand<string>(BodyPartChanged);
            CustomersPracticing = new ObservableCollection<Customer>();
            DeleteCustomerCommand = new RelayCommand(async () => { await DeleteCustomer(); });
            ToggleForcedDisableCommand = new RelayCommand<object>(async (par) => await TogleDisable(par), (par) => CanToggleDisable(par));
            CancelApointmentCommand = new RelayCommand(async () => { await CancelApointment(); });
            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });
            OpenPopupCommand = new RelayCommand(() => { PopupOpen = true; });
            OpenActiveCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedPracticingCustomer); });
            OpenActiveCustomerSideManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedApointment); });
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
        }

        #endregion Constructors

        private bool CanToggleDisable(object arg)
        {
            return SelectedCustomer != null && (int)SelectedCustomer.ForceDisable != Convert.ToInt32(arg);
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

        #region Fields

        private ObservableCollection<Customer> _Customers;

        private ICollectionView _CustomersCollectionView;

        private ObservableCollection<Customer> _CustomersPracticing;

        private string _SearchTerm;

        private Customer _SelectedApointment;

        private Customer _SelectedCustomer;

        private Customer _SelectedPracticingCustomer;

        private Customer _SelectedSideCustomer;

        private ObservableCollection<Customer> _TodaysApointments;

        #endregion Fields

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

        #region Properties

        private bool _PopupOpen;
        private bool _Is30min;

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

        public RelayCommand<string> BodyPartSelected { get; set; }

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
        public RelayCommand<object> ToggleForcedDisableCommand { get; set; }

        public bool Enabled => SelectedCustomer != null;

        public RelayCommand OpenActiveCustomerManagementCommand { get; set; }

        public RelayCommand OpenActiveCustomerSideManagementCommand { get; set; }

        public RelayCommand OpenCustomerManagementCommand { get; set; }
        public RelayCommand OpenPopupCommand { get; set; }

        public RelayCommand SaveCustomerCommand { get; set; }

        private int _SelectedAciveIndex;

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

        public Customer SelectedApointment
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

        public ObservableCollection<Customer> TodaysApointments
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

        public BasicDataManager BasicDataManager { get; }

        #endregion Properties

        #region Methods

        private List<BodyPartSelection> _SecBodyParts;

        public List<BodyPartSelection> SecBodyParts
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

        private BodyPart _SelectedBodyPart;

        public BodyPart SelectedBodyPart
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

        private bool _PopupFinishOpen;

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

        public async Task CustomerLeft()
        {
            SelectedPracticingCustomer.LastShowUp.Left = DateTime.Now;
            SelectedPracticingCustomer.LastShowUp.BodyPart = SelectedBodyPart;
            SelectedPracticingCustomer.LastShowUp.SecBodyPartsString = string.Join(",", SecBodyParts.Where(x => x.Selected).Select(t => ((int)t.SecBodyPart)));
            SelectedPracticingCustomer.IsPracticing = false;
            SelectedPracticingCustomer.RaisePropertyChanged(nameof(SelectedPracticingCustomer.LastPart));
            CustomersPracticing.Remove(SelectedPracticingCustomer);
            ResetList();
            await BasicDataManager.SaveAsync();
            PopupFinishOpen = false;
        }

        public async Task CustomerShowedUp(int programMode)
        {
            PopupOpen = false;
            if (SelectedCustomer != null)
            {
                CustomersPracticing.Add(SelectedCustomer);
                if (programMode > 50)
                    SelectedCustomer.ShowedUp(true, (ProgramMode)(programMode / 10), Is30min, (programMode % 10));
                else
                    SelectedCustomer.ShowedUp(true, (ProgramMode)programMode, Is30min);
                await BasicDataManager.SaveAsync();
                SelectedCustomer.SetColors();
                Is30min = false;
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
                BasicDataManager.Delete(SelectedApointment.Apointments.FirstOrDefault(a => a.DateTime.Date == DateTime.Today.Date));
                await BasicDataManager.SaveAsync();
                TodaysApointments.Remove(SelectedApointment);
            }
        }

        private bool CanSaveCustomer()
        {
            return SelectedCustomer != null && !string.IsNullOrEmpty(SelectedCustomer.Name) && !string.IsNullOrEmpty(SelectedCustomer.SureName) && !string.IsNullOrEmpty(SelectedCustomer.Tel);
        }

        private void CreateNewCustomer()
        {
            SelectedCustomer = new Customer();
            SelectedCustomer.InitialLoad();
        }

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;

            if ((SelectedAciveIndex == 1 && customer.IsActiveColor.ToString() != "#FF008000") || (SelectedAciveIndex == 2 && customer.IsActiveColor.ToString() == "#FF008000"))
            {
                return false;
            }
            if (SelectedProgramModeIndex > 0 && !customer.HasActiveProgram(((ProgramMode)(SelectedProgramModeIndex - 1))))
            {
                return false;
            }
            if (EnableStartAfterFilter && customer.FirstDate < StartAfterFilter)
            {
                return false;
            }

            if (string.IsNullOrEmpty(SearchTerm))
            {
                return true;
            }
            SearchTerm = SearchTerm.Trim().ToUpper();
            string tmpTerm = StaticResources.ToGreek(SearchTerm);
            return customer != null && (customer.Name.ToUpper().Contains(tmpTerm) || customer.SureName.ToUpper().Contains(tmpTerm) || customer.Name.ToUpper().Contains(SearchTerm) || customer.SureName.ToUpper().Contains(SearchTerm) || customer.Tel.Contains(tmpTerm));
        }

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

        private async Task DeleteCustomer()
        {
            //#if DEBUG
            //            string PhoneNumbers = "";
            //            foreach (var c in Customers)
            //            {
            //                //if (c.IsActiveColor != null && (c.IsActiveColor.Color == Colors.Red || c.IsActiveColor.Color == Colors.Green) && !string.IsNullOrEmpty(c.Tel) && c.Tel.StartsWith("69") && c.Tel.Count() == 10)
            //                if ( !string.IsNullOrEmpty(c.Tel) && c.Tel.StartsWith("69") && c.Tel.Count() == 10)
            //                {
            //                    PhoneNumbers += c.Tel + ",";
            //                }
            //            }
            //            PhoneNumbers = PhoneNumbers.Trim(',');
            //            Clipboard.SetText(PhoneNumbers ?? "");
            //            return;
            //#endif
            BasicDataManager.Add(new Change($"Απενεργοποιήθηκε Πελάτης με όνομα {SelectedCustomer.Name} και επίθετο {SelectedCustomer.SureName}", StaticResources.User));
            SelectedCustomer.Enabled = false;
            Customers.Remove(SelectedCustomer);
            await BasicDataManager.SaveAsync();
        }

        //private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    // RaisePropertyChanged(nameof(SelectedCustomer));

        //    //if (e.PropertyName == "IsActiveColor")
        //    //{
        //    //    Customers = new ObservableCollection<Customer>(Customers.OrderByDescending(c => c.ActiveCustomer).ThenBy(g => g.SureName));

        //    //}
        //}

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

        private async Task SaveCustomer()
        {
            if (SelectedCustomer != null)
            {
                if (!string.IsNullOrEmpty(SelectedCustomer.DistrictText) && BasicDataManager.Districts.All(d => d.Name != SelectedCustomer.DistrictText))
                {
                    var d = new District { Name = SelectedCustomer.DistrictText };
                    BasicDataManager.Add(d);
                    BasicDataManager.Districts.Add(d);
                }
                if (SelectedCustomer.Id == 0)
                {
                    BasicDataManager.Add(SelectedCustomer);
                    Customers.Add(SelectedCustomer);
                }
                await BasicDataManager.SaveAsync();
                SelectedCustomer.RaisePropertyChanged(nameof(Customer.BMI));
            }
        }

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

        //                case 172:
        //                    toReturn += 'T';
        //                    toReturn += 'H';
        //                    break;
        //            }
        //        }
        //    }
        //    return toReturn;
        //}



        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            TodaysApointments = new ObservableCollection<Customer>();
            CustomersPracticing.Clear();

            Apointment app;

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
                    app = c.Apointments.FirstOrDefault(a => a.DateTime.Date == DateTime.Today);
                    if (app != null)
                    {
                        c.AppointmentTime = app.DateTime;
                        TodaysApointments.Add(c);
                    }
                }
                catch (Exception ex)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                }
            }
            Customers = new ObservableCollection<Customer>(BasicDataManager.Customers.OrderByDescending(c => c.ActiveCustomer).ThenBy(g => g.SureName));
            CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
            CustomersCollectionView.Filter = CustomerFilter;

            CustomersCollectionView.Refresh();
            TodaysApointments = new ObservableCollection<Customer>(TodaysApointments.OrderBy(ta => ta.AppointmentTime));
            SecBodyParts = new List<BodyPartSelection>();
            foreach (var part in (SecBodyPart[])Enum.GetValues(typeof(SecBodyPart)))
            {
                SecBodyParts.Add(new BodyPartSelection { SecBodyPart = part });
            }
        }

        public void ResetList()
        {
            foreach (var item in SecBodyParts)
            {
                item.Selected = false;
            }
        }




        private bool _CanAdd;


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

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}