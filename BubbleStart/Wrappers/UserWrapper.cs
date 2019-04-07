using BubbleStart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            get { return GetValue<byte[]>(); }
            set { SetValue(value); }
        }

        public int Level//
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Name//
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Surename//
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Tel//
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string UserName//
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
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