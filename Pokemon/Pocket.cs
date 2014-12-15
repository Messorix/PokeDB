using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class Pocket
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public Pocket(int i, string n)
        {
            ID = i;
            Name = n;
        }

        public Pocket(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Name = (string)info.GetValue("Name", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Name", Name);
        }
    }
}
