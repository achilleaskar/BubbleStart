using BubbleStart.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleStart.Model
{
    [Table("InProgramTypes")]
    public class ProgramType : BaseModel
    {



        private string _ProgramName;


        public string ProgramName
        {
            get
            {
                return _ProgramName;
            }

            set
            {
                if (_ProgramName == value)
                {
                    return;
                }

                _ProgramName = value;
                RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            return ProgramName;
        }


        private ProgramMode _ProgramMode;


        public ProgramMode ProgramMode
        {
            get
            {
                return _ProgramMode;
            }

            set
            {
                if (_ProgramMode == value)
                {
                    return;
                }

                _ProgramMode = value;
                RaisePropertyChanged();
            }
        }
    }
}
