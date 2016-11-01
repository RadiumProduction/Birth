using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Birth_First
{


    class Num_With : EventArgs
    {
        private Check_Num _message;
        private int num;

        public Num_With(Check_Num s)
        {
            _message = s;
        }


        public Check_Num Message
        {
            get { return _message; }
        }


        public Num_With(int i)
        {
            num = i;
        }

        public int Number
        {
            get { return num; }
        }
    }

    class Check_Num
    {
        private int? val = null;
        private int? limit = null;


        public int? NUM
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                if (val == limit)
                    On_RT_Limited_Event(new Num_With(this));
            }
        }

        public delegate void RT_Limited_EventHandler(object sender, Num_With args);
        public RT_Limited_EventHandler RT_Limited_Event;
        
        protected virtual void On_RT_Limited_Event(Num_With args)
        {
    
            if (RT_Limited_Event != null)
            {
                RT_Limited_Event(this, args);
            }
        }


        public void Limit_Set(int x)
        {
            limit = x;
        }

        public void Num_Reset()
        {
            NUM = 0;
        }

        public void Num_Inc()
        {
            NUM++;
        }

        public void Num_Dec()
        {
            NUM--;
        }

    }
}
