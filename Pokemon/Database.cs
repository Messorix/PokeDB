using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Net;
using System.Collections.Specialized;
using System.Windows;

namespace Pokemon
{
    public class Database
    {
        private static string _connstring;
        private static MySqlConnection _connection;
        private static MySqlDataReader _reader;
        private static MySqlCommand _command;

        private const string URL = "http://www.mythixinteractive.nl/sql/PokeDB/";
        private const string login = "login.php";
        private const string getAccount = "getAccount.php";
        private const string getTypes = "getTypes.php";
        private const string getEggGroups = "getEggGroups.php";
        private const string getEvoWays = "getEvoWays.php";
        private const string getEvoTrees = "getEvoTrees.php";
        private const string getPockets = "getPockets.php";
        private const string getItems = "getItems.php";
        private const string getVersions = "getVersions.php";
        private const string getNatures = "getNatures.php";
        private const string getAbilities = "getAbilities.php";
        private const string getMoves = "getMoves.php";
        private const string getMoveMethods = "getMoveMethods.php";
        private const string getMoveSets = "getMoveSets.php";
        private const string getPokedex = "getPokedex.php";

        private List<Pokémon> pokedex = new List<Pokémon>();
        private List<Pokémon> _dex = new List<Pokémon>();
        private static List<PokeType> Types;
        private static List<EggGroup> EggGroups;
        private static List<EvolutionTree> EvoTrees;
        private static List<EvoMethod> EvoMethods;
        private static List<Pocket> Pockets;
        private static List<Item> Items;
        private static List<Move> Moves;
        private static List<MoveMethod> MoveMethods;
        private static List<GameVersion> Versions;
        private static List<Nature> Natures;
        private static List<Ability> Abilities;

        public static List<Pokémon> Pokedex { get; set; }
        public static List<MoveSet> MoveSets { get; private set; }        

        public Database()
        {
            Connect();
        }

        private void SortEvoTrees()
        {
            foreach (EvolutionTree et in EvoTrees)
            {
                BubbleSort(et.Evolutions);
            }
        }
         
        // sort the items of an array using bubble sort
        public void BubbleSort(List<EvolutionInstance> ar)
        {
            for (int pass = 1; pass < ar.Count; pass++)
                for (int i = 0; i < ar.Count - 1; i++)
                    if (ar[i].Place > ar[i + 1].Place)
                        Swap(ar, i);
        }

        // swap two items of an array
        public void Swap(List<EvolutionInstance> ar, int first)
        {
            EvolutionInstance hold;
            hold = ar[first];
            ar[first] = ar[first + 1];
            ar[first + 1] = hold;
        }

        private static string GetConnectionString()
        {
            // To avoid storing the connection string in your code,  
            // you can retrieve it from a configuration file. 
            return "SERVER=db.mythixinteractive.nl;DATABASE=md328371db292785;UID=md328371db292785;PASSWORD=f7KAMJuu;";
        }

