using System;
using System.Collections.Generic;
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
using System.Windows.Threading;


namespace Birth_First
{
    class Key_Erea :Canvas
    {
        public List<Key_Data> key = new List<Key_Data>();
        public ScrollViewer sv = new ScrollViewer();

        public string name = "New File";
        public Image   back    = new Image();

        Image   key_add = new Image();
        Canvas  c       = new Canvas();
        public Image   heart   = new Image();

        Color_List color_list;
        Title title;

        public bool WithHeart = false;
        public int interval=5000;

        public Key_Erea(Canvas c_in,Color_List fromcolor,Title fromtitle)
        {
            c = c_in;
            color_list = fromcolor;
            title = fromtitle;

            c.Children.Add(sv);
            sv.Content = this;
            //sv.Width = 120;
            //sv.Height = 10;
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            color_list.Scroll_Bar = sv;

            

            //this.Background = new SolidColorBrush(Color.FromArgb(255, 100, 0, 100));
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.Children.Add(back);
            back.Opacity = 1.0;
            //back.Width = 300;
            //back.Height = 300;


            heart.Source = new BitmapImage(new Uri("img/heart_off.png", UriKind.Relative));
            heart.MouseLeftButtonDown += Heart_Clicked;
            heart.Width = 25;
            Canvas.SetTop(heart,15);
            Canvas.SetRight(heart,20);

            c.Children.Add(heart);
            key_add.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(Key_Add_On_Clicked);


            First_Keyword_Add();
            this.Children.Add(key_add);
            Change_Size();

        }

        private void Heart_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (WithHeart)
            {
                WithHeart = false;
                heart.Source = new BitmapImage(new Uri("img/heart_off.png", UriKind.Relative));
            }
            else
            {
                WithHeart = true;
                heart.Source = new BitmapImage(new Uri("img/heart_on.png", UriKind.Relative));
            }
            title.Title_NoSave();
        }

        public void First_Keyword_Add()
        {
            key.Add(new Key_Data(this,color_list,title));
            Canvas.SetTop(key[0], 0.0);
            Canvas.SetLeft(key[0], 0.0);
            key[key.Count - 1].Name = "c" + (key.Count - 1).ToString();
            key[key.Count - 1].DeleteKeyWordEvent += new Key_Data.DeleteKeyWordEventHandler(First_Keyword_Close_Click);
            key[key.Count - 1].ChangeKeyWordEvent += Key_Erea_AddKeyWordEvent;

            key_add.Source = new BitmapImage(new Uri(Pass.img_key+"/"+Pass.colors[color_list.num]+"/key_add.png", UriKind.Relative));
            key_add.Height = 20;
            Canvas.SetTop(key_add, (Canvas.GetTop(key[0]) + 1) * key[0].Height + 20);
            Canvas.SetLeft(key_add, 15);

        }

        private void First_Keyword_Close_Click(object sender, Data_With args)
        {
            var st_board = new Storyboard();

            var nums = args.Message.Name;
            int num = int.Parse((nums.Trim('c')));
     

            if (num == 0 && key.Count==1)
                return;
            key[num].keyword[0].Delete();
            key[num].keyword.RemoveAt(0);
            key[num].Delete();
            key.RemoveAt(num);

            for (int i = num; i < key.Count; i++)
            {
                var anime = new DoubleAnimation();
                Storyboard.SetTarget(anime, key[i]);
                Storyboard.SetTargetProperty(anime, new PropertyPath("(Canvas.Top)"));
                anime.To = key[i].Height * i;
                anime.Duration = TimeSpan.FromSeconds(0.075);
                st_board.Children.Add(anime);
                key[i].Name = "c" + i.ToString();
            }
            var add_up = new DoubleAnimation();
            Storyboard.SetTarget(add_up, key_add);
            Storyboard.SetTargetProperty(add_up, new PropertyPath("(Canvas.Top)"));
            //add_up.To = Canvas.GetTop(key[key.Count - 1]) + key[0].Height + 20;
            Canvas.SetTop(key_add, Canvas.GetTop(key_add) - key[0].Height);
            add_up.Duration = TimeSpan.FromSeconds(0.075);
            st_board.Children.Add(add_up);

            st_board.Begin();
            Change_Size();


        }

        private void Key_Add_On_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            Add_Key();
            title.Title_NoSave();
        }

        public void Add_Key()
        {
            Key_Data newkey = new Key_Data(this,color_list,title);
            Canvas.SetTop(newkey, key[0].Height * key.Count);
            key.Add(newkey);
            Canvas.SetTop(key_add, Canvas.GetTop(key[key.Count - 1]) + key[0].Height + 20);
            key[key.Count - 1].Name = "c" + (key.Count - 1).ToString();
            key[key.Count - 1].DeleteKeyWordEvent += new Key_Data.DeleteKeyWordEventHandler(First_Keyword_Close_Click);
            key[key.Count - 1].ChangeKeyWordEvent += Key_Erea_AddKeyWordEvent;
            Change_Size();
            
        }

        private void Key_Erea_AddKeyWordEvent(object sender, Data_With args)
        {
            Change_Size();
        }

        public void Change_Color(int num)
        {
            foreach(var keydata in key)
            {
                foreach(var keyword in keydata.keyword.Select((v,i)=> new{v,i}))
                {
                    if(keyword.i == (keydata.keyword.Count)-1)
                        keyword.v.and.Source= new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/and_off.png", UriKind.Relative));
                    else
                        keyword.v.and.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/and_on.png", UriKind.Relative));
                }
                if(keydata.IsEnabled)
                    keydata.use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/use_on.png", UriKind.Relative));
                else
                    keydata.use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/use_off.png", UriKind.Relative));
            }
            key_add.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/key_add.png", UriKind.Relative));

        }

        public void Change_Size()
        {
            this.Height = Canvas.GetTop(key_add) + key_add.Height;
            //Console.WriteLine(this.Height);
            this.Width = 0;
            foreach (var data in key)
                if (data.Width > this.Width)
                    this.Width = data.Width;
            Canvas.SetTop(heart, 10);
            Canvas.SetRight(heart, 25);

        }
    }
}
