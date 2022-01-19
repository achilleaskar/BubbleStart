using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    internal class ExpenseCategoriesManagement_Viewmodel : AddEditBase<ExpenseCategoryClassWrapper, ExpenseCategoryClass>
    {
        public ExpenseCategoriesManagement_Viewmodel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Κατηγοριών εξόδων";
        }

        public override void AddedItem(ExpenseCategoryClass entity,bool removed)
        {
            base.AddedItem(entity,removed);
            if (entity is ExpenseCategoryClass e)
            {
                if (removed)
                {
                    if (Parents.Contains(e))
                    {
                        Parents.Remove(e);
                    }
                }
                else
                {
                    Parents.Add(e);
                }
            }
        }

        public ObservableCollection<ExpenseCategoryClass> Parents { get; set; }
        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<ExpenseCategoryClassWrapper>(Context.ExpenseCategoryClasses.Where(p => p.Id > 1).Select(p => new ExpenseCategoryClassWrapper(p)));
            Parents = new ObservableCollection<ExpenseCategoryClass>(Context.ExpenseCategoryClasses.Where(p => p.Parent == null || p.ParentId == 1));
        }

        public override async Task ReloadAsync()
        {
            MainCollection = new ObservableCollection<ExpenseCategoryClassWrapper>(Context.ExpenseCategoryClasses.Where(p => p.Id > 1).Select(p => new ExpenseCategoryClassWrapper(p)));
            Parents = new ObservableCollection<ExpenseCategoryClass>(Context.ExpenseCategoryClasses.Where(p => p.Parent == null || p.ParentId == 1));
        }
    }
}