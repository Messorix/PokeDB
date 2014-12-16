using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class EvolutionTree
    {
        public int ID { get; private set; }
        public int TreeID { get; private set; }
        public List<EvolutionInstance> Evolutions { get; private set; }

        public EvolutionTree (int i, int t)
        {
            ID = i;
            TreeID = t;
            Evolutions = new List<EvolutionInstance>();
        }

        public EvolutionTree (SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            TreeID = (int)info.GetValue("TreeID", typeof(int));
            Evolutions = (List<EvolutionInstance>)info.GetValue("Evolutions", typeof(List<EvolutionInstance>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("TreeID", TreeID);
            info.AddValue("Evolutions", Evolutions);
        }
    }
}
