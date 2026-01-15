using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Wrappers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    internal class ExpenseCategoriesManagement_Viewmodel : AddEditBase<ExpenseCategoryClassWrapper, ExpenseCategoryClass>
    {
        private const int IncomeCategoryParentId = 20;

        public ExpenseCategoriesManagement_Viewmodel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Κατηγοριών Εξόδων/Εσόδων";
        }

        public override void AddedItem(ExpenseCategoryClass entity, bool removed)
        {
            base.AddedItem(entity, removed);
            if (entity is ExpenseCategoryClass e)
            {
                var wrapper = MainCollection.FirstOrDefault(w => w.Model == e);
                
                if (removed)
                {
                    if (Parents.Contains(e))
                    {
                        Parents.Remove(e);
                    }
                    if (wrapper != null)
                    {
                        ExpenseCategories.Remove(wrapper);
                        IncomeCategories.Remove(wrapper);
                    }
                }
                else
                {
                    Parents.Add(e);
                    if (wrapper != null)
                    {
                        if (e.ParentId == IncomeCategoryParentId)
                            IncomeCategories.Add(wrapper);
                        else
                            ExpenseCategories.Add(wrapper);
                    }
                }
            }
        }

        private ObservableCollection<ExpenseCategoryClassWrapper> _ExpenseCategories;
        public ObservableCollection<ExpenseCategoryClassWrapper> ExpenseCategories
        {
            get => _ExpenseCategories;
            set
            {
                if (_ExpenseCategories == value)
                    return;
                _ExpenseCategories = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ExpenseCategoryClassWrapper> _IncomeCategories;
        public ObservableCollection<ExpenseCategoryClassWrapper> IncomeCategories
        {
            get => _IncomeCategories;
            set
            {
                if (_IncomeCategories == value)
                    return;
                _IncomeCategories = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ExpenseCategoryClass> _ExpenseParents;
        public ObservableCollection<ExpenseCategoryClass> ExpenseParents
        {
            get => _ExpenseParents;
            set
            {
                if (_ExpenseParents == value)
                    return;
                _ExpenseParents = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<ExpenseCategoryClass> _IncomeParents;
        public ObservableCollection<ExpenseCategoryClass> IncomeParents
        {
            get => _IncomeParents;
            set
            {
                if (_IncomeParents == value)
                    return;
                _IncomeParents = value;
                RaisePropertyChanged();
            }
        }

        private bool _IsIncomeTabSelected;
        public bool IsIncomeTabSelected
        {
            get => _IsIncomeTabSelected;
            set
            {
                if (_IsIncomeTabSelected == value)
                    return;
                _IsIncomeTabSelected = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CurrentParents));
            }
        }

        public ObservableCollection<ExpenseCategoryClass> CurrentParents => IsIncomeTabSelected ? IncomeParents : ExpenseParents;

        public ObservableCollection<ExpenseCategoryClass> Parents { get; set; }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            var allCategories = Context.ExpenseCategoryClasses.Where(p => p.Id > 1).Select(p => new ExpenseCategoryClassWrapper(p)).ToList();
            MainCollection = new ObservableCollection<ExpenseCategoryClassWrapper>(allCategories);
            
            // Split into expense and income categories
            ExpenseCategories = new ObservableCollection<ExpenseCategoryClassWrapper>(allCategories.Where(p => p.ParentId != IncomeCategoryParentId && p.Id != IncomeCategoryParentId));
            IncomeCategories = new ObservableCollection<ExpenseCategoryClassWrapper>(allCategories.Where(p => p.ParentId == IncomeCategoryParentId));
            
            // Parents for expense categories (exclude income parent)
            ExpenseParents = new ObservableCollection<ExpenseCategoryClass>(Context.ExpenseCategoryClasses.Where(p => (p.Parent == null || p.ParentId == 1) && p.Id != IncomeCategoryParentId));
            
            // Parents for income categories (only income parent)
            IncomeParents = new ObservableCollection<ExpenseCategoryClass>(Context.ExpenseCategoryClasses.Where(p => p.Id == IncomeCategoryParentId));
            
            Parents = new ObservableCollection<ExpenseCategoryClass>(Context.ExpenseCategoryClasses.Where(p => p.Parent == null || p.ParentId == 1));
        }

        public override async Task ReloadAsync()
        {
            await LoadAsync();
        }

        public override async void SaveChanges()
        {
            var selectedCategory = SelectedEntity;
            var wasIncomeCategory = selectedCategory?.Model != null && selectedCategory.ParentId == IncomeCategoryParentId;
            
            base.SaveChanges();

            // Move category between lists if parent changed
            if (selectedCategory != null)
            {
                var isNowIncomeCategory = selectedCategory.ParentId == IncomeCategoryParentId;
                
                if (wasIncomeCategory != isNowIncomeCategory)
                {
                    if (isNowIncomeCategory)
                    {
                        ExpenseCategories.Remove(selectedCategory);
                        if (!IncomeCategories.Contains(selectedCategory))
                            IncomeCategories.Add(selectedCategory);
                    }
                    else
                    {
                        IncomeCategories.Remove(selectedCategory);
                        if (!ExpenseCategories.Contains(selectedCategory))
                            ExpenseCategories.Add(selectedCategory);
                    }
                }
            }
        }
    }
}