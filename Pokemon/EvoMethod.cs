using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class EvoMethod
    {
        public int ID { get; private set; }
        public string Method { get; private set; }

        public EvoMethod(int i, string m)
        {
            ID = i;
            Method = m;
        }

        public EvoMethod(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Method = (string)info.GetValue("Method", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Method", Method);
        }
    }
}
