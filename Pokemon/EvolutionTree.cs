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
        public List<EvolutionInstance> Evolutions { get; private set; }

        public EvolutionTree (int i)
        {
            ID = i;
            Evolutions = new List<EvolutionInstance>();
        }

        public EvolutionTree (SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Evolutions = (List<EvolutionInstance>)info.GetValue("Evolutions", typeof(List<EvolutionInstance>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Evolutions", Evolutions);
        }
    }
}
