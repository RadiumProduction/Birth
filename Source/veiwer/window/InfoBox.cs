using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Birth_First
{
    class InfoBox
    {
        MainWindow  window;
        Panel       mother;
        Canvas      backcanvus = null;
        Grid        Base = new Grid();
        Grid        back = new Grid();
        TextBlock Text = new TextBlock();
        Image       close = new Image();
        //Image       back = new Image();
        Label       button = new Label();

        Key_Erea key_erea;
        Title title;
        Color_List color_list;
        
        Timer timer = null;
        public class ButtonType
        {
            /*
            public static int New { get{ return 0;} }
            public static int Interval { get { return 1; } }
            public static int Times { get { return 2; } }
            */
            public const int New = 0;
            public const int Interval = 1;
            public const int Time = 2;

        }


        public InfoBox(MainWindow fromwin,Panel frommain)
        {

            //Console.WriteLine("Infobox Make");
            window = fromwin;
            mother = frommain;

            double border = 2;


            Base.Width = 200;
            Base.Height = 80;
            Base.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Base.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            //Base.Margin = new Thickness(0, 25, 0, 0);
            Canvas.SetTop(Base, 0);
            Canvas.SetLeft(Base, Base.Width);

            Base.Background= new SolidColorBrush(Color.FromArgb(150, 200, 20, 20));
            back.Background = new SolidColorBrush(Color.FromArgb(220, 255, 255, 170));
            back.Margin = new Thickness(border);
            back.Width = Base.Width - (border * 2);
            back.Height = Base.Height - (border * 2);
            //Base.bo = new SolidColorBrush(Color.FromArgb(205, 50, 220, 220));
            //back.Source = new BitmapImage(new Uri("img/infoback.png", UriKind.Relative));
            Base.Children.Add(back);

            Set_Back();

            Text.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            Text.Margin = new Thickness(10, 25, 0, 0);
        
            Base.Children.Add(Text);

            close.Source = new BitmapImage(new Uri("img/close_over.png", UriKind.Relative));
            close.Width = 15;
            close.Margin = new Thickness(Base.Width-30, 5, 0, 0);
            close.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            close.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            close.Opacity = 0.7;

            close.MouseLeftButtonDown += CloseClicked;
            Base.Children.Add(button);
            Base.Children.Add(close);


            //backcanvus.Visibility = Visibility.Collapsed;

        }
        public void GetKeyAndTitleAndColor(Key_Erea fromkeyerea ,Title fromtitle,Color_List fromcolor)
        {
            key_erea = fromkeyerea;
            title= fromtitle;
            color_list = fromcolor;
        }

        private void Set_Back()
        {
            if(backcanvus!=null)
            {
                backcanvus.Children.Remove(Base);
                mother.Children.Remove(backcanvus);

            }
            backcanvus = new Canvas();
            mother.Children.Add(backcanvus);
            backcanvus.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            backcanvus.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            backcanvus.Margin = new Thickness(0, 25, 0, 0);
            backcanvus.Width = Base.Width;
            backcanvus.Height = Base.Height;

            backcanvus.Children.Add(Base);
            
        }

        public void Open(string text)
        {
            
            
            Set_Back();
            Text.Text = text;


            var st_board = new Storyboard();
            var add_up = new DoubleAnimation();
            Storyboard.SetTarget(add_up, Base);
            Storyboard.SetTargetProperty(add_up, new PropertyPath("(Canvas.Left)"));
            add_up.To = 0;
            add_up.Duration = TimeSpan.FromSeconds(0.2);
            st_board.Children.Add(add_up);
            st_board.Begin();
            backcanvus.Visibility = Visibility.Visible;
            Base.Visibility = Visibility.Visible;
            
        }

        public void Open(string text, int time = -1)
        {
            if (time > 0)
            {
                timer = new Timer(new TimerCallback(Close), null, time, time);
            }
            Open(text);
        }

        public void Open(string text,int btn_act, int time = -1)
        {
            Open(text,time);
            SetButton(btn_act);
        }

        private void SetButton(int btn_act)
        {

            Base.Children.Remove(button);
            button = new Label();
            //button.VerticalContentAlignment = VerticalAlignment.Top;
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.Margin = new Thickness(Base.Width - 90, 50, 0, 0);
            button.Background= new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            button.BorderBrush= new SolidColorBrush(Color.FromArgb(255, 0, 0, 10));
            button.BorderThickness = new Thickness(1);
            button.Height = 25;
            button.Width = 80;
            button.Content = Properties.Resources.info_return;
            button.Name = "infobtn"+btn_act;
            button.MouseLeftButtonDown += Button_Click;
            Base.Children.Add(button);

        }

        private void Button_Click(object sender, EventArgs e)
        {
            int act = int.Parse(((Label)sender).Name.Trim('i', 'n', 'f', 'o', 'b', 't', 'n'));
            switch (act)
            {
                case ButtonType.New:
                    Keys_Save keys_save = new Keys_Save();
                    keys_save.Load(key_erea, Pass.tmpkey, color_list);
                    keys_save.Free();
                    window.Title = "Birth   - " + key_erea.name + " -";
                    title.Title_Change();
                    break;
            }
            Close();
        }

        private void Close(object o)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);

            window.Dispatcher.BeginInvoke(new Action(() => { Close(); }));
        }
        private void CloseClicked(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        public void Close()
        {
            var st_board = new Storyboard();
            var add_up = new DoubleAnimation();
            Storyboard.SetTarget(add_up, Base);
            Storyboard.SetTargetProperty(add_up, new PropertyPath("(Canvas.Left)"));
            add_up.To = Base.Width;
            add_up.Duration = TimeSpan.FromSeconds(0.075);
            st_board.Children.Add(add_up);
            st_board.Begin();

            //Base.Visibility = Visibility.Collapsed;
        }


    }
}
