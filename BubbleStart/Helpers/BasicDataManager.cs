using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BubbleStart.Database;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OfficeOpenXml;

namespace BubbleStart.Helpers
{
    public class BasicDataManager : ViewModelBase
    {
        #region Constructors

        public BasicDataManager(GenericRepository genericRepository)
        {
            Context = genericRepository;
            RefreshCommand = new RelayCommand(async () => await Refresh());
        }

        #endregion Constructors


        #region Fields

        private ObservableCollection<Customer> _Customers;

        private ObservableCollection<District> _Districts;

        private ObservableCollection<Item> _Items;

        private ObservableCollection<ProgramType> _ProgramTypes;

        private ObservableCollection<Customer> _TodaysApointments;

        private ObservableCollection<User> _Users;

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; set; }

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

        public ObservableCollection<District> Districts
        {
            get => _Districts;

            set
            {
                if (_Districts == value)
                {
                    return;
                }

                _Districts = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Item> Items
        {
            get
            {
                return _Items;
            }

            set
            {
                if (_Items == value)
                {
                    return;
                }

                _Items = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ProgramType> ProgramTypes
        {
            get
            {
                return _ProgramTypes;
            }

            set
            {
                if (_ProgramTypes == value)
                {
                    return;
                }

                _ProgramTypes = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand RefreshCommand { get; set; }

        public ObservableCollection<Customer> TodaysApointments
        {
            get => _TodaysApointments;

            set
            {
                if (_TodaysApointments == value)
                {
                    return;
                }

                _TodaysApointments = value;
                RaisePropertyChanged();
            }
        }



        private ObservableCollection<Deal> _Deals;


        public ObservableCollection<Deal> Deals
        {
            get
            {
                return _Deals;
            }

            set
            {
                if (_Deals == value)
                {
                    return;
                }

                _Deals = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<User> Users
        {
            get => _Users;

            set
            {
                if (_Users == value)
                {
                    return;
                }

                _Users = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public bool HasChanges()
        {
            return Context.HasChanges();
        }
        public async Task LoadAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ProgramTypes = new ObservableCollection<ProgramType>(await Context.GetAllAsync<ProgramType>());
            Deals = new ObservableCollection<Deal>(await Context.GetAllAsync<Deal>());
            Users = new ObservableCollection<User>(await Context.GetAllAsync<User>());
            Districts = new ObservableCollection<District>((await Context.GetAllAsync<District>()).OrderBy(d => d.Name));
            Customers = new ObservableCollection<Customer>(await Context.LoadAllCustomersAsync());

            Items = new ObservableCollection<Item>(await Context.GetAllAsync<Item>());
            _ = await Context.GetAllAsync<ItemPurchase>();

            StaticResources.User = StaticResources.User != null ? Users.FirstOrDefault(u => u.Id == StaticResources.User.Id) : null;


            //Create();


            foreach (var c in Customers.OrderBy(c => c.Id))
            {
                c.BasicDataManager = this;
                c.PropertyChanged += C_PropertyChanged;
                c.InitialLoad();
            }

            Messenger.Default.Send(new BasicDataManagerRefreshedMessage());
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Create()
        {
            int lineNum = 1;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Paketa.xlsx";

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
            myWorksheet.Cells["H1"].Value = "Μπλούζα";
            myWorksheet.Cells["I1"].Value = "Φούτερ";
            myWorksheet.Cells["J1"].Value = "Τσάντα";

            var ints = new List<int>
            {
                14,15,17,19,20,21,5
            };
            bool first = true;

            foreach (Customer customer in Customers)
            {
                foreach (var program in customer.Programs?.Where(pr => ints.Any(i => i == pr.ProgramTypeO.Id)) ?? new ObservableCollection<Program>())
                {
                    lineNum++;
                    if (first)
                    {
                        myWorksheet.Cells["A" + lineNum].Value = customer.FullName;

                    }
                    myWorksheet.Cells["B" + lineNum].Value = program.DayOfIssue.ToShortDateString();
                    myWorksheet.Cells["C" + lineNum].Value = program.ProgramTypeO.ProgramName;

                }
                myWorksheet.Column(1).Width = 16;
                myWorksheet.Column(2).Width = 16;
                myWorksheet.Column(3).Width = 18;
            }

            //fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
            p.SaveAs(fileInfo);
            Process.Start(path);
        }

        public async Task Refresh()
        {
            var oldContext = Context;
            Context = new GenericRepository();
            oldContext.Dispose();
            await LoadAsync();
        }
        internal void Add<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            Context.Add(model);
        }

        internal void Delete<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            if (model is Program p)
            {
                p.ShowUpsList = null;
            }
            Context.Delete(model);
            if (model is Customer c)
            {
                Customers.Remove(c);
            }

        }

        internal void RollBack()
        {
            Context.RollBack();
        }

        internal async Task SaveAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            await Context.SaveAsync();
            Mouse.OverrideCursor = Cursors.Arrow;

        }
        private void C_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        #endregion Methods
    }
}