using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Model
{
   public class Weight:BaseModel
    {



        private DateTime _DateOfMeasure;


        public DateTime DateOfMeasure
        {
            get
            {
                return _DateOfMeasure;
            }

            set
            {
                if (_DateOfMeasure == value)
                {
                    return;
                }

                _DateOfMeasure = value;
                RaisePropertyChanged();
            }
        }




        private float _WeightValue;


        public float WeightValue
        {
            get
            {
                return _WeightValue;
            }

            set
            {
                if (_WeightValue == value)
                {
                    return;
                }

                _WeightValue = value;
                RaisePropertyChanged();
            }
        }
    }
}
