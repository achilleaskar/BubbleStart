using BubbleStart.Model;

namespace BubbleStart.Wrappers
{
    public class DealWrapper : ModelWrapper<Deal>
    {
        public DealWrapper() : base(new Deal())
        {
            Title = "? ????????";
        }

        public DealWrapper(Deal model) : base(model)
        {
            Title = "? ????????";
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}
