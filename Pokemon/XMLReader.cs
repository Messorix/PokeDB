using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Pokemon
{
    public static class XMLReader
    {
        private static string URL = "http://www.mythixinteractive.nl/sql/PokeDB/";
        private static XmlDocument _doc = new XmlDocument();
        
        public static List<PokeType> ReadTypes()
        {
            List<PokeType> returnable = new List<PokeType>();
            _doc.Load(URL + "types.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Type").Count; x++ )
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("TypeName")[0].InnerText;
                string hex = current.GetElementsByTagName("HEX")[0].InnerText;

                PokeType temp = new PokeType(id, name, hex);

                returnable.Add(temp);
            }

            for (int x = 0; x < root.GetElementsByTagName("Type").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                PokeType currenttype = returnable[x];

                foreach (PokeType type in returnable)
                {
                    if (type.ID < 19)
                    {
                        currenttype.Damage.Add(new Damage(type, Convert.ToDouble(((XmlElement)root.ChildNodes[x]).GetElementsByTagName(type.Name)[0].InnerText) / 100));
                    }
                }
            }

            return returnable;
        }

        public static List<EggGroup> ReadEggGroups()
        {
            List<EggGroup> returnable = new List<EggGroup>();
            _doc.Load(URL + "egggroups.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("EggGroup").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("Name")[0].InnerText;

                EggGroup temp = new EggGroup(id, name);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<EvoMethod> ReadEvoMethod()
        {
            List<EvoMethod> returnable = new List<EvoMethod>();
            _doc.Load(URL + "evolutionways.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("EvolutionWay").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("Way")[0].InnerText;

                EvoMethod temp = new EvoMethod(id, name);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<Pocket> ReadPockets()
        {
            List<Pocket> returnable = new List<Pocket>();
            _doc.Load(URL + "pockets.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Pocket").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("Name")[0].InnerText;

                Pocket temp = new Pocket(id, name);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<Item> ReadItems()
        {
            List<Item> returnable = new List<Item>();
            _doc.Load(URL + "items.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Item").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string hex = current.GetElementsByTagName("HEX")[0].InnerText;
                string name = current.GetElementsByTagName("Name")[0].InnerText;
                int pocketID = Convert.ToInt32(current.GetElementsByTagName("Pocket")[0].InnerText);

                Item temp = new Item(id, hex, name, Database.GetPocket(pocketID));

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<GameVersion> ReadVersions()
        {
            List<GameVersion> returnable = new List<GameVersion>();
            _doc.Load(URL + "versions.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Version").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("GameVersion")[0].InnerText;

                GameVersion temp = new GameVersion(id, name);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<Nature> ReadNatures()
        {
            List<Nature> returnable = new List<Nature>();
            _doc.Load(URL + "natures.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Nature").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("Name")[0].InnerText;
                string increase = current.GetElementsByTagName("Increases")[0].InnerText;
                string decrease = current.GetElementsByTagName("Decreases")[0].InnerText;

                Nature temp = new Nature(id, name, increase, decrease);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<Ability> ReadAbilities()
        {
            List<Ability> returnable = new List<Ability>();
            _doc.Load(URL + "abilities.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Ability").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("Name")[0].InnerText;
                int nrpokes = Convert.ToInt32(current.GetElementsByTagName("NumberPokemon")[0].InnerText);
                string descr = current.GetElementsByTagName("Description")[0].InnerText;
                int gen = Convert.ToInt32(current.GetElementsByTagName("Gen")[0].InnerText);

                Ability temp = new Ability(id, name, nrpokes, descr, gen);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<Move> ReadMoves()
        {
            List<Move> returnable = new List<Move>();
            _doc.Load(URL + "moves.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Move").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string name = current.GetElementsByTagName("Name")[0].InnerText;
                PokeType type = Database.GetType(Convert.ToInt32(current.GetElementsByTagName("Type")[0].InnerText));
                Category cat = Database.GetCategory(current.GetElementsByTagName("Category")[0].InnerText);
                int pp = Convert.ToInt32(current.GetElementsByTagName("PP")[0].InnerText);
                string power = current.GetElementsByTagName("Power")[0].InnerText;
                string accuracy = current.GetElementsByTagName("Accuracy")[0].InnerText;

                Move temp = new Move(id, name, type, cat, pp, power, accuracy);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<MoveSet> ReadMoveSets()
        {
            List<MoveSet> returnable = new List<MoveSet>();
            _doc.Load(URL + "movesets.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("MoveSet").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("Pokemon")[0].InnerText);
                int version = Convert.ToInt32(current.GetElementsByTagName("VersionGroup")[0].InnerText);
                Move move = Database.GetMove(Convert.ToInt32(current.GetElementsByTagName("Move")[0].InnerText));
                MoveMethod method = Database.GetMoveMethod(Convert.ToInt32(current.GetElementsByTagName("Method")[0].InnerText));
                int lvl = Convert.ToInt32(current.GetElementsByTagName("Level")[0].InnerText);
                int order = Convert.ToInt32(current.GetElementsByTagName("Index")[0].InnerText);

                MoveSet temp = new MoveSet(id, version, move, method, lvl, order);

                returnable.Add(temp);
            }

            return returnable;
        }

        public static List<EvolutionTree> ReadEvoTrees()
        {
            List<int> test = new List<int>();

            List<EvolutionTree> returnable = new List<EvolutionTree>();
            _doc.Load(URL + "evotrees.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Instance").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                int treenumber = Convert.ToInt32(current.GetElementsByTagName("TreeNumber")[0].InnerText);
                int placeintree = Convert.ToInt32(current.GetElementsByTagName("PlaceInTree")[0].InnerText);
                EvoMethod evomethod = Database.GetEvoMethod(Convert.ToInt32(current.GetElementsByTagName("EvoWay")[0].InnerText));
                string condition = current.GetElementsByTagName("Condition")[0].InnerText;
                int level = Convert.ToInt32(current.GetElementsByTagName("Level")[0].InnerText);

                EvolutionTree et = new EvolutionTree(id, treenumber);
                EvolutionInstance ei = null;

                if (returnable.Count > 0)
                {
                    foreach (EvolutionTree temp in returnable)
                    {
                        if (et.TreeID == temp.TreeID)
                        {
                            et = temp;
                            break;
                        }
                    }
                }

                switch (evomethod.Method)
                {
                    case "Level":
                    case "Level At Time":
                    case "Level At Location":
                    case "Level With Friendship":
                    case "Level With Unique Condition":
                    case "Trade":
                    case "Trade for Specific Pokemon":
                        ei = new EvolutionInstance(id,
                                                    et.TreeID,
                                                    placeintree,
                                                    evomethod,
                                                    level,
                                                    condition);
                        break;
                    case "Level With Item":
                    case "Trade With Item":
                    case "Use Stone":
                        ei = new EvolutionInstance(id,
                                                    et.TreeID,
                                                    placeintree,
                                                    evomethod,
                                                    level,
                                                    condition,
                                                    Database.GetItem(Convert.ToInt32(current.GetElementsByTagName("Item")[0].InnerText)));
                        break;
                    case "Level With Move":
                        ei = new EvolutionInstance(id,
                                                    et.TreeID,
                                                    placeintree,
                                                    evomethod,
                                                    level,
                                                    condition,
                                                    Database.GetMove(Convert.ToInt32(current.GetElementsByTagName("Move")[0].InnerText)));
                        break;
                    case "Egg":
                        ei = new EvolutionInstance(id,
                                                    et.TreeID,
                                                    placeintree,
                                                    evomethod);
                        break;
                }

                if (!Database.CheckIfAdded(et, ei)) { et.Evolutions.Add(ei); }
                if (!returnable.Contains(et)) { returnable.Add(et); }

                foreach (Pokémon p in Database.Pokedex)
                {
                    if (p.EvoInstanceID == id)
                    {
                        p.AddEvolutionInstance(ei);
                    }
                }
            }

            return returnable;
        }

        public static List<Pokémon> ReadPokedex()
        {
            List<Pokémon> returnable = new List<Pokémon>();
            _doc.Load(URL + "pokedex.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("Pokemon").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                string natnr = current.GetElementsByTagName("NationalNr")[0].InnerText;
                string name = current.GetElementsByTagName("Name")[0].InnerText;
                int height = Convert.ToInt32(current.GetElementsByTagName("Height")[0].InnerText);
                int weight = Convert.ToInt32(current.GetElementsByTagName("Weight")[0].InnerText);
                string johtonr = current.GetElementsByTagName("JohtoNr")[0].InnerText;
                string hoennnr = current.GetElementsByTagName("HoennNr")[0].InnerText;
                string sinnohnr = current.GetElementsByTagName("SinnohNr")[0].InnerText;
                string unovanr = current.GetElementsByTagName("UnovaNr")[0].InnerText;
                string kcenr = current.GetElementsByTagName("KalosCentralNr")[0].InnerText;
                string kconr = current.GetElementsByTagName("KalosCoastalNr")[0].InnerText;
                string kmonr = current.GetElementsByTagName("KalosMountainNr")[0].InnerText;

                bool caught = Convert.ToBoolean(Convert.ToInt32(current.GetElementsByTagName("Caught")[0].InnerText));

                int bhp = Convert.ToInt32(current.GetElementsByTagName("HP")[0].InnerText);
                int batt = Convert.ToInt32(current.GetElementsByTagName("ATT")[0].InnerText);
                int bdef = Convert.ToInt32(current.GetElementsByTagName("DEF")[0].InnerText);
                int bspatt = Convert.ToInt32(current.GetElementsByTagName("SPATT")[0].InnerText);
                int bspdef = Convert.ToInt32(current.GetElementsByTagName("SPDEF")[0].InnerText);
                int bspeed = Convert.ToInt32(current.GetElementsByTagName("SPEED")[0].InnerText);

                PokeType mtype = Database.GetType(Convert.ToInt32(current.GetElementsByTagName("MType")[0].InnerText));
                PokeType otype = Database.GetType(Convert.ToInt32(current.GetElementsByTagName("OType")[0].InnerText));

                EggGroup egg1 = Database.GetEggGroup(Convert.ToInt32(current.GetElementsByTagName("EggGroup1")[0].InnerText));
                EggGroup egg2 = Database.GetEggGroup(Convert.ToInt32(current.GetElementsByTagName("EggGroup2")[0].InnerText));

                string forme = current.GetElementsByTagName("Forme")[0].InnerText;
                int instanceid = Convert.ToInt32(current.GetElementsByTagName("EvoInstanceID")[0].InnerText);
                int gen = Convert.ToInt32(current.GetElementsByTagName("Gen")[0].InnerText);
                DateTime update = Convert.ToDateTime(current.GetElementsByTagName("Updated")[0].InnerText);

                Pokémon temp = new Pokémon(caught, natnr, name, height, weight, johtonr, hoennnr, sinnohnr, unovanr, kcenr, kconr, kmonr, 
                                            bhp, batt, bdef, bspatt, bspdef, bspeed, mtype, otype, egg1, egg2, forme, instanceid, gen, update);

                returnable.Add(temp);
            }

            return returnable;
        }

        internal static List<MoveMethod> ReadMoveMethods()
        {
            List<MoveMethod> returnable = new List<MoveMethod>();
            _doc.Load(URL + "movemethods.xml");
            XmlElement root = _doc.DocumentElement;
            XmlElement current;

            for (int x = 0; x < root.GetElementsByTagName("MoveMethod").Count; x++)
            {
                current = (XmlElement)root.ChildNodes[x];

                int id = Convert.ToInt32(current.GetElementsByTagName("ID")[0].InnerText);
                string method = current.GetElementsByTagName("Method")[0].InnerText;

                MoveMethod temp = new MoveMethod(id, method);

                returnable.Add(temp);
            }

            return returnable;
        }
    }
}