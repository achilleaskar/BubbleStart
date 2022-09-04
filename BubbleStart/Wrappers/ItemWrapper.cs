using BubbleStart.Model;

namespace BubbleStart.Wrappers
{
    public class ItemWrapper : ModelWrapper<Item>
    {
        public ItemWrapper() : this(new Item())
        {
        }

        public ItemWrapper(Item model) : base(model)
        {
            Title = "Το προϊόν";
        }

        public string Name//
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool Shop//
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}