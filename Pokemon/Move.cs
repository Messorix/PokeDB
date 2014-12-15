using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class Move
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public PokeType Type { get; private set; }
        public Category Category { get; private set; }
        public int PP { get; private set; }
        public string Power { get; private set; }
        public string Accuracy { get; private set; }

        public Move (int i, string n, PokeType t, Category c, int pp, string p, string a)
        {
            ID = i;
            Name = n;
            Type = t;
            Category = c;
            PP = pp;
            Power = p;
            Accuracy = a;
        }

        public Move(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Name = (string)info.GetValue("Name", typeof(string));
            Type = (PokeType)info.GetValue("Type", typeof(PokeType));
            Category = (Category)info.GetValue("Category", typeof(Category));
            PP = (int)info.GetValue("PP", typeof(int));
            Power = (string)info.GetValue("Power", typeof(string));
            Accuracy = (string)info.GetValue("Accuracy", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Name", Name);
            info.AddValue("Type", Type);
            info.AddValue("Category", Category);
            info.AddValue("PP", PP);
            info.AddValue("Power", Power);
            info.AddValue("Accuracy", Accuracy);
        }
    }

    public enum Category
    {
        Physical,
        Special,
        Status
    }
}
