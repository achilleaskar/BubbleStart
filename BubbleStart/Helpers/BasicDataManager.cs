using BubbleStart.Database;
using BubbleStart.Messages;
using BubbleStart.Model;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Item = BubbleStart.Model.Item;

namespace BubbleStart.Helpers
{
    public class BasicDataManager : ViewModelBase
    {
        #region Constructors

        public DateTime Created { get; set; }

        public BasicDataManager(GenericRepository genericRepository)
        {
            Created = DateTime.Now;
            Context = genericRepository;
            RefreshCommand = new RelayCommand(async () => await Refresh());
            //Messenger.Default.Register<LoginLogOutMessage>(this, async msg => await LogedIn(msg.Login));
        }

        //private async Task LogedIn(bool login)
        //{
        //    if (LogedOut)
        //    {
        //        await Refresh();
        //        LogedOut = false;
        //    }
        //}

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

        public bool LogedOut { get; set; }
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
            ExpenseCategoryClasses = new ObservableCollection<ExpenseCategoryClass>((await Context.GetAllAsync<ExpenseCategoryClass>()).OrderBy(e => e.Name));
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

            var aspps = await Context.Context.Apointments.Where(a => a.DateTime > DateTime.Now).ToListAsync();

            var grouped = aspps.GroupBy(c => new
            {
                c.DateTime,
                c.Customer,
                c.Room,
                c.Person
            });
            //.Select(gcs => new Apointment()
            //{
            //    DateTime = gcs.Key.DateTime,
            //    Customer = gcs.Key.Customer,
            //    Room = gcs.Key.Room,
            //    Person = gcs.Key.Person
            //});

            foreach (var item in grouped.Where(d => d.Count() > 1))
            {
                if (item.Count() > 1)
                    Context.Delete(item.First());
            }

            await Context.SaveAsync();

            //var not = DateTime.Today;
            //var t = await Context.GetAllAsync<Customer>(c => c.Apointments.Any(a => a.DateTime > not) && c.Enabled == false);

            //await Context.SaveAsync();

            //await Create(false);

            foreach (var c in Customers.OrderBy(c => c.Id))
            {
                c.BasicDataManager = this;
                c.InitialLoad();
            }

            string date = $"{DateTime.Today.AddDays(-1):yyyy-MM-dd} 00:00:00";

            var count = await Context.Context.Database.ExecuteSqlCommandAsync($"delete FROM `ProgramChanges` WHERE Date <'{date}'");

            Messenger.Default.Send(new BasicDataManagerRefreshedMessage());

            //var t = await Context.Context.Programs
            //    .Where(p => p.Customer != null)
            //    .Select(p =>
            //    new
            //    {
            //        p.Customer.Id,
            //        CustomerName = p.Customer.Name + " " + p.Customer.SureName,
            //        p.ProgramTypeO.ProgramName,
            //        Type = p.ProgramTypeO.ProgramMode == ProgramMode.massage ? "Massage" :
            //        p.ProgramTypeO.ProgramMode == ProgramMode.aerialYoga ? "Seminar" :
            //        p.ProgramTypeO.ProgramMode == ProgramMode.yoga ? "Book" :
            //        "Gym",
            //        p.Showups,
            //        p.Months,
            //        p.Amount,
            //        p.DayOfIssue,
            //        p.StartDay,
            //    })
            //    .ToListAsync();

            //var mixedData = new Dictionary<string, object> {
            //    {"Programs",t }
            //};

            //var inc
            //    = await Context.Context.Expenses
            // .Where(r => r.Income)
            // .Select(a => new
            // {
            //     a.Date,
            //     a.Reason,
            //     a.From,
            //     a.To,
            //     MainCategory = a.MainCategory.Name,
            //     SecondaryCategory = a.SecondaryCategory.Name,
            //     a.Amount,
            //     a.Bank,
            //     a.Cash
            // })
            // .ToListAsync();

