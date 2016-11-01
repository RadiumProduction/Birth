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
    class Title :StackPanel
    {
        MainWindow window;

        public Label WindowTitle = new Label();

        Image min   = new Image();
        Image max   = new Image();
        Image close = new Image();


        Pass pass = new Pass();


        bool IsMaxed = false;

        public void Title_Image(Image img,string url)
        {
            img.Source = new BitmapImage(new Uri(url, UriKind.Relative));
            img.Width = 18;
            this.Children.Add(img);
            //img.Margin = new Thickness(7, 0, 3, 0);

        }

        public Title(MainWindow fromwindow,Grid mother)
        {
            window = fromwindow;
            this.Orientation = Orientation.Horizontal;
            mother.Children.Add(this);
            this.Children.Add(WindowTitle);

            WindowTitle.Foreground = new SolidColorBrush(Color.FromArgb(255, 217, 217, 217));
            //WindowTitle.FontFamily = new FontFamily("AR RoundGothicJP Medium");
            Title_Image(min, Pass.img + "/wmin.png");
            Title_Image(max, Pass.img + "/wmax.png");
            Title_Image(close, Pass.img + "/wclose.png");
            min.MouseLeftButtonDown += Min_Clicked;
            max.MouseLeftButtonDown += Max_Clicked;
            close.MouseLeftButtonDown += Close_Clicked;


            Title_Length();
            Title_Change();

        }



        private void Min_Clicked(object sender, MouseButtonEventArgs e)
        {
            window.WindowState=WindowState.Minimized;
        }

        private void Max_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (!IsMaxed)
            {
                window.WindowState = WindowState.Maximized;

            }
            else
            {
                window.WindowState = WindowState.Normal;
            }
        }

        private void Close_Clicked(object sender, MouseButtonEventArgs e)
        {
            window.Close();
        }

        public void Max_On()
        {
            IsMaxed = true;
            max.Source = new BitmapImage(new Uri(Pass.img + "/wnormal.png", UriKind.Relative));

        }

        public void Max_Off()
        {
            IsMaxed = false;
            max.Source = new BitmapImage(new Uri(Pass.img + "/wmax.png", UriKind.Relative));
        }

        public void Title_Length(double border =0)
        {
            WindowTitle.Width =(window.RenderSize.Width-border - (min.Width + 10) * 3 )>0? window.RenderSize.Width - border - (min.Width)*3:0;
        }

        public void Title_Change()
        {
            WindowTitle.Content = window.Title;

        }
        public void Title_NoSave()
        {
            string t = window.Title;
            t=t.Remove(t.Count() - 2, 2);
            t = t + "* -";
            WindowTitle.Content = t;

        }
    }
}
