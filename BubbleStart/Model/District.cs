namespace BubbleStart.Model
{
    public class District : BaseModel
    {
        private string _Name;

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