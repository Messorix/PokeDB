using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class PokémonInstance
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public Pokémon Species { get; set; }
        public int Level { get; set; }
        public bool Gender { get; private set; }
        public Nature Nature { get; set; }
        public Ability Ability { get; private set; }
        public bool HA { get; private set; }
        public List<int> HealthPoints { get; private set; }
        public List<int> Attack { get; private set; }
        public List<int> Defense { get; private set; }
        public List<int> SpecialAttack { get; private set; }
        public List<int> SpecialDefense { get; private set; }
        public List<int> Speed { get; private set; }
        public bool Shiny { get; private set; }
        public bool Egg { get; private set; }


        //TO-DO Constructor and serializing methods
    }
}