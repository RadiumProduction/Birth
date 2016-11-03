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
//using emanual.Wpf.Utility;
//using CustomWindow;


namespace Birth_First
{
    /// <summary>
    /// PIN_Code.xaml の相互作用ロジック
    /// </summary>
    /// 
    
    public partial class PIN_Code : Window
    {
        CoreTweet.OAuth.OAuthSession Session { get; set; }
        Birth_First.Tokens tokens { get; set; }//    new Birth_First.Tokens();

        string  CK  = "";
        string  CS  = "";
 
        bool    recieved    = false;
        string  old_pincode_text = "";
        int     old_pin_selection = 0;
        bool    old_pin_changing = false;

        Pass pass = new Pass();


        public PIN_Code()
        {
            InitializeComponent();
            this.Session = CoreTweet.OAuth.Authorize(CK, CS);
            this.pinbrowser.Source = this.Session.AuthorizeUri;
            //this.Pin_Number.TextInput += Pin_Number_TextInput;  //+= Pin_Number_TextInput;
            tokens = new Tokens(); 

        }



        private void Pin_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(Directory.Exists(Pass.pindir)))
            {
                Directory.CreateDirectory(Pass.pindir);
                tokens.IsEnabled = true;
                tokens.no= 0;
            }
            else
            {
                tokens.IsEnabled = true;
                tokens.no = Directory.GetFiles(Pass.pindir, "*", SearchOption.TopDirectoryOnly).Length; ;
            }
            try
            {
                tokens.token = CoreTweet.OAuth.GetTokens(this.Session, this.Pin_Number.Text);
            }
            catch
            {
                var dlg = new emanual.Wpf.Utility.MessageBoxEx();
                dlg.Message = "Brith";
                dlg.Width = 350;
                dlg.Height = 165;
                dlg.TextBlock.Height = 65;
                dlg.TextBlock.Inlines.Add(new System.Windows.Documents.Bold(new System.Windows.Documents.Run(Properties.Resources.pincode_error_1)));
                dlg.TextBlock.Inlines.Add(Properties.Resources.pincode_error_2);

                dlg.Owner = this;
                dlg.Left = this.Left + 50;
                dlg.Top = this.Top + 50;

                //dlg.Background = Brushes.Wheat;
                dlg.Button = MessageBoxButton.OK;
                dlg.Image = MessageBoxImage.Warning;

                dlg.Result = MessageBoxResult.OK;
                dlg.ShowDialog();

                return;
            }
            try
            {
                CoreTweet.User followedUser = tokens.token.Friendships.Create(user_id => "");
            }
            catch
            {

            }
            DataContractSerializer serializer =new DataContractSerializer(typeof(Birth_First.Tokens));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            XmlWriter xw = XmlWriter.Create(Pass.pindir +"/"+tokens.token.UserId+".xml", settings);
            serializer.WriteObject(xw,tokens);
            xw.Close();
            recieved = true;
            this.Close();

        }


        private void closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if ((recieved == false) &&  (!(Directory.Exists(Pass.pindir))))
            {
                var dlg = new emanual.Wpf.Utility.MessageBoxEx();
                // 単なるテキストを設定する場合は Message プロパティを設定する
                dlg.Message = "Brith";
                dlg.Width = 350;
                dlg.Height = 165;
                dlg.TextBlock.Height = 65;
                // Inlines プロパティを設定する場合
                dlg.TextBlock.Inlines.Add(new System.Windows.Documents.Bold(new System.Windows.Documents.Run(Properties.Resources.pincode_check_1)));//Run("Birth認証を終了しますか？\n\n")));
                var span = new System.Windows.Documents.Span(new System.Windows.Documents.Run(Properties.Resources.pincode_check_2));
                span.Foreground = new SolidColorBrush(Colors.Red);
                dlg.TextBlock.Inlines.Add(span);
                dlg.TextBlock.Inlines.Add(Properties.Resources.pincode_check_3);

                dlg.Owner = this;
                dlg.Left = this.Left + 50;
                dlg.Top = this.Top + 50;

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
                    e.Cancel=true;
                }
                
            }
        }


        private void Pin_Number_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (old_pin_changing)
                return;
            old_pin_changing = true;
            float x;
            if ((!(Single.TryParse(Pin_Number.Text, out x))) && (!(Pin_Number.Text == "")))
            {
                Pin_Number.Text = old_pincode_text;
                Pin_Number.Select( old_pin_selection,0);
            }

            old_pin_selection = Pin_Number.SelectionStart;
            old_pincode_text = Pin_Number.Text;
            old_pin_changing = false;
        }

    }
}
