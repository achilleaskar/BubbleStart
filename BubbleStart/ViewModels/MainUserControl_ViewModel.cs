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
using System.Windows.Media;

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
            OpenItemsEditCommand = new RelayCommand(async () => await OpenItemsWindow(), CanEditWindows);
            OpenProgramTypesEditCommand = new RelayCommand(async () => await OpenProgramTypesWindow(), CanEditWindows);
            OpenExpenseCategoriesCommand = new RelayCommand(async () => await OpenExpenseCategories(), CanEditWindows);
            PrintCustomersCommand = new RelayCommand(async () => await PrintCustomers());

            RefreshAllDataCommand = new RelayCommand(async () => { await RefreshAllData(); });

            SearchCustomer_ViewModel = new SearchCustomer_ViewModel(basicDataManager);
            EconomicData_ViewModel = new EconomicData_ViewModel(basicDataManager);
            Apointments_ViewModel = new Apointments_ViewModel(BasicDataManager, GymNum: 0);
            Apointments2_ViewModel = new Apointments_ViewModel(BasicDataManager, GymNum: 1);
            ShowUpsPerDay_ViewModel = new ShowUpsPerDay_ViewModel(BasicDataManager);
            EmployeeManagement_ViewModel = new EmployeeManagement_ViewModel(BasicDataManager);
            Shop_ViewModel = new Shop_ViewModel(BasicDataManager);
            InActiveCustomers_ViewModel = new InActiveCustomers_ViewModel(BasicDataManager, SearchCustomer_ViewModel);
            MarketingCustomers_ViewModel = new InActiveCustomers_ViewModel(BasicDataManager, SearchCustomer_ViewModel, true);
            StudentsCustomers_ViewModel = new StudentsCustomers_ViewModel(BasicDataManager);
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
        }

        private async Task OpenProgramTypesWindow()
        {
            var vm = new ProgramTypesManagement_ViewModel(BasicDataManager);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new ProgramTypesManagement_Window { DataContext = vm }));
        }

        private async Task OpenItemsWindow()
        {
            var vm = new ItemsManagement_ViewModel(BasicDataManager);
            await vm.LoadAsync();
            MessengerInstance.Send(new OpenChildWindowCommand(new ItemsManagement_Window { DataContext = vm }));
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

        private async Task PrintCustomers()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            int lineNum = 1;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Πελάτες.xlsx";
            var inactiveCustomers = BasicDataManager.Context.Context.Customers.Where(c => !c.Enabled);
            var studentsIds = await BasicDataManager.Context.GetStudentsIdsAsync();
            var nonStudentsIds = await BasicDataManager.Context.GetNonStudentsIdsAsync();

            FileInfo fileInfo = new FileInfo(path);
            ExcelPackage p = new ExcelPackage();
            p.Workbook.Worksheets.Add("Active");
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
            myWorksheet.Cells["L1"].Value = "Θερμό";
            myWorksheet.Cells["M1"].Value = "Τσάντα";
            myWorksheet.Cells["N1"].Value = "Google";
            myWorksheet.Cells["O1"].Value = "Μαλιαρ";
            myWorksheet.Cells["P1"].Value = "Μαθητής";
            myWorksheet.Cells["Q1"].Value = "Αθλητής";


            foreach (Customer customer in BasicDataManager.Customers)
            {
                lineNum++;
                bool yes;
                myWorksheet.Cells["A" + lineNum].Value = lineNum - 1;
                myWorksheet.Cells["B" + lineNum].Value = customer.Name;
                myWorksheet.Cells["C" + lineNum].Value = customer.SureName;
                myWorksheet.Cells["D" + lineNum].Value = !string.IsNullOrEmpty(customer.Tel) && customer.Tel.Length >= 10 && !customer.Tel.StartsWith("000") ? customer.Tel : "";
                myWorksheet.Cells["E" + lineNum].Value = customer.Email;
                myWorksheet.Cells["F" + lineNum].Value = customer.ActiveCustomer ? "ΝΑΙ" : "ΟΧΙ";
                myWorksheet.Cells["F" + lineNum].Style.Font.Color.SetColor(customer.ActiveCustomer ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["G" + lineNum].Value = customer.Vacinated ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["G" + lineNum].Style.Font.Color.SetColor(customer.Vacinated ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["H" + lineNum].Value = customer.ThirdDose ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["H" + lineNum].Style.Font.Color.SetColor(customer.ThirdDose ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["I" + lineNum].Value = customer.Doctor ? "Έχει" : "Δεν έχει";
                myWorksheet.Cells["J" + lineNum].Value = customer.Items?.Where(t => t.ItemId == 2) is IEnumerable<ItemPurchase> l1 && l1?.Any()==true ?
                    string.Join(", ", l1.Select(i => i.Size.ToString()).Distinct()) : "OXI";
                myWorksheet.Cells["K" + lineNum].Value = customer.Items.Where(t => t.ItemId == 1) is IEnumerable<ItemPurchase> l2 && l2.Any() ?
                    string.Join(", ", l2.Select(i => i.Size.ToString()).Distinct()) : "OXI";
                yes = customer.Items.Where(t => t.ItemId == 17) is IEnumerable<ItemPurchase> l3 && l3.Any();
                myWorksheet.Cells["L" + lineNum].Value = yes ? "NAI" : "OXI";
                myWorksheet.Cells["L" + lineNum].Style.Font.Color.SetColor(yes ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                yes = customer.Items.Where(t => t.ItemId == 3) is IEnumerable<ItemPurchase> l4 && l4.Any();
                myWorksheet.Cells["M" + lineNum].Value = yes ? "NAI" : "OXI";
                myWorksheet.Cells["M" + lineNum].Style.Font.Color.SetColor(yes ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                myWorksheet.Cells["N" + lineNum].Value = customer.Google ? "NAI" : "OXI";
                myWorksheet.Cells["N" + lineNum].Style.Font.Color.SetColor(customer.Google ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                myWorksheet.Cells["O" + lineNum].Value = customer.Maliar ? "NAI" : "OXI";
                myWorksheet.Cells["O" + lineNum].Style.Font.Color.SetColor(customer.Maliar ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                var student = studentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["P" + lineNum].Value = student ? "NAI" : "OXI";
                myWorksheet.Cells["P" + lineNum].Style.Font.Color.SetColor(student ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                var athlete = nonStudentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["Q" + lineNum].Value = athlete ? "NAI" : "OXI";
                myWorksheet.Cells["Q" + lineNum].Style.Font.Color.SetColor(athlete ? System.Drawing.Color.Black : System.Drawing.Color.Red);

            }
            myWorksheet.Column(1).Width = 5;
            myWorksheet.Column(2).Width = 16;
            myWorksheet.Column(3).Width = 18;
            myWorksheet.Column(4).Width = 12;
            myWorksheet.Column(5).Width = 30;
            myWorksheet.Column(6).Width = 10;
            myWorksheet.Column(7).Width = 13;
            myWorksheet.Column(8).Width = 13;
            myWorksheet.Column(9).Width = 13;
            myWorksheet.Column(10).Width = 13;
            myWorksheet.Column(11).Width = 10;
            myWorksheet.Column(12).Width = 10;
            myWorksheet.Column(13).Width = 10;
            myWorksheet.Column(14).Width = 10;
            myWorksheet.Column(15).Width = 10;
            myWorksheet.Column(16).Width = 10;
            myWorksheet.Column(17).Width = 10;
            myWorksheet.Cells[myWorksheet.Dimension.Address].AutoFilter = true;


            p.Workbook.Worksheets.Add("Inactive");
            myWorksheet = p.Workbook.Worksheets[2];
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
            myWorksheet.Cells["M1"].Value = "Google";
            myWorksheet.Cells["N1"].Value = "Μαλιαρ";
            myWorksheet.Cells["O1"].Value = "Μαθητής";
            myWorksheet.Cells["P1"].Value = "Αθλητής";



            lineNum = 1;

            foreach (Customer customer in inactiveCustomers.Where(c => c.ForceDisable != ForceDisable.marketing))
            {
                lineNum++;
                myWorksheet.Cells["A" + lineNum].Value = lineNum - 1;
                myWorksheet.Cells["B" + lineNum].Value = customer.Name;
                myWorksheet.Cells["C" + lineNum].Value = customer.SureName;
                myWorksheet.Cells["D" + lineNum].Value = !string.IsNullOrEmpty(customer.Tel) && customer.Tel.Length >= 10 && !customer.Tel.StartsWith("000") ? customer.Tel : "";
                myWorksheet.Cells["E" + lineNum].Value = customer.Email;
                myWorksheet.Cells["F" + lineNum].Value = customer.ActiveCustomer ? "ΝΑΙ" : "ΟΧΙ";
                myWorksheet.Cells["F" + lineNum].Style.Font.Color.SetColor(customer.ActiveCustomer ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["G" + lineNum].Value = customer.Vacinated ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["G" + lineNum].Style.Font.Color.SetColor(customer.Vacinated ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["H" + lineNum].Value = customer.ThirdDose ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["H" + lineNum].Style.Font.Color.SetColor(customer.ThirdDose ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["I" + lineNum].Value = customer.Doctor ? "Έχει" : "Δεν έχει";
                myWorksheet.Cells["M" + lineNum].Value = customer.Google ?
                    "NAI" : "OXI";
                myWorksheet.Cells["M" + lineNum].Style.Font.Color.SetColor(customer.Google ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["N" + lineNum].Value = customer.Maliar ?
                    "NAI" : "OXI";
                myWorksheet.Cells["N" + lineNum].Style.Font.Color.SetColor(customer.Maliar ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                var student = studentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["O" + lineNum].Value = student ? "NAI" : "OXI";
                myWorksheet.Cells["O" + lineNum].Style.Font.Color.SetColor(student ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                var athlete = nonStudentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["P" + lineNum].Value = athlete ? "NAI" : "OXI";
                myWorksheet.Cells["P" + lineNum].Style.Font.Color.SetColor(athlete ? System.Drawing.Color.Black : System.Drawing.Color.Red);

            }


            myWorksheet.Column(1).Width = 5;
            myWorksheet.Column(2).Width = 16;
            myWorksheet.Column(3).Width = 18;
            myWorksheet.Column(4).Width = 12;
            myWorksheet.Column(5).Width = 30;
            myWorksheet.Column(6).Width = 10;
            myWorksheet.Column(7).Width = 13;
            myWorksheet.Column(8).Width = 13;
            myWorksheet.Column(9).Width = 13;
            myWorksheet.Column(10).Width = 13;
            myWorksheet.Column(11).Width = 10;
            myWorksheet.Column(12).Width = 10;
            myWorksheet.Column(13).Width = 10;
            myWorksheet.Column(14).Width = 10;
            myWorksheet.Column(15).Width = 10;
            myWorksheet.Column(16).Width = 10;
            myWorksheet.Cells[myWorksheet.Dimension.Address].AutoFilter = true;


            p.Workbook.Worksheets.Add("Marketing");
            myWorksheet = p.Workbook.Worksheets[3];
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
            myWorksheet.Cells["M1"].Value = "Google";
            myWorksheet.Cells["N1"].Value = "Μαλιαρ";
            myWorksheet.Cells["O1"].Value = "Μαθητής";
            myWorksheet.Cells["P1"].Value = "Αθλητής";


            lineNum = 1;

            foreach (Customer customer in inactiveCustomers.Where(c => c.ForceDisable == ForceDisable.marketing))
            {
                lineNum++;
                myWorksheet.Cells["A" + lineNum].Value = lineNum - 1;
                myWorksheet.Cells["B" + lineNum].Value = customer.Name;
                myWorksheet.Cells["C" + lineNum].Value = customer.SureName;
                myWorksheet.Cells["D" + lineNum].Value = !string.IsNullOrEmpty(customer.Tel) && customer.Tel.Length >= 10 && !customer.Tel.StartsWith("000") ? customer.Tel : "";
                myWorksheet.Cells["E" + lineNum].Value = customer.Email;
                myWorksheet.Cells["F" + lineNum].Value = customer.ActiveCustomer ? "ΝΑΙ" : "ΟΧΙ";
                myWorksheet.Cells["F" + lineNum].Style.Font.Color.SetColor(customer.ActiveCustomer ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["G" + lineNum].Value = customer.Vacinated ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["G" + lineNum].Style.Font.Color.SetColor(customer.Vacinated ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["H" + lineNum].Value = customer.ThirdDose ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["H" + lineNum].Style.Font.Color.SetColor(customer.ThirdDose ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["I" + lineNum].Value = customer.Doctor ? "Έχει" : "Δεν έχει";
                myWorksheet.Cells["M" + lineNum].Value = customer.Google ?
                    "NAI" : "OXI";
                myWorksheet.Cells["M" + lineNum].Style.Font.Color.SetColor(customer.Google ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["N" + lineNum].Value = customer.Maliar ?
                    "NAI" : "OXI";
                myWorksheet.Cells["N" + lineNum].Style.Font.Color.SetColor(customer.Maliar ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                var student = studentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["O" + lineNum].Value = student ? "NAI" : "OXI";
                myWorksheet.Cells["O" + lineNum].Style.Font.Color.SetColor(student ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                var athlete = nonStudentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["P" + lineNum].Value = athlete ? "NAI" : "OXI";
                myWorksheet.Cells["P" + lineNum].Style.Font.Color.SetColor(athlete ? System.Drawing.Color.Black : System.Drawing.Color.Red);

            }


            myWorksheet.Column(1).Width = 5;
            myWorksheet.Column(2).Width = 16;
            myWorksheet.Column(3).Width = 18;
            myWorksheet.Column(4).Width = 12;
            myWorksheet.Column(5).Width = 30;
            myWorksheet.Column(6).Width = 10;
            myWorksheet.Column(7).Width = 13;
            myWorksheet.Column(8).Width = 13;
            myWorksheet.Column(9).Width = 13;
            myWorksheet.Column(10).Width = 13;
            myWorksheet.Column(11).Width = 10;
            myWorksheet.Column(12).Width = 10;
            myWorksheet.Column(13).Width = 10;
            myWorksheet.Column(14).Width = 10;
            myWorksheet.Column(15).Width = 10;
            myWorksheet.Column(16).Width = 10;
            myWorksheet.Cells[myWorksheet.Dimension.Address].AutoFilter = true;

            var all = BasicDataManager.Customers;
            foreach (var item in inactiveCustomers)
            {
                if (all.Any(a => a.Id != item.Id))
                {
                    all.Add(item);
                }
            }

            p.Workbook.Worksheets.Add("All");
            myWorksheet = p.Workbook.Worksheets[4];
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
            myWorksheet.Cells["M1"].Value = "Google";
            myWorksheet.Cells["N1"].Value = "Μαλιαρ";
            myWorksheet.Cells["O1"].Value = "Μαθητής";
            myWorksheet.Cells["P1"].Value = "Αθλητής";

            lineNum = 1;

            foreach (Customer customer in all.OrderBy(a => a.SureName))
            {
                lineNum++;
                myWorksheet.Cells["A" + lineNum].Value = lineNum - 1;
                myWorksheet.Cells["B" + lineNum].Value = customer.Name;
                myWorksheet.Cells["C" + lineNum].Value = customer.SureName;
                myWorksheet.Cells["D" + lineNum].Value = !string.IsNullOrEmpty(customer.Tel) && customer.Tel.Length >= 10 && !customer.Tel.StartsWith("000") ? customer.Tel : "";
                myWorksheet.Cells["E" + lineNum].Value = customer.Email;
                myWorksheet.Cells["F" + lineNum].Value = customer.ActiveCustomer ? "ΝΑΙ" : "ΟΧΙ";
                myWorksheet.Cells["F" + lineNum].Style.Font.Color.SetColor(customer.ActiveCustomer ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["G" + lineNum].Value = customer.Vacinated ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["G" + lineNum].Style.Font.Color.SetColor(customer.Vacinated ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["H" + lineNum].Value = customer.ThirdDose ? "Έκανε" : "Δεν έκανε";
                myWorksheet.Cells["H" + lineNum].Style.Font.Color.SetColor(customer.ThirdDose ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["I" + lineNum].Value = customer.Doctor ? "Έχει" : "Δεν έχει";
                myWorksheet.Cells["M" + lineNum].Value = customer.Google ?
                    "NAI" : "OXI";
                myWorksheet.Cells["M" + lineNum].Style.Font.Color.SetColor(customer.Google ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                myWorksheet.Cells["N" + lineNum].Value = customer.Maliar ?
                    "NAI" : "OXI";
                myWorksheet.Cells["N" + lineNum].Style.Font.Color.SetColor(customer.Maliar ? System.Drawing.Color.Black : System.Drawing.Color.Red);
                var student = studentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["O" + lineNum].Value = student ? "NAI" : "OXI";
                myWorksheet.Cells["O" + lineNum].Style.Font.Color.SetColor(student ? System.Drawing.Color.Black : System.Drawing.Color.Red);

                var athlete = nonStudentsIds.Any(s => s == customer.Id);
                myWorksheet.Cells["P" + lineNum].Value = athlete ? "NAI" : "OXI";
                myWorksheet.Cells["P" + lineNum].Style.Font.Color.SetColor(athlete ? System.Drawing.Color.Black : System.Drawing.Color.Red);

            }


            myWorksheet.Column(1).Width = 5;
            myWorksheet.Column(2).Width = 16;
            myWorksheet.Column(3).Width = 18;
            myWorksheet.Column(4).Width = 12;
            myWorksheet.Column(5).Width = 30;
            myWorksheet.Column(6).Width = 10;
            myWorksheet.Column(7).Width = 13;
            myWorksheet.Column(8).Width = 13;
            myWorksheet.Column(9).Width = 13;
            myWorksheet.Column(10).Width = 13;
            myWorksheet.Column(11).Width = 10;
            myWorksheet.Column(12).Width = 10;
            myWorksheet.Column(13).Width = 10;
            myWorksheet.Column(14).Width = 10;
            myWorksheet.Column(15).Width = 10;
            myWorksheet.Column(16).Width = 10;
            myWorksheet.Cells[myWorksheet.Dimension.Address].AutoFilter = true;

            //fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
            p.SaveAs(fileInfo);
            Process.Start(path);
            Mouse.OverrideCursor = Cursors.Arrow;

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




        private Apointments_ViewModel _Apointments2_ViewModel;


        public Apointments_ViewModel Apointments2_ViewModel
        {
            get
            {
                return _Apointments2_ViewModel;
            }

            set
            {
                if (_Apointments2_ViewModel == value)
                {
                    return;
                }

                _Apointments2_ViewModel = value;
                RaisePropertyChanged();
            }
        }

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
        public RelayCommand OpenItemsEditCommand { get; set; }
        public RelayCommand OpenProgramTypesEditCommand { get; set; }
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




        private Shop_ViewModel _Shop_ViewModel;


        public Shop_ViewModel Shop_ViewModel
        {
            get
            {
                return _Shop_ViewModel;
            }

            set
            {
                if (_Shop_ViewModel == value)
                {
                    return;
                }

                _Shop_ViewModel = value;
                RaisePropertyChanged();
            }
        }








        private StudentsCustomers_ViewModel _StudentsCustomers_ViewModel;


        public StudentsCustomers_ViewModel StudentsCustomers_ViewModel
        {
            get
            {
                return _StudentsCustomers_ViewModel;
            }

            set
            {
                if (_StudentsCustomers_ViewModel == value)
                {
                    return;
                }

                _StudentsCustomers_ViewModel = value;
                RaisePropertyChanged();
            }
        }


        private InActiveCustomers_ViewModel _MarketingCustomers_ViewModel;


        public InActiveCustomers_ViewModel MarketingCustomers_ViewModel
        {
            get
            {
                return _MarketingCustomers_ViewModel;
            }

            set
            {
                if (_MarketingCustomers_ViewModel == value)
                {
                    return;
                }

                _MarketingCustomers_ViewModel = value;
                RaisePropertyChanged();
            }
        }

        private InActiveCustomers_ViewModel _InActiveCustomers_ViewModel;

        public InActiveCustomers_ViewModel InActiveCustomers_ViewModel
        {
            get
            {
                return _InActiveCustomers_ViewModel;
            }

            set
            {
                if (_InActiveCustomers_ViewModel == value)
                {
                    return;
                }

                _InActiveCustomers_ViewModel = value;
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
            BasicDataManager.LogedOut = true;
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