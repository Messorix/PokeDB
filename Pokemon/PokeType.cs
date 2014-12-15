using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class PokeType : ISerializable
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string HEX { get; private set; }
        public List<Damage> Damage { get; private set; }

        public PokeType(int id, string n, string h)
        {
            ID = id;
            Name = n;
            HEX = h;
            Damage = new List<Damage>();
        }
        
        public PokeType(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Name = (string)info.GetValue("Name", typeof(string));
            HEX = (string)info.GetValue("HEX", typeof(string));
            Damage = (List<Damage>)info.GetValue("Damage", typeof(List<Damage>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Name", Name);
            info.AddValue("HEX", HEX);
            info.AddValue("Damage", Damage);
        }
    }
}
