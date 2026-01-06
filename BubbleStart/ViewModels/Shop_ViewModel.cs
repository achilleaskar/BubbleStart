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
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class Shop_ViewModel : MyViewModelBase
    {
        public Shop_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            ShowSellsCommand = new RelayCommand(async () => await ShowSells());
            MakePurchaseCommand = new RelayCommand(async () => await MakePurchase(), CanMakePurchase);
            FullyLoadCustomerCommand = new RelayCommand<Customer>(async (c) => { await FullyLoadCustomer(c); });
            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });
            From = DateTime.Today;
            Customers = BasicDataManager.Customers;
            NewPurchase = new ItemPurchase { Date = DateTime.Now };
            Purchases = new ObservableCollection<ItemPurchase>();
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

        private void OpenCustomerManagement(Customer c)
        {
            if (c != null)
            {
                c.EditedInCustomerManagement = true;
                c.BasicDataManager = BasicDataManager;
                c.UpdateCollections();
                c.FillDefaultPrograms();

                Window window = new CustomerManagement
                {
                    DataContext = c
                };
                Messenger.Default.Send(new OpenChildWindowCommand(window));
            }
        }
        private async Task MakePurchase()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            BasicDataManager.Add(NewPurchase);
            Purchases.Add(NewPurchase);
            Total = Purchases.Sum(t => t.Price);
            NewPurchase = new ItemPurchase { Date = DateTime.Now };
            await BasicDataManager.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool CanMakePurchase()
        {
            return NewPurchase.Customer != null && !string.IsNullOrWhiteSpace(NewPurchase.ColorString) && NewPurchase.Item != null && NewPurchase.Price >= 0 && NewPurchase.Size != null;
        }

        private ItemPurchase _NewPurchase;

        public ItemPurchase NewPurchase
        {
            get
            {
                return _NewPurchase;
            }

            set
            {
                if (_NewPurchase == value)
                {
                    return;
                }

                _NewPurchase = value;
                RaisePropertyChanged();
            }
        }




        private Customer _SelectedCustomer;


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

                _SelectedCustomer = value;
                RaisePropertyChanged();
            }
        }

        private async Task ShowSells()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var nextDay = To.AddDays(1);
            int id = SelectedItemIdFilter > 0 ? BasicDataManager.ShopItems[SelectedItemIdFilter - 1].Id : 0;
            Purchases = new ObservableCollection<ItemPurchase>(await BasicDataManager.Context.Context.ItemPurchases.Where(p => p.Item.Shop == true &&
            p.Date >= From &&
            p.Date < nextDay &&
            (SelectedItemIdFilter == 0 || id == p.ItemId)).Include(r => r.Customer).ToListAsync());

            Total = Purchases.Sum(p => p.Price);
            Mouse.OverrideCursor = Cursors.Arrow;
        }
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




        private string _SearchTerm;


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

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;

            if (string.IsNullOrEmpty(SearchTerm))
            {
                return false;
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

        public RelayCommand ShowSellsCommand { get; set; }
        public RelayCommand<Customer> FullyLoadCustomerCommand { get; set; }
        public RelayCommand OpenCustomerManagementCommand { get; }
        public RelayCommand MakePurchaseCommand { get; set; }

        private DateTime _From;


        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                _From = value;
                if (value > To)
                {
                    To = value;

                }
                RaisePropertyChanged();
            }
        }




        private DateTime _To;


        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }

                _To = value;
                if (value < From)
                {
                    From = value;
                }
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<ItemPurchase> _Purchases;


        public ObservableCollection<ItemPurchase> Purchases
        {
            get
            {
                return _Purchases;
            }

            set
            {
                if (_Purchases == value)
                {
                    return;
                }

                _Purchases = value;
                RaisePropertyChanged();
            }
        }


        private int _SelectedItemIdFilter;
        private ObservableCollection<Customer> _Customers;
        private ICollectionView _CustomersCollectionView;

        public int SelectedItemIdFilter
        {
            get
            {
                return _SelectedItemIdFilter;
            }

            set
            {
                if (_SelectedItemIdFilter == value)
                {
                    return;
                }

                _SelectedItemIdFilter = value;
                RaisePropertyChanged();
            }
        }

        public BasicDataManager BasicDataManager { get; }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}
