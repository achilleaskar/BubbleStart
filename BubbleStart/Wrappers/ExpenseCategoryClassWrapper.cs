using BubbleStart.Model;

namespace BubbleStart.Wrappers
{
    internal class ExpenseCategoryClassWrapper : ModelWrapper<ExpenseCategoryClass>
    {
        public ExpenseCategoryClassWrapper() : base(new ExpenseCategoryClass())
        {
            Title = "Η κατηγορία";
        }
        public ExpenseCategoryClassWrapper(ExpenseCategoryClass model) : base(model)
        {
            Title = "Η κατηγορία";
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ExpenseCategoryClass Parent
        {
            get { return GetValue<ExpenseCategoryClass>(); }
            set { SetValue(value); }
        }

        public int? ParentId
        {
            get { return GetValue<int?>(); }
            set { SetValue(value); }
        }
    }
}