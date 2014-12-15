using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pokemon
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private MainWindow main;
        private Database _db;
        private Theme theme;

        private string fc = "";

        private List<Account> accounts;
        private List<Pokémon> pokedex;

        public bool loaded = false;

        private Grid cards;

        private string FriendCode 
        { 
            get
            {
                if (fc.Length == 4)
                    fc += "-";
                if (fc.Length == 9)
                    fc += "-";

                return fc;
            } 

            set 
            { 
                fc = value; 
            } 
        }

        public Window1()
        {
            Random r = new Random();
            int x = r.Next();
            Uri source;

            if (x % 2 == 0)
            {
                source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "Splashvids\\splash-x.mp4");
                theme = Theme.Blue;
            }
            else
            {
                source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "Splashvids\\splash-y.mp4");
                theme = Theme.Red;
            }

            this.Resources.MergedDictionaries.Clear();
            AddResourceDictionary(GetTheme());

            InitializeComponent();

            MovieBackground.BeginInit();
            MovieBackground.Source = source;
            MovieBackground.EndInit();

            Thread loadDataThread = new Thread(LoadDataAsync);
            loadDataThread.Start();
            btLogin.IsEnabled = true;

            if (File.Exists("accounts.bin"))
            {
                Serializer serializer = new Serializer();
                accounts = serializer.DeSerializeAccounts("accounts.bin");
            }
            else
            {
                accounts = new List<Account>();
            }

            GenerateSavedGameCard(accounts);

            if (File.Exists("pokedex.bin"))
            {
                Serializer serializer = new Serializer();
                pokedex = serializer.DeSerializePokedex("pokedex.bin");
            }
            else
            {
                pokedex = new List<Pokémon>();
            }
        }

        public string GetTheme()
        {
            string output = null;
            Type type = theme.GetType();

            FieldInfo fi = type.GetField(theme.ToString());
            StringValueAttribute[] attrs =
                fi.GetCustomAttributes(typeof(StringValueAttribute),
                                        false) as StringValueAttribute[];
            output = attrs[0].Value;
            return output;
        }

        void AddResourceDictionary(string source)
        {
            string loc = String.Format(CultureInfo.InvariantCulture, "/{0};component/" + source, Assembly.GetExecutingAssembly());
            Uri x = new Uri(loc, UriKind.Relative);
            ResourceDictionary resourceDictionary = Application.LoadComponent(x) as ResourceDictionary;
            this.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        public void LoadDataAsync()
        {
            Database db = new Database();
            _db = db;

            this.Dispatcher.BeginInvoke(new UpdaterDelegate(() =>
            {
                btCreateAccount.IsEnabled = true;
                cbTypes.IsEnabled = true;

                cbTypes.Items.Add("Unknown");

                for (int i = 0; i < db.GetTypes().Count - 1; i++)
                {
                    ComboBoxItem cbi1 = new ComboBoxItem();
                    cbi1.Background = (Brush)new BrushConverter().ConvertFrom(db.GetTypes()[i].HEX);
                    cbi1.Content = db.GetTypes()[i].Name;
                    cbTypes.Items.Add(cbi1);
                }
            }));

            List<Pokémon> _pokedex = new List<Pokémon>();

            if (_db.IsUpdated(pokedex))
            {
                _pokedex.AddRange(pokedex);
                Database.Pokedex = _pokedex;
            }
            else
            {
                Database.LoadPokedex();
                _pokedex = Database.Pokedex;
            }

            this.Dispatcher.BeginInvoke(new UpdaterDelegate(() =>
            {
                pbProgress.IsIndeterminate = false;
                main = new MainWindow(theme, db, _pokedex);
                main.Accounts = accounts;
                tbLoading.Text = "Done Loading";
                btLogin.IsEnabled = true;
                loaded = true;
            }));
        }

        /// <summary>
	    /// Deletage used for updating values
	    /// </summary>
        public delegate void UpdaterDelegate();

        bool AreAllValidNumericChars(string str)
        {
            bool ret = true;
            if (str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
                return ret;

            int l = str.Length;
            for (int i = 0; i < l; i++)
            {
                char ch = str[i];
                ret &= Char.IsDigit(ch);
            }

            return ret;
        }

        private void tbFriendCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            FriendCode = ((TextBox)sender).Text;
            ((TextBox)sender).Text = FriendCode;
            tbFriendCode.CaretIndex = tbFriendCode.Text.Length;

            e.Handled = !AreAllValidNumericChars(e.Text);
        }

        private void cbTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selected = (ComboBoxItem)((ComboBox)sender).SelectedItem;

            if (selected != null)
            {
                if (selected.Content.ToString() != "Unknown")
                {
                    cbSafari1.IsEnabled = true;
                    cbSafari2.IsEnabled = true;
                    cbSafari3.IsEnabled = true;

                    cbSafari1.Items.Clear();
                    cbSafari2.Items.Clear();
                    cbSafari3.Items.Clear();

                    PokeType type = _db.GetType(selected.Content.ToString());

                    switch (type.Name)
                    {
                        case "Normal":
                            cbSafari1.Items.Add("Aipom");
                            cbSafari1.Items.Add("Dunsparce");
                            cbSafari1.Items.Add("Teddiursa");
                            cbSafari1.Items.Add("Lillipup");

                            cbSafari2.Items.Add("Loudred");
                            cbSafari2.Items.Add("Kecleon");
                            cbSafari2.Items.Add("Audino");
                            cbSafari2.Items.Add("Minccino");

                            cbSafari3.Items.Add("Chansey");
                            cbSafari3.Items.Add("Ditto");
                            cbSafari3.Items.Add("Eevee");
                            cbSafari3.Items.Add("Smeargle");
                            break;
                        case "Bug":
                            cbSafari1.Items.Add("Butterfree");
                            cbSafari1.Items.Add("Paras");
                            cbSafari1.Items.Add("Ledyba");
                            cbSafari1.Items.Add("Combee");

                            cbSafari2.Items.Add("Beautifly");
                            cbSafari2.Items.Add("Masquerain");
                            cbSafari2.Items.Add("Volbeat");
                            cbSafari2.Items.Add("Illumise");

                            cbSafari3.Items.Add("Venomoth");
                            cbSafari3.Items.Add("Pinsir");
                            cbSafari3.Items.Add("Heracross");
                            cbSafari3.Items.Add("Vivillon");
                            break;
                        case "Dark":
                            cbSafari1.Items.Add("Mightyena");
                            cbSafari1.Items.Add("Nuzleaf");
                            cbSafari1.Items.Add("Pawniard");
                            cbSafari1.Items.Add("Vullaby");

                            cbSafari2.Items.Add("Sneasel");
                            cbSafari2.Items.Add("Cacturne");
                            cbSafari2.Items.Add("Crawdaunt");
                            cbSafari2.Items.Add("Sandile");

                            cbSafari3.Items.Add("Sableye");
                            cbSafari3.Items.Add("Absol");
                            cbSafari3.Items.Add("Liepard");
                            cbSafari3.Items.Add("Inkay");
                            break;
                        case "Dragon":
                            cbSafari1.Items.Add("Gabite");
                            cbSafari1.Items.Add("Fraxure");

                            cbSafari2.Items.Add("Dragonair");
                            cbSafari2.Items.Add("Shelgon");
                            cbSafari2.Items.Add("Noibat");

                            cbSafari3.Items.Add("Druddigon");
                            cbSafari3.Items.Add("Sliggoo");
                            break;
                        case "Electric":
                            cbSafari1.Items.Add("Electrode");
                            cbSafari1.Items.Add("Pachirisu");
                            cbSafari1.Items.Add("Emolga");
                            cbSafari1.Items.Add("Dedenne");

                            cbSafari2.Items.Add("Pikachu");
                            cbSafari2.Items.Add("Electabuzz");
                            cbSafari2.Items.Add("Stunfisk");
                            cbSafari2.Items.Add("Helioptile");

                            cbSafari3.Items.Add("Manectric");
                            cbSafari3.Items.Add("Luxio");
                            cbSafari3.Items.Add("Zebstrika");
                            cbSafari3.Items.Add("Galvantula");
                            break;
                        case "Fairy":
                            cbSafari1.Items.Add("Togepi");
                            cbSafari1.Items.Add("Snubbull");
                            cbSafari1.Items.Add("Kirlia");
                            cbSafari1.Items.Add("Dedenne");

                            cbSafari2.Items.Add("Jigglypuff");
                            cbSafari2.Items.Add("Mawile");
                            cbSafari2.Items.Add("Spritzee");
                            cbSafari2.Items.Add("Swirlix");

                            cbSafari3.Items.Add("Clefairy");
                            cbSafari3.Items.Add("Floette");
                            break;
                        case "Fighting":
                            cbSafari1.Items.Add("Mankey");
                            cbSafari1.Items.Add("Machoke");
                            cbSafari1.Items.Add("Meditite");
                            cbSafari1.Items.Add("Mienfoo");

                            cbSafari2.Items.Add("Troh");
                            cbSafari2.Items.Add("Sawk");
                            cbSafari2.Items.Add("Pancham");

                            cbSafari3.Items.Add("Tyrogue");
                            cbSafari3.Items.Add("Breloom");
                            cbSafari3.Items.Add("Hariyama");
                            cbSafari3.Items.Add("Riolu");
                            break;
                        case "Fire":
                            cbSafari1.Items.Add("Growlithe");
                            cbSafari1.Items.Add("Ponyta");
                            cbSafari1.Items.Add("Magmar");
                            cbSafari1.Items.Add("Pansear");

                            cbSafari2.Items.Add("Charmeleon");
                            cbSafari2.Items.Add("Slugma");
                            cbSafari2.Items.Add("Larvesta");
                            cbSafari2.Items.Add("Pyroar");

                            cbSafari3.Items.Add("Ninetales");
                            cbSafari3.Items.Add("Braixen");
                            cbSafari3.Items.Add("Fletchinder");
                            break;
                        case "Flying":
                            cbSafari1.Items.Add("Pidgey");
                            cbSafari1.Items.Add("Spearow");
                            cbSafari1.Items.Add("Farfetch'd");
                            cbSafari1.Items.Add("Doduo");

                            cbSafari2.Items.Add("Hoothoot");
                            cbSafari2.Items.Add("Tranquil");
                            cbSafari2.Items.Add("Woobat");
                            cbSafari2.Items.Add("Swanna");

                            cbSafari3.Items.Add("Tropius");
                            cbSafari3.Items.Add("Rufflet");
                            cbSafari3.Items.Add("Fletchinder");
                            cbSafari3.Items.Add("Hawlucha");
                            break;
                        case "Ghost":
                            cbSafari1.Items.Add("Shuppet");
                            cbSafari1.Items.Add("Lampent");

                            cbSafari2.Items.Add("Phantump");
                            cbSafari2.Items.Add("Pumpkaboo");

                            cbSafari3.Items.Add("Dusclops");
                            cbSafari3.Items.Add("Drifblim");
                            cbSafari3.Items.Add("Spiritomb");
                            cbSafari3.Items.Add("Golurk");
                            break;
                        case "Grass":
                            cbSafari1.Items.Add("Oddish");
                            cbSafari1.Items.Add("Tangela");
                            cbSafari1.Items.Add("Sunkern");
                            cbSafari1.Items.Add("Pansage");

                            cbSafari2.Items.Add("Ivysaur");
                            cbSafari2.Items.Add("Swadloon");
                            cbSafari2.Items.Add("Petilil");
                            cbSafari2.Items.Add("Sawsbuck");

                            cbSafari3.Items.Add("Maractus");
                            cbSafari3.Items.Add("Quilladin");
                            cbSafari3.Items.Add("Gogoat");
                            break;
                        case "Ground":
                            cbSafari1.Items.Add("Sandshrew");
                            cbSafari1.Items.Add("Wooper");
                            cbSafari1.Items.Add("Phanpy");
                            cbSafari1.Items.Add("Trapinch");

                            cbSafari2.Items.Add("Dugtrio");
                            cbSafari2.Items.Add("Marowak");
                            cbSafari2.Items.Add("Nincada");
                            cbSafari2.Items.Add("Camerupt");

                            cbSafari3.Items.Add("Gastrodon");
                            cbSafari3.Items.Add("Palpitoad");
                            cbSafari3.Items.Add("Diggersby");
                            break;
                        case "Ice":
                            cbSafari1.Items.Add("Delibird");
                            cbSafari1.Items.Add("Snorunt");
                            cbSafari1.Items.Add("Spheal");
                            cbSafari1.Items.Add("Snover");

                            cbSafari2.Items.Add("Sneasel");
                            cbSafari2.Items.Add("Beartic");
                            cbSafari2.Items.Add("Bergmite");

                            cbSafari3.Items.Add("Dewgong");
                            cbSafari3.Items.Add("Cloyster");
                            cbSafari3.Items.Add("Lapras");
                            cbSafari3.Items.Add("Piloswine");
                            break;
                        case "Poison":
                            cbSafari1.Items.Add("Kakuna");
                            cbSafari1.Items.Add("Gloom");
                            cbSafari1.Items.Add("Cascoon");
                            cbSafari1.Items.Add("Seviper");

                            cbSafari2.Items.Add("Venomoth");
                            cbSafari2.Items.Add("Ariados");
                            cbSafari2.Items.Add("Swalot");
                            cbSafari2.Items.Add("Garbodor");

                            cbSafari3.Items.Add("Muk");
                            cbSafari3.Items.Add("Drapion");
                            cbSafari3.Items.Add("Toxicroak");
                            cbSafari3.Items.Add("Whirlipede");
                            break;
                        case "Psychic":
                            cbSafari1.Items.Add("Abra");
                            cbSafari1.Items.Add("Drowzee");
                            cbSafari1.Items.Add("Grumpig");
                            cbSafari1.Items.Add("Munna");

                            cbSafari2.Items.Add("Wobbuffet");
                            cbSafari2.Items.Add("Sigilyph");
                            cbSafari2.Items.Add("Espurr");

                            cbSafari3.Items.Add("Xatu");
                            cbSafari3.Items.Add("Girafarig");
                            cbSafari3.Items.Add("Gothorita");
                            cbSafari3.Items.Add("Duosion");
                            break;
                        case "Rock":
                            cbSafari1.Items.Add("Nosepass");
                            cbSafari1.Items.Add("Boldore");
                            cbSafari1.Items.Add("Dwebble");

                            cbSafari2.Items.Add("Onix");
                            cbSafari2.Items.Add("Magcargo");
                            cbSafari2.Items.Add("Corsola");
                            cbSafari2.Items.Add("Pupitar");

                            cbSafari3.Items.Add("Rhydon");
                            cbSafari3.Items.Add("Shuckle");
                            cbSafari3.Items.Add("Barbaracle");
                            break;
                        case "Steel":
                            cbSafari1.Items.Add("Magneton");
                            cbSafari1.Items.Add("Mawile");
                            cbSafari1.Items.Add("Ferroseed");

                            cbSafari2.Items.Add("Forretress");
                            cbSafari2.Items.Add("Skarmory");
                            cbSafari2.Items.Add("Metang");
                            cbSafari2.Items.Add("Klang");

                            cbSafari3.Items.Add("Bronzong");
                            cbSafari3.Items.Add("Excadrill");
                            cbSafari3.Items.Add("Klefki");
                            break;
                        case "Water":
                            cbSafari1.Items.Add("Krabby");
                            cbSafari1.Items.Add("Octillery");
                            cbSafari1.Items.Add("Bibarel");
                            cbSafari1.Items.Add("Panpour");

                            cbSafari2.Items.Add("Wartortle");
                            cbSafari2.Items.Add("Gyarados");
                            cbSafari2.Items.Add("Quagsire");
                            cbSafari2.Items.Add("Floatzel");

                            cbSafari3.Items.Add("Poliwhirl");
                            cbSafari3.Items.Add("Azumarill");
                            cbSafari3.Items.Add("Frogadier");
                            break;
                    }
                }
            }
            else
            {
                cbSafari1.IsEnabled = false;
                cbSafari2.IsEnabled = false;
                cbSafari3.IsEnabled = false;
            }
        }

        private void btCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (tbName.Text.Length > 3 && pbPassword.SecurePassword.Length > 3 && (rbX.IsChecked.Value || rbY.IsChecked.Value))
            {
                GameVersion version = null;
                string versiontarget = null;
                string sp1 = "";
                string sp2 = "";
                string sp3 = "";
                string type = "–";

                if (rbX.IsChecked.Value)
                    versiontarget = "X";
                if (rbY.IsChecked.Value)
                    versiontarget = "Y";

                foreach (GameVersion v in _db.GetVersionList())
                {
                    if (v.Version == versiontarget)
                        version = v;
                }

                if (cbSafari1.SelectedValue != null)
                    sp1 = cbSafari1.SelectedValue.ToString();

                if (cbSafari2.SelectedValue != null)
                    sp2 = cbSafari2.SelectedValue.ToString();

                if (cbSafari3.SelectedValue != null)
                    sp3 = cbSafari3.SelectedValue.ToString();

                if (cbTypes.SelectedItem != null)
                    type = ((ComboBoxItem)cbTypes.SelectedItem).Content.ToString();

                _db.CreateAccount(tbName.Text, pbPassword.Password.GetHashCode(), version, FriendCode, _db.GetType(type), sp1, sp2, sp3);
            }
        }

        private void MovieBackground_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.ToString());
        }

        private void MovieBackground_MediaOpened(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void btlogin_Click(object sender, RoutedEventArgs e)
        {
            Account account = _db.GetAccount(tbNameLogin.Text, pbPasswordLogin.Password.GetHashCode().ToString());

            if (account != null)
            {
                main.Account = account;

                if (cbSave.IsChecked == true)
                {
                    bool x = true;

                    try
                    {
                        foreach (Account a in main.Accounts)
                        {
                            if (a.ID == account.ID)
                                x = false;
                        }
                    }
                    catch { }
                        
                    if (x)
                        main.Accounts.Add(account);
                }

                List<Account> objectToSerialize = new List<Account>();
                objectToSerialize = main.Accounts;

                Serializer serializer = new Serializer();
                serializer.SerializeAccounts("accounts.bin", objectToSerialize);

                main.Show();
                this.Close();
            }
        }

        private void GenerateSavedGameCard(List<Account> accounts)
        {
            cards = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = this.Height,
                Width = this.Width
            };

            for (int i = 0; i < accounts.Count; i++)
            {
                Account a = accounts[i];
                ImageBrush x = new ImageBrush(new BitmapImage(new Uri("LoginCard.png", UriKind.Relative)));

                Grid grid = new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 130,
                    Width = 270,
                    Background = x,
                    Opacity = 0.6
                };
                grid.Margin = new Thickness(20, (170 + (i * (grid.Height + 20))), 0, 0);

                Image logo = new Image()
                {
                    Source = new BitmapImage(new Uri(a.Version.Version + "-logo.png", UriKind.Relative)),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = System.Windows.VerticalAlignment.Top,
                    Height = 50,
                    Width = 50,
                    Margin = new Thickness(35, (185 + (i * (grid.Height + 20))), 0, 0),
                    Opacity = 0.8
                };

                TextBlock name = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextAlignment = TextAlignment.Left,
                    Height = 20,
                    Width = 150,
                    Margin = new Thickness(50 + logo.Width, (185 + (i * (grid.Height + 20))), 0, 0),
                    Opacity = 0.8,
                    Text = a.Name
                };

                TextBlock friendcode = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextAlignment = TextAlignment.Left,
                    Height = 20,
                    Width = 150,
                    Margin = new Thickness(50 + logo.Width, (200 + (i * (grid.Height + 20))), 0, 0),
                    Opacity = 0.8,
                    Text = a.Friendcode
                };

                TextBlock safaritype = new TextBlock()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    TextAlignment = TextAlignment.Left,
                    Height = 20,
                    Margin = new Thickness(50 + logo.Width, (220 + (i * (grid.Height + 20))), 0, 0),
                    Opacity = 0.8,
                    Text = a.SafariType.Name,
                    Background = (Brush)(new BrushConverter().ConvertFrom(a.SafariType.HEX))
                };

                Image poke1img = new Image()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 32,
                    Width = 40,
                    Margin = new Thickness(60, (200 + (i * (grid.Height + 20)) + logo.Height), 0, 0),
                    Opacity = 0.8,
                };

                if (a.SafariPokes.Count >= 1)
                    poke1img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/minisprites/" + Convert.ToInt32(a.SafariPokes[0].NationalNumber) + ".png"));

                Image poke2img = new Image()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 32,
                    Width = 40,
                    Margin = new Thickness(130, (200 + (i * (grid.Height + 20)) + logo.Height), 0, 0),
                    Opacity = 0.8
                };

                if (a.SafariPokes.Count >= 2)
                {
                    poke2img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/minisprites/" + Convert.ToInt32(a.SafariPokes[1].NationalNumber) + ".png"));
                }

                Image poke3img = new Image()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 32,
                    Width = 40,
                    Margin = new Thickness(210, (200 + (i * (grid.Height + 20)) + logo.Height), 0, 0),
                    Opacity = 0.8
                };

                if (a.SafariPokes.Count >= 3)
                {
                    poke3img.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/minisprites/" + Convert.ToInt32(a.SafariPokes[2].NationalNumber) + ".png"));
                }

                Grid overlay = new Grid()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 130,
                    Width = 270,
                    Margin = new Thickness(20, (170 + (i * (grid.Height + 20))), 0, 0),
                    Background = Brushes.Transparent,
                    Name = "overlaynr" + i.ToString()
                };

                Image exit = new Image()
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 20,
                    Width = 20,
                    Margin = new Thickness(grid.Width - 15, 170 + (i *(grid.Height + 20)) + 15, 0, 0),
                    Opacity = 0.8,
                    Source = new BitmapImage(new Uri("exit.png", UriKind.Relative)),
                    Name = "exitnr" + i.ToString()
                };

                exit.MouseUp += exit_MouseUp;

                overlay.PreviewMouseLeftButtonUp += overlay_MouseLeftButtonUp;

                cards.Children.Add(grid);
                cards.Children.Add(logo);
                cards.Children.Add(name);
                cards.Children.Add(friendcode);
                if (!a.SafariType.Name.Equals("–"))
                {
                    cards.Children.Add(safaritype);
                    cards.Children.Add(poke1img);
                    cards.Children.Add(poke2img);
                    cards.Children.Add(poke3img);
                }
                cards.Children.Add(overlay);
                cards.Children.Add(exit);
            }

            layout.Children.Add(cards);
        }

        void exit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int id = Convert.ToInt32(((Image)sender).Name.Remove(0, ((Image)sender).Name.Length - 1));

            accounts.RemoveAt(id);
            layout.Children.Remove(cards);

            List<Account> objectToSerialize = new List<Account>();
            objectToSerialize = accounts;

            Serializer serializer = new Serializer();
            serializer.SerializeAccounts("accounts.bin", objectToSerialize);

            GenerateSavedGameCard(accounts);
        }

        void overlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (loaded)
            {
                main.Account = accounts[Convert.ToInt32(((Grid)sender).Name.Remove(0, 9))];
                main.SetTheme(main.Account.Version.Version);

                main.Show();
                this.Close();
            }
            else
                MessageBox.Show("Program is still loading data. Please be patient");
        }
    }
}
