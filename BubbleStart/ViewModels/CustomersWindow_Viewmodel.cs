using BubbleStart.Database;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BubbleStart.ViewModels
{
    public class CustomersWindow_Viewmodel : MyViewModelBase
    {
        #region Constructors

        public CustomersWindow_Viewmodel(GenericRepository context, int type, Hour hour)
        {
            Context = context;
            Type = type;
            Hour = hour;
            AddCustomerCommand = new RelayCommand(async () => { await AddCustomer(); });
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Customer> _Customers;

        private ICollectionView _CustomersCollectionView;

        private bool _IsGogoChecked = true;

        private bool _IsGymnastChecked;

        private string _SearchTerm;

        private Customer _SelectedCustomer;

        private int _SelectedPerson;

        #endregion Fields

        #region Properties

        public RelayCommand AddCustomerCommand { get; set; }

        public GenericRepository Context { get; }

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

        public bool IsGogoChecked
        {
            get
            {
                return _IsGogoChecked;
            }

            set
            {
                if (_IsGogoChecked == value)
                {
                    return;
                }

                _IsGogoChecked = value;
                SelectedPerson = 1;
                RaisePropertyChanged();
            }
        }




        private bool _IsDimitrisChecked;


        public bool IsDimitrisChecked
        {
            get
            {
                return _IsDimitrisChecked;
            }

            set
            {
                if (_IsDimitrisChecked == value)
                {
                    return;
                }

                _IsDimitrisChecked = value;
                SelectedPerson = 2;
                RaisePropertyChanged();
            }
        }

        public bool IsYogaChecked
        {
            get
            {
                return _IsGymnastChecked;
            }

            set
            {
                if (_IsGymnastChecked == value)
                {
                    return;
                }

                _IsGymnastChecked = value;
                SelectedPerson = 2;
                RaisePropertyChanged();
            }
        }

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

                _SelectedCustomer = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedPerson
        {
            get
            {
                return _SelectedPerson;
            }

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

        public int Type { get; }
        public Hour Hour { get; }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            Customers = new ObservableCollection<Customer>((await Context.LoadAllCustomersAsyncb()).OrderBy(n => n.Name));

            CustomersCollectionView = CollectionViewSource.GetDefaultView(Customers);
            CustomersCollectionView.Filter = CustomerFilter;
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private async Task AddCustomer()
        {
            await Hour.AddCustomer(SelectedCustomer, SelectedPerson, Type);
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