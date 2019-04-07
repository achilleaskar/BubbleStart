using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleStart.Model
{
    public class Illness : BaseModel
    {
        #region Fields

        private string _afxenas;

        private string _agonasA;

        private string _agonasD;

        private bool _allergia;
        private bool _Arthritida;

        private bool _Asthma;

        private string _DaxtylaA;

        private string _DaxtylaD;

        private bool _Diavitis;

        private bool _Eggymosini;

        private bool _Emminopafsi;

        private bool _Epilipsia;

        private string _GonatoA;

        private string _gonatoD;

        private string _isxioA;

        private string _IsxioD;

        private bool _Kardia;

        private bool _Karkinos;

        private string _karposD;

        private string _karsposA;

        private bool _Katagma;

        private string _mesh;

        private bool _NevrikiVlavi;

        private bool _omoiopathitiki;
        private string _omosA;

        private string _omosd;

        private bool _Osteoporosi;

        private bool _Piesi;

        private bool _PiesiXamili;
        private string _PodoknhmhA;

        private string _podoknhmhD;

        private string _SelectedIllnessPropertyName;

        private bool _Stomaxika;

        private bool _Thiroidis;

        private string _thorakas;

        private bool _travmatismos;

        #endregion Fields

        #region Properties

        [DisplayName("ΑΜΣΣ")]
        public string afxenas
        {
            get
            {
                return _afxenas;
            }

            set
            {
                if (_afxenas == value)
                {
                    return;
                }

                _afxenas = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Αριστερός Αγκώνας")]
        public string agonasA
        {
            get
            {
                return _agonasA;
            }

            set
            {
                if (_agonasA == value)
                {
                    return;
                }

                _agonasA = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Δεξιός Αγκώνας")]
        public string agonasD
        {
            get
            {
                return _agonasD;
            }

            set
            {
                if (_agonasD == value)
                {
                    return;
                }

                _agonasD = value;
                RaisePropertyChanged();
            }
        }

        public bool allergia
        {
            get
            {
                return _allergia;
            }

            set
            {
                if (_allergia == value)
                {
                    return;
                }

                _allergia = value;
                RaisePropertyChanged();
            }
        }




        private string _allergiaText;


        public string allergiaText
        {
            get
            {
                return _allergiaText;
            }

            set
            {
                if (_allergiaText == value)
                {
                    return;
                }

                _allergiaText = value;
                RaisePropertyChanged();
            }
        }

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




        private string _arthritidaText;


        public string arthritidaText
        {
            get
            {
                return _arthritidaText;
            }

            set
            {
                if (_arthritidaText == value)
                {
                    return;
                }

                _arthritidaText = value;
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




        private string _asthmaText;


        public string asthmaText
        {
            get
            {
                return _asthmaText;
            }

            set
            {
                if (_asthmaText == value)
                {
                    return;
                }

                _asthmaText = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Δάχτυλα Αριστερού Χεριού")]
        public string DaxtylaA
        {
            get
            {
                return _DaxtylaA;
            }

            set
            {
                if (_DaxtylaA == value)
                {
                    return;
                }

                _DaxtylaA = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Δάχτυλα Δεξιού Χεριού")]
        public string DaxtylaD
        {
            get
            {
                return _DaxtylaD;
            }

            set
            {
                if (_DaxtylaD == value)
                {
                    return;
                }

                _DaxtylaD = value;
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



        private string _diavitisText;


        public string diavitisText
        {
            get
            {
                return _diavitisText;
            }

            set
            {
                if (_diavitisText == value)
                {
                    return;
                }

                _diavitisText = value;
                RaisePropertyChanged();
            }
        }

        public bool Eggymosini
        {
            get
            {
                return _Eggymosini;
            }

            set
            {
                if (_Eggymosini == value)
                {
                    return;
                }

                _Eggymosini = value;
                RaisePropertyChanged();
            }
        }




        private string _eggymosynhText;


        public string eggymosynhText
        {
            get
            {
                return _eggymosynhText;
            }

            set
            {
                if (_eggymosynhText == value)
                {
                    return;
                }

                _eggymosynhText = value;
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




        private string _emmhnopafshText;


        public string emmhnopafshText
        {
            get
            {
                return _emmhnopafshText;
            }

            set
            {
                if (_emmhnopafshText == value)
                {
                    return;
                }

                _emmhnopafshText = value;
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




        private string _epilipsiaText;


        public string epilipsiaText
        {
            get
            {
                return _epilipsiaText;
            }

            set
            {
                if (_epilipsiaText == value)
                {
                    return;
                }

                _epilipsiaText = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Αριστερό Γόνατο")]
        public string GonatoA
        {
            get
            {
                return _GonatoA;
            }

            set
            {
                if (_GonatoA == value)
                {
                    return;
                }

                _GonatoA = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Δεξί Γόνατο")]
        public string gonatoD
        {
            get
            {
                return _gonatoD;
            }

            set
            {
                if (_gonatoD == value)
                {
                    return;
                }

                _gonatoD = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Αριστερό Ισχίο")]
        public string isxioA
        {
            get
            {
                return _isxioA;
            }

            set
            {
                if (_isxioA == value)
                {
                    return;
                }

                _isxioA = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Δεξί Ισχίο")]
        public string IsxioD
        {
            get
            {
                return _IsxioD;
            }

            set
            {
                if (_IsxioD == value)
                {
                    return;
                }

                _IsxioD = value;
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




        private string _kardiaText;


        public string kardiaText
        {
            get
            {
                return _kardiaText;
            }

            set
            {
                if (_kardiaText == value)
                {
                    return;
                }

                _kardiaText = value;
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





        private string _cancerText;


        public string cancerText
        {
            get
            {
                return _cancerText;
            }

            set
            {
                if (_cancerText == value)
                {
                    return;
                }

                _cancerText = value;
                RaisePropertyChanged();
            }
        }
        [DisplayName("Δεξιός Καρπός")]
        public string karposD
        {
            get
            {
                return _karposD;
            }

            set
            {
                if (_karposD == value)
                {
                    return;
                }

                _karposD = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Αριστερός Καρπός")]
        public string karsposA
        {
            get
            {
                return _karsposA;
            }

            set
            {
                if (_karsposA == value)
                {
                    return;
                }

                _karsposA = value;
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




        private string _KatagmaText;


        public string KatagmaText
        {
            get
            {
                return _KatagmaText;
            }

            set
            {
                if (_KatagmaText == value)
                {
                    return;
                }

                _KatagmaText = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("ΟΜΣΣ")]
        public string mesh
        {
            get
            {
                return _mesh;
            }

            set
            {
                if (_mesh == value)
                {
                    return;
                }

                _mesh = value;
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



        private string _nevrikoText;


        public string nevrikoText
        {
            get
            {
                return _nevrikoText;
            }

            set
            {
                if (_nevrikoText == value)
                {
                    return;
                }

                _nevrikoText = value;
                RaisePropertyChanged();
            }
        }

        public bool omoiopathitiki
        {
            get
            {
                return _omoiopathitiki;
            }

            set
            {
                if (_omoiopathitiki == value)
                {
                    return;
                }

                _omoiopathitiki = value;
                RaisePropertyChanged();
            }
        }




        private string _opoiopathitikiText;


        public string opoiopathitikiText
        {
            get
            {
                return _opoiopathitikiText;
            }

            set
            {
                if (_opoiopathitikiText == value)
                {
                    return;
                }

                _opoiopathitikiText = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Αριστερός Ώμος")]
        public string omosA
        {
            get
            {
                return _omosA;
            }

            set
            {
                if (_omosA == value)
                {
                    return;
                }

                _omosA = value;
                RaisePropertyChanged();
            }
        }
        [DisplayName("Δεξιός Ώμος")]
        public string omosd
        {
            get
            {
                return _omosd;
            }

            set
            {
                if (_omosd == value)
                {
                    return;
                }

                _omosd = value;
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






        private string _osteoporosiText;


        public string osteoporosiText
        {
            get
            {
                return _osteoporosiText;
            }

            set
            {
                if (_osteoporosiText == value)
                {
                    return;
                }

                _osteoporosiText = value;
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




        private string _pieshText;


        public string pieshText
        {
            get
            {
                return _pieshText;
            }

            set
            {
                if (_pieshText == value)
                {
                    return;
                }

                _pieshText = value;
                RaisePropertyChanged();
            }
        }



        public bool PiesiXamili
        {
            get
            {
                return _PiesiXamili;
            }

            set
            {
                if (_PiesiXamili == value)
                {
                    return;
                }

                _PiesiXamili = value;
                RaisePropertyChanged();
            }
        }




        private string _PiesiXamiliText;


        public string PiesiXamiliText
        {
            get
            {
                return _PiesiXamiliText;
            }

            set
            {
                if (_PiesiXamiliText == value)
                {
                    return;
                }

                _PiesiXamiliText = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Αριστερή Ποδοκνήμη")]
        public string PodoknhmhA
        {
            get
            {
                return _PodoknhmhA;
            }

            set
            {
                if (_PodoknhmhA == value)
                {
                    return;
                }

                _PodoknhmhA = value;
                RaisePropertyChanged();
            }
        }

        [DisplayName("Δεξιά Ποδοκνήμη")]
        public string podoknhmhD
        {
            get
            {
                return _podoknhmhD;
            }

            set
            {
                if (_podoknhmhD == value)
                {
                    return;
                }

                _podoknhmhD = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public string SelectedIllness
        {
            get
            {
                if (SelectedIllnessPropertyName != null)
                {
                    return (string)GetType().GetProperty(SelectedIllnessPropertyName).GetValue(this);
                }
                return null;
            }

            set
            {
                GetType().GetProperty(SelectedIllnessPropertyName).SetValue(this, value);
            }
        }

        [NotMapped]
        public string SelectedIllnessPropertyName
        {
            get
            {
                return _SelectedIllnessPropertyName;
            }

            set
            {
                if (_SelectedIllnessPropertyName == value)
                {
                    return;
                }

                _SelectedIllnessPropertyName = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedIllness));
                RaisePropertyChanged(nameof(SeleptedPropertyFriendlyName));
            }
        }

        public string SeleptedPropertyFriendlyName => SelectedIllnessPropertyName != null ?
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    (GetType().GetProperty(SelectedIllnessPropertyName).GetCustomAttributes(typeof(DisplayNameAttribute), false)[0] as DisplayNameAttribute).DisplayName : "Keno";

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




        private string _stomaxikaText;


        public string stomaxikaText
        {
            get
            {
                return _stomaxikaText;
            }

            set
            {
                if (_stomaxikaText == value)
                {
                    return;
                }

                _stomaxikaText = value;
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



        private string _thiroidisText;


        public string thiroidisText
        {
            get
            {
                return _thiroidisText;
            }

            set
            {
                if (_thiroidisText == value)
                {
                    return;
                }

                _thiroidisText = value;
                RaisePropertyChanged();
            }
        }
        [DisplayName("ΘΜΣΣ")]
        public string thorakas
        {
            get
            {
                return _thorakas;
            }

            set
            {
                if (_thorakas == value)
                {
                    return;
                }

                _thorakas = value;
                RaisePropertyChanged();
            }
        }

        public bool travmatismos
        {
            get
            {
                return _travmatismos;
            }

            set
            {
                if (_travmatismos == value)
                {
                    return;
                }

                _travmatismos = value;
                RaisePropertyChanged();
            }
        }



        private string _travmatismosText;


        public string travmatismosText
        {
            get
            {
                return _travmatismosText;
            }

            set
            {
                if (_travmatismosText == value)
                {
                    return;
                }

                _travmatismosText = value;
                RaisePropertyChanged();
            }
        }
        #endregion Properties
    }
}