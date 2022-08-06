using BubbleStart.Helpers;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class InActiveCustomers_ViewModel : MyViewModelBase
    {
        public InActiveCustomers_ViewModel(BasicDataManager basicDataManager, SearchCustomer_ViewModel searchCustomer_ViewModel)
        {
            BasicDataManager = basicDataManager;
            SearchCustomer_ViewModel = searchCustomer_ViewModel;
            ShowCustomersCommand = new RelayCommand(async () => await ShowCustomers());
            ReActivateCustomerCommand = new RelayCommand<Customer>(async (obj) => { await ReActivateCustomer(obj); });
            DeleteCustomerCommand = new RelayCommand<Customer>(async (obj) => { await DeleteCustomer(obj); });
        }

        private async Task DeleteCustomer(Customer obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            obj.ForceDisable = ForceDisable.forceDisable;
            Customers.Remove(obj);

            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private async Task ReActivateCustomer(Customer obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var c = await BasicDataManager.Context.GetFullCustomerByIdAsync(obj.Id);
            c.Enabled = true;
            await BasicDataManager.SaveAsync();
            c.BasicDataManager = BasicDataManager;
            c.InitialLoad();
            c.Loaded = true;
            c.GetRemainingDays();
            c.CalculateRemainingAmount();
            SearchCustomer_ViewModel.Customers.Add(c);
            SearchCustomer_ViewModel.CustomersCollectionView.Refresh();
            Customers.Remove(obj);
            CustomersCollectionView.Refresh();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private ObservableCollection<Customer> _Customers;
        private string _SearchTerm;
        private ICollectionView _CustomersCollectionView;

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

                if (value != null)
                {
                    CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
                    CustomersCollectionView.Filter = CustomerFilter;
                }

                RaisePropertyChanged();
            }
        }

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;

            if (string.IsNullOrEmpty(SearchTerm))
            {
                return true;
            }
            SearchTerm = SearchTerm.Trim().ToUpper();
            string tmpTerm = StaticResources.ToGreek(SearchTerm);
            return customer != null && (customer.Name.ToUpper().Contains(tmpTerm) || customer.SureName.ToUpper().Contains(tmpTerm) || customer.Name.ToUpper().Contains(SearchTerm) || customer.SureName.ToUpper().Contains(SearchTerm) || customer.Tel.Contains(tmpTerm));
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
        private async Task ShowCustomers()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Customers = new ObservableCollection<Customer>(await BasicDataManager.Context.GetAllAsync<Customer>(c => !c.Enabled && c.ForceDisable == ForceDisable.normal));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public RelayCommand ShowCustomersCommand { get; set; }
        public RelayCommand<Customer> ReActivateCustomerCommand { get; set; }
        public RelayCommand<Customer> DeleteCustomerCommand { get; set; }
        public BasicDataManager BasicDataManager { get; }
        public SearchCustomer_ViewModel SearchCustomer_ViewModel { get; }
    }
}
