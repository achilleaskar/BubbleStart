using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
            ShowedUpCommand = new RelayCommand(async () => { await CustomerShowedUp(); });
            CustomerLeftCommand = new RelayCommand(async () => { await CustomerLeft(); });
            BodyPartSelected = new RelayCommand<string>(BodyPartChanged);
            CustomersPracticing = new ObservableCollection<Customer>();
            DeleteCustomerCommand = new RelayCommand(async () => { await DeleteCustomer(); });
            CancelApointmentCommand = new RelayCommand(async () => { await CancelApointment(); });
            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });
            OpenActiveCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedPracticingCustomer); });
            OpenActiveCustomerSideManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedApointment); });
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
        }

        #endregion Constructors

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

        #region Properties

        public RelayCommand<string> BodyPartSelected { get; set; }

        public RelayCommand CancelApointmentCommand { get; set; }

        public RelayCommand CreateNewCustomerCommand { get; set; }

        public RelayCommand CustomerLeftCommand { get; set; }

        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                Customers.CollectionChanged += Customers_CollectionChanged;

                RaisePropertyChanged();
            }
        }

        public ICollectionView CustomersCollectionView
        {
            get
            {
                return _CustomersCollectionView;
            }

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
            get
            {
                return _CustomersPracticing;
            }

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

        public bool Enabled => SelectedCustomer != null;

        public RelayCommand OpenActiveCustomerManagementCommand { get; set; }

        public RelayCommand OpenActiveCustomerSideManagementCommand { get; set; }

        public RelayCommand OpenCustomerManagementCommand { get; set; }

        public RelayCommand SaveCustomerCommand { get; set; }

        public string SearchTerm
        {
            get
            {
                return _SearchTerm;
            }

            set
            {
                if (_SearchTerm == value)
                {
                    return;
                }
                _SearchTerm = value;
                if (CustomersCollectionView != null)
                    CustomersCollectionView.Refresh();
                RaisePropertyChanged();
            }
        }

        public Customer SelectedApointment
        {
            get
            {
                return _SelectedApointment;
            }

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
            get
            {
                return _SelectedCustomer;
            }

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
                if (_SelectedCustomer != null)
                {
                    _SelectedCustomer.SelectProperProgram();
                }
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Enabled));
            }
        }

        public Customer SelectedPracticingCustomer
        {
            get
            {
                return _SelectedPracticingCustomer;
            }

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
            get
            {
                return _SelectedSideCustomer;
            }

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

        public RelayCommand ShowedUpCommand { get; set; }

        public ObservableCollection<Customer> TodaysApointments
        {
            get
            {
                return _TodaysApointments;
            }

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

        public async Task CustomerLeft()
        {
            SelectedPracticingCustomer.LastShowUp.Left = DateTime.Now;
            SelectedPracticingCustomer.IsPracticing = false;
            CustomersPracticing.Remove(SelectedPracticingCustomer);
            await BasicDataManager.SaveAsync();
        }

        public async Task CustomerShowedUp()
        {
            if (SelectedCustomer != null)
            {
                CustomersPracticing.Add(SelectedCustomer);
                SelectedCustomer.ShowedUp(true);
                await BasicDataManager.SaveAsync();
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
                BasicDataManager.Delete(SelectedApointment.Apointments.Where(a => a.DateTime.Date == DateTime.Today.Date).FirstOrDefault());
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
        }

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;

            if (string.IsNullOrEmpty(SearchTerm))
            {
                return true;
            }
            SearchTerm = SearchTerm.ToUpper();
            string tmpTerm = ToGreek(SearchTerm);
            return customer.Name.ToUpper().Contains(tmpTerm) || customer.SureName.ToUpper().Contains(tmpTerm) || customer.Name.ToUpper().Contains(SearchTerm) || customer.SureName.ToUpper().Contains(SearchTerm) || customer.Tel.Contains(tmpTerm);
        }

        private void Customers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Customer customer in e.OldItems)
                {
                    //Removed items
                    customer.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Customer customer in e.NewItems)
                {
                    customer.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        private async Task DeleteCustomer()
        {
            BasicDataManager.Add(new Change($"Διαγράφηκε Πελάτης με όνομα {SelectedCustomer.Name} και επίθετο {SelectedCustomer.SureName}", StaticResources.User));
            BasicDataManager.Delete(SelectedCustomer);
            Customers.Remove(SelectedCustomer);
            await BasicDataManager.SaveAsync();
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // RaisePropertyChanged(nameof(SelectedCustomer));

            //if (e.PropertyName == "IsActiveColor")
            //{
            //    Customers = new ObservableCollection<Customer>(Customers.OrderByDescending(c => c.ActiveCustomer).ThenBy(g => g.SureName));

            //}
        }

        private void OpenCustomerManagement(Customer c)
        {
            if (c != null)
            {
                //if (BasicDataManager.HasChanges())
                //{
                //    MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //    if (result == MessageBoxResult.No)
                //    {
                //        return;
                //    }
                //    BasicDataManager.RollBack();
                //}

                c.BasicDataManager = BasicDataManager;
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
                if (!string.IsNullOrEmpty(SelectedCustomer.DistrictText) && !BasicDataManager.Districts.Any(d => d.Name == SelectedCustomer.DistrictText))
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

        private string ToEng(string searchTerm)
        {
            string toReturn = "";
            foreach (char c in searchTerm.ToUpper())
            {
                if (c < 134 || c > 255)
                {
                    toReturn += c;
                }
                else
                {
                    switch ((int)c)
                    {
                        case 164:
                        case 134:
                            toReturn += 'A';
                            break;

                        case 165:
                            toReturn += 'B';
                            break;

                        case 166:
                            toReturn += 'G';
                            break;

                        case 167:
                            toReturn += 'D';
                            break;

                        case 168:
                        case 141:
                            toReturn += 'E';
                            break;

                        case 169:
                            toReturn += 'Z';
                            break;

                        case 170:
                            toReturn += 'I';
                            break;

                        case 172:
                            toReturn += 'T';
                            toReturn += 'H';
                            break;

                        default:
                            break;
                    }
                }
            }
            return toReturn;
        }

        private string ToGreek(string searchTerm)
        {
            string toReturn = "";
            foreach (char c in searchTerm)
            {
                if (c < 65 || c > 90)
                {
                    toReturn += c;
                }
                else
                {
                    switch ((int)c)
                    {
                        case 65:
                            toReturn += 'Α';
                            break;

                        case 66:
                            toReturn += 'Β';
                            break;

                        case 68:
                            toReturn += 'Δ';
                            break;

                        case 69:
                            toReturn += 'Ε';
                            break;

                        case 70:
                            toReturn += 'Φ';
                            break;

                        case 71:
                            toReturn += 'Γ';
                            break;

                        case 72:
                            toReturn += 'Η';
                            break;

                        case 73:
                            toReturn += 'Ι';
                            break;

                        case 75:
                            toReturn += 'Κ';
                            break;

                        case 76:
                            toReturn += 'Λ';
                            break;

                        case 77:
                            toReturn += 'Μ';
                            break;

                        case 78:
                            toReturn += 'Ν';
                            break;

                        case 79:
                            toReturn += 'Ο';
                            break;

                        case 80:
                            toReturn += 'Π';
                            break;

                        case 82:
                            toReturn += 'Ρ';
                            break;

                        case 83:
                            toReturn += 'Σ';
                            break;

                        case 84:
                            toReturn += 'Τ';
                            break;

                        case 86:
                            toReturn += 'Β';
                            break;

                        case 88:
                            toReturn += 'Χ';
                            break;

                        case 89:
                            toReturn += 'Υ';
                            break;

                        case 90:
                            toReturn += 'Ζ';
                            break;

                        default:
                            toReturn += c;
                            break;
                    }
                }
            }
            return toReturn;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            TodaysApointments = new ObservableCollection<Customer>() ;
            Customers = new ObservableCollection<Customer>(BasicDataManager.Customers);
            CustomersPracticing.Clear();
            CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
            CustomersCollectionView.Filter = CustomerFilter;

            foreach (var c in Customers)
            {
                try
                {
                    c.PropertyChanged += EntityViewModelPropertyChanged;
                    c.Loaded = true;
                    c.SetColors();
                    c.SelectProperProgram();
                    if (c.LastShowUp != null && c.LastShowUp.Left < c.LastShowUp.Arrived && c.LastShowUp.Left.Year != 1234)
                    {
                        c.IsPracticing = true;
                        CustomersPracticing.Add(c);
                    }
                    c.CalculateRemainingAmount();
                    if (c.Apointments.Any(a => a.DateTime.Date == DateTime.Today))
                    {
                        TodaysApointments.Add(c);
                    }
                }
                catch (Exception ex)
                {
                    MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                }
            }
            CustomersCollectionView.Refresh();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}