using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Media;
using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Model;
using BubbleStart.Security;
using BubbleStart.Wrappers;

namespace BubbleStart.ViewModels
{
    public class UsersManagement_viewModel : AddEditBase<UserWrapper, User>
    {
        #region Constructors

        public UsersManagement_viewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Χρηστών";
            PassWord = new SecureString();
            PasswordRepeat = new SecureString();
        }

        #endregion Constructors

        #region Fields

        private SecureString _PassWord;

        private SecureString _PasswordRepeat;

        private ObservableCollection<User> _Users;

        private ObservableCollection<UserWrapper> _EnabledUsers;

        private ObservableCollection<UserWrapper> _DisabledUsers;

        #endregion Fields

        #region Properties

        public ObservableCollection<UserWrapper> EnabledUsers
        {
            get => _EnabledUsers;
            set
            {
                if (_EnabledUsers == value)
                    return;
                _EnabledUsers = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<UserWrapper> DisabledUsers
        {
            get => _DisabledUsers;
            set
            {
                if (_DisabledUsers == value)
                    return;
                _DisabledUsers = value;
                RaisePropertyChanged();
            }
        }

        public SecureString PassWord
        {
            get => _PassWord;

            set
            {
                if (_PassWord == value)
                {
                    return;
                }

                _PassWord = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the PasswordRepeat property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public SecureString PasswordRepeat
        {
            get => _PasswordRepeat;

            set
            {
                if (_PasswordRepeat == value)
                {
                    return;
                }
                _PasswordRepeat = value;
                if (ArePasswordsOk())
                {
                    SelectedEntity.HashedPassword = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PassWord));
                }
                RaisePropertyChanged();
            }
        }

        public override bool CanAddEntity()
        {
            return base.CanAddEntity() && ArePasswordsOk();
        }

        /// <summary>
        /// Sets and gets the Users property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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

        private bool ArePasswordsOk()
        {
            if (PassWord != null && PasswordRepeat != null)
            {
                byte[] enteredValueHash = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PassWord));
                byte[] enteredValueHash2 = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PasswordRepeat));

                return PasswordHashing.SequenceEquals(enteredValueHash, enteredValueHash2);
            }
            return false;
        }

        public override bool CanSaveChanges()
        {
            if (SelectedEntity == null)
            {
                SelectedEntity = new UserWrapper();
            }

            return Context.HasChanges();
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            var allUsers = (await Context.Context.GetAllUsersAsyncSortedByUserName()).Select(u => new UserWrapper(u)).ToList();
            MainCollection = new ObservableCollection<UserWrapper>(allUsers);
            EnabledUsers = new ObservableCollection<UserWrapper>(allUsers.Where(u => !u.Disabled));
            DisabledUsers = new ObservableCollection<UserWrapper>(allUsers.Where(u => u.Disabled));
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public override async void SaveChanges()
        {
            var selectedUser = SelectedEntity;
            var wasDisabled = selectedUser?.Model != null && Context.Context.Context.Entry(selectedUser.Model).OriginalValues.GetValue<bool>(nameof(User.Disabled));
            var isNowDisabled = selectedUser?.Disabled ?? false;

            base.SaveChanges();

            // Move user between lists if Disabled status changed
            if (selectedUser != null && wasDisabled != isNowDisabled)
            {
                if (isNowDisabled)
                {
                    EnabledUsers.Remove(selectedUser);
                    if (!DisabledUsers.Contains(selectedUser))
                        DisabledUsers.Add(selectedUser);
                }
                else
                {
                    DisabledUsers.Remove(selectedUser);
                    if (!EnabledUsers.Contains(selectedUser))
                        EnabledUsers.Add(selectedUser);
                }
            }
        }

        public override void AddedItem(User entity, bool removed)
        {
            base.AddedItem(entity, removed);
            
            var wrapper = MainCollection.FirstOrDefault(w => w.Model == entity);
            if (wrapper == null)
                return;

            if (removed)
            {
                EnabledUsers.Remove(wrapper);
                DisabledUsers.Remove(wrapper);
            }
            else
            {
                if (entity.Disabled)
                    DisabledUsers.Add(wrapper);
                else
                    EnabledUsers.Add(wrapper);
            }
        }

        #endregion Methods

        private Color? _SelectedColor;

        public override void SelectedEntityChanged()
        {
            if (!string.IsNullOrEmpty(SelectedEntity?.ColorHash))
            {
                SelectedColor = (Color)ColorConverter.ConvertFromString(SelectedEntity.ColorHash);
            }
            else
                SelectedColor = null;
        }
        public Color? SelectedColor
        {
            get
            {
                return _SelectedColor;
            }

            set
            {
                if (_SelectedColor == value)
                {
                    return;
                }
                if (SelectedEntity is UserWrapper u && value != null)
                {
                    u.ColorHash = value.ToString();
                }
                _SelectedColor = value;
                RaisePropertyChanged();
            }
        }
    }
}