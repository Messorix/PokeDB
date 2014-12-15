using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class Item
    {
        public int ID { get; private set; }
        public string HEX { get; private set; }
        public string Name { get; private set; }
        public Pocket Pocket { get; private set; }

        public Item (int i, string h, string n, Pocket p)
        {
            ID = i;
            HEX = h;
            Name = n;
            Pocket = p;
        }

        public Item(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            HEX = (string)info.GetValue("HEX", typeof(string));
            Name = (string)info.GetValue("Name", typeof(string));
            Pocket = (Pocket)info.GetValue("Pocket", typeof(Pocket));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("HEX", HEX);
            info.AddValue("Name", Name);
            info.AddValue("Pocket", Pocket);
        }
    }
}
