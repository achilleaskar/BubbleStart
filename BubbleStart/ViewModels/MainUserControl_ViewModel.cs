using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public class MainUserControl_ViewModel : MyViewModelBase
    {
        public MainUserControl_ViewModel(GenericRepository startingRepository)
        {
            LogOutCommand = new RelayCommand(TryLogOut, CanLogout);
            OpenUsersEditCommand = new RelayCommand(async () => { await OpenUsersWindow(); }, CanEditWindows);

            StartingRepository = startingRepository;
        }

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

        public GenericRepository StartingRepository { get; }

        public string Username => Helpers.StaticResources.User.Name;

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
             await Task.Delay(0);
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