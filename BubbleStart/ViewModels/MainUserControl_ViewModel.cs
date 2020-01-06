using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBase
    {
        #region Constructors

        public MainUserControl_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            LogOutCommand = new RelayCommand(TryLogOut);
            OpenUsersEditCommand = new RelayCommand(async () => { await OpenUsersWindow(); }, CanEditWindows);

            RefreshAllDataCommand = new RelayCommand(async () => { await RefreshAllData(); });

            SearchCustomer_ViewModel = new SearchCustomer_ViewModel(basicDataManager);
            EconomicData_ViewModel = new EconomicData_ViewModel(basicDataManager);
            Apointments_ViewModel = new Apointments_ViewModel(BasicDataManager);
            ShowUpsPerDay_ViewModel = new ShowUpsPerDay_ViewModel(BasicDataManager);

            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());

        }

        #endregion Constructors

        #region Fields

        private Apointments_ViewModel _Apointments_ViewModel;

        private EconomicData_ViewModel _EconomicData_ViewModel;

        private SearchCustomer_ViewModel _SearchCustomer_ViewModel;

        private ShowUpsPerDay_ViewModel _ShowUpsPerDay_ViewModel;

        #endregion Fields

        #region Properties

        public static string UserName => StaticResources.User != null ? StaticResources.User.Name : "Error";

        public Apointments_ViewModel Apointments_ViewModel
        {
            get
            {
                return _Apointments_ViewModel;
            }

            set
            {
                if (_Apointments_ViewModel == value)
                {
                    return;
                }

                _Apointments_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        public BasicDataManager BasicDataManager { get; }

        public EconomicData_ViewModel EconomicData_ViewModel
        {
            get
            {
                return _EconomicData_ViewModel;
            }

            set
            {
                if (_EconomicData_ViewModel == value)
                {
                    return;
                }

                _EconomicData_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LogOutCommand { get; set; }

        public RelayCommand OpenUsersEditCommand { get; set; }

        public RelayCommand RefreshAllDataCommand { get; set; }

        public SearchCustomer_ViewModel SearchCustomer_ViewModel
        {
            get
            {
                return _SearchCustomer_ViewModel;
            }

            set
            {
                if (_SearchCustomer_ViewModel == value)
                {
                    return;
                }

                _SearchCustomer_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ShowUpsPerDay_ViewModel ShowUpsPerDay_ViewModel
        {
            get
            {
                return _ShowUpsPerDay_ViewModel;
            }

            set
            {
                if (_ShowUpsPerDay_ViewModel == value)
                {
                    return;
                }

                _ShowUpsPerDay_ViewModel = value;
                RaisePropertyChanged();
            }
        }


        public string Username => StaticResources.User.Name;

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
           
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }

        public void TryLogOut()
        {
            if (BasicDataManager.HasChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κάνετε ανανέωση?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
                Mouse.OverrideCursor = Cursors.Wait;
                MessengerInstance.Send(new LoginLogOutMessage(false));

                Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool CanEditWindows()
        {
            return true;
        }

        private async Task OpenUsersWindow()
        {
            var vm = new UsersManagement_viewModel(BasicDataManager.Context);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window { DataContext = vm }));
        }

        private async Task RefreshAllData()
        {
            if (SearchCustomer_ViewModel != null)
            {
                if (BasicDataManager.HasChanges())
                {
                    MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κάνετε ανανέωση?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    Mouse.OverrideCursor = Cursors.Wait;
                    await BasicDataManager.Refresh();

                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }

        #endregion Methods
    }
}