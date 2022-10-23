using BubbleStart.Helpers;
using BubbleStart.Model;

namespace BubbleStart.Wrappers
{
    public class ProgramTypeWrapper : ModelWrapper<ProgramType>
    {
        public ProgramTypeWrapper() : base(new ProgramType())
        {
            Title = "Το ωράριο";
        }

        public ProgramTypeWrapper(ProgramType model) : base(model)
        {
            Title = "Το ωράριο";
        }

        public string ProgramName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ProgramMode? ProgramMode
        {
            get { return GetValue<ProgramMode?>(); }
            set { SetValue(value); }
        }

        public string CategoryText => StaticResources.GetDescription(ProgramMode);
    }
}