using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.Serialization;
using System.Xml;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Win32;

namespace Birth_First
{

    class W
    {
        public const int RT = 0;
        public const int AC = 1;
        public const int CL = 2;
        public const int NG = 3;
        public const int RE = 4;
        public const int AS = 5;
    }


    class Text_With : EventArgs
    {
        private string  str;
        public Text_With(string t)
        {
            str=t;
        }

        public string Name
        {
            get { return str; }
        }
    }



    class Range
    {
        public class Num
        {
            public int? min = null;
            public int? max = null;
            public Num(int? fmin, int? fmax)
            {
                min = fmin;
                max = fmax;
            }
            public bool Inside(int num)
            {
                return ((min == null) || (num >= min)) && ((max == null) || (num <= max));
            }
            public bool Outside(int num)
            {
                return ((min != null) && (num < min)) || ((max != null) && (num > max));
            }
            public string ToString()
            {
                return "(" + min + "～" + max + ")";
            }
        }

        public static Num limit_num = new Num(null, 10000);
        public static Num interval = new Num(5,10000); 

}



    class NG_Contents
    {
        public Canvas c;
        public TextBox textbox;
        public TextBlock text;
        public Image close;
        public TextBlock at;
        public bool IsNew;
        public long? userid;

        Color_List color_list;

        TextBlock warning;

        public NG_Contents(StackPanel s,Color_List fromcolor)
        {
            color_list = fromcolor;
            IsNew = true;
            c       = new Canvas();
            text    = new TextBlock();
            textbox = new TextBox();
            close   = new Image();
            warning = new TextBlock();
            at = new TextBlock();
            double size = 15;
            c.Children.Add(at);
            at.Text = "@";
            color_list.Setting_TextBlock = at;
            Canvas.SetTop(at, 20);
            Canvas.SetLeft(at, -10);
            text.FontSize   = size;
            textbox.FontSize= size;
            at.FontSize     = size;

            userid = null;
            s.Children.Add(c);
            c.Height =45;
            c.Width = 155;
            textbox.Width = 120;
            close.Source = new BitmapImage(new Uri("img/close_off.png", UriKind.Relative));
            close.MouseEnter += new MouseEventHandler(Close_On_Enter);
            close.MouseLeave += new MouseEventHandler(Close_On_Leave);
            Canvas.SetLeft(close, 135);
            Canvas.SetTop(close, 25);
            color_list.Setting_TextBlock = text;
            color_list.Setting_TextBox = textbox;
            //color_list.Setting_TextBlock = warning;
            warning.Foreground = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));
           
            if (s.Name == "NG_Word_St")
                at.Visibility = Visibility.Hidden;
            c.Children.Add(warning);
            Canvas.SetLeft(warning, 10);

