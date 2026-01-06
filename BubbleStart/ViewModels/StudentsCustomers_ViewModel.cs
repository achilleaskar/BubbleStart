using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace BubbleStart.ViewModels
{
    public class StudentsCustomers_ViewModel : MyViewModelBase
    {
        #region Constructors

        public StudentsCustomers_ViewModel(BasicDataManager basicDataManage)
        {
            ShowStudentsCommand = new RelayCommand(async () => { await ShowStudents(); });
            OpenCustomerManagementCommand = new RelayCommand(() => { OpenCustomerManagement(SelectedCustomer); });


            this.basicDataManager = basicDataManage;
        }

        #endregion Constructors


        private async Task OpenCustomerManagement(Customer c)
        {
            if (c != null)
            {
                if (!c.Loaded)
                {
                    await basicDataManager.Context.GetFullCustomerByIdAsync(c.Id);
                    c.InitialLoad();
                }
                c.EditedInCustomerManagement = true;
                c.BasicDataManager = basicDataManager;
                c.UpdateCollections();
                c.FillDefaultPrograms();

                Window window = new CustomerManagement
                {
                    DataContext = c
                };
                Messenger.Default.Send(new OpenChildWindowCommand(window));
            }
        }


        #region Fields

        private readonly BasicDataManager basicDataManager;
        private ICollectionView _StudentsCollectionView;
        private string _SearchTerm;
        private ObservableCollection<Customer> _Students;

        #endregion Fields

        #region Properties

        public RelayCommand OpenCustomerManagementCommand { get; set; }




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

        public bool ActiveOnly { get; set; }

        public ICollectionView StudentsCollectionView
        {
            get => _StudentsCollectionView;

            set
            {
                if (_StudentsCollectionView == value)
                {
                    return;
                }

                _StudentsCollectionView = value;
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
                StudentsCollectionView?.Refresh();
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowStudentsCommand { get; set; }

        public ObservableCollection<Customer> Students
        {
            get
            {
                return _Students;
            }

            set
            {
                if (_Students == value)
                {
                    return;
                }

                _Students = value;

                if (value != null)
                {
                    StudentsCollectionView = CollectionViewSource.GetDefaultView(Students);
                    StudentsCollectionView.Filter = StudentsFilter;
                }
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private async Task ShowStudents()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Students = new ObservableCollection<Customer>(await basicDataManager.Context.GetStudentsAsync(ActiveOnly));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool StudentsFilter(object item)
        {
            Customer customer = item as Customer;

            if (string.IsNullOrEmpty(SearchTerm))
            {
                return true;
            }
            SearchTerm = SearchTerm.Trim().ToUpper();
            string tmpTerm = StaticResources.ToGreek(SearchTerm);
            return customer != null && (customer.Name.ToUpper().Contains(tmpTerm) ||
                customer.SureName.ToUpper().Contains(tmpTerm) ||
                customer.Name.ToUpper().Contains(SearchTerm) ||
                customer.SureName.ToUpper().Contains(SearchTerm) ||
                customer.Tel.Contains(tmpTerm));
        }

        #endregion Methods
    }
}