using BubbleStart.Helpers;
using GalaSoft.MvvmLight;

namespace BubbleStart.Model
{
    public class BodyPartSelection : ObservableObject
    {
        private bool _Selected;

        public bool Selected
        {
            get
            {
                return _Selected;
            }

            set
            {
                if (_Selected == value)
                {
                    return;
                }

                _Selected = value;
                RaisePropertyChanged();
            }
        }

        private SecBodyPart _SecBodyPart;

        public SecBodyPart SecBodyPart
        {
            get
            {
                return _SecBodyPart;
            }

            set
            {
                if (_SecBodyPart == value)
                {
                    return;
                }

                _SecBodyPart = value;
                RaisePropertyChanged();
            }
        }
    }
}