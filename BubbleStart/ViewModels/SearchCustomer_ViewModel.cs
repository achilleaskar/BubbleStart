using BubbleStart.Database;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class SearchCustomer_ViewModel : MyViewModelBase
    {
        public SearchCustomer_ViewModel(GenericRepository Context)
        {
            this.Context = Context;
            CreateNewCustomerCommand = new RelayCommand(CreateNewCustomer);
            SaveCustomerCommand = new RelayCommand(async () => { await SaveCustomer(); });
            ShowedUpCommand = new RelayCommand(async () => { await CustomerShowedUp(); });
            CustomersPracticing = new ObservableCollection<Customer>();
            PaymentCommand = new RelayCommand<int>(async (obj) => { await MakePayment(obj); });
            CreateNewCustomer();
        }

        private async Task MakePayment(int obj)
        {
            if (obj == 0)
            {
                SelectedPracticingCustomer.LastShowUp.Paid = false;

            }
            else
            {
                SelectedPracticingCustomer.LastShowUp.Paid = true;

            }
            SelectedPracticingCustomer.IsPracticing = false;
            Customers.Add(SelectedPracticingCustomer);
            CustomersPracticing.Remove(SelectedPracticingCustomer);
            await Context.SaveAsync();
        }

        private ObservableCollection<Customer> _CustomersPracticing;


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

        private async Task CustomerShowedUp()
        {
            if (SelectedCustomer != null)
            {
                SelectedCustomer.ShowedUp(true);
                CustomersPracticing.Add(SelectedCustomer);
                CustomersCollectionView.Refresh();
                await Context.SaveAsync();
            }
        }

        private async Task SaveCustomer()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (SelectedCustomer != null)
            {
                if (SelectedCustomer.Id > 0)
                {
                    await Context.SaveAsync();
                }
                else
                {
                    Context.Add(SelectedCustomer);
                    await Context.SaveAsync();
                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void CreateNewCustomer()
        {
            SelectedCustomer = new Customer();
        }

        public RelayCommand CreateNewCustomerCommand { get; set; }
        public RelayCommand<int> PaymentCommand { get; set; }
        public RelayCommand SaveCustomerCommand { get; set; }

        private Customer _SelectedCustomer;




        private Customer _SelectedPracticingCustomer;


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

        public ICollectionView CustomersCollectionView { get; set; }
        public ICollectionView CustomersPracticingCollectionView { get; set; }

        private string _SearchTerm;

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

        public RelayCommand ShowedUpCommand { get; set; }

        private ObservableCollection<Customer> _Customers;

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

        public GenericRepository Context { get; }

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;
            return !customer.IsPracticing && (string.IsNullOrEmpty(SearchTerm) || customer.Name.ToLower().Contains(SearchTerm) || customer.SureName.ToLower().Contains(SearchTerm) || customer.Tel.Contains(SearchTerm));
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            Customers = new ObservableCollection<Customer>(await Context.LoadAllCustomersAsync());
            foreach (var item in Customers)
            {
                if (item.LastShowUp.Left<item.LastShowUp.Arrived)
                {
                    item.IsPracticing = true;
                    CustomersPracticing.Add(item);
                }
            }
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}