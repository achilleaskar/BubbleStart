using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using BubbleStart.Database;
using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using BubbleStart.Security;
using GalaSoft.MvvmLight.CommandWpf;

namespace BubbleStart.ViewModels
{
    public class LoginViewModel : MyViewModelBase
    {
        private readonly GenericRepository startingRepository;

        public LoginViewModel(BasicDataManager basicDataManager)
        {
            LoginCommand = new RelayCommand(async () => { await TryLogin(); }, CanLogin);
            PossibleUser = new User();
            BasicDataManager = basicDataManager;
        }

        private string _ErrorMessage;
        private SecureString _PasswordSecureString;
        private User _PossibleUser;


        public string ErrorMessage
        {
            get => _ErrorMessage;

            set
            {
                if (_ErrorMessage == value)
                {
                    return;
                }

                _ErrorMessage = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LoginCommand { get; set; }


        public SecureString PasswordSecureString
        {
            get => _PasswordSecureString;

            set
            {
                if (_PasswordSecureString == value)
                {
                    return;
                }

                _PasswordSecureString = value;
                RaisePropertyChanged();
            }
        }

        public User PossibleUser
        {
            get => _PossibleUser;

            set
            {
                if (_PossibleUser == value)
                {
                    return;
                }

                _PossibleUser = value;
                RaisePropertyChanged();
            }
        }

        public BasicDataManager BasicDataManager { get; }
 

        private bool CanLogin()
        {
            //Execution should only be possible if both Username and Password have been supplied
            if (IsLoaded && !string.IsNullOrWhiteSpace(PossibleUser.UserName) && PasswordSecureString != null && PasswordSecureString.Length > 0)
                return true;
            return false;
        }

        async private Task TryLogin()
        {
            try
            {
                //Search for the existance of the specified username, otherwise
                //set the relevant error message if the user is not found.
                Mouse.OverrideCursor = Cursors.Wait;
                User userFound = null;
                ErrorMessage = "Παρακαλώ περιμένετε...";

                userFound = await BasicDataManager.Context.FindUserAsync(PossibleUser.UserName.ToLower());
                if (userFound == null)
                {
                    ErrorMessage = "Δεν βρέθηκε χρηστης.";
                    return;
                }

                //User exists. Check if specified password matches the actual
                //password for this user stored in the database

                //Get the Hash of the entered data
                byte[] enteredValueHash = null;
                if (PasswordSecureString.Length > 0)
                {
                    enteredValueHash = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PasswordSecureString));
                }
                else
                {
                    ErrorMessage = "Παρακαλώ εισάγετε κωδικό.";
                    return;
                }

                if (!PasswordHashing.SequenceEquals(enteredValueHash, userFound.HashedPassword))
                {
                    ErrorMessage = "Λάθος κωδικός.";
                    return;
                }

                ErrorMessage = "Επιτυχής σύνδεση!";
                StaticResources.User = userFound;
                MessengerInstance.Send(new LoginLogOutMessage(true));
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}