            //var pays = await Context.Context.Payments
            //.Select(a => new
            //{
            //    a.Date,
            //    Reason = a.Customer.Name + " " + a.Customer.SureName,
            //    From = a.Date, // No equivalent in Payments
            //    To = a.Date, // No equivalent in Payments
            //    MainCategory = "Γυμναστήριο",
            //    SecondaryCategory = a.Program.ProgramTypeO.ProgramName,
            //    a.Amount,
            //    Bank = 0m, // Assuming no Bank value in Payments
            //    Cash = 0m  // Assuming no Cash value in Payments
            //})
            //.ToListAsync();
            //inc.AddRange(pays);

            //inc = inc.OrderBy(a => a.Date).ToList();

            //mixedData.Add("Incomes", inc);

            //var outc = await Context.Context.Expenses
            //    .Where(r => !r.Income)
            //.Select(a => new
            //{
            //    a.Date,
            //    a.Reason,
            //    a.From,
            //    a.To,
            //    MainCategory = a.MainCategory.Name,
            //    SecondaryCategory = a.SecondaryCategory.Name,
            //    a.Amount,
            //    a.Bank,
            //    a.Cash
            //})
            //.ToListAsync();

            //mixedData.Add("Expenses", outc);

            //var gh = await Context.Context.GymnastHours
            //    .Include(a => a.Gymnast)
            //    .ToListAsync();

            //var su = await Context.Context.Apointments.Where(c => c.Customer != null && !c.Waiting && !c.Canceled)
            //    .Select(s =>
            //        new AppointmentInfo
            //        {
            //            Id = s.Customer.Id,
            //            CustomerName = s.Customer.Name + " " + s.Customer.SureName,
            //            DateTime = s.DateTime,
            //            Aithusa = s.Room + "",
            //            Cost = s.Cost,
            //            Gymnast = ""
            //        })
            //    .ToListAsync();

            //su.ForEach(s =>
            //{
            //    s.Gymnast = gh.FirstOrDefault(g => g.Datetime == s.DateTime)?.Gymnast?.Name ?? "";
            //});

