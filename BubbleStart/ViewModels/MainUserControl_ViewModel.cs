using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BubbleStart.ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBase
    {
        public MainUserControl_ViewModel(GenericRepository startingRepository)
        {
            LogOutCommand = new RelayCommand(TryLogOut, CanLogout);
            OpenUsersEditCommand = new RelayCommand(async () => { await OpenUsersWindow(); }, CanEditWindows);

            RefreshAllDataCommand = new RelayCommand(async () => { await RefreshAllData(); });

            StartingRepository = startingRepository;
            SearchCustomer_ViewModel = new SearchCustomer_ViewModel(startingRepository);
            EconomicData_ViewModel = new EconomicData_ViewModel(startingRepository);
        }

        private async Task RefreshAllData()
        {
            if (SearchCustomer_ViewModel != null)
            {
                MessageBoxResult result = MessageBoxResult.Yes;
                if (StartingRepository.HasChanges())
                {
                    result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κάνετε ανανέωση?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                }
                if (result == MessageBoxResult.Yes)
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    StartingRepository = new GenericRepository();
                    SearchCustomer_ViewModel = new SearchCustomer_ViewModel(StartingRepository);
                    await SearchCustomer_ViewModel.LoadAsync();
                    EconomicData_ViewModel = new EconomicData_ViewModel(StartingRepository);
                    await EconomicData_ViewModel.LoadAsync();
                    StaticResources.User = StartingRepository.GetById<User>(StaticResources.User.Id);
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
        }






        private EconomicData_ViewModel _EconomicData_ViewModel;


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

        private SearchCustomer_ViewModel _SearchCustomer_ViewModel;
        private Apointments_ViewModel _Apointments_ViewModel;

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

        public RelayCommand RefreshAllDataCommand { get; set; }

        private bool CanEditWindows()
        {
            return true;
        }

        public RelayCommand OpenUsersEditCommand { get; set; }
        public string UserName => StaticResources.User != null ? StaticResources.User.Name : "Error";

        private async Task OpenUsersWindow()
        {
            var vm = new UsersManagement_viewModel(StartingRepository);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new UsersManagement_Window { DataContext = vm }));
        }

        public RelayCommand LogOutCommand { get; set; }

        public GenericRepository StartingRepository { get; set; }

        public string Username => StaticResources.User.Name;

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





        private ShowUpsPerDay_ViewModel _ShowUpsPerDay_ViewModel;


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

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            //await RefreshAllData();
            await StartingRepository.GetAllAsync<Payment>();
            await StartingRepository.GetAllAsync<Program>();
            await StartingRepository.GetAllAsync<ShowUp>();
            await SearchCustomer_ViewModel.LoadAsync();
            await EconomicData_ViewModel.LoadAsync();
            Apointments_ViewModel = new Apointments_ViewModel();
            ShowUpsPerDay_ViewModel = new ShowUpsPerDay_ViewModel();
            await Apointments_ViewModel.LoadAsync();
        }

        public override async Task ReloadAsync()
        {
            await Task.Delay(0);
        }

        public void TryLogOut()
        {
            MessengerInstance.Send(new LoginLogOutMessage(false));
        }

        private bool CanLogout()
        {
            //ToDO
            return true;
        }
    }
}