using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using System.Windows.Forms;
using System.Diagnostics;//for console_debug
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Reactive.Linq;
using System.Threading;
using CoreTweet.Streaming;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace Birth_First
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    [DataContract]
    class Tokens //: CoreTweet.Tokens
    {

        [DataMember]
        private bool enable_state;
        [DataMember]
        private int no_state;
        [DataMember]
        private bool br_state;
        [DataMember]
        public bool IsEnabled
        {
            get { return enable_state; }
            set { enable_state = value; }
        }
        [DataMember]
        public int no
        {
            get { return no_state; }
            set { no_state = value; }
        }
        [DataMember]
        public bool UseBlock
        {
            get { return br_state; }
            set { br_state = value; }
        }
        [DataMember]
        public CoreTweet.Tokens token = new CoreTweet.Tokens();


        public Tokens()
        {
            this.enable_state = true;
            this.no_state = -1;
            this.br_state = true;
        }

    }

    [DataContract]
    class Pass
    {
        [DataMember]
        public static string pindir         = "users";
        [DataMember]
        public static string img            = "img";
        [DataMember]
        public static string img_key        = img + "/key";
        [DataMember]
        public static string img_menu       = img + "/menu";
        [DataMember]
        public static string img_settingdir = img + "/setting";
        [DataMember]
        public static string setting        = "setting.xml";
        [DataMember]
        public static string ng             = "ng_data.xml";
        [DataMember]
        public static string resx           = "Properties/Resources.resx";
        [DataMember]
        public static string[] colors       = { "Aliceblue", "Coral", "Purple", "Seagreen", "Gold" };
        [DataMember]
        public static string[] language     = { "Auto", "Japanese(ja-JP)", "English(en-US)", "Français(fr)" };
        [DataMember]
        public static string data            = "data";
        [DataMember]
        public static string tmp            = "tmp";
        [DataMember]
        public static string tmpkey         = tmp+"/latest.br"; 

    }

    [DataContract]
    class NG_Account
    {
        [DataMember]
        public long id;
        [DataMember]
        public string screen;
    }

    [DataContract]
    class NG_Data
    {
        [DataMember]
        public List<string> words;
        [DataMember]
        public List<NG_Account> account;

        public NG_Data()
        {
            words = new List<string>();
            account = new List<NG_Account>();
        }
        public void AcountAdd(string screen ,long id)
        {
            NG_Account tmp = new NG_Account();
            tmp.screen = screen;
            tmp.id = id;
            account.Add(tmp);
        }
        public void WordAdd(string word)
        {
            words.Add(word);
        }
        public void Clear()
        {
            words.Clear();
            account.Clear();
        }

    }

    
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }


    public partial class MainWindow : Window
    {


        CoreTweet.OAuth.OAuthSession Session { get; set; }
        Tokens Token { get; set; }
        List<Tokens> tokens = new List<Tokens>();
        Account account;
        Key_Erea key_erea;
        Title title;
        Menu menu;
        Setting setting;
        Twitter twitter;
        SetData setdata;
        NG_Data ng_datas;
        InfoBox info;

        public Size win_size;
        public double win_top;
        public double win_left;
        bool IsWindowMaxed=false;
        bool IsWindownomarize = false;

        Tokens curotoken =new Tokens();

        Pass pass = new Pass();

        Color_List color_list = new Color_List();

        Canvas Cover = new Canvas();
        double[] rt_pos = new double[2];

        bool IsStarted = false;
        DateTime tcstartDt = DateTime.Now;
        bool title_clicking= false;
        bool second_run = false;
        System.Drawing.Point oldmousepos;
        Rectangle neosize = new Rectangle();

        List<Point> monitor_point   = new List<Point>();
        List<Point> monitor_size    = new List<Point>();

        private Mutex mutex = new Mutex(false, "birth");

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        const int GWL_STYLE = -16;
        const int WS_SYSMENU = 0x80000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            
            base.OnSourceInitialized(e);
            IntPtr handle = new WindowInteropHelper(this).Handle;
            int style = GetWindowLong(handle, GWL_STYLE);
            style = style & (~WS_SYSMENU);
            SetWindowLong(handle, GWL_STYLE, style);
        }


        private void Load_Token()
        {
            tokens.Clear();
            DirectoryInfo dir = new DirectoryInfo(Pass.pindir);
            foreach (FileInfo f in dir.GetFiles())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Tokens));
                XmlReader xr = XmlReader.Create(Pass.pindir + "/" + f.Name);
                try
                {
                    this.Token = (Tokens)serializer.ReadObject(xr);
                    this.tokens.Add(this.Token);
                    xr.Close();
                    //this.set_acc++;
                }
                catch
                {
                    xr.Close();
                    System.IO.File.Delete(Pass.pindir + "/" + f.Name);
                }
            }
            tokens.Sort((a, b) => a.no - b.no);
            if(IsStarted)
            {
                account.Show_Account();
            }
        }

        private void Load_Setting()
        {
            setdata = new SetData();
            if (System.IO.File.Exists(Pass.setting))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(D));
                XmlReader xr = XmlReader.Create(Pass.setting);
                try
                {
                    setdata.d = (D)serializer.ReadObject(xr);
                    xr.Close();

                }
                catch
                {
                    xr.Close();
                    System.IO.File.Delete(Pass.setting);
                }
            }
            setdata.Open_Ready();
        }

        private void Load_NGdata()
        {
            ng_datas = new NG_Data();
            if (System.IO.File.Exists(Pass.ng))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(NG_Data));
                XmlReader xr = XmlReader.Create(Pass.ng);
                try
                {
                    ng_datas = (NG_Data)serializer.ReadObject(xr);
                    xr.Close();

                }
                catch
                {
                    xr.Close();
                    System.IO.File.Delete(Pass.ng);
                }
            }
        }


        private void PIN_Code_Closed(object sender, EventArgs e)
        {
            if (!(Directory.Exists(Pass.pindir)))
                this.Close();
            else
            {
                this.IsEnabled = true;
                this.Load_Token();
                this.Show();
                setting = null;
                setting = new Setting(Base_Grid, setdata, account, tokens,ng_datas,color_list);
                //ng_datas = setting.ng_datas;
                NewSetting();
            }
        }

        public MainWindow()
        {
            bool IsInScreen = false;


            InitializeComponent();

            this.Visibility = Visibility.Hidden;

            Load_Setting();
            Load_NGdata();

            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show(Properties.Resources.multiple_startup_1, Properties.Resources.multiple_startup_title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                second_run = true;
                this.Close();
            }

            if (!(Network.Network_Connected()))
            {
                var dlg = new emanual.Wpf.Utility.MessageBoxEx();
                dlg.Message = "Brith";
                dlg.Width = 350;
                dlg.Height = 165;
                dlg.TextBlock.Height = 65;
                dlg.TextBlock.Inlines.Add(new System.Windows.Documents.Bold(new System.Windows.Documents.Run(Properties.Resources.network_error_1)));
                dlg.TextBlock.Inlines.Add(Properties.Resources.network_error_2);

                //dlg.Owner = this;
                dlg.Left = this.Left + 50;
                dlg.Top = this.Top + 50;

                //dlg.Background = Brushes.Wheat;
                dlg.Button = MessageBoxButton.OK;
                dlg.Image = MessageBoxImage.Warning;

                dlg.Result = MessageBoxResult.OK;
                dlg.ShowDialog();

                this.Close();
                return;
            }





            window.SizeChanged += OnSizeChanged;

            window.BorderThickness = new Thickness(0);
            info = new InfoBox(this,Base_Grid);
            //window.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 10, 10));



            Change_Lang();


            if (!(Directory.Exists(Pass.tmp)))
                Directory.CreateDirectory(Pass.tmp);
            if (!(Directory.Exists(Pass.data)))
                Directory.CreateDirectory(Pass.data);


            if (!(Directory.Exists(Pass.pindir)))
            {
                var Pincode = new PIN_Code();
                Pincode.Topmost = true;
                Pincode.Closed += this.PIN_Code_Closed;
                Pincode.Show();
                this.Hide();
                this.IsEnabled = false;
            }
            else
            {
                this.Load_Token();
            }

            

            IsStarted = true;

            
            color_list.SetWindow(window);
            color_list.SetNum(setdata.d.colors);

            title = new Title(window,Title_Grid);

            account = new Account(Accont_Erea,tokens,color_list);
            account.add.MouseLeftButtonDown += Add_Account;


            key_erea = new Key_Erea(Canvas_Erea,color_list,title);
            this.Title = "Birth   - " + key_erea.name + " -"; 

            menu = new Menu(this,Menu_Canvas,0,color_list);
            menu.new_data.MouseLeftButtonDown += On_New;
            menu.load_data.MouseLeftButtonDown += On_Load;
            menu.load_data.TouchDown += On_Load;
            menu.save_data.MouseLeftButtonDown += On_Save;
            menu.save_data.TouchDown += On_Save;
            menu.rt_data.MouseLeftButtonDown += On_Run;
            menu.setting.MouseLeftButtonDown += On_Setting;

            NewSetting();


            Base_Grid.Children.Add(Cover);
            Cover.Margin = new Thickness(0, Title_Grid.Height, 0, 0);

            color_list.Main_Back_Panel = Canvas_Erea;
            Color_Set();

            window.KeyDown += Window_KeyDown;
            title.Title_Change();



            Title_Grid.MouseLeftButtonDown += (sender, e) => { this.DragMove(); };
            title.WindowTitle.MouseLeftButtonDown += Title_Clicking;
            title.WindowTitle.MouseLeftButtonUp += Title_Clicked;

            foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
            {
                monitor_point.Add(new Point(s.Bounds.X, s.Bounds.Y));
                monitor_size.Add(new Point(s.Bounds.Width, s.Bounds.Height));
                if ((s.Bounds.Top < setdata.d.window_pos.Y) && (s.Bounds.Bottom > setdata.d.window_pos.Y) && (s.Bounds.Left < setdata.d.window_pos.X) && (s.Bounds.Right > setdata.d.window_pos.X))
                    IsInScreen = true;
            }
            if (IsInScreen)
            {
                window.Top = setdata.d.window_pos.Y;
                window.Left = setdata.d.window_pos.X;
                window.Height = setdata.d.window_size.Height;
                window.Width = setdata.d.window_size.Width;
                window.Activated += Window_Activated;
            }
            //Title_Grid.Visibility = Visibility.Hidden;
            /*
            Border winborder = new Border();
            winborder.BorderThickness = new Thickness(7);
            winborder.Child = Base_Grid;
            */
            /*neosize.Stroke = new SolidColorBrush(Color.FromArgb(255, 10, 10, 10));
            neosize.StrokeThickness = 3;
            neosize.Width = 100;
            neosize.Height = 100;
            neosize.Visibility = Visibility.Collapsed;
            Base_Grid.Children.Add(neosize);
            */
            //info.Refresh();

            info.GetKeyAndTitleAndColor(key_erea,title, color_list);

            this.Visibility = Visibility.Visible;
        }


        private void Window_Activated(object sender, EventArgs e)
        {
            if (setdata.d.IsWindow_Full)
                window.WindowState = WindowState.Maximized;
            window.Activated -= Window_Activated;

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ModifierKeys modkeys= Keyboard.Modifiers;
            if ((modkeys & ModifierKeys.Control) != ModifierKeys.None && (!setting.IsSetting))
            {
                switch (e.Key.ToString())
                {
                    case "N":
                        New();
                        break;
                    case "O":
                        Load();
                        break;
                    case "S":
                        Save();
                        break;
                    case "R":
                        Run();
                        break;
                    case "E":
                        Setting();
                        break;
                    case "Q":
                        window.Close();
                        break;

                }
            }
            else if ((modkeys & ModifierKeys.Control) != ModifierKeys.None && (setting.IsSetting) && e.Key.ToString() == "E")
                setting.Close();


            else if ((modkeys & ModifierKeys.Windows) != ModifierKeys.None) {
                Console.WriteLine("Windows");
                Console.WriteLine(e.Key.ToString());
                    }
            //Console.WriteLine(e.Key.ToString());
            //Console.WriteLine(modkeys);
            //Console.WriteLine( e.ToString());
            //Console.WriteLine(Keyboard.Modifiers);
            //IsValidKey
            //e.KeyboardDevice.Modifiers
            //Console.WriteLine("Down is "+Keyboard.GetKeyStates(Key.Down));
            //if (e.Key.Equals(Key.LWin))
                //if (Keyboard.IsKeyDown(Key.Down))
                    //Console.WriteLine("a");
                        //    window.WindowState = WindowState.Maximized;
            //    window.PreviewKeyDown += Window_PreviewKeyDown;
            //Console.WriteLine(System.Windows.Forms.Control.ModifierKeys.ToString());
            //Console.WriteLine(e.SystemKey);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
            //Console.WriteLine(e.Key.ToString());
            //Console.WriteLine(e.KeyboardDevice.IsKeyDown(Key.Down));
            var a =System.Windows.Forms.Keys.Down;
            Console.WriteLine(User32.Key.IsKeyPress(System.Windows.Forms.Keys.Down));
            //Console.WriteLine(Keyboard.GetKeyStates(Key.Down));
            if (Keyboard.IsKeyDown(Key.Down))
                Console.WriteLine("a");
            //window.PreviewKeyDown -= Window_PreviewKeyDown;

        }

        private void Add_Account(object sender, MouseButtonEventArgs e)
        {
            PIN_Code Add_Pin = new PIN_Code();
            Add_Pin.Topmost = true;
            Add_Pin.Closed += this.PIN_Code_Closed;
            Add_Pin.Show();
            this.IsEnabled = false;
        }

        private void Delete_Account(object sender, Num_With e)
        {
            int num = e.Number;
            try
            {
                if (num != -1)
                    System.IO.File.Delete(Pass.pindir + "/" + tokens[num].token.UserId + ".xml");
                else
                    foreach (var token in tokens)
                        System.IO.File.Delete(Pass.pindir + "/" + token.token.UserId + ".xml");
            }
            catch { }
            if (num != -1)
                tokens.RemoveAt(num);
            else
                tokens.Clear();
            account.Clear(account);
            account = new Account(Accont_Erea, tokens,color_list);
            account.add.MouseLeftButtonDown += Add_Account;

            
            setting.Clear(setting);
            NewSetting();

            
            setting.Visibility = Visibility.Visible;
            setting.Tab_Hidden(setting.tab[W.RT]);
            if (num != -1)
            {
                setting.Tab_Visility(setting.tab[W.AC]);
                setting.num = W.AC;
            }
            else
            {
                setting.Tab_Visility(setting.tab[W.RE]);
                setting.num = W.RE;
            }
            setting.Set_KeyEvent();

            
        }

        private void Change_Langage(object sender, Num_With args)
        {
            setdata.Close_Ready();
            setting.Clear(setting);
            NewSetting();
            setting.Visibility = Visibility.Visible;
            setting.Tab_Hidden(setting.tab[W.RT]);
            setting.Tab_Visility(setting.tab[W.CL]);
            setting.num = W.CL;
            setting.Set_KeyEvent();
        }

        public void NewSetting()
        {
            setting = new Setting(Base_Grid, setdata, account, tokens, ng_datas, color_list);
            setting.Window(this);
            setting.tab[W.AC].DelAccEvent += Delete_Account;
            setting.tab[W.RE].DelAccEvent += Delete_Account;
            setting.tab[W.CL].ChangeLangEvent += Change_Langage;

            foreach (var tab in setting.tab)
            {
                tab.SetInfoBox(info);
                tab.SettingKeyErea(key_erea);
            }
            ng_datas = setting.ng_datas;
            Color_Set();
            setting.Margin = new Thickness(0, Title_Grid.Height, 0, 0);
            Change_Size();
        }

        public void Change_Lang()
        {
            switch (setdata.d.language)
            {
                case 0:
                    Properties.Resources.Culture = System.Globalization.CultureInfo.CurrentCulture;
                    break;
                case 1:
                    Properties.Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo("ja-JP");
                    break;
                case 2:
                    Properties.Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                    break;
                case 3:
                    Properties.Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo("fr");
                    break;
            }
        }

        private void Color_Set()
        {
            color_list.SetData(tokens,account,key_erea,menu,setting);
            color_list.Update();
            color_list.SetColor(setdata.d.colors);

        }

        private void On_New(object sender, MouseButtonEventArgs e)
        {
            New();
        }
        private void On_Load(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null)
                return;
            Load();
        }
        private void On_Load(object sender, TouchEventArgs e)
        {
            Load();
        }
        private void On_Save(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null)
                return;
            Save();
        }
        private void On_Save(object sender, TouchEventArgs e)
        {
            Save();
        }
        private void On_Run(object sender, MouseButtonEventArgs e)
        {
            Run();
        }
        private void On_Setting(object sender, MouseButtonEventArgs e)
        {
            Setting();
        }

        private void New()
        {
            menu.New_Data(key_erea, this);
            title.Title_Change();
            info.Open(Properties.Resources.info_newfile,InfoBox.ButtonType.New,5000);           
        }
        private void Load()
        {
            menu.Load_Data(key_erea, Canvas_Erea);
            key_erea.Change_Size();
            Change_Size();
            title.Title_Change();
        }
        private void Save()
        {
            menu.Save_Data(key_erea);
            title.Title_Change();
        }
        private void Run()
        {
            if (tokens.Count <= 0)
            {
                return;
            }
            if (menu.IsRun == false)
            {
                menu.rt_data.Source = new BitmapImage(new Uri("img/rt_on.png", UriKind.Relative));
                rt_pos[0] = Canvas.GetBottom(menu.rt_data);
                rt_pos[1] = Canvas.GetLeft(menu.rt_data);

                Cover.Width = Base_Grid.Width;
                Cover.Height = Base_Grid.Height-Title_Grid.Height;
                Cover.Background = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                Menu_Canvas.Children.Remove(menu.rt_data);
                Cover.Children.Add(menu.rt_data);
                Cover.Visibility = Visibility.Visible;

                Canvas.SetBottom(menu.rt_data, Menu_Canvas.Margin.Bottom + Canvas.GetBottom(menu.rt_data));
                Canvas.SetLeft(menu.rt_data, Main_Grid.Margin.Left + Menu_Canvas.Margin.Left + Canvas.GetLeft(menu.rt_data));
                //DoEvents();

                //DateTime startDt = DateTime.Now;
                //Console.WriteLine(setdata.d.interval +","+ setdata.d.limit_num);
                twitter = new Twitter(tokens, setdata.d.interval, setdata, ng_datas);
                twitter.RT_Stop_Event += RT_Stop;
                //TimeSpan twitterDt = DateTime.Now - startDt;
                twitter.Make_Regex(key_erea);
                //TimeSpan regexDt = DateTime.Now - startDt;
                twitter.Run();
                //TimeSpan runDt = DateTime.Now - startDt;

                //Console.WriteLine(twitterDt);
                //Console.WriteLine(regexDt);
                //Console.WriteLine(runDt);

                menu.IsRun = true;
            }
            else
            {
                RT_Stop();
            }

        }
        private void RT_Stop(object sender, EventArgs a)
        {
            this.Dispatcher.BeginInvoke(new Action(() => { RT_Stop(); }));
           
        }
        private void RT_Stop()
        {
            menu.rt_data.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color_list.num] + "/rt.png", UriKind.Relative));
            twitter.RT_Stop_Event -= RT_Stop;
            twitter.Stop();
            Cover.Visibility = Visibility.Collapsed;
            Cover.Children.Remove(menu.rt_data);
            Menu_Canvas.Children.Add(menu.rt_data);
            Canvas.SetBottom(menu.rt_data, rt_pos[0]);
            Canvas.SetLeft(menu.rt_data, rt_pos[1]);

            menu.IsRun = false;

        }
        private void Setting()
        {
            setting.Visibility = Visibility.Visible;
            foreach (var tab in setting.tab)
                setting.Tab_Hidden(tab);
            setting.Tab_Visility(setting.tab[setting.num]);
            setting.IsSetting=true;
            setting.Set_KeyEvent();
            //setting.tab[W.RT].Contents.Height = setting.tab[W.RT].Contents.RenderSize.Height;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (second_run)
            {
                mutex.Close();
                mutex = null;
            }
            else
            {
                mutex.ReleaseMutex();
                mutex.Close();
                try
                {
                    setting.Setting_Store();
                }
                catch
                {

                }

            }

        }


        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Change_Size();
        }

        public void Change_Size()
        {
            //Console.WriteLine("Change_Size");
            double border = window.BorderThickness.Top * 2;

            double main_height = window.RenderSize.Height-border - Title_Grid.Height >= 0.0 ? window.RenderSize.Height - border - Title_Grid.Height : 0.0;
            double main_width = window.RenderSize.Width - border - Accont_Erea.Width >= 0.0 ? window.RenderSize.Width - border - Accont_Erea.Width : 0.0;

            double task_bar = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height- System.Windows.Forms.SystemInformation.WorkingArea.Height;

            Point pt = new Point();
            Size size = new Size();
            Point near0 = new Point(double.PositiveInfinity, double.PositiveInfinity);
            int monitor_num = -1;


            /*
            if (window.WindowState.Equals(WindowState.Maximized))
            {
                pt = window.PointToScreen(new Point(0.0, 0.0));
                
                size = window.RenderSize;
                window.SizeChanged -= OnSizeChanged;
                window.StateChanged -= OnStateChanged;
                window.WindowState = WindowState.Normal;

                foreach(var point in monitor_point.Select((v,i) => new { v, i }))
                {
                    if((System.Math.Abs(point.v.X-pt.X) <= near0.X) && (System.Math.Abs(point.v.Y - pt.Y) <= near0.Y))
                    {
                        near0.X = System.Math.Abs(point.v.X - pt.X);
                        near0.Y = System.Math.Abs(point.v.Y - pt.Y);
                        monitor_num = point.i;
                    }
                }

                
                window.Top = monitor_point[monitor_num].Y;
                window.Left = monitor_point[monitor_num].X;
                window.Height = monitor_size[monitor_num].Y - task_bar;
                window.Width = monitor_size[monitor_num].X;
                //Console.WriteLine(pt);
                window.StateChanged += OnStateChanged;
                //window.SizeChanged += OnSizeChanged;
                main_width -= 14;
                main_height -= (task_bar+14);
                window.ResizeMode = ResizeMode.NoResize;

            }
            */


            Title_Grid.Width    = window.RenderSize.Width;

            Accont_Erea.Height  = main_height;
            account.sv.Height   = main_height;

            Main_Grid.Height    = main_height;

            Menu_Canvas.Width = main_width;


            Canvas_Erea.Height  = window.RenderSize.Height - border > Menu_Canvas.Height + Title_Grid.Height ? window.RenderSize.Height - border - Menu_Canvas.Height - Title_Grid.Height : 0;
            Canvas_Erea.Width   = main_width;

           
            key_erea.sv.Height  = window.RenderSize.Height - border > Menu_Canvas.Height + Title_Grid.Height ? window.RenderSize.Height - border - Menu_Canvas.Height - Title_Grid.Height : 0;
            key_erea.sv.Width   = main_width;

            foreach (var tab in setting.tab) {
                tab.sv.Height   = main_height;
                tab.sv.Width    = window.RenderSize.Width - border - 100 >= 0.0 ? window.RenderSize.Width - border - 100 : 0.0;
                //tab.Contents.Width = tab.Contents.RenderSize.Width;
                //tab.Contents.Height = tab.Contents.RenderSize.Height;

            }

            title.Title_Length(border);

            //Console.WriteLine("IsWindowMaxed: " + IsWindowMaxed);


            //Console.WriteLine(System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height-System.Windows.Forms.SystemInformation.WorkingArea.Height);
            //Console.WriteLine(System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width - System.Windows.Forms.SystemInformation.WorkingArea.Width);
            /*
            Accont_Erea.Height = window.RenderSize.Height - 39;
            account.sv.Height = window.RenderSize.Height - 39;
            Main_Grid.Width = window.RenderSize.Width - Accont_Erea.Width + 10;
            Main_Grid.Height = window.RenderSize.Height - 39;
            Canvas_Erea.Width = window.RenderSize.Width - Accont_Erea.Width - 15;
            Canvas_Erea.Height = window.RenderSize.Height > Menu_Canvas.Height + 39 ? window.RenderSize.Height - Menu_Canvas.Height - 39 : 0;
            Menu_Canvas.Width = window.RenderSize.Width - Accont_Erea.Width - 15;


            key_erea.sv.Width = window.RenderSize.Width - Accont_Erea.Width - 15;
            key_erea.sv.Height = window.RenderSize.Height > Menu_Canvas.Height + 39 ? window.RenderSize.Height - Menu_Canvas.Height - 39 : 0;
            
            foreach(var tab in setting.tab) {
                tab.sv.Width= window.RenderSize.Width-100-15;
                tab.sv.Height = window.RenderSize.Height - 39;
                //tab.Contents.Width = tab.Contents.RenderSize.Width;
                //tab.Contents.Height = tab.Contents.RenderSize.Height;

            }
            */
            //Console.WriteLine(setting.tab[0].Contents.RenderSize.Height);
            //Console.WriteLine(setting.tab[0].Contents.RenderSize.Width);

            //Console.WriteLine(key_erea.sv.Height);
            //Console.WriteLine(window.MaxWidth);
        }


        private void Title_Clicking(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine("Title_Clicking");
            //Console.WriteLine("IsWindowMaxed: " + IsWindowMaxed + ", IsWindownomarize: " + IsWindownomarize);
            if (IsWindowMaxed && !(IsWindownomarize))
            {
                IsWindownomarize = true;

                window.WindowState = WindowState.Normal;
                    
                //window.ResizeMode = ResizeMode.CanResizeWithGrip;

                //Point a = window.PointToScreen(new Point(0,0));
                //Console.WriteLine(a);
                //neosize.Visibility
                //window.SizeChanged += OnSizeChanged;
                //window.Width = win_size.Width;
                //window.Height = win_size.Height;
                tcstartDt = DateTime.Now;
                //User32.Mouse.SetPosition((int)a.X + 20,(int)a.Y+10);
                //oldmousepos = System.Windows.Forms.Cursor.Position;
                //Console.WriteLine(System.Windows.Forms.Cursor.Position);

            }
            else
            {
                TimeSpan span = DateTime.Now - tcstartDt;
                if (span.TotalSeconds < 0.3)
                    window.WindowState = WindowState.Maximized;
                tcstartDt = DateTime.Now;

            }
            title_clicking = true;

        }

        private void Title_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (!title_clicking)
                return;
            //Console.WriteLine("Title_Clicked");
            if (IsWindowMaxed && IsWindownomarize)
            {
                TimeSpan span = DateTime.Now - tcstartDt;
                if(span.TotalSeconds<0.3)
                {
                    window.Top = win_top;
                    window.Left = win_left;
                }
                //IsWindowMaxed = !IsWindowMaxed;
                IsWindownomarize = !IsWindownomarize;
                Change_Size();
            }
            title_clicking = false;
            IsWindownomarize = false;

        }

        private void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }

        private void OnStateChanged(object sender, EventArgs e)
        {

            //Console.WriteLine("State Changed");

            switch (((Window)sender).WindowState)
            {
                case WindowState.Normal:
                    title.Max_Off();
                    title.Title_Length();
                    IsWindowMaxed = false;
                    setdata.d.IsWindow_Full = false;
                    window.BorderThickness = new Thickness(0);

                    break;
                case WindowState.Maximized:
                    title.Max_On();
                    title.Title_Length();
                    win_size = window.RenderSize;
                    win_top = window.Top;
                    win_left = window.Left;
                    IsWindowMaxed = true;
                    setdata.d.IsWindow_Full = true;
                    window.BorderThickness = new Thickness(8);
                    break;
            }
            //Console.WriteLine(setdata.d.IsWindow_Full);

            /*
                    switch (((Window)sender).WindowState)
                    {
                        case WindowState.Normal:
                            break;
                        case WindowState.Maximized:
                            if (!IsWindowMaxed) { 
                                win_size    = window.RenderSize;
                                win_top     = window.Top;
                                win_left    = window.Left;
                                title.Max_On();
                                //Console.WriteLine("OnStateChanged Maximamed");
                            }
                            else
                            {
                                window.SizeChanged += OnSizeChanged;
                                window.WindowState = WindowState.Normal;
                                window.Width = win_size.Width;
                                window.Height = win_size.Height;
                                window.Top = win_top;
                                window.Left = win_left;
                                window.ResizeMode = ResizeMode.CanResizeWithGrip;
                                title.Max_Off();
                            }
                            IsWindowMaxed = !IsWindowMaxed;
                            break;

                    }
            */


        }

    }


}
