using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon
{
    [Serializable]
    public class MoveSet
    {
        public int PokemonID { get; private set; }
        public int Version { get; private set; }
        public Move Move { get; private set; }
        public MoveMethod Method { get; private set; }
        public int Level { get; private set; }
        public int Order { get; private set;}

        public MoveSet(int p, int v, Move mo, MoveMethod me, int l, int o)
        {
            PokemonID = p;
            Version = v;
            Move = mo;
            Method = me;
            Level = l;
            Order = o;
        }

        public MoveSet(SerializationInfo info, StreamingContext ctxt)
        {
            PokemonID = (int)info.GetValue("PokemonID", typeof(int));
            Version = (int)info.GetValue("Version", typeof(int));
            Move = (Move)info.GetValue("Move", typeof(Move));
            Method = (MoveMethod)info.GetValue("Method", typeof(MoveMethod));
            Level = (int)info.GetValue("Level", typeof(int));
            Order = (int)info.GetValue("Order", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("PokemonID", PokemonID);
            info.AddValue("Version", Version);
            info.AddValue("Move", Move);
            info.AddValue("Method", Method);
            info.AddValue("Level", Level);
            info.AddValue("Order", Order);
        }
    }
}
