using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Birth_First
{

    [DataContract]
    class D
    {
        [DataMember]
        public int limit_num;
        [DataMember]
        public int interval;
        [DataMember]
        public bool rt_from_rt;
        [DataMember]
        public bool rt_from_me;
        [DataMember]
        public bool propaganda;
        [DataMember]
        public int stream_from;
        [DataMember]
        public int colors;
        [DataMember]
        public string burl;
        [DataMember]
        public double back_opacity;
        [DataMember]
        public int language;
        [DataMember]
        public bool Use_NG_WD;
        [DataMember]
        public bool Use_NG_ACC;
        [DataMember]
        public Point window_pos;
        [DataMember]
        public Size window_size;
        [DataMember]
        public bool IsWindow_Full;

        public D()
        {
            window_pos = new Point(100, 100);
            window_size = new Size(525, 350);
            IsWindow_Full = false;
        }
    }

    class SetData
    {

        public D d = new D();
        public string limit_num = "limit_num";
        public string interval = "interval";
        public string rt_from_rt = "rt_from_rt";
        public string propaganda = "propaganda";
        public string rt_from_me = "rt_from_me";
        public string stream_from = "stream_from";
        public string back_opacity = "back_opacity";

        Type type = typeof(D);
        FieldInfo[] fields = typeof(D).GetFields();
        Object o = Activator.CreateInstance(typeof(D));

        public SetData()
        {
            Retweet_Reset();
            Thema_Reset();
            NG_Reset();
        }
        public object GetValue(string name)
        {
            foreach (FieldInfo f in fields)
            {
                if (f.Name == name)
                    return f.GetValue(o);
            }
            return -1;
        }
        public void SetValue(string name, object data)
        {

            foreach (FieldInfo f in fields)
            {
                if (f.Name == name)
                {
                    f.SetValue(o, data);
                    break;
                }
            }

        }
        public void Open_Ready()
        {
            SetValue("limit_num", d.limit_num);
            SetValue("interval", d.interval);
            SetValue("rt_from_rt", d.rt_from_rt);
            SetValue("rt_from_me", d.rt_from_me);
            SetValue("propaganda", d.propaganda);
            SetValue("stream_from", d.stream_from);
            SetValue("colors", d.colors);
            SetValue("burl", d.burl);
            SetValue("back_opacity", d.back_opacity);
            SetValue("language", d.language);
            SetValue("Use_NG_WD", d.Use_NG_WD);
            SetValue("Use_NG_ACC", d.Use_NG_ACC);
            //SetValue("", d.);

        }
        public void Close_Ready()
        {
            d.limit_num = int.Parse(fields[0].GetValue(o).ToString());
            d.interval = int.Parse(fields[1].GetValue(o).ToString());
            d.rt_from_rt = bool.Parse(fields[2].GetValue(o).ToString());
            d.rt_from_me = bool.Parse(fields[3].GetValue(o).ToString());
            d.propaganda = bool.Parse(fields[4].GetValue(o).ToString());
            d.stream_from = int.Parse(fields[5].GetValue(o).ToString());
            d.colors = int.Parse(fields[6].GetValue(o).ToString());
            if (fields[7].GetValue(o) != null)
                d.burl = fields[7].GetValue(o).ToString();
            d.back_opacity = double.Parse(fields[8].GetValue(o).ToString());
            d.language = int.Parse(fields[9].GetValue(o).ToString());
            d.Use_NG_WD = bool.Parse(fields[10].GetValue(o).ToString());
            d.Use_NG_ACC = bool.Parse(fields[11].GetValue(o).ToString());

        }
        public void Clear()
        {
            type = null;
            fields = null;
            o = null;
            d = null;
        }
        public void Retweet_Reset()
        {
            d.limit_num = 10;
            d.interval = 10;
            d.rt_from_rt = true;
            d.rt_from_me = false;
            d.propaganda = true;
            d.stream_from = 0;

            SetValue("limit_num", d.limit_num);
            SetValue("interval", d.interval);
            SetValue("rt_from_rt", d.rt_from_rt);
            SetValue("rt_from_me", d.rt_from_me);
            SetValue("propaganda", d.propaganda);
            SetValue("stream_from", d.stream_from);

        }
        public void Thema_Reset()
        {
            d.colors = 0;
            SetValue("colors", d.colors);
            d.burl = "";
            SetValue("burl", d.burl);
            d.back_opacity = 1.0001;
            SetValue("back_opacity", d.back_opacity);
            d.language = 0;
            SetValue("language", d.language);

        }
        public void NG_Reset()
        {
            NG_Word_Reset();
            NG_Account_Reset();
        }
        public void NG_Word_Reset()
        {
            d.Use_NG_WD = true;
            SetValue("Use_NG_WD", d.Use_NG_WD);
        }
        public void NG_Account_Reset()
        {
            d.Use_NG_ACC = true;
            SetValue("Use_NG_ACC", d.Use_NG_ACC);

        }

    }

}
