using BubbleStart.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BubbleStart.Model
{
    [Table("BubbleWorkingRules")]
    public class WorkingRule : BaseModel
    {
        #region Constructors

        public WorkingRule()
        {
            DailyWorkingShifts = new ObservableCollection<DayWorkingShift>();
            From = StaticResources.GetNextWeekday(DateTime.Today.AddDays(1), DayOfWeek.Monday);
            To = From.AddDays(6);
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<DayWorkingShift> _DailyWorkingShifts;

        private DateTime _From;

        private DateTime _To;

        #endregion Fields

        #region Properties

        public ObservableCollection<DayWorkingShift> DailyWorkingShifts
        {
            get
            {
                return _DailyWorkingShifts;
            }

            set
            {
                if (_DailyWorkingShifts == value)
                {
                    return;
                }

                _DailyWorkingShifts = value;
                RaisePropertyChanged();
                DailyWorkingShifts.CollectionChanged += DailyWorkingShifts_CollectionChanged;
            }
        }

        private void DailyWorkingShifts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (DayWorkingShift item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DayWorkingShift item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        private void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Shift")
            {
                Shift prev = null;
                foreach (var item in DailyWorkingShifts)
                {
                    if (item.Shift != null)
                    {
                        prev = item.Shift;
                    }
                    else if (prev != null)
                    {
                        item.Shift = prev;
                    }
                }
            }
        }

        public DateTime From
        {
            get
            {
                return _From;
            }

            set
            {
                if (_From == value)
                {
                    return;
                }

                if (_From.Year > 2000)
                {
                    _From = value;
                    SetUpWeek();
                }
                else
                    _From = value;

                RaisePropertyChanged();
            }
        }

        public DateTime To
        {
            get
            {
                return _To;
            }

            set
            {
                if (_To == value)
                {
                    return;
                }
                if (_To.Year > 2000)
                {
                    _To = value;
                    SetUpWeek();
                }
                else
                    _To = value;
                RaisePropertyChanged();
            }
        }

        public User User { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{From.ToShortDateString()}-{To.ToShortDateString()}";
        }

        public void SetUpWeek()
        {
            if (Id>0)
            {
                return;
            }
            int startDay = (int)From.DayOfWeek;
            DateTime tmpDay = From;
            Dictionary<int, bool> tmpdict = new Dictionary<int, bool>();
            for (int i = 0; i < 7; i++)
            {
                tmpdict.Add(i, false);
            }
            int currentDay = startDay;

            do
            {
                tmpdict[(int)tmpDay.DayOfWeek] = true;
                tmpDay = tmpDay.AddDays(1);
                currentDay++;
            }
            while (tmpDay <= To && startDay != currentDay % 7);

            foreach (var day in tmpdict)
            {
                if (day.Value)
                {
                    if (!DailyWorkingShifts.Any(w => w.NumOfDay == day.Key))
                    {
                        DailyWorkingShifts.Add(new DayWorkingShift { NumOfDay = day.Key });
                    }
                }
                else if (!day.Value)
                {
                    if (DailyWorkingShifts.Any(w => w.NumOfDay == day.Key))
                    {
                        DailyWorkingShifts.Remove(DailyWorkingShifts.FirstOrDefault(w => w.NumOfDay == day.Key));
                    }
                }
            }

            DayWorkingShift tmp = DailyWorkingShifts.FirstOrDefault(r => r.NumOfDay == 0);
            if (tmp != null)
            {
                DailyWorkingShifts.Move(DailyWorkingShifts.IndexOf(tmp), DailyWorkingShifts.Count - 1);
            }
        }

        #endregion Methods
    }
}