using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class Damage
    {
        public PokeType Type { get; private set; }
        public double DamageNr { get; private set; }

        public Damage (PokeType t, double d)
        {
            Type = t;
            DamageNr = d;
        }

        public Damage(SerializationInfo info, StreamingContext ctxt)
        {
            Type = (PokeType)info.GetValue("Type", typeof(PokeType));
            DamageNr = (double)info.GetValue("DamageNr", typeof(double));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Type", Type);
            info.AddValue("DamageNr", DamageNr);
        }
    }
}
