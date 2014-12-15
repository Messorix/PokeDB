using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class Account : ISerializable
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public GameVersion Version { get; private set; }
        public string Friendcode { get; private set; }
        public PokeType SafariType { get; private set; }
        public List<Pokémon> SafariPokes { get; private set; }
        public List<PokémonInstance> Pokemons { get; set; }

        public Account (int i, string n, GameVersion v, string f, PokeType st, List<Pokémon> sp)
        {
            ID = i;
            Name = n;
            Version = v;
            Friendcode = f;
            SafariType = st;
            SafariPokes = sp;
        }

        public Account(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Name = (string)info.GetValue("Name", typeof(string));
            Version = (GameVersion)info.GetValue("Version", typeof(GameVersion));
            Friendcode = (string)info.GetValue("Friendcode", typeof(string));
            SafariType = (PokeType)info.GetValue("SafariType", typeof(PokeType));
            SafariPokes = (List<Pokémon>)info.GetValue("SafariPokes", typeof(List<Pokémon>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Name", Name);
            info.AddValue("Version", Version);
            info.AddValue("Friendcode", Friendcode);
            info.AddValue("SafariType", SafariType);
            info.AddValue("SafariPokes", SafariPokes);
        }
    }
}
