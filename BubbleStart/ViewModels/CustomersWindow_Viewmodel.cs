using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace BubbleStart.ViewModels
{
    public class CustomersWindow_Viewmodel : MyViewModelBase
    {
        #region Constructors

        public CustomersWindow_Viewmodel(BasicDataManager basicDataManager, int type, Hour hour)
        {
            BasicDataManager = basicDataManager;
            Type = type;
            Hour = hour;
            AddCustomerCommand = new RelayCommand<string>(async obj => { await AddCustomer(obj); }, CanAdd);
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());

        }

        private bool Busy;

        private bool CanAdd(object p)
        {
            return !Busy && SelectedCustomer != null && (IsGogoChecked || IsDimitrisChecked || IsYogaChecked || IsMassageChecked || IsOnlineChecked || IsPersonalChecked);
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Customer> _Customers;

        private ICollectionView _CustomersCollectionView;

        private bool _IsGogoChecked = true;

        private bool _IsGymnastChecked;

        private string _SearchTerm;

        private Customer _SelectedCustomer;

        private SelectedPersonEnum _SelectedPerson;

        #endregion Fields

        #region Properties

        public RelayCommand<string> AddCustomerCommand { get; set; }

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



        public bool IsGogoChecked
        {
            get => _IsGogoChecked;

            set
            {
                if (_IsGogoChecked == value)
                {
                    return;
                }

                _IsGogoChecked = value;
                if (value)
                {
                    SelectedPerson = 0;
                }
                RaisePropertyChanged();
            }
        }

        private bool _IsDimitrisChecked;

        public bool IsDimitrisChecked
        {
            get => _IsDimitrisChecked;

            set
            {
                if (_IsDimitrisChecked == value)
                {
                    return;
                }

                _IsDimitrisChecked = value;
                if (value)
                {
                    SelectedPerson = SelectedPersonEnum.Dimitris;
                }
                RaisePropertyChanged();
            }
        }




        private bool _IsPersonalChecked;


        public bool IsPersonalChecked
        {
            get
            {
                return _IsPersonalChecked;
            }

            set
            {
                if (_IsPersonalChecked == value)
                {
                    return;
                }

                _IsPersonalChecked = value;
                if (value)
                {
                    SelectedPerson = SelectedPersonEnum.Personal;
                }
                RaisePropertyChanged();
            }
        }

        public bool IsYogaChecked
        {
            get => _IsGymnastChecked;

            set
            {
                if (_IsGymnastChecked == value)
                {
                    return;
                }

                _IsGymnastChecked = value;
                if (value)
                {
                    SelectedPerson = SelectedPersonEnum.Yoga;
                }
                RaisePropertyChanged();
            }
        }





        private bool _IsOnlineChecked;


        public bool IsOnlineChecked
        {
            get
            {
                return _IsOnlineChecked;
            }

            set
            {
                if (_IsOnlineChecked == value)
                {
                    return;
                }
                if (value)
                {
                    SelectedPerson = SelectedPersonEnum.Online;
                }
                _IsOnlineChecked = value;
                RaisePropertyChanged();
            }
        }

        private bool _IsMassageChecked;


        public bool IsMassageChecked
        {
            get => _IsMassageChecked;

            set
            {
                if (_IsMassageChecked == value)
                {
                    return;
                }

                _IsMassageChecked = value;
                if (value)
                {
                    SelectedPerson = SelectedPersonEnum.Massage;
                }
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

        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;

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

        public SelectedPersonEnum SelectedPerson
        {
            get => _SelectedPerson;

            set
            {
                if (_SelectedPerson == value)
                {
                    return;
                }

                _SelectedPerson = value;
                RaisePropertyChanged();
            }
        }

        public BasicDataManager BasicDataManager { get; }
        public int Type { get; }
        public Hour Hour { get; }

        #endregion Properties

        #region Methods

        private async Task AddCustomer(string p)
        {
            Busy = true;
            if (p == "2")
            {
                if (Hour.AppointmentsFunctional.Any(q => q.Customer.Id == SelectedCustomer.Id))
                    Hour.AppointmentsFunctional.Remove(Hour.AppointmentsFunctional.Where(q => q.Customer.Id == SelectedCustomer.Id).FirstOrDefault());
                if (Hour.AppointmentsReformer.Any(q => q.Customer.Id == SelectedCustomer.Id))
                    Hour.AppointmentsReformer.Remove(Hour.AppointmentsReformer.Where(q => q.Customer.Id == SelectedCustomer.Id).FirstOrDefault());
                await BasicDataManager.Context.DeleteFromThis(SelectedCustomer, Hour.Time);
                await BasicDataManager.SaveAsync();
            }
            else
            {
                await Hour.AddCustomer(SelectedCustomer, SelectedPerson, Type, p == "1");
            }
            Busy = false;
        }

        private bool CustomerFilter(object item)
        {
            Customer customer = item as Customer;

            if (string.IsNullOrEmpty(SearchTerm))
            {
                return true;
            }
            SearchTerm = SearchTerm.ToUpper();
            string tmpTerm = StaticResources.ToGreek(SearchTerm);
            return customer.Name.ToUpper().Contains(tmpTerm) || customer.SureName.ToUpper().Contains(tmpTerm) || customer.Name.ToUpper().Contains(SearchTerm) || customer.SureName.ToUpper().Contains(SearchTerm) || customer.Tel.Contains(tmpTerm);
        }



        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Customers = new ObservableCollection<Customer>(BasicDataManager.Customers);

            CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
            CustomersCollectionView.Filter = CustomerFilter;
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}