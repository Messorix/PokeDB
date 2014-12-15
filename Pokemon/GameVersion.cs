using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class GameVersion : ISerializable
    {
        public int ID { get; private set; }
        public string Version { get; private set; }

        public GameVersion(int i, string v)
        {
            ID = i;
            Version = v;
        }

        public GameVersion(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Version = (string)info.GetValue("Version", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Version", Version);
        }
    }
}
