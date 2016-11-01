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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Birth_First
{


    class KeyWord_Data
    {
        public Canvas  c       = new Canvas();

        public  Grid    grid        = new Grid();
        public  TextBox kw_text     = new TextBox();
        public  Image   and         = new Image();
        public  Image   close       = new Image();
        public  Image   reverse     = new Image();
        public  bool    IsReversal  = false;

        Color_List color_list;
        Title title;


        public int num=0;

        public double x
        {   
            get
            {
                return Canvas.GetLeft(grid);
            }
            set
            {
                Canvas.SetLeft(grid, value);
            }
        }

        public double y
        {
            get
            {
                return Canvas.GetTop(grid);
            }
            set
            {
                Canvas.SetTop(grid, value);
            }
        }

        public double Width
        {
            get
            {
                return grid.Width;
            }
            set
            {
                grid.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return grid.Height;
            }
            set
            {
                grid.Height = value;
            }
        }

        public void Paint_Img(Image img, string dir, double top, double left, double height, double width)
        {
            img.Source = new BitmapImage(new Uri(dir, UriKind.Relative));
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            img.Margin = new Thickness(left, top, 0, 0);
            //Canvas.SetTop(img, top);
            //Canvas.SetLeft(img, left);
            img.Height = height;
            img.Width = width;
            grid.Children.Add(img);

        }

        public KeyWord_Data(Canvas c_in,Color_List fromcolor,Title fromtitle)
        {
            color_list = fromcolor;
            c = c_in;
            title = fromtitle;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment   = VerticalAlignment.Top;
            Canvas.SetTop(grid, 0.0);
            Canvas.SetLeft(grid, 0.0);
            grid.Margin = new Thickness(0, 0, 0, 0);
            grid.Height = 50;
            grid.Width  = 175;

            //grid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            c.Children.Add(grid);


            kw_text.Margin  = new Thickness(0, 20, 0, 0);
            kw_text.Width   = 120;
            kw_text.Height  = 20;
            kw_text.AcceptsReturn = false;
            kw_text.Foreground  = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            kw_text.Background  = new SolidColorBrush(Color.FromArgb(255, 255,255, 255));
            kw_text.FontFamily  = new FontFamily("Meiryo UI");
            kw_text.FontWeight  = FontWeights.Bold;
            kw_text.FontSize    = 15;
            kw_text.HorizontalAlignment = HorizontalAlignment.Left;
            kw_text.VerticalAlignment   = VerticalAlignment.Top;
            kw_text.TextChanged += Kw_text_TextChanged;
            grid.Children.Add(kw_text);


            Paint_Img(and,Pass.img_key+"/"+Pass.colors[color_list.num]+"/and_off.png", 13, 130, 35, 35);
            and.MouseEnter += new MouseEventHandler(And_On_Enter);
            and.MouseLeave += new MouseEventHandler(And_On_Leave);
            and.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(And_On_Clicked);
           
            Paint_Img(close, "img/close_off.png", 1.0, 100, 15, 15);
            close.MouseEnter += new MouseEventHandler(Close_On_Enter);
            close.MouseLeave += new MouseEventHandler(Close_On_Leave);

            Paint_Img(reverse, "img/rt_on.png", 3, 80, 15, 15);
            reverse.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(Reverse_On_Clicked);

        }

        private void Kw_text_TextChanged(object sender, TextChangedEventArgs e)
        {
            title.Title_NoSave();
        }

        private void Reverse_On_Clicked(object sender, MouseButtonEventArgs e)
        {
            Reverse();
            title.Title_NoSave();
        }

        public void Reverse()
        {
            if (IsReversal == true)
            {
                reverse.Source = new BitmapImage(new Uri("img/rt_on.png", UriKind.Relative));
                kw_text.Background = color_list.GetKeyBackColor();
                kw_text.Foreground = color_list.GetKeyForeColor();
                IsReversal = false;
            }
            else
            {
                reverse.Source = new BitmapImage(new Uri("img/rt_off.png", UriKind.Relative));
                kw_text.Background = new SolidColorBrush(Color.FromArgb(255, 85, 85, 85));
                kw_text.Foreground = new SolidColorBrush(Color.FromArgb(255, 185, 185, 185));
                IsReversal = true;
            }

        }

        public void And_On_Leave(object sender, MouseEventArgs e)
        {
            and.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/and_off.png", UriKind.Relative));
        }

        public void And_On_Enter(object sender, MouseEventArgs e)
        {
            and.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/and_over.png", UriKind.Relative));
        }
        private void Close_On_Leave(object sender, MouseEventArgs e)
        {
            close.Source = new BitmapImage(new Uri("img/close_off.png", UriKind.Relative));
        }

        private void Close_On_Enter(object sender, MouseEventArgs e)
        {
            close.Source = new BitmapImage(new Uri("img/close_over.png", UriKind.Relative));
        }

        public void Delete()
        {
            //and.ClearValue()
            foreach (UIElement elem in grid.Children)
            {
                UIElement rm = null;
                rm = elem;
            }
            grid.Children.RemoveRange(0,grid.Children.Count);
            c.Children.Remove(grid);
            grid = null;            
        }

        public void And_On_Clicked(object sender, RoutedEventArgs e)
        {
            Change_And();
            title.Title_NoSave();
        }
        public void Change_And()
        {
            and.MouseEnter -= new MouseEventHandler(And_On_Enter);
            and.MouseLeave -= new MouseEventHandler(And_On_Leave);
            and.MouseLeftButtonDown -= new System.Windows.Input.MouseButtonEventHandler(And_On_Clicked);
            and.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/and_on.png", UriKind.Relative));
            
        }
    }
}
