namespace BubbleStart.Model
{
    public class Illness : BaseModel
    {
        #region Fields

        private bool _Arthritida;
        private bool _Asthma;
        private bool _Diavitis;
        private bool _Emminopafsi;
        private bool _Epilipsia;
        private bool _Kardia;
        private bool _Karkinos;
        private bool _Katagma;
        private bool _NevrikiVlavi;
        private bool _Osteoporosi;
        private bool _Piesi;

        private bool _Stomaxika;

        private bool _Thiroidis;

        private string _Travmatismos;

        #endregion Fields

        #region Properties

        public bool Arthritida
        {
            get
            {
                return _Arthritida;
            }

            set
            {
                if (_Arthritida == value)
                {
                    return;
                }

                _Arthritida = value;
                RaisePropertyChanged();
            }
        }

        public bool Asthma
        {
            get
            {
                return _Asthma;
            }

            set
            {
                if (_Asthma == value)
                {
                    return;
                }

                _Asthma = value;
                RaisePropertyChanged();
            }
        }

        public bool Diavitis
        {
            get
            {
                return _Diavitis;
            }

            set
            {
                if (_Diavitis == value)
                {
                    return;
                }

                _Diavitis = value;
                RaisePropertyChanged();
            }
        }

        public bool Emminopafsi
        {
            get
            {
                return _Emminopafsi;
            }

            set
            {
                if (_Emminopafsi == value)
                {
                    return;
                }

                _Emminopafsi = value;
                RaisePropertyChanged();
            }
        }

        public bool Epilipsia
        {
            get
            {
                return _Epilipsia;
            }

            set
            {
                if (_Epilipsia == value)
                {
                    return;
                }

                _Epilipsia = value;
                RaisePropertyChanged();
            }
        }

        public bool Kardia
        {
            get
            {
                return _Kardia;
            }

            set
            {
                if (_Kardia == value)
                {
                    return;
                }

                _Kardia = value;
                RaisePropertyChanged();
            }
        }

        public bool Karkinos
        {
            get
            {
                return _Karkinos;
            }

            set
            {
                if (_Karkinos == value)
                {
                    return;
                }

                _Karkinos = value;
                RaisePropertyChanged();
            }
        }

        public bool Katagma
        {
            get
            {
                return _Katagma;
            }

            set
            {
                if (_Katagma == value)
                {
                    return;
                }

                _Katagma = value;
                RaisePropertyChanged();
            }
        }

        public bool NevrikiVlavi
        {
            get
            {
                return _NevrikiVlavi;
            }

            set
            {
                if (_NevrikiVlavi == value)
                {
                    return;
                }

                _NevrikiVlavi = value;
                RaisePropertyChanged();
            }
        }

        public bool Osteoporosi
        {
            get
            {
                return _Osteoporosi;
            }

            set
            {
                if (_Osteoporosi == value)
                {
                    return;
                }

                _Osteoporosi = value;
                RaisePropertyChanged();
            }
        }

        public bool Piesi
        {
            get
            {
                return _Piesi;
            }

            set
            {
                if (_Piesi == value)
                {
                    return;
                }

                _Piesi = value;
                RaisePropertyChanged();
            }
        }

        public bool Stomaxika
        {
            get
            {
                return _Stomaxika;
            }

            set
            {
                if (_Stomaxika == value)
                {
                    return;
                }

                _Stomaxika = value;
                RaisePropertyChanged();
            }
        }

        public bool Thiroidis
        {
            get
            {
                return _Thiroidis;
            }

            set
            {
                if (_Thiroidis == value)
                {
                    return;
                }

                _Thiroidis = value;
                RaisePropertyChanged();
            }
        }

        public string Travmatismos
        {
            get
            {
                return _Travmatismos;
            }

            set
            {
                if (_Travmatismos == value)
                {
                    return;
                }

                _Travmatismos = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}