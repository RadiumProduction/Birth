using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Birth_First
{
    class Menu
    {
        public Image new_data = new Image();
        public Image load_data = new Image();
        public Image save_data = new Image();
        public Image interval = new Image();
        public Image rt_data = new Image();
        public Image setting = new Image();

        public bool IsRun = false;
        Pass pass = new Pass();

        Color_List color_list;

        List<Image> images = new List<Image>();
        MainWindow m;

        Canvas c = new Canvas();


        public void Load_Image(int color = 0)
        {
            new_data.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color] + "/new.png", UriKind.Relative));
            load_data.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color] + "/load.png", UriKind.Relative));
            save_data.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color] + "/save.png", UriKind.Relative));
            //interval.Source     = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color] + "/interval.png", UriKind.Relative));
            rt_data.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color] + "/rt.png", UriKind.Relative));
            setting.Source = new BitmapImage(new Uri(Pass.img_menu + "/" + Pass.colors[color] + "/setting.png", UriKind.Relative));

        }

        public Menu(MainWindow main, Canvas c_in, int color, Color_List fromcolor)
        {
            m = main;
            c = c_in;
            color_list = fromcolor;
            Load_Image(color);
            images.Add(new_data);
            images.Add(load_data);
            images.Add(save_data);
            //images.Add(interval);
            images.Add(rt_data);
            images.Add(setting);
            foreach (var data in images.Select((v, i) => new { v, i }))
            {
                data.v.Width = 30;
                data.v.Height = 30;
                c.Children.Add(data.v);
                Canvas.SetBottom(data.v, 10);
                if (data.v == setting)
                    Canvas.SetRight(setting, 20);
                else
                    Canvas.SetLeft(data.v, ((data.v.Height + 10) * data.i) + 20);
                data.v.MouseEnter += MenuContentsEnter;
                data.v.MouseLeave += MenuContentsLeave;
            }

            color_list.Menu_Back_Panel = c;

        }

        private void MenuContentsEnter(object sender, MouseEventArgs e)
        {

            Image img = (Image)sender;
            var st_board = new Storyboard();
            var add_up = new DoubleAnimation();
            Storyboard.SetTarget(add_up, img);
            Storyboard.SetTargetProperty(add_up, new PropertyPath("(Canvas.Bottom)"));
            add_up.To = 16;
            add_up.Duration = TimeSpan.FromSeconds(0.075);
            st_board.Children.Add(add_up);
            st_board.Begin();

        }

        private void MenuContentsLeave(object sender, MouseEventArgs e)
        {

            Image img = (Image)sender;
            var st_board = new Storyboard();
            var add_up = new DoubleAnimation();
            Storyboard.SetTarget(add_up, img);
            Storyboard.SetTargetProperty(add_up, new PropertyPath("(Canvas.Bottom)"));
            add_up.To = 10;
            add_up.Duration = TimeSpan.FromSeconds(0.075);
            st_board.Children.Add(add_up);
            st_board.Begin();

        }

        public void New_Data(Key_Erea key_erea, MainWindow main)
        {
            /*
            var dlg = new emanual.Wpf.Utility.MessageBoxEx();
            // 単なるテキストを設定する場合は Message プロパティを設定する
            dlg.Message = "Brith";
            dlg.Width = 350;
            dlg.Height = 165;
            dlg.TextBlock.Height = 65;
            // Inlines プロパティを設定する場合
            dlg.TextBlock.Inlines.Add(new System.Windows.Documents.Bold(new System.Windows.Documents.Run("新しいファイルを作ります。\n\n")));
            dlg.TextBlock.Inlines.Add(new System.Windows.Documents.Run("保存していないデータは失われますがよろしいですか？\n"));

            dlg.Owner = main;
            dlg.Left = main.Left + 50;
            dlg.Top = main.Top + 50;

            // デフォルトでは薄い水色のグラデーションですが、好みの色に指定可能
            //dlg.Background = Brushes.Wheat;
            dlg.Button = MessageBoxButton.YesNo;
            dlg.Image = MessageBoxImage.Warning;

            // ダイアログを開いたときにフォーカスをあてるボタン
            dlg.Result = MessageBoxResult.No;
            dlg.ShowDialog();

            MessageBoxResult result = dlg.Result;
            if (result == MessageBoxResult.No)
            {
                return;
            }

             key_erea.name = "New File";
            Keys_Save keys_save = new Keys_Save();
            keys_save.New(key_erea);
            m.Title = "Birth   -" + key_erea.name + "-";

            */
            Keys_Save keys_save = new Keys_Save();
            keys_save.Save(key_erea, Pass.tmpkey);
            keys_save.Free();

            key_erea.name = "New File";
            keys_save = new Keys_Save();
            keys_save.New(key_erea);
            m.Title = "Birth   - " + key_erea.name + " -";
        }

        public void Load_Data(Key_Erea key_erea, Canvas c)
        {
            string exePath = Environment.GetCommandLineArgs()[0];
            string exeFullPath = System.IO.Path.GetFullPath(exePath);
            string path = System.IO.Path.GetDirectoryName(exeFullPath);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = "Birth File(.br)|*.br";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string fname = openFileDialog.FileName;
                Keys_Save keys_save = new Keys_Save();
                keys_save.Load(key_erea, fname, color_list);
                keys_save.Free();
                key_erea.name = openFileDialog.SafeFileName.TrimEnd('.', 'b', 'r');
                //key_erea.name = openFileDialog.SafeFileName;
                m.Title = "Birth   - " + key_erea.name + " -";

            }
            //openFileDialog = null;
        }

        public void Save_Data(Key_Erea key_erea)
        {
            string exePath = Environment.GetCommandLineArgs()[0];
            string exeFullPath = System.IO.Path.GetFullPath(exePath);
            string path = System.IO.Path.GetDirectoryName(exeFullPath);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = path + "\\";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.FileName = key_erea.name;
            saveFileDialog.Filter = "Birth ファイル(.br)|*.br";
            //saveFileDialog.s
            //Console.WriteLine(saveFileDialog.InitialDirectory);

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string fname = saveFileDialog.FileName;
                Keys_Save keys_save = new Keys_Save();
                keys_save.Save(key_erea, fname);
                keys_save.Free();
                key_erea.name = saveFileDialog.SafeFileName.TrimEnd('.', 'b', 'r');
                m.Title = "Birth   - " + key_erea.name + " -";
            }

        }

    }
}
