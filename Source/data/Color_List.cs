using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Birth_First
{

    [DataContract]
    class Color_Data
    {
        [DataMember]
        public SolidColorBrush Main_Back_Panel;
        [DataMember]
        public SolidColorBrush Account_Back_Panel;
        [DataMember]
        public SolidColorBrush Menu_Back_Panel;
        [DataMember]
        public SolidColorBrush Setting_Back_Panel;

        [DataMember]
        public SolidColorBrush[] Key_TextBox = new SolidColorBrush[2];
        [DataMember]
        public SolidColorBrush[] Setting_TextBox = new SolidColorBrush[2];

        [DataMember]
        public SolidColorBrush[] Setting_TextBlock = new SolidColorBrush[2];

        [DataMember]
        public SolidColorBrush[] Setting_Subject = new SolidColorBrush[3];
        [DataMember]
        public SolidColorBrush[] Setting_Button = new SolidColorBrush[3];
        [DataMember]
        public SolidColorBrush[] Setting_Combo = new SolidColorBrush[3];

        [DataMember]
        public SolidColorBrush[] Setting_Label = new SolidColorBrush[2];

        //[DataMember]
        //public SolidColorBrush[] Scroll_Bar = new SolidColorBrush[4];


        public Color_Data(int num)
        {
            Init(num);
        }
        private void Init(int num)
        {
            switch (num)
            {
                case 0:
                    Main_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 135, 206, 250));
                    Account_Back_Panel  = new SolidColorBrush(Color.FromArgb(255, 156, 167, 226));
                    Menu_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 39, 38, 114));
                    Setting_Back_Panel  = new SolidColorBrush(Color.FromArgb(196, 196, 255, 255));

                    Key_TextBox[0]      = new SolidColorBrush(Color.FromArgb(255, 22, 64, 130));
                    Key_TextBox[1]      = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_TextBox[0]  = new SolidColorBrush(Color.FromArgb(255, 22, 64, 130));
                    Setting_TextBox[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_TextBlock[0]= new SolidColorBrush(Color.FromArgb(255, 22, 64, 130));
                    Setting_TextBlock[1]= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_Subject[0]  = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Subject[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Subject[2]  = new SolidColorBrush(Color.FromArgb(255, 11, 47, 215));
                    Setting_Button[0]   = Setting_TextBlock[0];
                    Setting_Button[1]   = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Button[2]   = Setting_TextBlock[0];

                    Setting_Combo[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Combo[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Combo[2] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                    Setting_Label[0] = new SolidColorBrush(Color.FromArgb(255, 22, 64, 130));
                    Setting_Label[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));


                    break;
                case 1:
                    Main_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 235, 121, 136));
                    Account_Back_Panel  = new SolidColorBrush(Color.FromArgb(255, 235, 116, 29));
                    Menu_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 149, 0, 0));
                    Setting_Back_Panel  = new SolidColorBrush(Color.FromArgb(190, 242, 202, 170));

                    Key_TextBox[0]      = new SolidColorBrush(Color.FromArgb(255, 122, 30, 40));
                    Key_TextBox[1]      = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_TextBox[0]  = new SolidColorBrush(Color.FromArgb(255, 122, 30, 40));
                    Setting_TextBox[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_TextBlock[0]= new SolidColorBrush(Color.FromArgb(255, 122, 30, 40));
                    Setting_TextBlock[1]= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_Subject[0]  = new SolidColorBrush(Color.FromArgb(255, 122, 30, 40));
                    Setting_Subject[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Subject[2]  = new SolidColorBrush(Color.FromArgb(255, 204, 0, 10));
                    Setting_Button[0]   = Setting_TextBlock[0];
                    Setting_Button[1]   = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Button[2]   = Setting_TextBlock[0];

                    Setting_Combo[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Combo[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Combo[2] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                    Setting_Label[0] = new SolidColorBrush(Color.FromArgb(255, 122, 30, 40));
                    Setting_Label[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    break;
                case 2:
                    Main_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 142, 0, 204));
                    //Main_Back_Panel = new SolidColorBrush(Color.FromArgb(255, 142, 0, 24));//１羽サブタイの色
                    Account_Back_Panel  = new SolidColorBrush(Color.FromArgb(255, 73, 0, 129));
                    Menu_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 59, 44, 82));
                    Setting_Back_Panel  = new SolidColorBrush(Color.FromArgb(190, 75, 0, 130));

                    Key_TextBox[0]      = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Key_TextBox[1]      = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_TextBox[0]  = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_TextBox[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
               
                    Setting_TextBlock[0]= new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_TextBlock[1]= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_Subject[0]  = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_Subject[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Subject[2]  = new SolidColorBrush(Color.FromArgb(255, 178, 60, 224));
                    Setting_Button[0]   = Setting_TextBlock[0];
                    Setting_Button[1]   = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Button[2]   = Setting_TextBlock[0];

                    Setting_Combo[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Combo[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Combo[2] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                    Setting_Label[0] = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_Label[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    break;
                case 3:
                    Main_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                    Account_Back_Panel  = new SolidColorBrush(Color.FromArgb(255, 53, 38, 45));
                    Menu_Back_Panel     = new SolidColorBrush(Color.FromArgb(255, 0, 160, 0));
                    Setting_Back_Panel  = new SolidColorBrush(Color.FromArgb(190, 0, 51, 0));

                    Key_TextBox[0]      = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Key_TextBox[1]      = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_TextBox[0]  = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_TextBox[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_TextBlock[0]= new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_TextBlock[1]= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_Subject[0]  = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_Subject[1]  = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Subject[2]  = new SolidColorBrush(Color.FromArgb(255, 50, 205, 50));
                    Setting_Button[0]   = Setting_TextBlock[0];
                    Setting_Button[1]   = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Button[2]   = Setting_TextBlock[0];

                    Setting_Combo[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Combo[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Combo[2] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                    Setting_Label[0] = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                    Setting_Label[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    break;
                case 4:
                    Main_Back_Panel = new SolidColorBrush(Color.FromArgb(255, 255, 225, 106));
                    Account_Back_Panel = new SolidColorBrush(Color.FromArgb(255, 224, 192, 75));
                    Menu_Back_Panel = new SolidColorBrush(Color.FromArgb(255, 245, 245, 225));
                    Setting_Back_Panel = new SolidColorBrush(Color.FromArgb(190, 214, 182, 65));

                    Key_TextBox[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Key_TextBox[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_TextBox[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_TextBox[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_TextBlock[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_TextBlock[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    Setting_Subject[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Subject[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Subject[2] = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    Setting_Button[0] = Setting_TextBlock[0];
                    Setting_Button[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Button[2] = Setting_TextBlock[0];

                    Setting_Combo[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Combo[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    Setting_Combo[2] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                    Setting_Label[0] = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    Setting_Label[1] = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

                    break;
                default:
                    break;
            }

        }


    }
    class Color_List
    {

        List<Panel> Main_Back_Panels = new List<Panel>();
        List<Panel> Account_Back_Panels = new List<Panel>();
        List<Panel> Menu_Back_Panels = new List<Panel>();
        List<Panel> Setting_Back_Panels = new List<Panel>();

        List<TextBox> Key_TextBoxs = new List<TextBox>();
        List<TextBox> Setting_TextBoxs = new List<TextBox>();

        List<TextBlock> Setting_TextBlocks = new List<TextBlock>();

        List<Label> Setting_Subjects = new List<Label>();
        List<Label> Setting_Buttons = new List<Label>();
        List<ComboBox> Setting_Combos = new List<ComboBox>();
        List<Label> Setting_Labels = new List<Label>();

        List<ScrollViewer> Scroll_Bars = new List<ScrollViewer>();



        MainWindow window;
        List<Tokens> tokens = new List<Tokens>();
        Account account;
        Key_Erea key_erea;
        Menu menu;
        Setting setting;
        /*Twitter twitter;
        SetData setdata;
        NG_Data ng_datas;
        */
        public int num;

        public Panel Main_Back_Panel
        {
            set { Main_Back_Panels.Add(value); }
        }
        public Panel Account_Back_Panel
        {
            set { Account_Back_Panels.Add(value); }
        }
        public Panel Menu_Back_Panel
        {
            set { Menu_Back_Panels.Add(value); }
        }
        public Panel Setting_Back_Panel
        {
            set { Setting_Back_Panels.Add(value); }
        }

        public TextBox Key_TextBox
        {
            set
            {
                value.Foreground = color_datas[num].Key_TextBox[0];
                value.Background = color_datas[num].Key_TextBox[1];
                Key_TextBoxs.Add(value);
            }
        }
        public TextBox Setting_TextBox
        {
            set
            {
                value.Foreground = color_datas[num].Setting_TextBox[0];
                value.Background = color_datas[num].Setting_TextBox[1];
                Setting_TextBoxs.Add(value);
            }
        }

        public TextBlock Setting_TextBlock
        {
            set {
                value.Foreground = color_datas[num].Setting_TextBlock[0];
                value.Background = color_datas[num].Setting_TextBlock[1];
                Setting_TextBlocks.Add(value);
            }
        }

        public Label Setting_Subject
        {
            set { Setting_Subjects.Add(value); }
        }
        public Label Setting_Button
        {
            set { Setting_Buttons.Add(value); }
        }
        public ComboBox Setting_Combo
        {
            set { Setting_Combos.Add(value); }
        }

        public Label Setting_Label
        {
            set { Setting_Labels.Add(value); }
        }

        public ScrollViewer Scroll_Bar
        {
            set { Scroll_Bars.Add(value); }
        }



        private List<Color_Data> color_datas = new List<Color_Data>();


        //private List<SolidColorBrush> backcolorlist = new List<SolidColorBrush>();
        //private List<List<SolidColorBrush>> textcolorlist = new List<List<SolidColorBrush>>();


        public void SetWindow(MainWindow fromwindow)
        {
            window = fromwindow;
        }


        public void SetNum(int fromnum)
        {
            num = fromnum;
            for (int i = 0; i < 5; i++)
                color_datas.Add(new Color_Data(i));
        }
        public void SetData(List<Tokens> fromtokens, Account fromaccount, Key_Erea fromkeyerea, Menu frommenu, Setting fromsetting)
        {
            tokens  = fromtokens;
            account = fromaccount;
            key_erea= fromkeyerea;
            menu    = frommenu;
            setting = fromsetting;
        }



        public void Update()
        {
            Main_Back_Panels.RemoveAll(s => s == null);
            Account_Back_Panels.RemoveAll(s => s == null);
            Menu_Back_Panels.RemoveAll(s => s == null);
            Setting_Back_Panels.RemoveAll(s => s == null);
            Key_TextBoxs.RemoveAll(s => s == null);
            Setting_TextBoxs.RemoveAll(s => s == null);
            Setting_TextBlocks.RemoveAll(s => s == null);
            Setting_Subjects.RemoveAll(s => s == null);
            Setting_Buttons.RemoveAll(s => s == null);
            Setting_Combos.RemoveAll(s => s == null);
            Setting_Labels.RemoveAll(s => s == null);
            Scroll_Bars.RemoveAll(s => s == null);
        }



        public void SetColor(int fromnum)
        {
            num = fromnum;

            window.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

            foreach (var data in Main_Back_Panels)
            {
                data.Background = color_datas[num].Main_Back_Panel;
            }
            foreach (var data in Account_Back_Panels)
            {
                data.Background = color_datas[num].Account_Back_Panel;
            }
            foreach (var data in Menu_Back_Panels)
            {
                data.Background = color_datas[num].Menu_Back_Panel;
            }
            foreach (var data in Setting_Back_Panels)
            {
                data.Background = color_datas[num].Setting_Back_Panel;
            }

            foreach (var data in Key_TextBoxs)
            {
                data.Foreground = color_datas[num].Key_TextBox[0];
                data.Background = color_datas[num].Key_TextBox[1];
            }
            foreach (var data in Setting_TextBoxs)
            {
                data.Foreground = color_datas[num].Setting_TextBox[0];
                data.Background = color_datas[num].Setting_TextBox[1];
            }

            foreach (var data in Setting_TextBlocks)
            {
                data.Foreground = color_datas[num].Setting_TextBlock[0];
                data.Background = color_datas[num].Setting_TextBlock[1];
            }

            foreach (var data in Setting_Subjects)
            {
                data.Foreground = color_datas[num].Setting_Subject[0];
                data.Background = color_datas[num].Setting_Subject[1];
                if ((data.Content.ToString()).Contains(">"))
                    data.Foreground = color_datas[num].Setting_Subject[2];
            }
            foreach (var data in Setting_Buttons)
            {
                data.Foreground = color_datas[num].Setting_Button[0];
                data.Background = color_datas[num].Setting_Button[1];
                data.BorderBrush = color_datas[num].Setting_Button[2];
                data.BorderThickness = new Thickness(1);

            }
            foreach (var data in Setting_Combos)
            {
                data.Foreground = color_datas[num].Setting_Combo[0];
                data.Background = color_datas[num].Setting_Combo[1];
                //data.BorderBrush = color_datas[num].Setting_Combo[2];
            }
            foreach (var data in Setting_Labels)
            {
                data.Foreground = color_datas[num].Setting_Label[0];
                data.Background = color_datas[num].Setting_Label[1];
            }

            foreach (var data in Scroll_Bars)
            {
                data.Style = (Style)window.Resources["CustomScrollViewerStyle"];

                /*
                data.Template = new ControlTemplate();
                SolidColorBrush a = new SolidColorBrush();

                //data.Resources.Add();
                Style s = new Style(data.GetType());
                //SetterBase sb;// = new SetterBase();
                ControlTemplate ct = new ControlTemplate(targetType:typeof(ScrollViewer)); 
                
                s.Setters.Add(new Setter(ScrollViewer.TemplateProperty,ct));
                FrameworkElementFactory ingrid = new FrameworkElementFactory(typeof(Grid));
                FrameworkElementFactory scp = new FrameworkElementFactory(typeof(ScrollContentPresenter));
                FrameworkElementFactory sb = new FrameworkElementFactory(typeof(ScrollBar));
                FrameworkElementFactory ingridrow = new FrameworkElementFactory(typeof(RowDefinition));
                FrameworkElementFactory ingridcolumn = new FrameworkElementFactory(typeof(ColumnDefinition));
                ingridrow.SetValue(RowDefinition.HeightProperty, GridLength.Auto);
                ingridcolumn.SetValue(ColumnDefinition.WidthProperty, GridLength.Auto);
                
                //ingrid.SetValue(Grid.RowSpanProperty,ingridrow);
                //ingrid.SetValue(Grid.ColumnProperty, ingridcolumn);
                
                //ingrid.AppendChild.

                //ingrid.SetResourceReference(Grid.);
                //ingrid.SetValue(Grid.RowProperty., 1);
                sb.SetValue(Grid.ColumnProperty,1);
                
                ingrid.AppendChild(scp);
                ingrid.AppendChild(sb);
                ct.VisualTree = ingrid; 
                //data.Style = new Style
                //ct.Templat
                //data.Template.
                */
            }

            account.Change_Color(num);
            key_erea.Change_Color(num);
            setting.Change_Color(num);
            menu.Load_Image(num);

        }

        public SolidColorBrush GetChangeSubjectColor(bool on)
        {
            if (on)
                return color_datas[num].Setting_Subject[2];
            return color_datas[num].Setting_Subject[0];
        }

        public SolidColorBrush GetKeyForeColor()
        {
            return color_datas[num].Key_TextBox[0];
        }
        public SolidColorBrush GetKeyBackColor()
        {
            return color_datas[num].Key_TextBox[1];
        }
        public SolidColorBrush GetSettingTextForeColor()
        {
            return color_datas[num].Setting_TextBox[0];
        }
        public SolidColorBrush GetSettingTextBackColor()
        {
            return color_datas[num].Setting_TextBox[1];
        }

    }

}
