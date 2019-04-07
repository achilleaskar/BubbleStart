using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleStart.Model
{
    [Table("BubblePayments")]
    public class Payment:BaseModel
    {
        public Payment()
        {
            Date = DateTime.Now;
        }


        private DateTime _Date;


        public DateTime Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date == value)
                {
                    return;
                }

                _Date = value;
                RaisePropertyChanged();
            }
        }



        private int _Amount;


        public int Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }
    }
}
