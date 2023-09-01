using BubbleStart.Model;
using BubbleStart.Views;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BubbleStart.Helpers
{
    public static class StaticResources
    {
        public static Guid Guid { get; set; } = Guid.NewGuid();

        public static List<DateTime> GetHours(int minHour, int minMin, int maxHour, int maxMin, int interval)
        {
            List<DateTime> List = new List<DateTime>();
            DateTime tmpTime = new DateTime(2019, 1, 1, minHour, minMin, 0);
            DateTime maxTime = new DateTime(2019, 1, 1, maxHour, maxMin, 0);

            while (tmpTime <= maxTime)
            {
                List.Add(new DateTime(tmpTime.Ticks));
                tmpTime = tmpTime.AddMinutes(interval);
            }
            return List;
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        public static string GetDescription(Enum en)
        {
            if (en == null)
            {
                return "";
            }
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        public static User User { get; set; }

        internal static BasicDataManager context;

        //public static string[] Districts { get; set; } = { "Ευζώνων", "Λαογραφικό Μουσείο", "Μπότσαρη", "Πανόραμα", "Σχολή Τυφλων", "Φάληρο", "Άλλο" };
        public static List<int> Months { get; set; } = new List<int> { 0, 1, 2, 3,4,5, 6, 12 };

        public static CustomerManagement OpenWindow { get; internal set; }
        public static DateTime OneWeekBefore { get; internal set; } = DateTime.Today.AddDays(-7);

        //public static ObservableCollection<District> Districts { get; set; } = new ObservableCollection<District>();

        public static string DecimalToString(decimal value) => value.ToString("C2").Replace(".00", "").Replace(",00", "").Replace(" ", "");

        public static void PrintDatagrid(DataGrid dgrid,int par=0)
        {
            dgrid.SelectionMode = DataGridSelectionMode.Extended;
            dgrid.SelectAllCells();
            dgrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dgrid);
            string result = Clipboard.GetText();
            dgrid.UnselectAll();

            ExcelPackage p = new ExcelPackage();
            p.Workbook.Worksheets.Add("Αποτέλεσμα");
            ExcelWorksheet myWorksheet = p.Workbook.Worksheets.Last();

            int row = 1;
            int column = 1;
            int colCount = 0;

            var rows = result.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            if (rows.Count <= 1)
            {
                return;
            }

            bool first;
            var colWidths = dgrid.Columns.Where(c => c.Visibility == Visibility.Visible).Select(t => t.Width).ToList();
            foreach (var CurrRow in rows)
            {
                if (row > 1 && column == colCount + 1)
                {
                    column = 1;
                }
                if (column > 1)
                {
                    column--;
                    row--;
                    first = true;
                    foreach (var content in CurrRow.Split(new string[] { "\t" }, StringSplitOptions.None))
                    {
                        if (first)
                        {
                            myWorksheet.Cells[row, column].Value = myWorksheet.Cells[row, column].Value + "\r\n" + content;

                            first = false;
                        }
                        else
                        {
                            myWorksheet.Cells[row, column].Value = GetCellValue(content);
                        }
                        column++;
                    }
                }
                else
                {
                    if (row == 1)
                        foreach (var header in CurrRow.Split(new string[] { "\t" }, StringSplitOptions.None))
                        {
                            myWorksheet.Cells[row, column].Value = header;
                            if (header == "Σημείωση" || header == "Οδ. Ξενοδοχείου")
                            {
                                myWorksheet.Column(column).Style.WrapText = true;
                            }
                            myWorksheet.Column(column).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            myWorksheet.Column(column).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            myWorksheet.Cells[row, column].Style.Font.Bold = true;
                            myWorksheet.Column(column).Width = colWidths[column - 1].DisplayValue / 6.5;

                            column++;
                            colCount++;
                        }
                    else
                        foreach (var content in CurrRow.Split(new string[] { "\t" }, StringSplitOptions.None))
                        {
                            myWorksheet.Cells[row, column].Value = GetCellValue(content);
                            column++;
                        }
                }
                row++;
            }

            if (par==1)
            {
                myWorksheet.DeleteColumn(1);
                myWorksheet.DeleteColumn(12, 2);
                myWorksheet.DeleteColumn(4);
            }

            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Prints");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\tmpprint" + ".xlsx";

            FileInfo fileInfo = new FileInfo(path);
            p.SaveAs(fileInfo);
            Clipboard.Clear();
            ProcessStartInfo startInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            Process.Start(startInfo);
            dgrid.SelectionMode = DataGridSelectionMode.Single;
        }

        private static string GetCellValue(string content)
        {
            CultureInfo elGR = new CultureInfo("el-GR");
            if (content == "True")
            {
                return "Ναι";
            }
            if (content == "False")
            {
                return "Όχι";
            }
            if (DateTime.TryParseExact(content, "G", elGR,
                                             DateTimeStyles.None, out DateTime dateValue))
            {
                return dateValue.ToString("dd/MM/yy");
            }

            return content;
        }

        public static string ToGreek(string searchTerm)
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
    }
}