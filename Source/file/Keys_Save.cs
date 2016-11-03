using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Birth_First
{
    [DataContract]
    class KeyWord_Save
    {
        [DataMember]
        public string   text;
        [DataMember]
        public bool     IsReversal;


    }

    [DataContract]
    class Key_Save
    {
        [DataMember]
        public List<KeyWord_Save> keyword = new List<KeyWord_Save>();
        [DataMember]
        public bool IsEnabled;

    }

    [DataContract]
    class Keys_Save
    {
        [DataMember]
        public List<Key_Save> key = new List<Key_Save>();
        [DataMember]
        public bool WithHeart;
        [DataMember]
        public int interval;
        [DataMember]
        public string name;


        public Keys_Save()
        {


        }
        public void New(Key_Erea key_erea)
        {
            Key_Erea_Clear(key_erea);
            key_erea.First_Keyword_Add();
            key_erea.heart.Source = new BitmapImage(new Uri("img/heart_off.png", UriKind.Relative));
            key_erea.Change_Size();


        }

        public void Load(Key_Erea key_erea, String fname,Color_List color_list)
        {
            Key_Erea_Clear(key_erea);
            Pass pass = new Pass();

            DataContractSerializer serializer = new DataContractSerializer(typeof(Keys_Save));
            XmlReader xr = XmlReader.Create(fname);
            Keys_Save tmp = (Keys_Save)serializer.ReadObject(xr);
            xr.Close();

            key_erea.WithHeart = tmp.WithHeart;
            key_erea.name= tmp.name;

            if (key_erea.WithHeart)
                key_erea.heart.Source = new BitmapImage(new Uri("img/heart_on.png", UriKind.Relative));
            else
                key_erea.heart.Source = new BitmapImage(new Uri("img/heart_off.png", UriKind.Relative));

            key_erea.interval = tmp.interval;
            foreach (var skey_data in tmp.key.Select((v, i) => new { v, i }))
            {
                if (skey_data.i == 0)
                    key_erea.First_Keyword_Add();
                else
                    key_erea.Add_Key();
                key_erea.key[skey_data.i].IsEnabled = skey_data.v.IsEnabled;
                if (key_erea.key[skey_data.i].IsEnabled == false)
                    key_erea.key[skey_data.i].use.Source = new BitmapImage(new Uri(Pass.img_key + "/" + Pass.colors[color_list.num] + "/use_off.png", UriKind.Relative));
                foreach (var skeyword_data in skey_data.v.keyword.Select((v, i) => new { v, i }))
                {

                    if (skeyword_data.i != 0)
                    {
                        key_erea.key[skey_data.i].keyword[skeyword_data.i - 1].and.MouseLeftButtonDown -= key_erea.key[skey_data.i].And_Click;
                        key_erea.key[skey_data.i].keyword[skeyword_data.i - 1].Change_And();
                        key_erea.key[skey_data.i].Add_Key();
                    }
                    key_erea.key[skey_data.i].keyword[skeyword_data.i].IsReversal = skeyword_data.v.IsReversal;
                    key_erea.key[skey_data.i].keyword[skeyword_data.i].kw_text.Text = skeyword_data.v.text;
                }
            }

        }

        public void Save(Key_Erea key_erea, String fname)
        {
            WithHeart = key_erea.WithHeart;
            //interval = key_erea.interval;
            name = key_erea.name;
            foreach (var key_data in key_erea.key)
            {
                Key_Save tmp_key = new Key_Save();
                tmp_key.IsEnabled = key_data.IsEnabled;
                foreach (var keyword_data in key_data.keyword)
                {
                    KeyWord_Save tmp_keyword = new KeyWord_Save();
                    tmp_keyword.IsReversal = keyword_data.IsReversal;
                    tmp_keyword.text = keyword_data.kw_text.Text;
                    tmp_key.keyword.Add(tmp_keyword);
                }
                this.key.Add(tmp_key);
            }

            DataContractSerializer serializer = new DataContractSerializer(typeof(Keys_Save));
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new System.Text.UTF8Encoding(false);
            XmlWriter xw = XmlWriter.Create(fname, settings);
            serializer.WriteObject(xw, this);
            xw.Close();


        }


        public void Free()
        {
            foreach (var key_data in key)
            {
                key_data.keyword.Clear();
            }
            key.Clear();
        }

        public void Key_Erea_Clear(Key_Erea key_erea)
        {
            /*
            foreach (var key_data in key_erea.key)
            {
                foreach (var keyword_data in key_data.keyword)
                {
                    keyword_data.kw_text = null;
                    keyword_data.reverse = null;
                    keyword_data.and = null;
                    keyword_data.close = null;
                    Console.WriteLine("keyword");              
                }
                key_data.keyword.Clear();
                //key_data.use.Dispose;
                key_data.use.Visibility = System.Windows.Visibility.Collapsed;
                key_data.use = null;
                key_data.key_c.Visibility=System.Windows.Visibility.Collapsed;
                Console.WriteLine("key");
            }
            key_erea.key.Clear();
            */
            foreach (var key_data in key_erea.key)
            {
                key_data.Delete();
            }
            key_erea.key.Clear();
            key_erea.WithHeart = false;
        }
    }

}
