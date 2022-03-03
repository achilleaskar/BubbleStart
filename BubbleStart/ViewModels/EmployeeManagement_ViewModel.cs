using BubbleStart.Helpers;
using BubbleStart.Messages;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BubbleStart.ViewModels
{
    public class EmployeeManagement_ViewModel : MyViewModelBase
    {
        #region Constructors

        public EmployeeManagement_ViewModel(BasicDataManager basicDataManager)
        {
            BasicDataManager = basicDataManager;
            NewRule = new WorkingRule();
            AddRuleCommand = new RelayCommand(async () => await AddRule(), CanAddRule);
            SaveWorkingRuleCommand = new RelayCommand(async () => await SaveWorkingRule(), CanSaveWorkingRule);
            ClearRuleCommand = new RelayCommand(ClearRule);
            Messenger.Default.Register<BasicDataManagerRefreshedMessage>(this, msg => Load());
            Messenger.Default.Register<UpdateShifts_Message>(this, msg => Load());
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<User> _Employees;

        private WorkingRule _NewRule;

        private User _SelectedEmployee;

        private WorkingRule _SelectedRule;

        private ObservableCollection<Shift> _Shifts;

        #endregion Fields

        #region Properties

        public RelayCommand AddRuleCommand { get; set; }

        public BasicDataManager BasicDataManager { get; }

        public RelayCommand ClearRuleCommand { get; set; }

        public ObservableCollection<User> Employees
        {
            get
            {
                return _Employees;
            }

            set
            {
                if (_Employees == value)
                {
                    return;
                }

                _Employees = value;
                RaisePropertyChanged();
            }
        }

        public bool HasSelection => SelectedRule != null && SelectedRule.Id > 0;

        public WorkingRule NewRule
        {
            get
            {
                return _NewRule;
            }

            set
            {
                if (_NewRule == value)
                {
                    return;
                }

                _NewRule = value;
                RaisePropertyChanged();
                if (value != null && value.Id == 0)
                    NewRule.SetUpWeek();
            }
        }

        public bool HasUser => SelectedEmployee != null;

        public RelayCommand SaveWorkingRuleCommand { get; set; }

        public User SelectedEmployee
        {
            get
            {
                return _SelectedEmployee;
            }

            set
            {
                if (_SelectedEmployee == value)
                {
                    return;
                }

                _SelectedEmployee = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasUser));
            }
        }

        public WorkingRule SelectedRule
        {
            get
            {
                return _SelectedRule;
            }

            set
            {
                if (_SelectedRule == value)
                {
                    return;
                }

                _SelectedRule = value;
                RaisePropertyChanged();

                RaisePropertyChanged(nameof(HasSelection));
            }
        }

        public ObservableCollection<Shift> Shifts
        {
            get
            {
                return _Shifts;
            }

            set
            {
                if (_Shifts == value)
                {
                    return;
                }

                _Shifts = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override void Load(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            Employees = new ObservableCollection<User>(BasicDataManager.Users.Where(e => e.Level == 4));

            Shifts = BasicDataManager.Shifts;
            RaisePropertyChanged(nameof(Shifts));
            NewRule.RaisePropertyChanged(nameof(NewRule.DailyWorkingShifts));
            NewRule.RaisePropertyChanged(null);
            foreach (var day in NewRule.DailyWorkingShifts)
            {
                day.RaisePropertyChanged(nameof(Shift));
            }
            if (SelectedRule != null)
                foreach (var day in SelectedRule.DailyWorkingShifts)
                {
                    day.RaisePropertyChanged(nameof(Shift));
                }
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        private async Task AddRule()
        {
            SelectedEmployee.WorkingRules.Add(NewRule);
            await BasicDataManager.SaveAsync();
            NewRule = new WorkingRule();
        }

        private bool CanAddRule()
        {
            return !NewRule.DailyWorkingShifts.Any(a => a.Shift == null) && NewRule.From >= DateTime.Today;
        }

        private bool CanContinue()
        {
            if (SelectedRule == null)
            {
                return false;
            }
            if (SelectedRule.Id == 0)
            {
                MessageBoxResult result = MessageBox.Show("Δέν έχετε αποθηκεύσει τον υπάρχον κανόνα. Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes)
                {
                    return false;
                }
                else
                    return true;
            }
            if (BasicDataManager.Context.HasChanges(SelectedRule))
            {
                MessageBoxResult result = MessageBox.Show("Δέν έχετε αποθηκεύσει τις αλλαγές στον υπάρχον κανόνα. Θέλετε σίγουρα να συνεχίσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes)
                {
                    return false;
                }
                else
                {
                    BasicDataManager.Context.RollBack(SelectedRule);
                    return true;
                }
            }
            return true;
        }

        private bool CanSaveWorkingRule()
        {
            return SelectedRule != null && !SelectedRule.DailyWorkingShifts.Any(a => a.Shift == null) && SelectedRule.From >= DateTime.Today && BasicDataManager.HasChanges();
        }

        private void ClearRule()
        {
            SelectedRule = new WorkingRule();
        }

        private async Task SaveWorkingRule()
        {
            await BasicDataManager.SaveAsync();
        }

        #endregion Methods
    }
}