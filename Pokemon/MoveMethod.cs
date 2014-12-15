using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class MoveMethod
    {
        public int ID { get; private set; }
        public string Method { get; private set; }

        public MoveMethod(int i, string m)
        {
            ID = i;
            Method = m;
        }

        public MoveMethod(SerializationInfo info, StreamingContext ctxt)
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
