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
    }

    [Serializable]
    public class ObjectToSerialize : ISerializable
    {
        private ObservableCollection<Account> accounts;

        public ObservableCollection<Account> Accounts
        {
            get { return this.accounts; }
            set { this.accounts = value; }
        }

        public ObjectToSerialize()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Accounts", Accounts);
        }
    }
}