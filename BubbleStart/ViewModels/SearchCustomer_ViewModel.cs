using BubbleStart.Database;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class SearchCustomer_ViewModel : MyViewModelBase
    {
        #region Constructors

        public SearchCustomer_ViewModel(GenericRepository Context)
        {
            this.Context = Context;
            CreateNewCustomerCommand = new RelayCommand(CreateNewCustomer);
            SaveCustomerCommand = new RelayCommand(async () => { await SaveCustomer(); });
            ShowedUpCommand = new RelayCommand(async () => { await CustomerShowedUp(); });
            CustomerLeftCommand = new RelayCommand(async () => { await CustomerLeft(); });
            BodyPartSelected = new RelayCommand<string>(BodyPartChanged);
            CustomersPracticing = new ObservableCollection<Customer>();
            PaymentCommand = new RelayCommand<int>(async (obj) => { await MakePayment(obj); });
            CreateNewCustomer();
        }

        private bool CanMakePayment(int arg)
        {
            return SelectedCustomer.ProgramDataCheck();
        }

        private void BodyPartChanged(string selectedIndex)
        {
            _SelectedCustomer.Illness.SelectedIllnessPropertyName = selectedIndex;
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Customer> _Customers;

        private ObservableCollection<Customer> _CustomersPracticing;

        private string _SearchTerm;

        private Customer _SelectedCustomer;

        private Customer _SelectedPracticingCustomer;

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; }

        public RelayCommand CreateNewCustomerCommand { get; set; }

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
                CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
                CustomersCollectionView.Filter = CustomerFilter;
                CustomersCollectionView.SortDescriptions.Add(new SortDescription(nameof(Customer.SureName), ListSortDirection.Ascending));
                RaisePropertyChanged();
            }
        }

        public ICollectionView CustomersCollectionView { get; set; }

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

        public RelayCommand<int> PaymentCommand { get; set; }

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
                RaisePropertyChanged();
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

        public RelayCommand ShowedUpCommand { get; set; }
        public RelayCommand CustomerLeftCommand { get; set; }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            Customers = new ObservableCollection<Customer>(await Context.LoadAllCustomersAsync());
            foreach (var item in Customers)
            {
                if (item.LastShowUp != null && item.LastShowUp.Left < item.LastShowUp.Arrived)
                {
                    item.IsPracticing = true;
                    CustomersPracticing.Add(item);
                }
            }
        }

        public RelayCommand<string> BodyPartSelected { get; set; }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private void CreateNewCustomer()
        {
            SelectedCustomer = new Customer();
        }

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;
            return string.IsNullOrEmpty(SearchTerm) || customer.Name.ToLower().Contains(SearchTerm) || customer.SureName.ToLower().Contains(SearchTerm) || customer.Tel.Contains(SearchTerm);
        }

        private async Task CustomerShowedUp()
        {
            if (SelectedCustomer != null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                CustomersPracticing.Add(SelectedCustomer);
                SelectedCustomer.ShowedUp(true);
                await Context.SaveAsync();
                SelectedCustomer = null;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private async Task MakePayment(int obj)
        {
            SelectedCustomer.AddNewProgram();
            if (obj == 1)
            {
                SelectedCustomer.MakePayment();
            }

            await Context.SaveAsync();
        }

        public async Task CustomerLeft()
        {
            SelectedPracticingCustomer.LastShowUp.Left = DateTime.Now;
            SelectedPracticingCustomer.IsPracticing = false;
            CustomersPracticing.Remove(SelectedPracticingCustomer);
            await Context.SaveAsync();
        }

        private async Task SaveCustomer()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SelectedCustomer != null)
            {
                if (SelectedCustomer.NewWeight > 0)
                {
                    SelectedCustomer.WeightHistory.Add(new Weight { WeightValue = SelectedCustomer.NewWeight });
                    SelectedCustomer.NewWeight = 0;
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

        #endregion Methods
    }
}