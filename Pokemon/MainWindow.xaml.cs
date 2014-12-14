using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace Pokemon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database _db;
        private List<Pokémon> _pokedex;
        private readonly List<Pokémon> _formes;
        private readonly DropShadowEffect _drop;

        private BrushConverter bc = new BrushConverter();

        private Pokémon selectedPokemon;

        public static Theme theme;

        public Account Account { get; set; }
        public List<Account> Accounts { get; set; }
        public List<Pokémon> Pokedex { get; set; }

        private GroupFilter gf { get; set; }

        public MainWindow(Theme t, Database d, List<Pokémon> p)
        {
            Accounts = new List<Account>();
            //Accounts.CollectionChanged += Accounts_CollectionChanged;
            Pokedex = new List<Pokémon>();

            gf = new GroupFilter();

            theme = t;

            this.Resources.MergedDictionaries.Clear();
            AddResourceDictionary(GetTheme());

            InitializeComponent();

            _db = d;
            _pokedex = p;

            this.Background = new ImageBrush(new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\background.png")));

            _formes = new List<Pokémon>();

            GridView grid = new GridView();

            GridViewColumn caught = new GridViewColumn
            {
            };

            ContextMenu cmCaught = new ContextMenu()
            {
                Name = "cmCaught"
            };
            this.RegisterName(cmCaught.Name, cmCaught);

            #region Sort
            MenuItem sortCaughtMethod = new MenuItem()
            {
                Header = "Sort by",
                Foreground = (Brush)bc.ConvertFrom("#000000")
            };

            MenuItem sortCaughtFirst = new MenuItem()
            {
                Header = "Caught first",
                Foreground = (Brush)bc.ConvertFrom("#000000"),
                IsCheckable = true
            };
            sortCaughtFirst.Click += new RoutedEventHandler(sortCaughtFirst_MouseDown);

            MenuItem sortCaughtLast = new MenuItem()
            {
                Header = "Caught last",
                Foreground = (Brush)bc.ConvertFrom("#000000"),
                IsCheckable = true
            };
            sortCaughtLast.Click += new RoutedEventHandler(sortCaughtLast_MouseDown);

            sortCaughtMethod.Items.Add(sortCaughtFirst);
            sortCaughtMethod.Items.Add(sortCaughtLast);
            #endregion
            #region Filter
            MenuItem filterCaughtMethod = new MenuItem()
            {
                Header = "Filter by",
                Foreground = (Brush)bc.ConvertFrom("#000000")
            };

            MenuItem filterCaught = new MenuItem()
            {
                Name = "FilterCaught",
                Header = "Caught",
                Foreground = (Brush)bc.ConvertFrom("#000000"),
                IsCheckable = true
            };
            filterCaught.Click += new RoutedEventHandler(filterCaught_MouseDown);

            MenuItem filterNotCaught = new MenuItem()
            {
                Name = "FilterNotCaught",
                Header = "Not caught",
                Foreground = (Brush)bc.ConvertFrom("#000000"),
                IsCheckable = true
            };
            filterNotCaught.Click += new RoutedEventHandler(filterNotCaught_MouseDown);

            filterCaughtMethod.Items.Add(filterCaught);
            filterCaughtMethod.Items.Add(filterNotCaught);
            #endregion

            cmCaught.Items.Add(sortCaughtMethod);
            cmCaught.Items.Add(filterCaughtMethod);

            GridViewColumnHeader gvchCaught = new GridViewColumnHeader()
            {
                ContextMenu = cmCaught,
                Content = "Caught"
            };

            caught.Header = gvchCaught;

            DataTemplate dTemp_Image = new DataTemplate();
            FrameworkElementFactory fef = new FrameworkElementFactory(typeof(Image));
            Binding bind_img = new Binding("Caught");
            bind_img.Converter = new ImageConverter();
            fef.SetBinding(Image.SourceProperty, bind_img);
            dTemp_Image.VisualTree = fef;
            caught.CellTemplate = dTemp_Image;
            caught.DisplayMemberBinding = null;
            

            GridViewColumn natnr = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("NationalNumber"),
                Header = "National Number",
                Width = 105
            };

            GridViewColumn name = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Name"),
                Header = "Name",
                Width = 130
            };

            GridViewColumn gen = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Gen"),
                Header = "Generation",
                Width = 105
            };

            grid.Columns.Add(caught);
            grid.Columns.Add(natnr);
            grid.Columns.Add(name);
            grid.Columns.Add(gen);

            lvPokedex.View = grid;

            _drop = new DropShadowEffect {ShadowDepth = 0.0, Opacity = 0.9, BlurRadius = 12};
            
            ImageSource imageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "button.png"));
            imgDonate.Source = imageSource;

            if (File.Exists("pokedex.bin"))
            {
                Serializer serializer = new Serializer();
                Pokedex = serializer.DeSerializePokedex("pokedex.bin");
                Database.Pokedex = Pokedex;
            }
            else
            {
                Database.LoadPokedex();
                foreach (Pokémon temp in Database.Pokedex)
                   Pokedex.Add(temp);
            }

            lvPokedex.ItemsSource = Pokedex;

            GetTheme();


            GridViewColumn level = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Level"),
                Header = "Level",
                Width = 50
            };

            GridViewColumn movename = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Name"),
                Header = "Move",
                Width = 105
            };

            GridViewColumn type = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Type.Name"),
                Header = "Type",
                Width = 60
            };

            #region Level up
            GridView levelup = new GridView();

            levelup.Columns.Add(level);
            levelup.Columns.Add(movename);
            levelup.Columns.Add(type);

            Style listViewItemStyle = new Style(typeof(ListViewItem));

            listViewItemStyle.Setters.Add(new Setter(ListViewItem.ToolTipProperty, new Binding("Move.ToolTip")));

            lvLevel.ItemContainerStyle = listViewItemStyle;

            lvLevel.View = levelup;
            #endregion
            #region Egg Move
            movename = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Name"),
                Header = "Move",
                Width = 105
            };

            type = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Type.Name"),
                Header = "Type",
                Width = 105
            };

            GridView eggmove = new GridView();

            eggmove.Columns.Add(movename);
            eggmove.Columns.Add(type);
            lvEgg.View = eggmove;
            #endregion
            #region Move Tutor
            movename = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Name"),
                Header = "Move",
                Width = 105
            };

            type = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Type.Name"),
                Header = "Type",
                Width = 105
            };

            GridView tutor = new GridView();

            tutor.Columns.Add(movename);
            tutor.Columns.Add(type);
            lvTutor.View = tutor;
            #endregion
            #region Machine
            movename = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Name"),
                Header = "Move",
                Width = 105
            };

            type = new GridViewColumn
            {
                DisplayMemberBinding = new Binding("Move.Type.Name"),
                Header = "Type",
                Width = 105
            };

            GridView machine = new GridView();

            machine.Columns.Add(movename);
            machine.Columns.Add(type);
            lvMachine.View = machine;
            #endregion

            /*PokémonInstance poke = new PokémonInstance();

            foreach (Pokémon z in Database.Pokedex)
            {
                if (z.Name == "Cryogonal")
                {
                    poke.Species = z;
                    break;
                }
            }

            poke.Level = 1;
            poke.Nature = _db.GetNatureFromName("Bold");

            List<int> IVHP = Calculator.CalculateIVHP(poke.Species.BaseHP, poke.Level, 12, 0);
            List<int> IVAtt = Calculator.CalculateIV(poke.Species.BaseAtt, poke.Level, 5, poke.Nature.Att, 0);
            List<int> IVDef = Calculator.CalculateIV(poke.Species.BaseDef, poke.Level, 5, poke.Nature.Def, 0);
            List<int> IVSpAtt = Calculator.CalculateIV(poke.Species.BaseSpAtt, poke.Level, 6, poke.Nature.SpAtt, 0);
            List<int> IVSpDef = Calculator.CalculateIV(poke.Species.BaseSpDef, poke.Level, 7, poke.Nature.SpDef, 0);
            List<int> IVSpeed = Calculator.CalculateIV(poke.Species.BaseSpeed, poke.Level, 7, poke.Nature.Sp, 0);*/

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvPokedex.ItemsSource);
            gf.AddFilter(NameFilter);
            gf.AddFilter(FormeFilter);
            view.Filter = gf.Filter;
        }

        private void filterNotCaught_MouseDown(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            ((MenuItem)((MenuItem)((ContextMenu)(this.FindName("cmCaught"))).Items[1]).Items[0]).IsChecked = false;

            if (item.IsChecked)
            {
                using (lvPokedex.Items.DeferRefresh())
                {
                    lvPokedex.Items.SortDescriptions.Clear();
                    SortDescription sd2 = new SortDescription("NationalNumber", ListSortDirection.Ascending);
                    lvPokedex.Items.SortDescriptions.Add(sd2);

                    gf.WipeFilters();
                    gf.AddFilter(NotCaughtFilter);
                }
            }
            else
            {
                using (lvPokedex.Items.DeferRefresh())
                {
                    lvPokedex.Items.SortDescriptions.Clear();
                    SortDescription sd2 = new SortDescription("NationalNumber", ListSortDirection.Ascending);
                    lvPokedex.Items.SortDescriptions.Add(sd2);

                    gf.WipeFilters();
                }
            }
        }

        private void filterCaught_MouseDown(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            
            ((MenuItem)((MenuItem)((ContextMenu)(this.FindName("cmCaught"))).Items[1]).Items[1]).IsChecked = false;
            
            if (item.IsChecked)
            {
                using (lvPokedex.Items.DeferRefresh())
                {
                    lvPokedex.Items.SortDescriptions.Clear();
                    SortDescription sd2 = new SortDescription("NationalNumber", ListSortDirection.Ascending);
                    lvPokedex.Items.SortDescriptions.Add(sd2);

                    gf.WipeFilters();
                    gf.AddFilter(CaughtFilter);
                }
            }
            else
            {
                using (lvPokedex.Items.DeferRefresh())
                {
                    lvPokedex.Items.SortDescriptions.Clear();
                    SortDescription sd2 = new SortDescription("NationalNumber", ListSortDirection.Ascending);
                    lvPokedex.Items.SortDescriptions.Add(sd2);

                    gf.WipeFilters();
                }
            }
        }

        private void sortCaughtFirst_MouseDown(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            if (item.IsChecked)
            {
                ((MenuItem)((MenuItem)((ContextMenu)(this.FindName("cmCaught"))).Items[0]).Items[1]).IsChecked = false;

                using (lvPokedex.Items.DeferRefresh())
                {
                    lvPokedex.Items.SortDescriptions.Clear();
                    SortDescription sd1 = new SortDescription("Caught", ListSortDirection.Descending);
                    SortDescription sd2 = new SortDescription("NationalNumber", ListSortDirection.Ascending);
                    lvPokedex.Items.SortDescriptions.Add(sd1);
                    lvPokedex.Items.SortDescriptions.Add(sd2);
                }
            }
        }

        private void sortCaughtLast_MouseDown(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            if (item.IsChecked)
            {
                ((MenuItem)((MenuItem)((ContextMenu)(this.FindName("cmCaught"))).Items[0]).Items[0]).IsChecked = false;

                using (lvPokedex.Items.DeferRefresh())
                {
                    lvPokedex.Items.SortDescriptions.Clear();
                    SortDescription sd1 = new SortDescription("Caught", ListSortDirection.Ascending);
                    SortDescription sd2 = new SortDescription("NationalNumber", ListSortDirection.Ascending);
                    lvPokedex.Items.SortDescriptions.Add(sd1);
                    lvPokedex.Items.SortDescriptions.Add(sd2);
                }
            }
        }

        private bool NotCaughtFilter(object obj)
        {
            return !((obj as Pokémon).Caught);
        }

        private bool CaughtFilter(object obj)
        {
            return ((obj as Pokémon).Caught);
        }

        private bool FormeFilter(object item)
        {
            List<string> formes = new List<string>(8);
            formes.Add("Normal");
            formes.Add("Plant");
            formes.Add("Incarnate");
            formes.Add("Aria");
            formes.Add("Average");
            formes.Add("Blade");
            formes.Add("Male");
            formes.Add("Standard");

            if (formes.Contains((item as Pokémon).Forme))
                return true;
            else
                return false;
        }
        
        private bool NameFilter(object item)
        {
            if (String.IsNullOrEmpty(tbNameSearch.Text) || tbNameSearch.Text == "Search name:")
                return true;
            else
                return ((item as Pokémon).Name.IndexOf(tbNameSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (tbNameSearch.Text != "Search name:")
                CollectionViewSource.GetDefaultView(lvPokedex.ItemsSource).Refresh();
        }

        private void ResetTxtFilter()
        {

        }

        void Accounts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            List<Account> objectToSerialize = new List<Account>();
            objectToSerialize = Accounts;

            Serializer serializer = new Serializer();
            serializer.SerializeAccounts("accounts.bin", objectToSerialize);
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
        
        private static HttpStatusCode DownloadRemoteImageFile(string fileName)
        {
            string uri = "http://img.pokemondb.net/artwork/" + fileName + ".jpg";

            if (fileName.Contains("dream/"))
            {
                fileName = fileName.Remove(0, 6);
                uri = uri.Replace("jpg", "png");
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException)
            {
                return HttpStatusCode.Forbidden;
            }

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase) &&
                !File.Exists("Pokemon Images/" + fileName + ".jpg"))
            {

                // if the remote file was found, download oit
                Stream inputStream = response.GetResponseStream();
                Stream outputStream = File.OpenWrite("Pokemon Images/" + fileName + ".jpg");

                byte[] buffer = new byte[4096];
                int bytesRead;
                do
                {
                    bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                    outputStream.Write(buffer, 0, bytesRead);
                } while (bytesRead != 0);

                inputStream.Close();
                outputStream.Close();
                inputStream.Dispose();
                outputStream.Dispose();
            }

            return response.StatusCode;
        }

        private static string StringToURL(string name)
        {
            try
            {
                name = name.Replace("♀", "-f");
                name = name.Replace("♂", "-m");
                name = name.Replace("'", "");
                name = name.Replace(".", "-");
                name = name.Replace(" ", "-");
                name = name.Replace("é", "e");
            }
            catch{}

            return name;
        }

        private void lvPokedex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btForme1.Effect = _drop;
            btForme2.Effect = null;
            btForme3.Effect = null;
            btForme4.Effect = null;
            btForme5.Effect = null;
            btForme6.Effect = null;
            btForme1.Visibility = Visibility.Hidden;
            btForme2.Visibility = Visibility.Hidden;
            btForme3.Visibility = Visibility.Hidden;
            btForme4.Visibility = Visibility.Hidden;
            btForme5.Visibility = Visibility.Hidden;
            btForme6.Visibility = Visibility.Hidden;
            gbEggGroups.Visibility = Visibility.Visible;
            gbTypes.Visibility = Visibility.Visible;
            rtBHP.Visibility = Visibility.Visible;
            rtBAtt.Visibility = Visibility.Visible;
            rtBDef.Visibility = Visibility.Visible;
            rtBSpAtt.Visibility = Visibility.Visible;
            rtBSpDef.Visibility = Visibility.Visible;
            rtBSpeed.Visibility = Visibility.Visible;

            selectedPokemon = ((Pokémon) (lvPokedex.SelectedItem));
            SetImage();
            SetData();
            _formes.Clear();

            SetMoveSets();

            lvPokedex.ScrollToCenterOfView(lvPokedex.SelectedItem);

            foreach (Pokémon p in Database.Pokedex.Where(p => String.CompareOrdinal(selectedPokemon.NationalNumber, p.NationalNumber) == 0))
            {
                _formes.Add(p);
            }

            if (_formes.Count <= 1) return;
            int index;
            if ((index = _formes.FindIndex(FindNormal)) != -1)
            {
                Pokémon x = _formes[0];
                Pokémon y = _formes[index];
                _formes[0] = y;
                _formes[index] = x;
            }

            int count = 1;
            switch (_formes.Count)
            {
                case 6:
                    btForme6.Visibility = Visibility.Visible;
                    btForme6.Content = _formes[_formes.Count - count].Forme;
                    count++;
                    goto case 5;
                case 5:
                    btForme5.Visibility = Visibility.Visible;
                    btForme5.Content = _formes[_formes.Count - count].Forme;
                    count++;
                    goto case 4;
                case 4:
                    btForme4.Visibility = Visibility.Visible;
                    btForme4.Content = _formes[_formes.Count - count].Forme;
                    count++;
                    goto case 3;
                case 3:
                    btForme3.Visibility = Visibility.Visible;
                    btForme3.Content = _formes[_formes.Count - count].Forme;
                    count++;
                    goto case 2;
                case 2:
                    btForme2.Visibility = Visibility.Visible;
                    btForme2.Content = _formes[_formes.Count - count].Forme;
                    count++;
                    goto case 1;
                case 1:
                    btForme1.Visibility = Visibility.Visible;
                    btForme1.Content = _formes[_formes.Count - count].Forme;
                    break;
            }
        }

        private void SetMoveSets()
        {
            List<MoveSet> movesByLevel = new List<MoveSet>();
            List<MoveSet> movesByEgg = new List<MoveSet>();
            List<MoveSet> movesByTutor = new List<MoveSet>();
            List<MoveSet> movesByMachine = new List<MoveSet>();

            foreach (MoveSet moveset in Database.MoveSets)
            {
                if (moveset.PokemonID == Convert.ToInt32(selectedPokemon.NationalNumber))
                {
                    switch (moveset.Method.Method)
                    {
                        case "Level up":
                            movesByLevel.Add(moveset);
                            break;
                        case "Egg Move":
                            movesByEgg.Add(moveset);
                            break;
                        case "Move Tutor":
                            movesByTutor.Add(moveset);
                            break;
                        case "TM/HM":
                            movesByMachine.Add(moveset);
                            break;
                        default:
                            break;
                    }
                }
            }

            lvLevel.ItemsSource = movesByLevel;
            lvEgg.ItemsSource = movesByEgg;
            lvTutor.ItemsSource = movesByTutor;
            lvMachine.ItemsSource = movesByMachine;
        }

        private void SetImage()
        {
            string pokeName = "";
            if (selectedPokemon != null)
                pokeName = StringToURL(selectedPokemon.Name.ToLower());

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Pokemon Images\\" + pokeName + ".jpg"))
            {
                imgPokemon.Source =
                    new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Pokemon Images\\" + pokeName + ".jpg"));
            }
            else
            {
                string name = "";

                foreach (Pokémon p in Database.Pokedex.Where(p => String.CompareOrdinal(selectedPokemon.NationalNumber, p.NationalNumber) == 0))
                {
                    name = StringToURL(p.Name.ToLower() + "-" + selectedPokemon.Forme.ToLower());
                    break;
                }

                imgPokemon.Source =
                    new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Pokemon Images\\" + name + ".jpg"));
            }
        }

        private void SetData()
        {
            lbPreEvo.Text = _db.GetPreEvo(selectedPokemon.EvolutionInstance);
            imgPreEvo.Source = GetSprite(_db.GetPreEvo(selectedPokemon.EvolutionInstance));
            cbEvo.Items.Clear();
            _db.GetEvo(selectedPokemon.EvolutionInstance, lbEvo, cbEvo, imgEvo);

            if (lbEvo.Text == "Final evolution")
            {
                lbEvo.Margin = new Thickness(10, lbEvo.Margin.Top, lbEvo.Margin.Right, lbEvo.Margin.Bottom);
            }
            else 
            {
                lbEvo.Margin = new Thickness(40, lbEvo.Margin.Top, lbEvo.Margin.Right, lbEvo.Margin.Bottom);
            }

            lbType1.Text = selectedPokemon.MainType.Name;
            lbType1.Background = (Brush)bc.ConvertFrom(selectedPokemon.MainType.HEX);
            lbType2.Text = selectedPokemon.SecondType.Name;
            lbType2.Background = (Brush)bc.ConvertFrom(selectedPokemon.SecondType.HEX);
            lbEggGroup1.Text = selectedPokemon.MainEggGroup.Name;
            lbEggGroup2.Text = selectedPokemon.SecondEggGroup.Name;

            lbBHP.Text = selectedPokemon.BaseHP.ToString();
            rtBHP.Width = 250 * ((double)(selectedPokemon.BaseHP / 255.00));
            rtBHP.Fill = GetBrush(selectedPokemon.BaseHP);
            lbBHPMax.Text = Calculator.CalculateMaxHP(selectedPokemon.BaseHP, Convert.ToInt32(slLevel.Value)).ToString();
            lbBHPMin.Text = Calculator.CalculateMinHP(selectedPokemon.BaseHP, Convert.ToInt32(slLevel.Value)).ToString();

            lbBAtt.Text = selectedPokemon.BaseAtt.ToString();
            rtBAtt.Width = 250 * ((double)(selectedPokemon.BaseAtt / 255.00));
            rtBAtt.Fill = GetBrush(selectedPokemon.BaseAtt);
            lbBAttMax.Text = Calculator.CalculateMax(selectedPokemon.BaseAtt, Convert.ToInt32(slLevel.Value)).ToString();
            lbBAttMin.Text = Calculator.CalculateMin(selectedPokemon.BaseAtt, Convert.ToInt32(slLevel.Value)).ToString();

            lbBDef.Text = selectedPokemon.BaseDef.ToString();
            rtBDef.Width = 250 * ((double)(selectedPokemon.BaseDef / 255.00));
            rtBDef.Fill = GetBrush(selectedPokemon.BaseDef);
            lbBDefMax.Text = Calculator.CalculateMax(selectedPokemon.BaseDef, Convert.ToInt32(slLevel.Value)).ToString();
            lbBDefMin.Text = Calculator.CalculateMin(selectedPokemon.BaseDef, Convert.ToInt32(slLevel.Value)).ToString();

            lbBSpAtt.Text = selectedPokemon.BaseSpAtt.ToString();
            rtBSpAtt.Width = 250 * ((double)(selectedPokemon.BaseSpAtt / 255.00));
            rtBSpAtt.Fill = GetBrush(selectedPokemon.BaseSpAtt);
            lbBSpAttMax.Text = Calculator.CalculateMax(selectedPokemon.BaseSpAtt, Convert.ToInt32(slLevel.Value)).ToString();
            lbBSpAttMin.Text = Calculator.CalculateMin(selectedPokemon.BaseSpAtt, Convert.ToInt32(slLevel.Value)).ToString();

            lbBSpDef.Text = selectedPokemon.BaseSpDef.ToString();
            rtBSpDef.Width = 250 * ((double)(selectedPokemon.BaseSpDef / 255.00));
            rtBSpDef.Fill = GetBrush(selectedPokemon.BaseSpDef);
            lbBSpDefMax.Text = Calculator.CalculateMax(selectedPokemon.BaseSpDef, Convert.ToInt32(slLevel.Value)).ToString();
            lbBSpDefMin.Text = Calculator.CalculateMin(selectedPokemon.BaseSpDef, Convert.ToInt32(slLevel.Value)).ToString();

            lbBSpeed.Text = selectedPokemon.BaseSpeed.ToString();
            rtBSpeed.Width = 250 * ((double)(selectedPokemon.BaseSpeed / 255.00));
            rtBSpeed.Fill = GetBrush(selectedPokemon.BaseSpeed);
            lbBSpeedMax.Text = Calculator.CalculateMax(selectedPokemon.BaseSpeed, Convert.ToInt32(slLevel.Value)).ToString();
            lbBSpeedMin.Text = Calculator.CalculateMin(selectedPokemon.BaseSpeed, Convert.ToInt32(slLevel.Value)).ToString();
        }

        private BitmapImage GetSprite(string px)
        {
            foreach (Pokémon p in Database.Pokedex)
            {
                if (p.Name == px)
                    return new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/minisprites/" + Convert.ToInt32(p.NationalNumber) + ".png"));
            }

            return null;
        }

        private Brush GetBrush(int stat)
        {
            if (stat < 69)
                return (Brush)bc.ConvertFrom("#FFFF0000");
            if (stat >= 100)
                return (Brush)bc.ConvertFrom("#FF008000");

            return (Brush)bc.ConvertFrom("#FFFFFF00");
        }

        private static bool FindNormal(Pokémon poke)
        {
            return poke.Forme == "Normal";
        }

        private void btForme_Click(object sender, RoutedEventArgs e)
        {
            btForme1.Effect = null;
            btForme2.Effect = null;
            btForme3.Effect = null;
            btForme4.Effect = null;
            btForme5.Effect = null;
            btForme6.Effect = null;

            string forme = ((Button)sender).Content.ToString();

            foreach (Pokémon x in _formes.Where(x => x.Forme == forme))
            {
                selectedPokemon = x;
                SetImage();
                SetData();
                ((Button)sender).Effect = _drop;
                break;
            }
        }

        private void slLevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                lbLevel.Text = "Level " + slLevel.Value.ToString();

                if (selectedPokemon != null)
                    SetData();
            }
            catch { }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) this.DragMove(); }

        private void lbEvo_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock block = ((TextBlock)(sender));

            if (block.Text != "Egg" && block.Text != "Final evolution")
            {
                block.TextDecorations = TextDecorations.Underline;
            }
        }

        private void lbEvo_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock block = ((TextBlock)(sender));
            block.TextDecorations = null;
        }

        private void lbPreEvo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock block = ((TextBlock)(sender));

            if (block.Text != "Egg" && block.Text != "Final evolution")
            {
                foreach (Pokémon p in lvPokedex.Items)
                {
                    if (block.Text == p.Name)
                    {
                        lvPokedex.SelectedValue = p;
                        break;
                    }
                }
            }
        }

        private void tbDonate_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Donate();
        }

        private void Donate()
        {
            string url = "";

            string business = "itontherun@hotmail.com";  // your paypal email
            string description = "Donation";            // '%20' represents a space. remember HTML!
            string country = "NL";                  // AU, US, etc.
            string currency = "EUR";                 // AUD, USD, etc.

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            System.Diagnostics.Process.Start(url);
        }

        private void cbEvo_SelectionChanged(object sender, EventArgs e)
        {
            foreach (Pokémon p in lvPokedex.Items)
            {
                if (((ComboBox)sender).Text == p.Name)
                {
                    lvPokedex.SelectedValue = p;
                    break;
                }
            }
        }

        internal void SetTheme(string p)
        {
            switch (p)
            {
                case "X":
                    theme = Theme.Blue;
                    break;
                case "Y":
                    theme = Theme.Red;
                    break;
            }

            this.Resources.MergedDictionaries.Clear();
            AddResourceDictionary(GetTheme());
        }

        private void imgExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void tbNameSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbNameSearch.Text = "";
        }

        private void tbNameSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            tbNameSearch.Text = "Search Name:";
        }
    }

    public enum Theme
    {
        [StringValue("ShinyRed.xaml")]
        Red,
        [StringValue("ShinyBlue.xaml")]
        Blue
    }
    public class StringValueAttribute : System.Attribute
    {

        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }
}