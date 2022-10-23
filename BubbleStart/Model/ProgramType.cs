using BubbleStart.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleStart.Model
{
    [Table("InProgramTypes")]
    public class ProgramType : BaseModel
    {
        private string _ProgramName;

        //[StringLength(100, MinimumLength = 2)]
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

        [Required]
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