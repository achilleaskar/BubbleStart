﻿using System;
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

        #endregion Fields

        #region Properties

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
            MainCollection = new ObservableCollection<UserWrapper>((await Context.Context.GetAllUsersAsyncSortedByUserName()).Select(u => new UserWrapper(u)));
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
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