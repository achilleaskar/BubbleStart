using System;
using System.ComponentModel.DataAnnotations;
using BubbleStart.Helpers;

namespace BubbleStart.Model
{
    public class Item : BaseModel
    {
        private string _Name;

        [StringLength(40)]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }
    }
}