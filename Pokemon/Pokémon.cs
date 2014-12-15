using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Pokemon
{
    [Serializable]
    public class Pokémon
    {
        public string NationalNumber { get; private set; }
        public string Name { get; set; }

        public int Height { get; private set; }
        public int Weight { get; private set; }

        public string JohtoNumber { get; private set; }
        public string SinnohNumber { get; private set; }
        public string HoennNumber { get; private set; }
        public string UnovaNumber { get; private set; }
        public string KalosCentralNumber { get; private set; }
        public string KalosCoastalNumber { get; private set; }
        public string KalosMountainNumber { get; private set; }

        public int BaseHP { get; private set; }
        public int BaseAtt { get; private set; }
        public int BaseDef { get; private set; }
        public int BaseSpAtt { get; private set; }
        public int BaseSpDef { get; private set; }
        public int BaseSpeed { get; private set; }

        public PokeType MainType { get; private set; }
        public PokeType SecondType { get; private set; }

        public EggGroup MainEggGroup { get; private set; }
        public EggGroup SecondEggGroup { get; private set; }

        public string Forme { get; private set; }
        public bool Caught { get; private set; }
        public int Gen { get; private set; }
        public int EvoInstanceID { get; private set; }

        public DateTime Updated { get; private set; }

        public EvolutionInstance EvolutionInstance { get; private set; }

        public Pokémon(bool c, string natnr, string n, int h, int w, string johnr, string sinnr, string hoenr, string unonr, string kcennr,
                       string kcoanr, string kmounr, int bhp, int batt, int bdef, int bspa, int bspd, int bspe, PokeType mtype,
                       PokeType stype, EggGroup megg, EggGroup segg, string f, int e, int g, DateTime u)
        {
            NationalNumber = natnr;
            Name = n;

            Height = h;
            Weight = w;

            JohtoNumber = johnr;
            SinnohNumber = sinnr;
            HoennNumber = hoenr;
            UnovaNumber = unonr;
            KalosCentralNumber = kcennr;
            KalosCoastalNumber = kcoanr;
            KalosMountainNumber = kmounr;

            BaseHP = bhp;
            BaseAtt = batt;
            BaseDef = bdef;
            BaseSpAtt = bspa;
            BaseSpDef = bspd;
            BaseSpeed = bspe;

            MainType = mtype;
            SecondType = stype;

            MainEggGroup = megg;
            SecondEggGroup = segg;

            Forme = f;
            Caught = c;
            Gen = g;
            EvoInstanceID = e;

            Updated = u;
        }

        public Pokémon(SerializationInfo info, StreamingContext ctxt)
        {
            NationalNumber = (string)info.GetValue("NationalNumber", typeof(string));
            Name = (string)info.GetValue("Name", typeof(string));
            Height = (int)info.GetValue("Height", typeof(int));
            Weight = (int)info.GetValue("Weight", typeof(int));
            JohtoNumber = (string)info.GetValue("JohtoNumber", typeof(string));
            SinnohNumber = (string)info.GetValue("SinnohNumber", typeof(string));
            HoennNumber = (string)info.GetValue("HoennNumber", typeof(string));
            UnovaNumber = (string)info.GetValue("UnovaNumber", typeof(string));
            KalosCentralNumber = (string)info.GetValue("KalosCentralNumber", typeof(string));
            KalosCoastalNumber = (string)info.GetValue("KalosCoastalNumber", typeof(string));
            KalosMountainNumber = (string)info.GetValue("KalosMountainNumber", typeof(string));
            BaseHP = (int)info.GetValue("BaseHP", typeof(int));
            BaseAtt = (int)info.GetValue("BaseAtt", typeof(int));
            BaseDef = (int)info.GetValue("BaseDef", typeof(int));
            BaseSpAtt = (int)info.GetValue("BaseSpAtt", typeof(int));
            BaseSpDef = (int)info.GetValue("BaseSpDef", typeof(int));
            BaseSpeed = (int)info.GetValue("BaseSpeed", typeof(int));
            MainType = (PokeType)info.GetValue("MainType", typeof(PokeType));
            SecondType = (PokeType)info.GetValue("SecondType", typeof(PokeType));
            MainEggGroup = (EggGroup)info.GetValue("MainEggGroup", typeof(EggGroup));
            SecondEggGroup = (EggGroup)info.GetValue("SecondEggGroup", typeof(EggGroup));
            Forme = (string)info.GetValue("Forme", typeof(string));
            Caught = (bool)info.GetValue("Caught", typeof(bool));
            Gen = (int)info.GetValue("Gen", typeof(int));
            EvoInstanceID = (int)info.GetValue("EvoInstanceID", typeof(int));
            Updated = (DateTime)info.GetValue("Updated", typeof(DateTime));
            EvolutionInstance = (EvolutionInstance)info.GetValue("EvolutionInstance", typeof(EvolutionInstance));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("NationalNumber", NationalNumber);
            info.AddValue("Name", Name);
            info.AddValue("Height", Height);
            info.AddValue("Weight", Weight);
            info.AddValue("JohtoNumber", JohtoNumber);
            info.AddValue("SinnohNumber", SinnohNumber);
            info.AddValue("HoennNumber", HoennNumber);
            info.AddValue("UnovaNumber", UnovaNumber);
            info.AddValue("KalosCentralNumber", KalosCentralNumber);
            info.AddValue("KalosCoastalNumber", KalosCoastalNumber);
            info.AddValue("KalosMountainNumber", KalosMountainNumber);
            info.AddValue("BaseHP", BaseHP);
            info.AddValue("BaseAtt", BaseAtt);
            info.AddValue("BaseDef", BaseDef);
            info.AddValue("BaseSpAtt", BaseSpAtt);
            info.AddValue("BaseSpDef", BaseSpDef);
            info.AddValue("BaseSpeed", BaseSpeed);
            info.AddValue("MainType", MainType);
            info.AddValue("SecondType", SecondType);
            info.AddValue("MainEggGroup", MainEggGroup);
            info.AddValue("SecondEggGroup", SecondEggGroup);
            info.AddValue("Forme", Forme);
            info.AddValue("Caught", Caught);
            info.AddValue("Gen", Gen);
            info.AddValue("EvoInstanceID", EvoInstanceID);
            info.AddValue("Updated", Updated);
            info.AddValue("EvolutionInstance", EvolutionInstance);
        }

        public void AddEvolutionInstance(EvolutionInstance t)
        {
            EvolutionInstance = t;
        }
    }
}
