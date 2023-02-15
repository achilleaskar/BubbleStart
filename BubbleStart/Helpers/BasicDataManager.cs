using BubbleStart.Database;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private List<Item> _Items;

        private ObservableCollection<ProgramType> _ProgramTypes;

        private ObservableCollection<Customer> _TodaysApointments;

        private ObservableCollection<User> _Users;

        #endregion Fields

        #region Properties

        public Dictionary<int, string> ProgramModes { get; set; }

        private ObservableCollection<Shift> _Shifts;

        public ObservableCollection<Shift> Shifts
        {
            get
            {
                return _Shifts;
            }

            set
            {
                if (_Shifts == value)
                {
                    return;
                }

                _Shifts = value;
                RaisePropertyChanged();
            }
        }

        public IMessenger CurrentMessenger => MessengerInstance;

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

        public List<Item> Items
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

        private ObservableCollection<ExpenseCategoryClass> _ExpenseCategoryClasses;

        public ObservableCollection<ExpenseCategoryClass> ExpenseCategoryClasses
        {
            get
            {
                return _ExpenseCategoryClasses;
            }

            set
            {
                if (_ExpenseCategoryClasses == value)
                {
                    return;
                }

                _ExpenseCategoryClasses = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<User> _Gymnasts;

        public ObservableCollection<User> Gymnasts
        {
            get
            {
                return _Gymnasts;
            }

            set
            {
                if (_Gymnasts == value)
                {
                    return;
                }

                _Gymnasts = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ProgramTypes = new ObservableCollection<ProgramType>(await Context.GetAllAsync<ProgramType>());
            Shifts = new ObservableCollection<Shift>(await Context.GetAllAsync<Shift>());
            Users = new ObservableCollection<User>(await Context.GetAllAsync<User>());
            _ = await Context.GetAllRulesAsync();
            Deals = new ObservableCollection<Deal>(await Context.GetAllAsync<Deal>());
            Districts = new ObservableCollection<District>((await Context.GetAllAsync<District>()).OrderBy(d => d.Name));
            Customers = new ObservableCollection<Customer>(await Context.LoadAllCustomersAsync());
            ExpenseCategoryClasses = new ObservableCollection<ExpenseCategoryClass>(await Context.GetAllAsync<ExpenseCategoryClass>());
            ExpenseCategoryClasses.Insert(0, new ExpenseCategoryClass { Id = -1, Name = " " });
            Gymnasts = new ObservableCollection<User>(Users.Where(u => u.Id == 4 || u.Level == 4));
            StaticResources.context = this;

            Items = new List<Item>(await Context.GetAllAsync<Item>());
            ItemsList = new ObservableCollection<Item>(Items.Where(i => !i.Shop));
            ShopItems = new ObservableCollection<Item>(Items.Where(i => i.Shop));
            _ = await Context.GetAllAsync<ItemPurchase>();

            ProgramModes = new Dictionary<int, string>();
            foreach (ProgramMode item in Enum.GetValues(typeof(ProgramMode)))
            {
                ProgramModes[(int)item] = StaticResources.GetDescription(item);
            }

            StaticResources.User = StaticResources.User != null ? Users.FirstOrDefault(u => u.Id == StaticResources.User.Id) : null;

            //var not = DateTime.Today;
            //var t = await Context.GetAllAsync<Customer>(c => c.Apointments.Any(a => a.DateTime > not) && c.Enabled == false);

            //await Context.SaveAsync();

            //await Create(false);

            foreach (var c in Customers.OrderBy(c => c.Id))
            {
                c.BasicDataManager = this;
                c.InitialLoad();
            }

            Messenger.Default.Send(new BasicDataManagerRefreshedMessage());
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private ObservableCollection<Item> _ShopItems;

        public ObservableCollection<Item> ShopItems
        {
            get
            {
                return _ShopItems;
            }

            set
            {
                if (_ShopItems == value)
                {
                    return;
                }

                _ShopItems = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Item> _ItemsList;

        public ObservableCollection<Item> ItemsList
        {
            get
            {
                return _ItemsList;
            }

            set
            {
                if (_ItemsList == value)
                {
                    return;
                }

                _ItemsList = value;
                RaisePropertyChanged();
            }
        }

        private async Task Create(bool massage)
        {
            int lineNum = 1;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Paketa.xlsx";

            FileInfo fileInfo = new FileInfo(path);
            ExcelPackage p = new ExcelPackage();
            p.Workbook.Worksheets.Add("Customers");
            ExcelWorksheet myWorksheet = p.Workbook.Worksheets[1];

            if (massage)
            {
                var massages = (await Context.GetAllShowUpsAsync(ProgramMode.massage)).GroupBy(m => new DateTime(m.Arrived.Year, m.Arrived.Month, 1)).OrderBy(r => r.Key);
                var programs = (await Context.GetAllAsync<Program>(r => r.ProgramTypeO.ProgramMode == ProgramMode.massage)).GroupBy(m => new DateTime(m.StartDay.Year, m.StartDay.Month, 1)).OrderBy(r => r.Key);

                myWorksheet.Cells["A1"].Value = "#";
                myWorksheet.Cells["B1"].Value = "Μηνας";
                myWorksheet.Cells["C1"].Value = "ΠΛηρωμένα";
                myWorksheet.Cells["D1"].Value = "Δεν έγιναν";
                myWorksheet.Cells["E1"].Value = "Δωρεάν";
                myWorksheet.Cells["F1"].Value = "Σύνολο";
                myWorksheet.Cells["G1"].Value = "Πωλήσεις";

                var dates = massages.Select(s => s.Key).ToList();
                dates.AddRange(programs.Select(t => t.Key));
                foreach (var month in dates.OrderBy(s => s).ToHashSet())
                {
                    var masses = massages.FirstOrDefault(m => m.Key == month);
                    var progs = programs.FirstOrDefault(m => m.Key == month);
                    lineNum++;
                    myWorksheet.Cells["B" + lineNum].Value = month.ToString("MMM yyyy");
                    myWorksheet.Cells["C" + lineNum].Value = masses?.Where(t => !t.Present).Count() ?? 0;
                    myWorksheet.Cells["D" + lineNum].Value = masses?.Where(t => !t.Real).Count() ?? 0;
                    myWorksheet.Cells["E" + lineNum].Value = masses?.Where(t => t.Present).Count() ?? 0;
                    myWorksheet.Cells["F" + lineNum].Value = masses?.Count() ?? 0;
                    myWorksheet.Cells["G" + lineNum].Value = (progs?.Sum(s => s.Amount) ?? 0) + " €";
                }
                //fileInfo = new FileInfo(wbPath ?? throw new InvalidOperationException());
                p.SaveAs(fileInfo);
                Process.Start(path);
                return;
            }
            lineNum = 1;
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Paketa.xlsx";

            fileInfo = new FileInfo(path);
            p = new ExcelPackage();
            p.Workbook.Worksheets.Add("Customers");
            myWorksheet = p.Workbook.Worksheets[1];

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
                14,15,17,19,20,21,5,6,7
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
            if (model is Shift s)
            {
                Shifts.Add(s);
            }
            else if (model is ExpenseCategoryClass e)
            {
                ExpenseCategoryClasses.Add(e);
            }
            else if (model is Item er)
            {
                Items.Add(er);
                ItemsList = new ObservableCollection<Item>(Items.Where(i => !i.Shop));
                ShopItems = new ObservableCollection<Item>(Items.Where(i => i.Shop));
            }
            Context.Add(model);
        }

        internal void Delete<TEntity>(TEntity model) where TEntity : BaseModel, new()
        {
            if (model is Program p)
            {
                p.ShowUpsList = null;
            }
            else if (model is Customer c)
            {
                Customers.Remove(c);
            }
            if (model is Shift s)
            {
                Shifts.Remove(s);
            }
            else if (model is ExpenseCategoryClass e)
            {
                ExpenseCategoryClasses.Remove(e);
            }
            else if (model is Item o)
            {
                Items.Remove(o);
                ItemsList = new ObservableCollection<Item>(Items.Where(i => !i.Shop));
                ShopItems = new ObservableCollection<Item>(Items.Where(i => i.Shop));
            }
            Context.Delete(model);
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

        #endregion Methods
    }
}