using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Views;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            OpenUsersEditCommand = new RelayCommand(async () => await OpenUsersWindow(), CanEditWindows);
            OpenSfiftsEditCommand = new RelayCommand(async () => await OpenShiftsWindow(), CanEditWindows);
            OpenExpenseCategoriesCommand = new RelayCommand(async () => await OpenExpenseCategories(), CanEditWindows);
            PrintCustomersCommand = new RelayCommand(PrintCustomers);

            RefreshAllDataCommand = new RelayCommand(async () => { await RefreshAllData(); });

            SearchCustomer_ViewModel = new SearchCustomer_ViewModel(basicDataManager);
            EconomicData_ViewModel = new EconomicData_ViewModel(basicDataManager);
            Apointments_ViewModel = new Apointments_ViewModel(BasicDataManager);
            ShowUpsPerDay_ViewModel = new ShowUpsPerDay_ViewModel(BasicDataManager);
            EmployeeManagement_ViewModel = new EmployeeManagement_ViewModel(BasicDataManager);

            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
        }

        private async Task OpenExpenseCategories()
        {
            var vm = new ExpenseCategoriesManagement_Viewmodel(BasicDataManager);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new ExpenseCategories_ManagementWindow { DataContext = vm }));
        }

        private async Task OpenShiftsWindow()
        {
            var vm = new ShiftsManagement_Viewmodel(BasicDataManager);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new ShiftsManagement_Window { DataContext = vm }));
        }

        private void PrintCustomers()
        {
            int lineNum = 1;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Πελάτες.xlsx";

            FileInfo fileInfo = new FileInfo(path);
            ExcelPackage p = new ExcelPackage();
            p.Workbook.Worksheets.Add("Customers");
            ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];

            myWorksheet.Cells["A1"].Value = "#";
            myWorksheet.Cells["B1"].Value = "Όνομα";
            myWorksheet.Cells["C1"].Value = "Επίθετο";
            myWorksheet.Cells["D1"].Value = "Τηλέφωνο";
            myWorksheet.Cells["E1"].Value = "Email";
            myWorksheet.Cells["F1"].Value = "Ενεργός";
            myWorksheet.Cells["G1"].Value = "Εμβόλιο";
            myWorksheet.Cells["H1"].Value = "3η δόση";
            myWorksheet.Cells["I1"].Value = "Χαρτί";
            myWorksheet.Cells["J1"].Value = "Μπλούζα";
            myWorksheet.Cells["K1"].Value = "Φούτερ";
            myWorksheet.Cells["L1"].Value = "Τσάντα";
            foreach (Customer customer in BasicDataManager.Customers)
            {
                lineNum++;
                myWorksheet.Cells["A" + lineNum].Value = lineNum - 1;
                myWorksheet.Cells["B" + lineNum].Value = customer.Name;
                myWorksheet.Cells["C" + lineNum].Value = customer.SureName;
                myWorksheet.Cells["D" + lineNum].Value = !string.IsNullOrEmpty(customer.Tel) && customer.Tel.Length >= 10 && !customer.Tel.StartsWith("000") ? customer.Tel : "";
                myWorksheet.Cells["E" + lineNum].Value = customer.Email;
                myWorksheet.Cells["F" + lineNum].Value = customer.ActiveCustomer ? "ΝΑΙ" : "ΟΧΙ";
                myWorksheet.Cells["G" + lineNum].Value = customer.Vacinated ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["H" + lineNum].Value = customer.ThirdDose ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["I" + lineNum].Value = customer.Doctor ? "Έχει" : "Δεν έχει";
                myWorksheet.Cells["J" + lineNum].Value = customer.Items.Where(t => t.ItemId == 2) is IEnumerable<ItemPurchase> l1 && l1.Count() > 0 ?
                    string.Join(", ", l1.Select(i => i.Size.ToString()).Distinct()) : "OXI";
                myWorksheet.Cells["K" + lineNum].Value = customer.Items.Where(t => t.ItemId == 1) is IEnumerable<ItemPurchase> l2 && l2.Count() > 0 ?
                    string.Join(", ", l2.Select(i => i.Size.ToString()).Distinct()) : "OXI";
                myWorksheet.Cells["L" + lineNum].Value = customer.Items.Where(t => t.ItemId == 3) is IEnumerable<ItemPurchase> l3 && l3.Count() > 0 ?
                    string.Join(", ", l3.Select(i => i.Size.ToString()).Distinct()) : "OXI";
            }
            myWorksheet.Column(1).Width = 4;
            myWorksheet.Column(2).Width = 16;
            myWorksheet.Column(3).Width = 18;
            myWorksheet.Column(4).Width = 12;
            myWorksheet.Column(5).Width = 30;
            myWorksheet.Column(6).Width = 8;
            myWorksheet.Column(7).Width = 13;
            myWorksheet.Column(8).Width = 13;
            myWorksheet.Column(9).Width = 13;
            myWorksheet.Column(10).Width = 13;

            //fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
            p.SaveAs(fileInfo);
            Process.Start(path);
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
            get => _Apointments_ViewModel;

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
            get => _EconomicData_ViewModel;

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
        public RelayCommand OpenSfiftsEditCommand { get; set; }
        public RelayCommand PrintCustomersCommand { get; set; }
        public RelayCommand OpenExpenseCategoriesCommand { get; set; }

        public RelayCommand RefreshAllDataCommand { get; set; }

        public SearchCustomer_ViewModel SearchCustomer_ViewModel
        {
            get => _SearchCustomer_ViewModel;

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

        private EmployeeManagement_ViewModel _EmployeeManagement_ViewModel;

        public EmployeeManagement_ViewModel EmployeeManagement_ViewModel
        {
            get
            {
                return _EmployeeManagement_ViewModel;
            }

            set
            {
                if (_EmployeeManagement_ViewModel == value)
                {
                    return;
                }

                _EmployeeManagement_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ShowUpsPerDay_ViewModel ShowUpsPerDay_ViewModel
        {
            get => _ShowUpsPerDay_ViewModel;

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
            throw new NotImplementedException();
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
            var vm = new UsersManagement_viewModel(BasicDataManager);
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
                }
                Mouse.OverrideCursor = Cursors.Wait;
                await BasicDataManager.Refresh();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #endregion Methods
    }
}