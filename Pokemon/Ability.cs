using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class Ability
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int NumberOfPokemon { get; private set; }
        public string Description { get; private set; }
        public int Generation { get; private set; }

        public Ability(int i, string n, int p, string d, int g)
        {
            ID = i;
            Name = n;
            NumberOfPokemon = p;
            Description = d;
            Generation = g;
        }

        public Ability(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Name = (string)info.GetValue("Name", typeof(string));
            NumberOfPokemon = (int)info.GetValue("NumberOfPokemon", typeof(int));
            Description = (string)info.GetValue("Description", typeof(string));
            Generation = (int)info.GetValue("Generation", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Name", Name);
            info.AddValue("NumberOfPokemon", NumberOfPokemon);
            info.AddValue("Description", Description);
            info.AddValue("Generation", Generation);
        }
    }
}