            //mixedData.Add("Appointments", su);
            //await ExportMixedDictionaryToExcelAsync(mixedData, "Export.xlsx");
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public static async Task ExportMixedDictionaryToExcelAsync(Dictionary<string, object> dataDictionary, string fileName)
        {
            if (dataDictionary == null || !dataDictionary.Any())
                throw new ArgumentException("The data dictionary is empty or null.");

            // Get the user's Downloads folder
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fullPath = Path.Combine(downloadsPath, fileName);

            using (var spreadsheetDocument = SpreadsheetDocument.Create(fullPath, SpreadsheetDocumentType.Workbook))
            {
                // Create the workbook
                var workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = CreateStylesheet();
                stylesPart.Stylesheet.Save();

                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                uint sheetId = 1;

                // Add two empty sheets before processing the dictionary
                for (int i = 1; i <= 2; i++)
                {
                    var emptyWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    emptyWorksheetPart.Worksheet = new Worksheet(new SheetData());

                    var emptySheet = new Sheet
                    {
                        Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(emptyWorksheetPart),
                        SheetId = sheetId++,
                        Name = $"Sheet{i}"
                    };
                    sheets.Append(emptySheet);
                }

                foreach (var kvp in dataDictionary)
                {
                    string sheetName = kvp.Key;
                    var data = kvp.Value;

                    if (data == null)
                        continue;

                    var dataType = data.GetType();
                    if (!dataType.IsGenericType || ((dataType.GetGenericTypeDefinition() != typeof(IEnumerable<>)) & dataType.GetGenericTypeDefinition() != typeof(List<>)))
                        throw new ArgumentException($"Data for sheet {sheetName} is not a valid IEnumerable<T>.");

                    var itemType = dataType.GetGenericArguments()[0];
                    var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // Create a new worksheet part
                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    worksheetPart.Worksheet = new Worksheet(sheetData);

                    // Add the worksheet to the workbook
                    var sheet = new Sheet
                    {
                        Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = sheetId++,
                        Name = sheetName
                    };
                    sheets.Append(sheet);

                    // Write headers
                    var headerRow = new Row();
                    var maxColumnLengths = new int[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        var header = properties[i].Name;
                        headerRow.Append(CreateCell(header, CellValues.String));
                        maxColumnLengths[i] = header.Length; // Initialize max length with header length
                    }
                    sheetData.Append(headerRow);

                    // Write data rows
                    foreach (var item in (IEnumerable<object>)data)
                    {
                        var dataRow = new Row();
                        for (int i = 0; i < properties.Length; i++)
                        {
                            var value = properties[i].GetValue(item);
                            if (value is DateTime dateValue)
                            {
                                dataRow.Append(CreateCell(dateValue.ToOADate().ToString(CultureInfo.InvariantCulture), CellValues.Number, 1));
                                maxColumnLengths[i] = Math.Max(maxColumnLengths[i], 12); // Adjust for DateTime width
                            }
                            else if (value is int || value is double || value is decimal)
                            {
                                dataRow.Append(CreateCell(Convert.ToString(value, CultureInfo.InvariantCulture), CellValues.Number));
                            }
                            else
                            {
                                var stringValue = value?.ToString() ?? "";
                                if (string.IsNullOrEmpty(stringValue) && ((properties[i].Name == "SecondaryCategory") || properties[i].Name == "MainCategory"))
                                {
                                    stringValue = "Others";
                                }

                                dataRow.Append(CreateCell(stringValue, CellValues.String));
                                maxColumnLengths[i] = Math.Max(maxColumnLengths[i], stringValue.Length);
                            }
                        }
                        sheetData.Append(dataRow);
                    }

                    // Adjust column widths
                    var columns = new Columns();
                    for (int i = 0; i < maxColumnLengths.Length; i++)
                    {
                        var columnWidth = Math.Min(maxColumnLengths[i] + 2, 50); // Limit width to 50
                        columns.Append(new Column
                        {
                            Min = (uint)(i + 1),
                            Max = (uint)(i + 1),
                            Width = columnWidth,
                            CustomWidth = true
                        });
                    }
                    worksheetPart.Worksheet.InsertAt(columns, 0);

                    worksheetPart.Worksheet.Save();
                }

                workbookPart.Workbook.Save();
            }

            await Task.CompletedTask; // For async signature
        }

        private static Cell CreateCell(string value, CellValues? dataType, uint? styleIndex = null)
        {
            if (dataType == null)
            {
                dataType = CellValues.String;
            }

            return new Cell
            {
                CellValue = new CellValue(value),
                DataType = dataType,
                StyleIndex = styleIndex
            };
        }

        private static Stylesheet CreateStylesheet()
        {
            var stylesheet = new Stylesheet();

            // Create a number format for dates (index 1)
            var numberFormats = new NumberingFormats();
            numberFormats.Append(new NumberingFormat
            {
                NumberFormatId = 1,
                FormatCode = "mm/dd/yyyy"
            });

            // Create a cell format for dates
            var cellFormats = new CellFormats();
            cellFormats.Append(new CellFormat()); // Default
            cellFormats.Append(new CellFormat
            {
                NumberFormatId = 1, // Date format
                ApplyNumberFormat = true
            });

            stylesheet.Append(numberFormats);
            stylesheet.Append(new Fonts(new Font())); // Default font
            stylesheet.Append(new Fills(new Fill(new PatternFill { PatternType = PatternValues.None }))); // Default fill
            stylesheet.Append(new Borders(new Border())); // Default border
            stylesheet.Append(cellFormats);

            return stylesheet;
        }

        public class AppointmentInfo
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public DateTime DateTime { get; set; }
            public string Aithusa { get; set; }
            public decimal Cost { get; set; }
            public string Gymnast { get; set; }
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

        internal async Task SaveAsync(bool cursor = true)
        {
            if (cursor)
                Mouse.OverrideCursor = Cursors.Wait;

            await Context.SaveAsync();
            if (cursor)
                Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion Methods
    }
}