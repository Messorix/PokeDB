using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pokemon
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public void SerializeAccounts(string filename, List<Account> objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public void SerializePokedex(string filename, List<Pokémon> objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public void SerializeEvoTrees(string filename, List<EvolutionTree> objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public List<Account> DeSerializeAccounts(string filename)
        {
            List<Account> objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (List<Account>)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }

        public List<Pokémon> DeSerializePokedex(string filename)
        {
            List<Pokémon> objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (List<Pokémon>)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }

        public List<EvolutionTree> DeSerializeEvoTrees(string filename)
        {
            List<EvolutionTree> objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (List<EvolutionTree>)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}