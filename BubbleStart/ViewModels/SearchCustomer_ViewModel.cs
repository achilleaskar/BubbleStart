using BubbleStart.Database;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public class CustomSorter : IComparer
    {
        #region Methods

        public int Compare(object x, object y)
        {
            int digitsX = x.ToString().Length;
            int digitsY = y.ToString().Length;
            if (digitsX < digitsY)
            {
                return 1;
            }
            else if (digitsX > digitsY)
            {
                return -1;
            }
            return (int)x - (int)y;
        }

        #endregion Methods
    }

    public class SearchCustomer_ViewModel : MyViewModelBase
    {
        #region Constructors

        public SearchCustomer_ViewModel()
        {
        }

        public SearchCustomer_ViewModel(GenericRepository context)
        {
            Context = context;
            CreateNewCustomerCommand = new RelayCommand(CreateNewCustomer);
            SaveCustomerCommand = new RelayCommand(async () => { await SaveCustomer(); }, CanSaveCustomer);
            ShowedUpCommand = new RelayCommand(async () => { await CustomerShowedUp(); });
            CustomerLeftCommand = new RelayCommand(async () => { await CustomerLeft(); });
            BodyPartSelected = new RelayCommand<string>(BodyPartChanged);
            CustomersPracticing = new ObservableCollection<Customer>();
            DeleteCustomerCommand = new RelayCommand(async () => { await DeleteCustomer(); });
            CancelApointmentCommand = new RelayCommand(async () => { await CancelApointment(); });
            //PaymentCommand.CanExecute((obj) => CanMakePayment(obj));
            // CreateNewCustomer();
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

        public GenericRepository Context { get; }

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

        #endregion Properties

        #region Methods

        public async Task CustomerLeft()
        {
            SelectedPracticingCustomer.LastShowUp.Left = DateTime.Now;
            SelectedPracticingCustomer.IsPracticing = false;
            CustomersPracticing.Remove(SelectedPracticingCustomer);
            await Context.SaveAsync();
        }

        public async Task CustomerShowedUp()
        {
            if (SelectedCustomer != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                CustomersPracticing.Add(SelectedCustomer);
                SelectedCustomer.ShowedUp(true);
                await Context.SaveAsync();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            List<District> Districts = (await Context.GetAllAsync<District>()).OrderBy(d => d.Name).ToList();
            TodaysApointments = new ObservableCollection<Customer>();
            Helpers.StaticResources.Districts.Clear();
            foreach (var item in Districts)
            {
                Helpers.StaticResources.Districts.Add(item);
            }

            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });
            OpenActiveCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedPracticingCustomer); });
            OpenActiveCustomerSideManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedApointment); });

            CustomersPracticing.Clear();
            var u = (await Context.GetAllAsync<User>());
            Customers = new ObservableCollection<Customer>((await Context.LoadAllCustomersAsync()));
            CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
            CustomersCollectionView.Filter = CustomerFilter;

            foreach (var c in Customers)
            {
                c.PropertyChanged += EntityViewModelPropertyChanged;
                c.Loaded = true;
                c.SetColors();
                SortPayments(c);

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
            await Context.SaveAsync();
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private void BodyPartChanged(string selectedIndex)
        {
            _SelectedCustomer.Illness.SelectedIllnessPropertyName = selectedIndex;
        }

        private async Task CancelApointment()
        {
            if (SelectedApointment != null)
            {
                Context.Delete(SelectedApointment.Apointments.Where(a => a.DateTime.Date == DateTime.Today.Date).FirstOrDefault());
                await Context.SaveAsync();
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

        private void Customers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            Mouse.OverrideCursor = Cursors.Wait;
            Context.Add(new Change($"Διαγράφηκε Πελάτης με όνομα {SelectedCustomer.Name} και επίθετο {SelectedCustomer.SureName}", Context.GetById<User>(Helpers.StaticResources.User.Id)));
            Context.Delete(SelectedCustomer);
            Customers.Remove(SelectedCustomer);
            await Context.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedCustomer));

            if (e.PropertyName == "IsActiveColor")
            {
                Customers = new ObservableCollection<Customer>(Customers.OrderByDescending(c => c.ActiveCustomer).ThenBy(g => g.SureName));
                CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
                CustomersCollectionView.Filter = CustomerFilter;
                CustomersCollectionView.Refresh();
            }
        }

        private void OpenActiveCustomerManagement()
        {
            throw new NotImplementedException();
        }

        private void OpenCustomerManagement(Customer c)
        {
            if (c != null)
            {
                if (Context.HasChanges())
                {
                    MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                if (Context.HasChanges())
                {
                    Context.RollBack();
                }

                c.Context = Context;
                Window window = new CustomerManagement
                {
                    DataContext = c
                };
                Application.Current.MainWindow.Visibility = Visibility.Hidden;
                window.ShowDialog();
                if (Context.HasChanges())
                {
                    Context.RollBack();
                }
                Application.Current.MainWindow.Visibility = Visibility.Visible;
            }
        }

        private async Task SaveCustomer()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SelectedCustomer != null)
            {
                //if (SelectedCustomer.NewWeight > 0)
                //{
                //    SelectedCustomer.WeightHistory.Add(new Weight { WeightValue = SelectedCustomer.NewWeight });
                //    SelectedCustomer.NewWeight = 0;
                //}
                if (!string.IsNullOrEmpty(SelectedCustomer.DistrictText) && !Helpers.StaticResources.Districts.Any(d => d.Name == SelectedCustomer.DistrictText))
                {
                    Context.Add(new District { Name = SelectedCustomer.DistrictText });
                }
                if (SelectedCustomer.Id > 0)
                {
                    await Context.SaveAsync();
                }
                else
                {
                    Context.Add(SelectedCustomer);
                    Customers.Add(SelectedCustomer);
                    await Context.SaveAsync();
                }
                SelectedCustomer.RaisePropertyChanged(nameof(Customer.BMI));
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SortPayments(Customer c)
        {
            foreach (Program program in c.Programs)
            {
                if (c.Payments.Any(p => p.Amount == program.Amount && p.Date == program.DayOfIssue))
                {
                    program.Paid = true;
                }
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

        #endregion Methods
    }
}