            warning.Visibility = Visibility.Collapsed;
        }

        public void Add_Screen(string screen,long? id)
        {
            c.Children.Add(text);
            c.Children.Add(close);
            text.Text = screen;
            textbox.Text = screen;
            userid = id;
            Canvas.SetTop(text, 20);
            Canvas.SetLeft(text, 5);
        }
        public void Add_TextBox(string word)
        {
            c.Children.Add(textbox);
            c.Children.Add(close);
            textbox.Text = word;
            Canvas.SetTop(textbox, 20);
            Canvas.SetLeft(textbox, 5);
        }
        public void Add_TextBox()
        {
            text.Foreground = color_list.GetSettingTextForeColor();
            textbox.Foreground = color_list.GetSettingTextForeColor();
            textbox.Background = color_list.GetSettingTextBackColor();
            c.Children.Add(textbox);
            c.Children.Add(close);
            Canvas.SetTop(textbox, 20);
            Canvas.SetLeft(textbox, 5);
            close.Visibility = Visibility.Collapsed;
        }

        public void Deside_Screen()
        {
            c.Children.Remove(textbox);
            c.Children.Add(text);
            text.Text = textbox.Text;
            color_list.Setting_TextBlock = text;
            Canvas.SetLeft(text, 5);
            Canvas.SetTop(text, 20);
        }

        public void On_Warning(string text)
        {
            warning.Text = text;
            warning.Visibility = Visibility.Visible;
        }

        public void Off_Warning(long? id)
        {
            warning.Text = "";
            warning.Visibility = Visibility.Collapsed;
            userid = id;
        }

        private void Close_On_Leave(object sender, MouseEventArgs e)
        {
            close.Source = new BitmapImage(new Uri("img/close_off.png", UriKind.Relative));
        }

        private void Close_On_Enter(object sender, MouseEventArgs e)
        {
            close.Source = new BitmapImage(new Uri("img/close_over.png", UriKind.Relative));
        }

    }



    class Setting_Tab
    {
        public Label Subject = new Label();
        public Canvas Window = new Canvas();
        public StackPanel Contents = new StackPanel();

        public ScrollViewer sv = new ScrollViewer();

        Color_List color_list;

        StackPanel  stack   = new StackPanel();
        StackPanel  instack = new StackPanel();
        TextBlock   itemlb  = new TextBlock();
        TextBlock   itemlb2 = new TextBlock();
        TextBlock   text    = new TextBlock();
        CheckBox    check   = new CheckBox();
        TextBox     num     = new TextBox();
        Image       img     = new Image();
        ComboBox    combo   = new ComboBox();
        TextBox     urlbox  = new TextBox();
        Slider      slider  = new Slider();

        Label       label1 = new Label();
        Label       label2 = new Label();

        StackPanel NG_Word_St       = new StackPanel();
        StackPanel NG_Account_St    = new StackPanel();

        public List<NG_Contents> NGWd_List = new List<NG_Contents>();
        public List<NG_Contents> NGAcc_List = new List<NG_Contents>();

        NG_Data ng_datas;

        Key_Erea key_erea;
        SetData setdata;
        Account account;
        InfoBox info;
        List<Tokens> tokens;

        bool    IsChanging= false;
        string  old_text = "";
        int     old_selection;

        

        Pass pass = new Pass();
        Setting setting;

        public void SettingKeyErea(Key_Erea fromkey)
        {
            key_erea = fromkey;
            if (setdata.d.burl != "")
            {
                try
                {
                    key_erea.back.Opacity = setdata.d.back_opacity;
                    key_erea.back.Source = new BitmapImage(new Uri(setdata.d.burl, UriKind.Absolute));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else { key_erea.back.Source = null; }

        }


        public Setting_Tab(Setting fromsetting, SetData fromsetdata,Account fromaccount, List<Tokens> fromtokens,NG_Data fromngdata,Color_List fromcolor)
        {
            setting = fromsetting;
            tokens = fromtokens;
            setdata = fromsetdata;
            account = fromaccount;
            ng_datas = fromngdata;
            color_list = fromcolor;
            //Window.Children.Add(Contents);

            
            Window.Children.Add(sv);
            sv.Content = Contents;

            Contents.Margin = new Thickness(10);
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Contents.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Contents.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            color_list.Scroll_Bar = sv;
            //Contents.Width = 100;
            //Contents.Height = 100;


        }

        public void SettingWindow(int num)
        {
            switch (num)
            {
                case W.RT:
                    Retweet_Window();
                    break;
                case W.AC:
                    Account_Window();
                    break;
                case W.CL:
                    Thema_Window();
                    break;
                case W.NG:
                    NG_Window();
                    break;
                case W.RE:
                    Reset_Window();
                    break;
                case W.AS:
                    About_Window();
                    break;
                default:
                    break;
                    
            }
            this.Window.Visibility = Visibility.Hidden;
        }

        private void Retweet_Window()
        {
            StackClear();
            Setting_Data(Properties.Resources.setting_retweet_1, ref setdata.d.limit_num,setdata.limit_num);
            Setting_Data(Properties.Resources.setting_retweet_2, ref setdata.d.interval, setdata.interval);
            Setting_Data(Properties.Resources.setting_retweet_3, ref setdata.d.rt_from_rt, setdata.rt_from_rt,Contents);
            Setting_Data(Properties.Resources.setting_retweet_4, ref setdata.d.rt_from_me,setdata.rt_from_me, Contents);
            Setting_Data(Properties.Resources.setting_retweet_5, ref setdata.d.propaganda, setdata.propaganda, Contents);
            List<string> accname = new List<string>();
            accname.Add(Properties.Resources.setting_retweet_6);
            foreach (var data in account.accs)
            {
                accname.Add(data.user);
            }
            if (account.accs.Count < setdata.d.stream_from)
                setdata.d.stream_from = 0;
            Setting_Data(Properties.Resources.setting_retweet_7,accname.ToArray(),ref setdata.d.stream_from, setdata.stream_from);
        }

        private void Account_Window()
        {
            StackClear();
            if (tokens.Count == 0)
                return;
            foreach(var acc in account.accs.Select((v, i) => new { v, i }))
            {
                //RadioButton radio = new RadioButton();
                Setting_Data(acc.v.user,acc.v.screen,acc.v.imgurl,acc.i);
            }
        }

        private void Thema_Window()
        {
            Setting_Data(Properties.Resources.setting_theme_1,Pass.colors,ref setdata.d.colors, "colors");
            Setting_Data(Properties.Resources.setting_theme_2,"...",ref setdata.d.burl, "burl");
            Setting_Data(Properties.Resources.setting_theme_3, 1.0,0.01, ref setdata.d.back_opacity, "back_opacity");
            Setting_Data(Properties.Resources.setting_theme_4, Pass.language, ref setdata.d.language, "language");
        }

        private void NG_Window()
        {
            StackClear();
            StackPanel NG_Stack = new StackPanel();

            NG_Stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            Label NG_Word_btn = new Label();
            Label NG_Account_btn = new Label();
            NG_Word_btn.Content =Properties.Resources.setting_ng_1;
            NG_Word_btn.Width = 135;
            NG_Word_btn.FontSize = 15;
            NG_Word_btn.Padding = new Thickness(5);
            NG_Account_btn.Content = Properties.Resources.setting_ng_2;
            NG_Account_btn.Width = 135;
            NG_Account_btn.FontSize = 15;
            NG_Account_btn.Padding = new Thickness(5);

            NG_Stack.Orientation = Orientation.Horizontal;
            NG_Stack.Children.Add(NG_Word_btn);
            NG_Stack.Children.Add(NG_Account_btn);
            NG_Word_btn.MouseLeftButtonDown += NG_Word_btn_Click;
            NG_Account_btn.MouseLeftButtonDown += NG_Account_btn_Click;
            color_list.Setting_Button = NG_Word_btn;
            color_list.Setting_Button = NG_Account_btn;

            Setting_Data(Properties.Resources.setting_ng_3, ref setdata.d.Use_NG_WD, "Use_NG_WD", NG_Word_St);
            Setting_Data(Properties.Resources.setting_ng_4, ref setdata.d.Use_NG_ACC, "Use_NG_ACC", NG_Account_St);

            NG_Word_St.Name = "NG_Word_St";
            NG_Account_St.Name = "NG_Account_St";
            NG_Word_St.Margin = new Thickness(20, 0, 0, 0);
            NG_Account_St.Margin = new Thickness(20, 0, 0, 0);
            NG_Word_St.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            NG_Account_St.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            NGWd_List = new List<NG_Contents>();
            NGAcc_List  = new List<NG_Contents>();



            NG_Contents tmp;
            foreach (var word in ng_datas.words.Select((v,i)=>new{v,i}))
            {
                tmp = new NG_Contents(NG_Word_St,color_list);
                tmp.Add_TextBox(word.v);
                tmp.c.Name = "NGWD" + word.i.ToString();
                tmp.close.MouseLeftButtonDown += Close_NG_Word;
                NGWd_List.Add(tmp);
            }
            tmp = new NG_Contents(NG_Word_St,color_list);
            tmp.Add_TextBox();
            tmp.textbox.TextChanged += Add_NG_Word;
            tmp.c.Name = "NGWD" + NGWd_List.Count.ToString();
            tmp.close.MouseLeftButtonDown += Close_NG_Word;
            NGWd_List.Add(tmp);


            foreach(var acc in ng_datas.account.Select((v, i) => new { v, i }))
            {
                tmp = new NG_Contents(NG_Account_St,color_list);
                if(acc.v.id!=-1)
                    tmp.Add_Screen(acc.v.screen,acc.v.id);
                else
                    tmp.Add_TextBox(acc.v.screen);
                tmp.c.Name = "NGAC" + acc.i.ToString();
                tmp.close.MouseLeftButtonDown += Close_NG_Accont;
                NGAcc_List.Add(tmp);
            }
            tmp = new NG_Contents(NG_Account_St,color_list);
            tmp.Add_TextBox();
            tmp.textbox.TextChanged += Add_NG_Account;
            tmp.textbox.LostFocus += Check_NG_Account;
            tmp.c.Name = "NGAC" + NGAcc_List.Count.ToString();
            tmp.close.MouseLeftButtonDown += Close_NG_Accont;
            NGAcc_List.Add(tmp);


            Contents.Children.Add(NG_Stack);
            Contents.Children.Add(NG_Word_St);
            Contents.Children.Add(NG_Account_St);
            NG_Account_St.Visibility = Visibility.Collapsed;
            StackClear();
        }

        private void Reset_Window()
        {
            StackClear();
            Label rt_reset = Setting_Data(Properties.Resources.setting_reset_1,330, Contents);
            color_list.Setting_Button = rt_reset;
            rt_reset.MouseLeftButtonDown += Rt_reset_Click;
            Label acc_reset = Setting_Data(Properties.Resources.setting_reset_2, 330, Contents);
            color_list.Setting_Button = acc_reset;
            acc_reset.MouseLeftButtonDown += Acc_reset_Click;
            Label ng_reset = Setting_Data(Properties.Resources.setting_reset_3, 330, Contents);
            color_list.Setting_Button = ng_reset;
            ng_reset.MouseLeftButtonDown += Ng_reset_Click;
            stack.Orientation = Orientation.Horizontal;
            stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Contents.Children.Add(stack);
            Label ngwd_reset = Setting_Data(Properties.Resources.setting_reset_4,160, stack);
            color_list.Setting_Button = ngwd_reset;
            ngwd_reset.MouseLeftButtonDown += Ngwd_reset_Click;
            Label ngacc_reset = Setting_Data(Properties.Resources.setting_reset_5, 160, stack);
            color_list.Setting_Button = ngacc_reset;
            ngacc_reset.MouseLeftButtonDown += Ngacc_reset_Click;
            Label all_reset = Setting_Data(Properties.Resources.setting_reset_6, 330, Contents);
            color_list.Setting_Button = all_reset;
            all_reset.MouseLeftButtonDown += All_reset_Click;

            StackClear();

        }

        private void About_Window()
        {
            StackClear();
            Assembly asm =Assembly.GetExecutingAssembly();
            SetData("Birth", 25, 10, "Version" + asm.GetName().Version, 16);
            SetData("Birth is an Immediate Retweet Tool with a Heart.\n", 16);
            SetData("Project Home:", 12, 3, "Birth", "https://github.com/curonet/Birth/", 12);
            SetData("Source:", 12, 3, "Github", "https://github.com/curonet/Birth/tree/master/Source", 12);
            SetData("Twitter:", 12, 3, "@birth_rt", "https://twitter.com/birth_rt", 12);
            SetData("Author:", 12, 3, "@curonet", "https://twitter.com/curonet", 12);
            SetData("Blog:", 12, 3, "RadiumProduction", "http://radiumproduction.blog.shinobi.jp/", 12);
            SetData(" ", 14);
            SetData("Amazon Wish List:", 12, 3, "wishlist@curonet", "https://www.amazon.co.jp/gp/registry/wishlist/306ZZ1E7TFK4V/ref=nav_wishlist_lists_1", 12);
            SetData(" ", 14);
            ShowCopyRight(Contents);
        }


        private void Setting_Data(string t, ref int i,string ins)
        {

            stack.Orientation = Orientation.Horizontal;
            itemlb.Text = t;
            stack.Children.Add(itemlb);
            num.Width = 50;
            num.Text = i.ToString();
            num.TextChanged += Num_Changed;
            stack.Children.Add(num);
            num.Name = ins;
            Contents.Children.Add(stack);
            StackClear();

        }

        private void Num_Changed(object sender, TextChangedEventArgs e)
        {
            var num = ((TextBox)(sender)).Text;
            var name = ((TextBox)(sender)).Name;
            if (IsChanging)
                return;
            IsChanging = true;

            float x;

            if (!(Single.TryParse(num, out x)))
            {
                if (!(num == ""))
                {
                    info.Open(Properties.Resources.info_number, 5000);
                    ((TextBox)(sender)).Text = old_text;
                    ((TextBox)(sender)).Select(old_selection, 0);
                }
            }
            else if (Range.limit_num.Outside(int.Parse(num)) && name==setdata.limit_num)
            {
                //制限の旨を表示
                info.Open(Properties.Resources.info_unvaild + Environment.NewLine +Range.limit_num.ToString(), 5000);
                if (int.Parse(num) >= Range.interval.min)
                {
                    ((TextBox)(sender)).Text = old_text;
                    ((TextBox)(sender)).Select(old_selection, 0);
                }
            }
            else if (Range.interval.Outside(int.Parse(num)) && name == setdata.interval)
            {
                //制限の旨を表示
                info.Open(Properties.Resources.info_unvaild + Environment.NewLine + Range.interval.ToString(), 5000);
                if(int.Parse(num) >= Range.interval.min)
                {
                    ((TextBox)(sender)).Text = old_text;
                    ((TextBox)(sender)).Select(old_selection, 0);
                }
            }
            else
            {
                if (!(num == ""))
                {
                    setdata.SetValue(((TextBox)(sender)).Name, int.Parse(num));
                    info.Close();
                }
            }/*
            if (!(Single.TryParse(num, out x)))
            {
                if (!(num == ""))
                {
                    info.Open(Properties.Resources.info_number, 5000);
                    ((TextBox)(sender)).Text = old_text;
                    ((TextBox)(sender)).Select(old_selection, 0);
                }
            }
            else if (int.Parse(num) > 10000)
            {
                //制限の旨を表示
                info.Open(Properties.Resources.info_unvaild+Environment.NewLine+"(～10000)", 5000);
                ((TextBox)(sender)).Text = old_text;
                ((TextBox)(sender)).Select(old_selection, 0);
            }
            else if ((int.Parse(num) < 5) && (((TextBox)(sender)).Name == setdata.interval))
            {
                //制限の旨を表示
                info.Open(Properties.Resources.info_unvaild + Environment.NewLine+ "(5～)", 5000);
            }
            else
            {
                if (!(num == ""))
                {
                    setdata.SetValue(((TextBox)(sender)).Name, int.Parse(num));
                    info.Close();
                }
            }
            */
            old_selection = ((TextBox)(sender)).SelectionStart;
            old_text = ((TextBox)(sender)).Text;
            IsChanging = false;
        }

        


        private void Setting_Data(string t, ref bool b,string ins,Panel mother)
        {
            stack.Orientation = Orientation.Horizontal;
            check.IsChecked = b;
            check.Name = ins;
            itemlb.Text = t;
            itemlb.MouseLeftButtonDown += Check_Label_Click;
            stack.Children.Add(check);
            check.Click += Check_Changed;
            stack.Children.Add(itemlb);
            mother.Children.Add(stack);
            StackClear();
        }


        private void Check_Changed(object sender, RoutedEventArgs e)
        {
            setdata.SetValue(((CheckBox)(sender)).Name, ((CheckBox)sender).IsChecked);
        }

        private void Check_Label_Click(object sender, MouseButtonEventArgs e)
        {
            CheckBox tmp = ((CheckBox)(((StackPanel)(((TextBlock)sender).Parent)).Children[0]));
            tmp.IsChecked = !tmp.IsChecked;
            setdata.SetValue(tmp.Name,tmp.IsChecked);
        }


        private void Setting_Data(string name,string screen,string imgurl,int number)
        {
            stack.Orientation = Orientation.Horizontal;
            img.Source = new BitmapImage(new Uri(imgurl));
            img.Width = 60;
            stack.Children.Add(img);

            Grid accdel = new Grid();
            accdel.HorizontalAlignment = HorizontalAlignment.Left;
            accdel.VerticalAlignment = VerticalAlignment.Top;

            //accdel.Orientation = Orientation.Horizontal;
            instack.Children.Add(accdel);
            StackPanel accname = new StackPanel();
            accdel.Children.Add(accname);
            itemlb.Text = name;
            accname.Children.Add(itemlb);
            itemlb2.Text = "@"+ screen;
            accname.Children.Add(itemlb2);
            accname.Margin = new Thickness(0,0,0,0);

            TextBlock delete_acc = new TextBlock();
            delete_acc.Text = Properties.Resources.setting_account_1;
            delete_acc.FontSize = 14;
            delete_acc.Foreground= new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            delete_acc.Background = new SolidColorBrush(Color.FromArgb(225, 255, 0, 0));
            delete_acc.VerticalAlignment = VerticalAlignment.Center;
            delete_acc.Padding = new Thickness(4);
            accdel.Children.Add(delete_acc);
            delete_acc.Name = "dlacinfo" + number.ToString();
            delete_acc.Margin = new Thickness(200, 0, 0, 0);
            delete_acc.MouseLeftButtonDown += Delete_acc_Click;

            StackPanel block = new StackPanel();
            CheckBox bcheck = new CheckBox();
            TextBlock btext = new TextBlock();
            block.Orientation = Orientation.Horizontal;
            bcheck.IsChecked = tokens[number].UseBlock;
            bcheck.Name = "bcheck" + number.ToString();
            bcheck.Click += Bcheck_Click;
            block.Children.Add(bcheck);
            btext.Text = Properties.Resources.setting_account_2;
            btext.MouseLeftButtonDown += Bchech_Text_Click;
            block.Children.Add(btext);
            color_list.Setting_TextBlock = btext;



            instack.Children.Add(block);
            stack.Children.Add(instack);
            //num.Name = name;
            Contents.Children.Add(stack);
            StackClear();
        }


        private void Delete_acc_Click(object sender, MouseButtonEventArgs e)
        {
            int num =int.Parse((((TextBlock)sender).Name.ToString()).Remove(0, 8));
            OnDelAccEvent(new Num_With(num));

        }

        public delegate void DelAccEventHandler(object sender, Num_With args);
        public event DelAccEventHandler DelAccEvent;        
        protected virtual void OnDelAccEvent(Num_With args)
        {
            DelAccEventHandler handler = DelAccEvent;

            //イベントのハンドラが割り当てられていない場合はイベントを発生させない
            if (handler != null)
            {
                //イベント発生
                handler(this, args);
            }
        }

        private void Bcheck_Click(object sender, RoutedEventArgs e)
        {
            var num = int.Parse((((CheckBox)(sender)).Name).Trim('b', 'c', 'h', 'e', 'c', 'k'));
            tokens[num].UseBlock= (bool)((CheckBox)sender).IsChecked;
        }

        private void Bchech_Text_Click(object sender, MouseButtonEventArgs e)
        {
            CheckBox tmp = ((CheckBox)(((StackPanel)(((TextBlock)sender).Parent)).Children[0]));
            tmp.IsChecked = !tmp.IsChecked;
            var num = int.Parse((tmp.Name).Trim('b', 'c', 'h', 'e', 'c', 'k'));
            tokens[num].UseBlock = (bool)tmp.IsChecked;
        }


        private void Setting_Data(string text,string[] lb,ref int init,string ins)
        {
            stack.Orientation = Orientation.Horizontal;

            //combo.AllowDrop = false;
            itemlb.Text = text;
            color_list.Setting_TextBlock = itemlb;
            stack.Children.Add(itemlb);
            combo.Name = ins;
            combo.DisplayMemberPath = "text";

            foreach (var data in lb.Select((v, i) => new { v, i }))
            {
                combo.Items.Add(new { text = data.v, value = data.i });

            }
            combo.SelectedIndex = init;

            combo.DropDownClosed += Combo_DropDownClosed;
            //combo. += Combo_LostFocus;
            combo.DropDownOpened += Combo_Click;

            color_list.Setting_Combo = combo;
            stack.Children.Add(combo);
            Contents.Children.Add(stack);
            StackClear();
        }

        private void Combo_Click(object sender, EventArgs e)
        {
            setting.UnSet_KeyEvent();
        }

        private void Combo_DropDownClosed(object sender, EventArgs e)
        {
            setdata.SetValue(((ComboBox)sender).Name, ((ComboBox)sender).SelectedIndex);
            if (((ComboBox)sender).Name =="colors")
                color_list.SetColor(((ComboBox)sender).SelectedIndex);
            if (((ComboBox)sender).Name == "language")
            {
                switch (((ComboBox)sender).SelectedIndex)
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
                OnChangeLangEvent(new Num_With(((ComboBox)sender).SelectedIndex));
            }
            setting.Set_KeyEvent();

            ((StackPanel)(((ComboBox)sender).Parent)).Focusable = true;
            //Console.WriteLine( ((StackPanel)(((ComboBox)sender).Parent)).Focus());
            ((StackPanel)(((ComboBox)sender).Parent)).Focusable = false;
 
        }

        public delegate void ChangeLangEventHandler(object sender, Num_With args);
        public event ChangeLangEventHandler ChangeLangEvent;
        protected virtual void OnChangeLangEvent(Num_With args)
        {
            ChangeLangEventHandler handler = ChangeLangEvent;

            //イベントのハンドラが割り当てられていない場合はイベントを発生させない
            if (handler != null)
            {
                //イベント発生
                handler(this, args);
            }
        }




        private void Setting_Data(string text, string btntxt, ref string init, string ins)
        {
            stack.Orientation = Orientation.Horizontal;
            itemlb.Text = text;
            stack.Children.Add(itemlb);
            urlbox.Width = 220;
            urlbox.Height = 20;
            urlbox.Text = init;
            urlbox.Name = ins;
            urlbox.TextChanged += Textbox1_TextChanged;
            stack.Children.Add(urlbox);
            Label img_open = Setting_Data(btntxt, 20, stack);
            img_open.Height = 20;
            img_open.Padding = new Thickness(0);
            img_open.Margin = new Thickness(2,-5, 0, 0);

            color_list.Setting_Button = img_open;
            img_open.MouseLeftButtonDown += Img_open_MouseLeftButtonDown;
            img_open.TouchDown += Img_open_TouchDown;
            Contents.Children.Add(stack);
            StackClear();
        }

        private void Img_open_TouchDown(object sender, TouchEventArgs e)
        {
            Load_Back_Img(sender);
        }

        private void Img_open_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.StylusDevice != null)
                return;
            Load_Back_Img(sender);
        }

        private void Load_Back_Img(object sender)
        {
            string exePath = Environment.GetCommandLineArgs()[0];
            string exeFullPath = System.IO.Path.GetFullPath(exePath);
            string path = System.IO.Path.GetDirectoryName(exeFullPath);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = "Image File(.jpg)|*.jpg;*.jpeg;*.png;*.bmp";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {              
                setdata.d.burl = Back_Image_Set(openFileDialog.FileName) ? openFileDialog.FileName:setdata.d.burl;
                ((TextBox)((StackPanel)((Label)sender).Parent).Children[1]).Text = setdata.d.burl;
            }
        }

        private void Textbox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Back_Image_Set(((TextBox)sender).Text))
                setdata.SetValue(((TextBox)sender).Name, ((TextBox)sender).Text);
        }

        bool Back_Image_Set(string url)
        {
            if (url == "")
                return true;
            try
            {
                key_erea.back.Source = new BitmapImage(new Uri(url, UriKind.Absolute));

            }
            catch
            {
                info.Open(Properties.Resources.info_image_error,2000);
                return false;
            }
            return true;
            
        }


        private void Setting_Data(string text,double max,double change,ref double init, string ins)
        {
            stack.Orientation = Orientation.Horizontal;
            itemlb.Text = text;
            stack.Children.Add(itemlb);
            slider.Name = ins;
            slider.Value = setdata.d.back_opacity;
            slider.Width = 150;
            slider.Minimum = 0.0001;
            slider.Maximum = max+0.0001;
            slider.LargeChange = change;
            slider.SmallChange = change;
           
            slider.ValueChanged += Slider_ValueChanged;
            stack.Children.Add(slider);
            itemlb2.Text =init.ToString().Substring(0, 4); ;
            stack.Children.Add(itemlb2);
            Contents.Children.Add(stack);

            StackClear();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider data = (Slider)sender;
            if (data.Name == setdata.back_opacity) {
                key_erea.back.Opacity = data.Value;
                setdata.SetValue(data.Name, data.Value);

                ((TextBlock)((StackPanel)data.Parent).Children[2]).Text = data.Value.ToString().Substring(0,4);
            }
        }


        private Label Setting_Data(string text,int width,Panel mother)
        {
            Label btn = new Label();
            btn.Content = text;
            btn.FontSize = 13;
            btn.Width = width;
            btn.Padding = new Thickness(5);
            btn.Margin = new Thickness(10, 20, 0, 0);
            mother.Children.Add(btn);
            return btn;
        }


        private void NG_Word_btn_Click(object sender, MouseButtonEventArgs e)
        {
            NG_Word_St.Visibility = Visibility.Visible;
            NG_Account_St.Visibility = Visibility.Collapsed;

        }

        private void NG_Account_btn_Click(object sender, MouseButtonEventArgs e)
        {
            NG_Word_St.Visibility = Visibility.Collapsed;
            NG_Account_St.Visibility = Visibility.Visible;
        }

        private void Add_NG_Word(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Length == 1)
            {
                ((TextBox)sender).TextChanged -= Add_NG_Word;
                NGWd_List[NGWd_List.Count - 1].close.Visibility = Visibility.Visible;
                var tmp = new NG_Contents(NG_Word_St, color_list);
                tmp.Add_TextBox();
                tmp.textbox.TextChanged += Add_NG_Word;
                tmp.c.Name = "NGWD" + NGWd_List.Count.ToString();
                tmp.close.MouseLeftButtonDown += Close_NG_Word;
                NGWd_List.Add(tmp);
                //Contents.Height = NG_Word_St.Height+10;
            }
        }

        private void Add_NG_Account(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Length == 1)
            {
                ((TextBox)sender).TextChanged -= Add_NG_Account;
                NGAcc_List[NGAcc_List.Count - 1].close.Visibility = Visibility.Visible;
                var tmp = new NG_Contents(NG_Account_St, color_list);
                tmp.Add_TextBox();
                tmp.textbox.TextChanged += Add_NG_Account;
                tmp.textbox.LostFocus += Check_NG_Account;
                tmp.c.Name = "NGAC" + NGAcc_List.Count.ToString();
                tmp.close.MouseLeftButtonDown += Close_NG_Accont;
                NGAcc_List.Add(tmp);
            }
        }

        private void Check_NG_Account(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("Accont Check");
            int num = int.Parse(((Canvas)((TextBox)sender).Parent).Name.Trim('N', 'G', 'A', 'C'));
            string screen_name = ((TextBox)sender).Text;
            Search_UserID(screen_name, num);

            /*
            if (screen_name != "")
            {

                try
                {
                    CoreTweet.User user = tokens[0].token.Users.Show(screen_name: screen_name);
                    ((TextBox)sender).LostFocus -= Check_NG_Accont;
                    NGAcc_List[num].Off_Warning(user.Id);
                    NGAcc_List[num].Deside_Screen();

                }
                catch (Exception ex)
                {
                    if (ex.Message == "Rate limit exceeded")
                    {
                    }
                    if (ex.Message == "User not found.")
                    {

                    }
                    NGAcc_List[num].On_Warning(ex.Message);
                    Console.WriteLine(ex.Message);
                }

            }
            */
        }

        public void Search_UserID(string screen_name, int num)
        {
            if (screen_name != "")
            {

                try
                {
                    CoreTweet.User user = tokens[0].token.Users.Show(screen_name: screen_name);
                    NGAcc_List[num].textbox.LostFocus -= Check_NG_Account;
                    NGAcc_List[num].Off_Warning(user.Id);
                    NGAcc_List[num].Deside_Screen();

                }
                catch (Exception ex)
                {
                    if (ex.Message == "Rate limit exceeded")
                    {
                    }
                    if (ex.Message == "User not found.")
                    {

                    }
                    NGAcc_List[num].On_Warning(ex.Message);
                    //Console.WriteLine(ex.Message);
                }

            }

        }


        private void Close_NG_Word(object sender, MouseButtonEventArgs e)
        {
            int num = int.Parse(((Canvas)((Image)sender).Parent).Name.Trim('N', 'G', 'W', 'D'));
            NG_Word_St.Children.RemoveAt(num + 1);
            NGWd_List.RemoveAt(num);
            foreach (var word in NGWd_List.Select((v, i) => new { v, i }))
            {
                word.v.c.Name = "NGWD" + word.i.ToString();
            }
            foreach (var word in NGWd_List.Select((v, i) => new { v, i }))
                Console.WriteLine(word.v.c.Name);
            Console.WriteLine(NGWd_List.Count);

        }


        private void Close_NG_Accont(object sender, MouseButtonEventArgs e)
        {
            int num = int.Parse(((Canvas)((Image)sender).Parent).Name.Trim('N', 'G', 'A', 'C'));
            NG_Account_St.Children.RemoveAt(num + 1);
            NGAcc_List.RemoveAt(num);
            foreach (var word in NGAcc_List.Select((v, i) => new { v, i }))
            {
                word.v.c.Name = "NGAC" + word.i.ToString();
            }
        }


        private void Rt_reset_Click(object sender, MouseButtonEventArgs e)
        {
            setdata.Retweet_Reset();
            OnSetting_ResetEvent(new Num_With(W.RT * 10 + 0));

        }

        private void Acc_reset_Click(object sender, MouseButtonEventArgs e)
        {
            OnDelAccEvent(new Num_With(-1));
            OnSetting_ResetEvent(new Num_With(W.AC * 10 + 0));
        }

        private void Ng_reset_Click(object sender, MouseButtonEventArgs e)
        {
            setdata.NG_Reset();
            ng_datas.Clear();
            OnSetting_ResetEvent(new Num_With(W.NG * 10 + 0));
        }

        private void Ngwd_reset_Click(object sender, MouseButtonEventArgs e)
        {
            setdata.NG_Word_Reset();
            ng_datas.words.Clear();
            OnSetting_ResetEvent(new Num_With(W.NG * 10 + 0));
        }

        private void Ngacc_reset_Click(object sender, MouseButtonEventArgs e)
        {
            setdata.NG_Account_Reset();
            ng_datas.account.Clear();
            OnSetting_ResetEvent(new Num_With(W.NG * 10 + 0));
        }

        public delegate void Setting_ResetEventHandler(object sender, Num_With args);
        public event Setting_ResetEventHandler Setting_ResetEvent;

        protected virtual void OnSetting_ResetEvent(Num_With args)
        {
            Setting_ResetEventHandler handler = Setting_ResetEvent;

            //イベントのハンドラが割り当てられていない場合はイベントを発生させない
            if (handler != null)
            {
                //イベント発生
                handler(this, args);
            }
        }

        private void All_reset_Click(object sender, RoutedEventArgs e)
        {
            setdata.Retweet_Reset();
            setdata.Thema_Reset();
            SettingKeyErea(key_erea);
            OnDelAccEvent(new Num_With(-1));
            setdata.NG_Reset();
            ng_datas.Clear();
            OnSetting_ResetEvent(new Num_With(-1));
        }


        private void SetData(string text,double size)
        {
            StackClear();
            label1.Content = text;
            label1.FontSize=size;
            label1.Margin = new Thickness(0, -5, 0, -5);
            Contents.Children.Add(label1);
        }
        private void SetData(string text, double size,double space, string text2, double size2)
        {
            StackClear();
            stack.Orientation = Orientation.Horizontal;
            label1.Content = text;
            label1.FontSize = size;
            label1.Margin = new Thickness(0,0,space,0);
            label1.VerticalAlignment = VerticalAlignment.Bottom;
            label2.Content = text2;
            label2.FontSize = size2;
            label2.VerticalAlignment = VerticalAlignment.Bottom;
            stack.Children.Add(label1);
            stack.Children.Add(label2);
            stack.Margin = new Thickness(0,-5,0,-5);
            Contents.Children.Add(stack);
            //stack.Background = new SolidColorBrush(Color.FromArgb(255, 255, 10, 10));
        }

        private void SetData(string text, double size, double space, string text2, string url,double size2)
        {
            StackClear();
            stack.Orientation = Orientation.Horizontal;
            label1.Content = text;
            label1.FontSize = size;
            label1.Margin = new Thickness(0, 0, space, 0);
            label1.VerticalAlignment = VerticalAlignment.Bottom;
            itemlb.Text = text2;
            itemlb.FontSize = size2;
            itemlb.VerticalAlignment = VerticalAlignment.Bottom;
            itemlb.DataContext = url;
            itemlb.MouseLeftButtonDown += ClickURL;
            itemlb.Margin = new Thickness(0, 0, 0 ,3);
            itemlb.TextDecorations=TextDecorations.Underline;

            stack.Children.Add(label1);
            stack.Children.Add(itemlb);
            stack.Margin = new Thickness(0, -5, 0, -5);
            Contents.Children.Add(stack);
            //stack.Background = new SolidColorBrush(Color.FromArgb(255, 255, 10, 10));
        }

        private void ClickURL(object sender, MouseButtonEventArgs e)
        {
            string url =((TextBlock)sender).DataContext.ToString();
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
            }
            //Console.WriteLine(url);
        }

        private void ShowCopyRight(Panel contents)
        {
            StackClear();
            AssemblyCopyrightAttribute asmcpy =(AssemblyCopyrightAttribute) Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(),typeof(AssemblyCopyrightAttribute));

            stack.Orientation = Orientation.Horizontal;
            label1.Content = asmcpy.Copyright+ ", Birth is released under";
            label1.FontSize = 10;
            label1.VerticalAlignment = VerticalAlignment.Bottom;
            itemlb.Text = "The MIT Lisence";
            itemlb.FontSize = 10;
            itemlb.VerticalAlignment = VerticalAlignment.Bottom;
            itemlb.DataContext = "https://github.com/curonet/Birth/blob/master/LICENSE";
            itemlb.MouseLeftButtonDown += ClickURL;
            itemlb.Margin = new Thickness(0, 0, 0, 3);
            itemlb.TextDecorations = TextDecorations.Underline;

            stack.Children.Add(label1);
            stack.Children.Add(itemlb);
            stack.Margin = new Thickness(0, -5, 0, -5);
            stack.VerticalAlignment = VerticalAlignment.Bottom;
            //itemlb.VerticalAlignment = VerticalAlignment.Bottom;
            Contents.Children.Add(stack);
            //Canvas.SetBottom(stack, 0);
            
            //Canvas.SetLeft(stack, 120);

        }


        public void SetInfoBox(InfoBox frominfo)
        {
            info=frominfo;
        }

        private void StackClear()
        {
            stack = new StackPanel();
            instack = new StackPanel();
            itemlb = new TextBlock();
            itemlb.FontSize = 12;
            itemlb.FontFamily = new FontFamily("游ゴシック");
            color_list.Setting_TextBlock = itemlb;
            itemlb2 = new TextBlock();
            color_list.Setting_TextBlock = itemlb2;
            text = new TextBlock();
            text.FontSize = 12;
            text.FontFamily = new FontFamily("りいてがき筆");
            color_list.Setting_TextBlock = text;
            check = new CheckBox();
            num = new TextBox();
            color_list.Setting_TextBox = num;
            img = new Image();
            combo = new ComboBox();
            label1 = new Label();
            color_list.Setting_Label = label1;
            label2 = new Label();
            color_list.Setting_Label = label2;
            urlbox = new TextBox();
            color_list.Setting_TextBox = urlbox;
            slider = new Slider();
            stack.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            stack.Margin = new Thickness(2);



        }

        public void Clear(object element)
        {
            if ((element is Grid) || (element is Canvas) || (element is StackPanel))
            {
                Panel panel = (Panel)element;
                foreach (var elem in panel.Children)
                {
                    this.Clear(elem);
                }
                panel.Visibility = Visibility.Collapsed;
            }
            object rm = null;
            rm = element;
        }
    }




    class Setting :Canvas
    {
        public MainWindow window;
        SetData setdata;
        Account account;
        public Grid    main;
        List<Tokens> tokens;


        public NG_Data ng_datas;
        Color_List color_list;

        public List<Setting_Tab> tab = new List<Setting_Tab>(); 
        Image Close_btn   = new Image();
        Canvas Show_Setting = new Canvas();


        string[] sub_title = {
            Properties.Resources.setting_subject1,
            Properties.Resources.setting_subject2,
            Properties.Resources.setting_subject3,
            Properties.Resources.setting_subject4,
            Properties.Resources.setting_subject5,
            Properties.Resources.setting_subject6 };

        public int num=W.RT;

        StackPanel Subject_List = new StackPanel();

        public bool IsSetting = false;

        public void Window(MainWindow fromwin)
        {
            window = fromwin;
        }

        public Setting(Grid frommain,SetData fromsetdata,Account fromaccont, List<Tokens> fromtokens,NG_Data fromngdata,Color_List fromcolor)
        {
            main = frommain;
            tokens = fromtokens;
            setdata = fromsetdata;
            account = fromaccont;
            ng_datas = fromngdata;
            color_list = fromcolor;
            
            main.Children.Add(this);


            Subject_List.Background = new SolidColorBrush(Color.FromArgb(0, 100, 100, 0));
            //Subject_List.BorderThickness = new Thickness(0);

            Canvas.SetTop(Subject_List, 5);
            Canvas.SetLeft(Subject_List, 5);
            this.Children.Add(Subject_List);


            Canvas.SetLeft(Show_Setting, 100);
            this.Children.Add(Show_Setting);

            foreach(var str in sub_title)
            {
                Setting_Tab tmp = Make_Tab(str, tab.Count);
                tab.Add(tmp);
                Subject_List.Children.Add(tmp.Subject);
            }

            //Close_btn.Source = new BitmapImage(new Uri(Pass.img_settingdir+ "/" + Pass.colors[color_list.num] + "/back.png", UriKind.Relative));
            //Close_btn.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color_list.num] + "/setting.png", UriKind.Relative));
            Close_btn.Source = new BitmapImage(new Uri(Pass.img_settingdir + "/setting.png", UriKind.Relative));
            Close_btn.Height = 30;
            Close_btn.MouseLeftButtonDown += Close_btn_MouseLeftButtonDown;
            Canvas.SetBottom(Close_btn, 10);
            Canvas.SetRight(Close_btn, 20);
            this.Children.Add(Close_btn);


            color_list.Setting_Back_Panel = this;
            tab[W.RT].Window.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;

        }

        private void Setting_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine(e.Key.ToString());
            switch (e.Key)
            {
                case Key.Down:
                    Tab_Hidden(tab[num]);
                    num =num+1>W.AS?W.RT:num+1;
                    Tab_Visility(tab[num]);

                    break;
                case Key.Up:
                    Tab_Hidden(tab[num]);
                    num = num - 1 < W.RT ? W.AS : num - 1;
                    Tab_Visility(tab[num]);
                    break;
            }

        }

        private Setting_Tab Make_Tab(string str,int num)
        {
            Setting_Tab tmp = new Setting_Tab(this,setdata, account, tokens, ng_datas,color_list);
            tmp.Subject.Content = str;
            color_list.Setting_Subject=tmp.Subject;
            tmp.SettingWindow(num);
            tmp.Subject.FontSize = 14;
            tmp.Subject.Name = "sub" + tab.Count.ToString();
            tmp.Subject.MouseLeftButtonDown += Subject_Click;
            tmp.Setting_ResetEvent += Setting_ResetEvent;
            Show_Setting.Children.Add(tmp.Window);
            //color_list.Scroll_Bar = tmp.Window;
            tmp.Contents.Width = 450;
            return tmp;
        }

        private void Setting_ResetEvent(object sender, Num_With args)
        {
            /*
            int num = args.Number/10;
            if (num != -1)
            {
                tab[num].Clear(tab[num].Window);
                tab[num] = Make_Tab(sub_title[num], num);
               // tab[num].SetInfoBox(info);
                //tab[num].SettingKeyErea(key_erea);


            }
            else
            {
                foreach (var panel in tab.Select((v, i) => new { v, i }))
                    if (panel.i != W.AC)
                    {
                        tab[panel.i].Clear(tab[panel.i].Window);
                        tab[panel.i] = Make_Tab(sub_title[panel.i], panel.i);
                    }
                color_list.SetColor(setdata.d.colors);
            }
            */
            this.Visibility = Visibility.Collapsed;
            Clear(this);
            window.NewSetting();
            window.Change_Lang();
            
            //this.Visibility = Visibility.Visible;
            //Tab_Hidden(tab[W.RT]);
            //Tab_Visility(tab[W.RE]);
        }

        private void Subject_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            num=int.Parse((((Label)sender).Name).Trim('s','u','b'));
            foreach(var data in tab)
                Tab_Hidden(data);
            Tab_Visility(tab[num]);

            foreach (var acc in tab[3].NGAcc_List.Select((v,i)=>new { v,i}))
            {
                if (acc.v.userid == null)
                {
                    tab[3].Search_UserID(acc.v.textbox.Text, acc.i);
                }
            }
            //Console.WriteLine(num);
        }

        public void Tab_Hidden(Setting_Tab tab)
        {
            tab.Window.Visibility = Visibility.Hidden;
            tab.Subject.Foreground = color_list.GetChangeSubjectColor(false);
            tab.Subject.Content = (tab.Subject.Content.ToString()).Replace(">", "");
        }

        public void Tab_Visility(Setting_Tab tab)
        {
            tab.Window.Visibility = Visibility.Visible;
            tab.Subject.Foreground = color_list.GetChangeSubjectColor(true);
            tab.Subject.Content = ">" + tab.Subject.Content;
        }

        public void Change_Color(int num)
        {
            //Close_btn.Source = new BitmapImage(new Uri(Pass.img_settingdir + "/" + Pass.colors[num] + "/back.png", UriKind.Relative));
            //Close_btn.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color_list.num] + "/setting.png", UriKind.Relative));
        }

  
        private void Close_btn_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }
        public  void Close()
        {
            this.Visibility = Visibility.Collapsed;
            Setting_Store();
            IsSetting = false;
            UnSet_KeyEvent();
        }

        private void Store_Window_Size()
        {
            if (window.WindowState == WindowState.Maximized)
            {
                setdata.d.window_pos.Y = window.win_top;
                setdata.d.window_pos.X = window.win_left;
                setdata.d.window_size.Height = window.win_size.Height;
                setdata.d.window_size.Width = window.win_size.Width;
            }
            else
            {
                setdata.d.window_pos.Y = window.Top;
                setdata.d.window_pos.X = window.Left;
                setdata.d.window_size.Height = window.Height;
                setdata.d.window_size.Width = window.Width;
            }
        }

        public void Setting_Store()
        {
            Store_Window_Size();
            setdata.Close_Ready();
            DataContractSerializer serializer = new DataContractSerializer(typeof(D));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            XmlWriter xw = XmlWriter.Create(Pass.setting, settings);
            serializer.WriteObject(xw, setdata.d);
            xw.Close();

            foreach (var data in tokens)
            {
                serializer = new DataContractSerializer(typeof(Tokens));
                settings = new XmlWriterSettings();
                settings.Encoding = new System.Text.UTF8Encoding(false);
                xw = XmlWriter.Create(Pass.pindir+"/"+data.token.UserId + ".xml", settings);
                serializer.WriteObject(xw, data);
                xw.Close();
            }
            Save_NG();
        }
        private void Save_NG()
        {
            ng_datas.Clear();


            foreach (var word in tab[3].NGWd_List)
            {
                if(word.textbox.Text!="")
                    ng_datas.words.Add(word.textbox.Text);
            }
            foreach (var acc in tab[3].NGAcc_List)
            {
                if (acc.textbox.Text != "")
                {
                    ng_datas.AcountAdd(acc.textbox.Text,(acc.userid!=null)?(long)acc.userid:-1);
                }
            }
            DataContractSerializer serializer = new DataContractSerializer(typeof(NG_Data));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            XmlWriter xw = XmlWriter.Create(Pass.ng, settings);
            serializer.WriteObject(xw, ng_datas);
            xw.Close();

        }
        public void Clear(object element)
        {
            if ((element is Setting) || (element is Grid) || (element is Canvas) || (element is StackPanel))
            {
                Panel panel = (Panel)element;
                foreach (var elem in panel.Children)
                {
                    this.Clear(elem);
                }
                panel.Visibility = Visibility.Collapsed;
            }
            object rm = null;
            rm = element;
        }

        public void Set_KeyEvent()
        {
            window.PreviewKeyDown += Setting_KeyDown;
        }
        public void UnSet_KeyEvent()
        {
            window.PreviewKeyDown -= Setting_KeyDown;
        }


    }
}
