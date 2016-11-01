using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Birth_First
{

    class Data_With : EventArgs
    {
        private Key_Data _message;

        public Data_With(Key_Data s)
        {
            _message = s;
        }


        public Key_Data Message
        {
            get { return _message; }
        }
    }

    class Key_Data : Canvas
    {
        public  List<KeyWord_Data> keyword = new List<KeyWord_Data>();
        public  Canvas  key_c       = new Canvas();
        public  new bool IsEnabled = true;
        public  Image use = new Image();

        Canvas c           = new Canvas();

        Color_List color_list;
        Title title;

        public delegate void DeleteKeyWordEventHandler(object sender, Data_With args);
        public event DeleteKeyWordEventHandler DeleteKeyWordEvent;
        protected virtual void OnDeleteKeyWordEvent(Data_With args)
        {
            DeleteKeyWordEventHandler handler = DeleteKeyWordEvent;

            //イベントのハンドラが割り当てられていない場合はイベントを発生させない
            if (handler != null)
            {
                //イベント発生
                handler(this, args);
            }
        }


        public delegate void ChangeKeyWordEventHandler(object sender, Data_With args);
        public event ChangeKeyWordEventHandler ChangeKeyWordEvent;
        protected virtual void OnChangeKeyWordEvent(Data_With args)
        {
            ChangeKeyWordEventHandler handler = ChangeKeyWordEvent;

            //イベントのハンドラが割り当てられていない場合はイベントを発生させない
            if (handler != null)
            {
                //イベント発生
                handler(this, args);
            }
        }



        public Key_Data(Key_Erea c_in,Color_List fromcolor,Title fromtitle)
        {
            c = c_in;
            color_list = fromcolor;
            title = fromtitle;
            c.Children.Add(this);
            this.Children.Add(key_c);
            key_c.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            key_c.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.Height = 60;
            Canvas.SetTop(key_c, 0);
            Canvas.SetLeft(key_c, 55);
            use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/use_on.png", UriKind.Relative));
            use.Width = 30;
           
            Canvas.SetTop(use,20);
            Canvas.SetLeft(use, 10);
            this.Children.Add(use);
            use.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(Use_On_Clicked);


            Add_Key();
        }


        public void Add_Key()
        {
            KeyWord_Data key = new KeyWord_Data(key_c,color_list,title);
            keyword.Add(key);
            color_list.Key_TextBox = key.kw_text;
            key.x = key.Width * (keyword.Count-1);
            key.grid.Name = "g"+(keyword.Count-1).ToString();

            key.and.MouseLeftButtonDown     += And_Click;
            key.close.MouseLeftButtonDown   += Close_Click;

            this.Width = Canvas.GetLeft(use)+use.Width + key.Width * keyword.Count + 50;
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var st_board = new Storyboard();
            var nums= ((Grid)((Image)sender).Parent).Name;
            int num = int.Parse((nums.Trim('g')));

            //Console.WriteLine(keyword.Count);

            if (keyword.Count > 1)
            {
                keyword[num].Delete();
                keyword.RemoveAt(num);
                if (num == keyword.Count && num != 0)
                {
                    keyword[num - 1].and.MouseLeftButtonDown += And_Click;
                    keyword[num - 1].and.MouseEnter += new MouseEventHandler(keyword[num - 1].And_On_Enter);
                    keyword[num - 1].and.MouseLeave += new MouseEventHandler(keyword[num - 1].And_On_Leave);
                    keyword[num - 1].and.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(keyword[num - 1].And_On_Clicked);
                    keyword[num - 1].and.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/and_off.png", UriKind.Relative));
                }

                for (int i = num; i < keyword.Count; i++)
                {
                    var anime = new DoubleAnimation();
                    Storyboard.SetTarget(anime, keyword[i].grid);
                    Storyboard.SetTargetProperty(anime, new PropertyPath("(Canvas.Left)"));
                    anime.To = keyword[i].Width * i;
                    anime.Duration = TimeSpan.FromSeconds(0.075);
                    st_board.Children.Add(anime);
                    keyword[i].grid.Name = "g" + i.ToString();
                }
                st_board.Begin();
                this.Width = Canvas.GetLeft(use) + use.Width + keyword[0].Width * keyword.Count + 50;
                OnChangeKeyWordEvent(new Data_With(this));

            }
            else
            {
                OnDeleteKeyWordEvent(new Data_With(this));

            }
            title.Title_NoSave();

        }



        public void And_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            keyword[keyword.Count - 1].and.MouseLeftButtonDown -= And_Click;
            Add_Key();
            OnChangeKeyWordEvent(new Data_With(this));
            title.Title_NoSave();

        }
        private void Use_On_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {

            if (this.IsEnabled == true)
            {
                use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/use_off.png", UriKind.Relative));
                this.IsEnabled = false;
            }
            else
            {
                use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/use_on.png", UriKind.Relative));
                this.IsEnabled = true;
            }
            title.Title_NoSave();

        }
        public void Delete()
        {
            foreach (UIElement elem in this.Children)
            {
                UIElement rm = null;
                rm = elem;
            }
            this.Children.RemoveRange(0, this.Children.Count);
            c.Children.Remove(this);
            //this = null;
        }
    }
}
