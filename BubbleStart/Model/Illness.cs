using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BubbleStart.Model
{
    public class Illness : BaseModel
    {
        #region Fields
        private string _afxenas;

        private string _agonasA;

        private string _agonasD;

        private bool _allergia;
        private string _allergiaText;
        private bool _Arthritida;

        private string _arthritidaText;
        private bool _Asthma;

        private string _asthmaText;
        private string _cancerText;
        private string _DaxtylaA;

        private string _DaxtylaD;

        private bool _Diavitis;

        private string _diavitisText;
        private bool _Eggymosini;

        private string _eggymosynhText;
        private string _emmhnopafshText;
        private bool _Emminopafsi;

        private bool _Epilipsia;

        private string _epilipsiaText;
        private string _GonatoA;

        private string _gonatoD;

        private string _isxioA;

        private string _IsxioD;

        private bool _Kardia;

        private string _kardiaText;
        private bool _Karkinos;

        private string _karposD;

        private string _karsposA;

        private bool _Katagma;

        private string _KatagmaText;
        private string _mesh;

        private bool _NevrikiVlavi;

        private string _nevrikoText;
        private bool _omoiopathitiki;
        private string _omosA;

        private string _omosd;

        private string _opoiopathitikiText;
        private bool _Osteoporosi;

        private string _osteoporosiText;
        private string _pieshText;
        private bool _Piesi;

        private bool _PiesiXamili;
        private string _PiesiXamiliText;
        private string _PodoknhmhA;

        private string _podoknhmhD;

        private string _SelectedIllnessPropertyName;

        private bool _Stomaxika;

        private string _stomaxikaText;
        private bool _Thiroidis;

        private string _thiroidisText;
        private string _thorakas;

        private bool _travmatismos;

        private string _travmatismosText;

        #endregion Fields

        #region Properties

        [DisplayName("ΑΜΣΣ")]
        public string afxenas
        {
            get => _afxenas;

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
            get => _agonasA;

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
            get => _agonasD;

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
            get => _allergia;

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

        public string allergiaText
        {
            get => _allergiaText;

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
            get => _Arthritida;

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

        public string arthritidaText
        {
            get => _arthritidaText;

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
            get => _Asthma;

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

        public string asthmaText
        {
            get => _asthmaText;

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

        public string cancerText
        {
            get => _cancerText;

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

        [DisplayName("Δάχτυλα Αριστερού Χεριού")]
        public string DaxtylaA
        {
            get => _DaxtylaA;

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
            get => _DaxtylaD;

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
            get => _Diavitis;

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

        public string diavitisText
        {
            get => _diavitisText;

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
            get => _Eggymosini;

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

        public string eggymosynhText
        {
            get => _eggymosynhText;

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

        public string emmhnopafshText
        {
            get => _emmhnopafshText;

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

        public bool Emminopafsi
        {
            get => _Emminopafsi;

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
            get => _Epilipsia;

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

        public string epilipsiaText
        {
            get => _epilipsiaText;

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
            get => _GonatoA;

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
            get => _gonatoD;

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
            get => _isxioA;

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
            get => _IsxioD;

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
            get => _Kardia;

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

        public string kardiaText
        {
            get => _kardiaText;

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
            get => _Karkinos;

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

        [DisplayName("Δεξιός Καρπός")]
        public string karposD
        {
            get => _karposD;

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
            get => _karsposA;

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
            get => _Katagma;

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

        public string KatagmaText
        {
            get => _KatagmaText;

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
            get => _mesh;

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
            get => _NevrikiVlavi;

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

        public string nevrikoText
        {
            get => _nevrikoText;

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
            get => _omoiopathitiki;

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

        [DisplayName("Αριστερός Ώμος")]
        public string omosA
        {
            get => _omosA;

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
            get => _omosd;

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

        public Dictionary<string, string> Problems => GetProblems();

        public string opoiopathitikiText
        {
            get => _opoiopathitikiText;

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

        public bool Osteoporosi
        {
            get => _Osteoporosi;

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

        public string osteoporosiText
        {
            get => _osteoporosiText;

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

        public string pieshText
        {
            get => _pieshText;

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

        public bool Piesi
        {
            get => _Piesi;

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

        public bool PiesiXamili
        {
            get => _PiesiXamili;

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

        public string PiesiXamiliText
        {
            get => _PiesiXamiliText;

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
            get => _PodoknhmhA;

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
            get => _podoknhmhD;

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

            set => GetType().GetProperty(SelectedIllnessPropertyName).SetValue(this, value);
        }

        [NotMapped]
        public string SelectedIllnessPropertyName
        {
            get => _SelectedIllnessPropertyName;

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

        public string SeleptedPropertyFriendlyName => SelectedIllnessPropertyName != null ? (GetType().GetProperty(SelectedIllnessPropertyName).GetCustomAttributes(typeof(DisplayNameAttribute), false)[0] as DisplayNameAttribute).DisplayName : "Keno";

        public bool Stomaxika
        {
            get => _Stomaxika;

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

        public string stomaxikaText
        {
            get => _stomaxikaText;

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
            get => _Thiroidis;

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

        public string thiroidisText
        {
            get => _thiroidisText;

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
            get => _thorakas;

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
            get => _travmatismos;

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

        public string travmatismosText
        {
            get => _travmatismosText;

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

        #region Methods

        private Dictionary<string, string> GetProblems()
        {
            string tmpValue;
            Dictionary<string, string> tmpDict = new Dictionary<string, string>();
            foreach (var p in GetType().GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(DisplayNameAttribute))))
            {
                tmpValue = (string)GetType().GetProperty(p.Name).GetValue(this);
                if (!string.IsNullOrEmpty(tmpValue))
                {
                    tmpDict.Add((GetType().GetProperty(p.Name).GetCustomAttributes(typeof(DisplayNameAttribute), false)[0] as DisplayNameAttribute).DisplayName, tmpValue);
                }
            }
            return tmpDict;
        }

        #endregion Methods
    }
}