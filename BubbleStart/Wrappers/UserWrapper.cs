using BubbleStart.Model;

namespace BubbleStart.Wrappers
{
    public class UserWrapper : ModelWrapper<User>
    {
        #region Constructors

        public UserWrapper() : this(new User())
        {
        }

        public UserWrapper(User model) : base(model)
        {
            Title = "Ο χρήστης";
        }

        #endregion Constructors

        #region Properties

        public byte[] HashedPassword//
        {
            get => GetValue<byte[]>();
            set => SetValue(value);
        }

        public int Level//
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Name//
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Surename//
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Tel//
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string UserName//
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return UserName;
        }

        #endregion Methods
    }
}