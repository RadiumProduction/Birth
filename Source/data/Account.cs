using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Birth_First
{

    class Acc_Data
    {
        public Image img   = new Image();
        public Image use   = new Image();
        public Grid  grid   = new Grid();
        public string screen;
        public string user;
        public string imgurl;
        Color_List color_list;
        Pass pass = new Pass();

        public Acc_Data(Canvas c_in,Color_List fromcolor)
        {
            color_list = fromcolor;
            grid.Width = 100;
            grid.Height = 70;
            //grid.Background = new SolidColorBrush(Color.FromArgb(255, 100, 100, 0));

            c_in.Children.Add(grid);
        }

        public void img_in(Tokens token)
        {

            CoreTweet.User sdata = token.token.Users.Show(id => token.token.UserId);
            //Console.WriteLine("Go token");
            img.Source = new BitmapImage(new Uri(sdata.ProfileImageUrl));
            img.Margin = new Thickness(28, 10, 0, 0);
            img.Width = 60;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            grid.Children.Add(img);

            //Console.WriteLine("Img OK");
            if (token.IsEnabled==true)
                use.Source = new BitmapImage(new Uri(Pass.img_key+"/"+Pass.colors[color_list.num]+"/use_on.png", UriKind.Relative));
            else
                use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/use_off.png", UriKind.Relative));
            use.Margin  = new Thickness(0, 35, 0, 0);
            use.Width   = 25;
            use.HorizontalAlignment = HorizontalAlignment.Left;
            use.VerticalAlignment = VerticalAlignment.Top;
            grid.Children.Add(use);

            screen  = sdata.ScreenName;
            user    = sdata.Name;
            imgurl  = sdata.ProfileImageUrl;
            //Console.WriteLine("use OK");
        }

        public void Position(int num)
        {
            Canvas.SetLeft(grid, 5);
            Canvas.SetTop(grid,grid.Height*num);
        }
    }




    class Account:Canvas
    {
        public Canvas c = new Canvas();
        public ScrollViewer sv = new ScrollViewer();
        List<Tokens> tokens;
        public List<Acc_Data> accs = new List<Acc_Data>();

        public Image add = new Image();
        Color_List color_list;
        Pass pass = new Pass();

        public Account(Canvas c_in, List<Tokens> fromtokens,Color_List fromcolor)
        {
            c       = c_in;
            c.Children.Add(sv);
            sv.Content=this;
            sv.Width = 120;
            sv.Height = 310;
            sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //c.Children.Add(this);
            tokens = fromtokens;
            color_list = fromcolor;

            color_list.Scroll_Bar = sv;

            //if (tokens.Count != 0)
                Show_Account();
            add.Source=new BitmapImage(new Uri(Pass.img_key+"/"+Pass.colors[color_list.num]+"/key_add.png", UriKind.Relative));
            add.Width = 20;
            this.Children.Add(add);
            //Console.WriteLine(Canvas.GetTop(add)+add.Width);
            this.Height = Canvas.GetTop(add) + add.Width;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            
            //c.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            //this.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            color_list.Account_Back_Panel=c;

        }

        public void Show_Account()
        {
            accs.Clear();
            foreach (var token in tokens)
            {
                Indicate_Acc(token);
            }
            Canvas.SetLeft(add, 10);
            if(tokens.Count!=0)
                Canvas.SetTop(add, (tokens.Count) * accs[0].grid.Height+20);
            else
                Canvas.SetTop(add, 20);
            this.Height = Canvas.GetTop(add) + add.Width;

        }

        private void Indicate_Acc(Tokens token)
        {
            //Console.WriteLine("In Indicate_Acc");
            Acc_Data add_acc = new Acc_Data(this,color_list);
            add_acc.Position(accs.Count);
            add_acc.img_in(token);
            add_acc.use.Name = "accuse" + accs.Count;
            add_acc.use.MouseLeftButtonDown += Use_MouseLeftButtonDown;
            accs.Add(add_acc);
            //Console.WriteLine("END Indicate_Acc");
 
       }

        private void Use_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int num = int.Parse((((string)((Image)sender).Name).Remove(0, 6)));
            if (tokens[num].IsEnabled == true)
            {
                accs[num].use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/use_off.png", UriKind.Relative));
                tokens[num].IsEnabled = false;
            }
            else
            {
                accs[num].use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/use_on.png", UriKind.Relative));
                tokens[num].IsEnabled = true;
            }
        }

        public void Change_Color(int num)
        {
            foreach (var token in tokens.Select((v,i) => new {v,i}))
                if (token.v.IsEnabled)
                    accs[token.i].use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/use_on.png", UriKind.Relative));
                else
                    accs[token.i].use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/use_off.png", UriKind.Relative));
            add.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[num] + "/key_add.png", UriKind.Relative));
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

}
