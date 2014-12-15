using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Pokemon
{
    [Serializable]
    public class Nature
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public double Att { get; private set; }
        public double Def { get; private set; }
        public double SpAtt { get; private set; }
        public double SpDef { get; private set; }
        public double Sp { get; private set; }

        public Nature (int id, string n, string i, string d)
        {
            ID = id;
            Name = n;
            Att = 1.0;
            Def = 1.0;
            SpAtt = 1.0;
            SpDef = 1.0;
            Sp = 1.0;

            if (i.CompareTo(d) != 0)
            {
                switch (i)
                {
                    case "Attack":
                        Att = 1.1;
                        break;
                    case "Defense":
                        Def = 1.1;
                        break;
                    case "Sp. Atk":
                        SpAtt = 1.1;
                        break;
                    case "Sp. Def":
                        SpDef = 1.1;
                        break;
                    case "Speed":
                        Sp = 1.1;
                        break;
                }

                switch (d)
                {
                    case "Attack":
                        Att = 0.9;
                        break;
                    case "Defense":
                        Def = 0.9;
                        break;
                    case "Sp. Atk":
                        SpAtt = 0.9;
                        break;
                    case "Sp. Def":
                        SpDef = 0.9;
                        break;
                    case "Speed":
                        Sp = 0.9;
                        break;
                }
            }
        }

        public Nature(SerializationInfo info, StreamingContext ctxt)
        {
            ID = (int)info.GetValue("ID", typeof(int));
            Name = (string)info.GetValue("Name", typeof(string));
            Att = (double)info.GetValue("Att", typeof(double));
            Def = (double)info.GetValue("Def", typeof(double));
            SpAtt = (double)info.GetValue("SpAtt", typeof(double));
            SpDef = (double)info.GetValue("SpDef", typeof(double));
            Sp = (double)info.GetValue("Sp", typeof(double));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ID", ID);
            info.AddValue("Name", Name);
            info.AddValue("Att", Att);
            info.AddValue("Def", Def);
            info.AddValue("SpAtt", SpAtt);
            info.AddValue("SpDef", SpDef);
            info.AddValue("Sp", Sp);
        }
    }
}
