using BubbleStart.Database;
using BubbleStart.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BubbleStart.ViewModels
{
    public class EconomicData_ViewModel : MyViewModelBase
    {
        #region Constructors

        public EconomicData_ViewModel(GenericRepository context)
        {
            Context = context;
            ShowCashDataCommand = new RelayCommand(async () => { await ShowCashData(); });
            StartDateCash = DateTime.Today;
        }




        private float _Sum;


        public float Sum
        {
            get
            {
                return _Sum;
            }

            set
            {
                if (_Sum == value)
                {
                    return;
                }

                _Sum = value;
                RaisePropertyChanged();
            }
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<Payment> _DailyPayments;

        private DateTime _EndDateCash;

        private DateTime _StartDateCash;

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; }

        public ObservableCollection<Payment> DailyPayments
        {
            get
            {
                return _DailyPayments;
            }

            set
            {
                if (_DailyPayments == value)
                {
                    return;
                }

                _DailyPayments = value;
                RaisePropertyChanged();
            }
        }

        public DateTime EndDateCash
        {
            get
            {
                return _EndDateCash;
            }

            set
            {
                if (_EndDateCash == value)
                {
                    return;
                }

                _EndDateCash = value;
                if (_StartDateCash > _EndDateCash)
                {
                    _StartDateCash = EndDateCash;
                }
                RaisePropertyChanged();
            }
        }

        public RelayCommand ShowCashDataCommand { get; set; }

        public DateTime StartDateCash
        {
            get
            {
                return _StartDateCash;
            }

            set
            {
                if (_StartDateCash == value)
                {
                    return;
                }

                _StartDateCash = value;

                if (_StartDateCash > _EndDateCash)
                {
                    EndDateCash = _StartDateCash;
                }
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            await Task.Delay(0);
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        private async Task ShowCashData()
        {
            Sum = 0;
            DailyPayments = new ObservableCollection<Payment>(await Context.GetAllPaymentsAsync(StartDateCash, EndDateCash));
            foreach (var p in DailyPayments)
            {
                Sum += p.Amount;
            }
        }

        #endregion Methods
    }
}