        public void Connect()
        {
            Types = new List<PokeType>();
            EggGroups = new List<EggGroup>();
            EvoTrees = new List<EvolutionTree>();
            EvoMethods = new List<EvoMethod>();
            Pockets = new List<Pocket>();
            Items = new List<Item>();
            Versions = new List<GameVersion>();
            Natures = new List<Nature>();
            Abilities = new List<Ability>();
            Moves = new List<Move>();
            MoveMethods = new List<MoveMethod>();
            MoveSets = new List<MoveSet>();
            Pokedex = new List<Pokémon>();

            #region Types
            string url = URL + getTypes;
            NameValueCollection formData = new NameValueCollection();

            WebClient webClient = new WebClient();

            byte[] responseBytes = webClient.UploadValues(url, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Types = XMLReader.ReadTypes();
            #endregion
            #region Egg Groups
            url = URL + getEggGroups;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            EggGroups = XMLReader.ReadEggGroups();
            #endregion
            #region Evolution Ways
            url = URL + getEvoWays;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            EvoMethods = XMLReader.ReadEvoMethod();
            #endregion
            #region Pockets
            url = URL + getPockets;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Pockets = XMLReader.ReadPockets();
            #endregion
            #region Items
            url = URL + getItems;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Items = XMLReader.ReadItems();
            #endregion
            #region Versions
            url = URL + getVersions;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Versions = XMLReader.ReadVersions();
            #endregion
            #region Natures
            url = URL + getNatures;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Natures = XMLReader.ReadNatures();
            #endregion
            #region Abilities
            url = URL + getAbilities;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Abilities = XMLReader.ReadAbilities();
            #endregion
            #region Moves
            url = URL + getMoves;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Moves = XMLReader.ReadMoves();
            #endregion
            #region MoveMethods
            url = URL + getMoveMethods;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            MoveMethods = XMLReader.ReadMoveMethods();
            #endregion
            #region MoveSets
            url = URL + getMoveSets;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            MoveSets = XMLReader.ReadMoveSets();
            #endregion
        }

        public static void LoadPokedex()
        {
            #region Pokedex
            string url = URL + getPokedex;
            NameValueCollection formData = new NameValueCollection();

            WebClient webClient = new WebClient();

            byte[] responseBytes = webClient.UploadValues(url, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);

            Pokedex = XMLReader.ReadPokedex();
            #endregion
            #region Evolution Trees
            url = URL + getEvoTrees;
            formData = new NameValueCollection();

            webClient = new WebClient();

            responseBytes = webClient.UploadValues(url, "POST", formData);
            responsefromserver = Encoding.UTF8.GetString(responseBytes);

            EvoTrees = XMLReader.ReadEvoTrees();
            #endregion


            List<Pokémon> objectToSerialize = Pokedex;

            Serializer serializer = new Serializer();
            serializer.SerializePokedex("pokedex.bin", objectToSerialize);
        }

        public static Pocket GetPocket(int p)
        {
            foreach (Pocket x in Pockets)
            {
                if (x.ID == p)
                    return x;
            }

            return null;
        }

        public static bool CheckIfAdded(EvolutionTree et, EvolutionInstance ei)
        {
            bool x = false;

            foreach (EvolutionInstance y in et.Evolutions)
            {
                if (ei.Place == y.Place &&
                    ei.Move == y.Move &&
                    ei.Item == y.Item &&
                    ei.Condition == y.Condition)
                {
                    x = true;
                    break;
                }
            }

            return x;
        }

        public static Item GetItem(int p)
        {
            foreach (Item i in Items)
            {
                if (i.ID == p)
                    return i;
            }

            return null;
        }

        public static Move GetMove(int p)
        {
            foreach (Move i in Moves)
            {
                if (i.ID == p)
                    return i;
            }

            return null;
        }

        public static EvoMethod GetEvoMethod(int p)
        {
            foreach (EvoMethod e in EvoMethods)
            {
                if (e.ID == p)
                    return e;
            }
            return null;
        }

        public static EggGroup GetEggGroup(int s)
        {
            return EggGroups.FirstOrDefault(eg => eg.ID == s);
        }

        public static PokeType GetType(int t)
        {
            return Types.FirstOrDefault(pt => pt.ID == t);
        }

        public PokeType GetType(string t)
        {
            return Types.FirstOrDefault(pt => pt.Name.Equals(t));
        }

        public static Category GetCategory(string c)
        {
            switch (c)
            {
                default:
                case "Physical":
                    return Category.Physical;
                case "Special":
                    return Category.Special;
                case "Status":
                    return Category.Status;
            }
        }

        public string GetPreEvo(EvolutionInstance ei)
        {
            EvolutionTree et;
            EvolutionInstance evoEi = null;
            string x = null;

            foreach (EvolutionTree etx in EvoTrees)
            {
                if (etx.ID == ei.TreeID)
                {
                    et = etx;

                    try
                    {
                        int i = 0;
                        foreach (EvolutionInstance y in et.Evolutions)
                        {
                            if (ei.Place - 1 > 0 && ei.Place - 1 == y.Place)
                            {
                                i = et.Evolutions.IndexOf(y);
                                break;
                            }
                        }

                        evoEi = et.Evolutions[i];
                    }
                    catch { }

                    break;
                }
            }

            switch (ei.Place)
            {
                case 1:
                    x = ei.Method.Method;
                    break;
                case 2:
                case 3:
                    foreach (Pokémon p in Pokedex)
                    {
                        if (p.EvolutionInstance == evoEi)
                            x = p.Name;
                    }
                    break;
            }

            return x;
        }

        public void GetEvo(EvolutionInstance ei, TextBlock lbevo, ComboBox cbevo, Image imgevo)
        {
            EvolutionTree et;
            List<EvolutionInstance> evoEi = new List<EvolutionInstance>();

            foreach (EvolutionTree etx in EvoTrees)
            {
                if (etx.ID == ei.TreeID)
                {
                    et = etx;

                    try
                    {
                        foreach (EvolutionInstance y in et.Evolutions)
                        {
                            if (ei.Place == y.Place)
                            {
                                foreach(EvolutionInstance x in et.Evolutions)
                                {
                                    if (x.Place == ei.Place + 1)
                                        evoEi.Add(x);
                                }
                            }
                        }

                    }
                    catch { }

                    break;
                }
            }

            if (evoEi.Count() > 1)
            {
                foreach (EvolutionInstance eix in evoEi)
                {
                    switch (ei.Place)
                    {
                        case 1:
                        case 2:
                            foreach (Pokémon p in Pokedex)
                            {
                                if (p.EvolutionInstance == eix)
                                {
                                    /*ComboBoxItem cbi = new ComboBoxItem();
                                    Image cbii = new Image*/
                                    cbevo.Items.Add(p.Name);
                                }
                            }
                            break;
                    }
                }

                lbevo.Visibility = System.Windows.Visibility.Collapsed;
                cbevo.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                string x = null;

                if (evoEi.Count > 0)
                {
                    switch (ei.Place)
                    {
                        case 1:
                        case 2:
                            foreach (Pokémon p in Pokedex)
                            {
                                if (p.EvolutionInstance == evoEi[0])
                                {
                                    x = p.Name;
                                    imgevo.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/minisprites/" + Convert.ToInt32(p.NationalNumber) + ".png"));
                                }
                            }
                            break;
                    }
                }

                if (x == null)
                {
                    x = "Final evolution";
                    imgevo.Source = null;
                }

                lbevo.Text = x;
                lbevo.Visibility = System.Windows.Visibility.Visible;
                cbevo.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        internal List<PokeType> GetTypes()
        {
            return Types;
        }

        internal List<GameVersion> GetVersionList()
        {
            return Versions;
        }

        internal bool CreateAccount(string p1, int p2, GameVersion version, string p3, PokeType type, string sp1, string sp2, string sp3)
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                _connection.Open();

            if (_command != null)
                _command = null;

            if (_reader != null)
                _reader = null;

            _command = new MySqlCommand("SELECT MAX(ID) FROM Accounts");
            _command.Connection = _connection;
            int id = 1;

            try
            {
                id = Convert.ToInt32(_command.ExecuteReader()[0].ToString());
            }
            catch { }

            id++;

            string query = String.Format("INSERT INTO Accounts VALUES ({0}, '{1}', '{2}', {3}, '{4}', {5}, '{6}', '{7}', '{8}')", id, p1, p2, version.ID, p3, type.ID, sp1, sp2, sp3);

            _command = new MySqlCommand(query);
            _command.Connection = _connection;
            int result = _command.ExecuteNonQuery();

            _connection.Close();

            if (result == 0)
                return false;

            return true;
        }

        public Account GetAccount (string name, string password)
        {
            if (!Login(name, password))
                return null;

            Account x = null;
            List<Pokémon> sp = new List<Pokémon>();
            
            string url = URL + getAccount;
            NameValueCollection formData = new NameValueCollection();
            formData["username"] = name;

            string[] response = GetResponse(url, formData);

            List<string> sps = new List<string>();
            List<string> pokes = new List<string>();

            sps.Add(response[4]);
            sps.Add(response[5]);
            sps.Add(response[6]);

            foreach (string z in sps)
            {
                if (z != "")
                    pokes.Add(z);
            }

            foreach (Pokémon y in Pokedex)
            {
                foreach (string poke in pokes)
                {
                    if (y.Name == poke)
                        sp.Add(y);

                    if (sp.Count == sps.Count)
                        break;
                }
            }

            x = new Account(Convert.ToInt32(response[0]),
                            name,
                            GetVersion(Convert.ToInt32(response[1])),
                            response[2],
                            GetType(Convert.ToInt32(response[3])),
                            sp
                            );

            //GetPokemonFromAccount(x);

            return x;
        }

        private bool Login(string name, string password)
        {
            string url = URL + login;
            NameValueCollection formData = new NameValueCollection();
            formData["username"] = name;
            formData["password"] = password;

            string[] response = GetResponse(url, formData);

            if (Convert.ToBoolean(response[0]))
                return true;

            return false;
        }

        private void GetPokemonFromAccount(Account x)
        {
            _connection.Open();

            _command = new MySqlCommand("SELECT * FROM Pokemon WHERE Account = " + x.ID + "");
            _command.Connection = _connection;
            _reader = _command.ExecuteReader();

            while (_reader.Read())
            {
                PokémonInstance poke = new PokémonInstance( 
                                                            );

                x.Pokemons.Add(poke);
            }
        }

        private GameVersion GetVersion(int i)
        {
            foreach (GameVersion v in Versions)
            {
                if (v.ID == i)
                    return v;
            }

            return null;
        }

        public Nature GetNatureFromName(string p)
        {
            foreach (Nature n in Natures)
            {
                if (n.Name == p)
                    return n;
            }

            return null;
        }

        internal bool IsUpdated(List<Pokémon> pokedex)
        {
            bool updated = true;

            string url = URL + getPokedex;
            NameValueCollection formData = new NameValueCollection();

            WebClient webClient = new WebClient();

            byte[] responseBytes = webClient.UploadValues(url, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);

            List<Pokémon> templist = XMLReader.ReadPokedex();

            try
            {
                for (int x = 0; x < templist.Count - 1; x++)
                {
                    if (templist[x].Updated.CompareTo(pokedex[x].Updated) == 1)
                    {
                        updated = false;
                        break;
                    }
                }
            }
            catch (Exception e){}

            return updated;
        }

        private string[] GetResponse(string url, NameValueCollection data)
        {
            WebClient webClient = new WebClient();

            byte[] responseBytes = webClient.UploadValues(url, "POST", data);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);
            return responsefromserver.Split(',');
        }

        internal static MoveMethod GetMoveMethod(int p)
        {
            foreach (MoveMethod i in MoveMethods)
            {
                if (i.ID == p)
                    return i;
            }

            return null;
        }
    }
}
