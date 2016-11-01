using CoreTweet.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Birth_First
{
    class RT_Data
    {
        public long id;
        public long usr;
        public string text;
    }
    class Twitter
    {
        List<Regex>  regex      = new List<Regex>();
        List<Tokens> tokens     = new List<Tokens>();
        public Check_Num    check_num  = new Check_Num();
        List<RT_Data>    rt_data     = new List<RT_Data>();
        IDisposable disposable;
        Timer   timer = null;
        int     interval;
        bool    rt_from_rt = true;           
        SetData setdata;
        NG_Data ng_datas;

        bool WithHeart;
        List<long> improper_tweet = new List<long>();

        List<List<long>> blocklist = new List<List<long>>();

        private void Get_Timeline(StatusMessage x)
        {
            if (rt_data.Count > 1000)
                return;

            String text = x.Status.Text;
   
            if (x.Status.RetweetedStatus != null)
                if (rt_from_rt == true)
                    text = text.Remove(0, text.IndexOf(":") + 1);
                else
                    return;
            text=text.Replace("\r", "").Replace("\n", "");
            foreach (var data in regex)
            {
                if (data.IsMatch(text))
                {
                    if (setdata.d.Use_NG_WD)
                    {
                        foreach (var ng_word in ng_datas.words)
                        {
                            Regex ng_regex = new Regex(".*" + ng_word + ".*");
                            if (ng_regex.IsMatch(text))
                                return;
                        }
                    }
                    try
                    {
                        RT_Data tmp = new RT_Data();
                        //CoreTweet.Status retweetedStatus = this.tokens[0].Statuses.Retweet(id => x.Status.Id);
                        //CoreTweet.Status retweetedStatus = this.curotoken.Statuses.Retweet(id => x.Status.Id);
                        //Console.WriteLine("{0}: {1}", x.Status.User.ScreenName, text);
                        if (x.Status.RetweetedStatus != null)
                            tmp.id =x.Status.RetweetedStatus.Id;
                        else
                            tmp.id = x.Status.Id;
                        if (x.Status.RetweetedStatus != null)
                            tmp.usr = (long)x.Status.RetweetedStatus.User.Id;
                        else
                            tmp.usr = (long)x.Status.User.Id;
                        tmp.text = text;
                        rt_data.Add(tmp);
                    }
                    catch
                    {

                    }
                    finally
                    {

                    }
                    return;
                }

            }

        }

        private void RT_Go(object o)
        {
            //Console.WriteLine("RT_GO");
            bool IsRetweeted = false;
            if (rt_data.Count != 0)
            {
                foreach (var ids in blocklist.Select((v, i) => new{v,i}))
                {
                    if (tokens[ids.i].UseBlock)
                    {
                        foreach (var id in ids.v)
                        {
                            if (id == rt_data[0].usr)
                            {
                                rt_data.RemoveAt(0);
                                RT_Go(o);
                                return;
                            }
                        }
                    }
                }
                if (setdata.d.Use_NG_ACC)
                {
                    foreach (var ng_acc in ng_datas.account.Select((v, i) => new { v, i }))
                    {
                        if (ng_acc.v.id == rt_data[0].usr)
                        {
                            rt_data.RemoveAt(0);
                            RT_Go(o);
                            return;
                        }
                    }  
                }

                foreach(var id in improper_tweet)
                {
                    if(id==rt_data[0].id)
                    {
                        rt_data.RemoveAt(0);
                        RT_Go(o);
                        return;
                    }
                }
                

                foreach (var token in tokens)
                {
                    if (token.IsEnabled)
                    {
                        if ((!(setdata.d.rt_from_me)) && (token.token.UserId == rt_data[0].usr))
                            continue;

                        try
                        {
                            CoreTweet.Status retweetedStatus = token.token.Statuses.Retweet(id => rt_data[0].id);
                            if(WithHeart)
                                token.token.Favorites.Create(id => rt_data[0].id);
                                //CoreTweet.Status favstatus =token.token.Favorites.Create(id => rt_data[0].id);
                            IsRetweeted = true;
                            //Console.WriteLine(token.token.ScreenName +";RT->" + rt_data[0].text);
                        }
                        //catch(CoreTweet.TwitterException e)
                        //{
                            //Console.WriteLine(rt_data[0].text);
                            //Console.WriteLine(e);
                        //}
                        catch
                        {
                            //Console.WriteLine(rt_data[0].text);
                            //Console.WriteLine(e);
                            if(rt_data.Count!=0)
                                improper_tweet.Add(rt_data[0].id);
                        }

                    }
                }
                if(IsRetweeted)
                    check_num.Num_Inc();
                //Console.WriteLine(rt_data.Count);
                if (rt_data.Count != 0)
                {
                    rt_data.RemoveAt(0);
                    if (!(IsRetweeted))
                        RT_Go(o);
                }//Console.WriteLine(rt_data.Count);
                //Console.WriteLine(DateTime.Now);

            }


        }

        public void RT_Limited(object sender, Num_With args)
        {
            //Stop();
            //Dispatcher.Run
            
            On_RT_Stop_Event(new EventArgs());
            
        }
        public delegate void RT_Stop_EventHandler(object sender, EventArgs args);
        public RT_Stop_EventHandler RT_Stop_Event;

        protected virtual void On_RT_Stop_Event(EventArgs args)
        {

            if (RT_Stop_Event != null)
            {
                RT_Stop_Event(this, args);
            }
        }



        public void Run()
        {

            //Console.WriteLine("Run");
            List<List<long>> tmp_bl = new List<List<long>>();

            foreach(var data in this.tokens.Select((v,i)=> new { v, i }))
            {

                try
                {
                    CoreTweet.Cursored<long> bl_ids = data.v.token.Blocks.Ids();
                    tmp_bl.Add(new List<long>());
                    foreach (var ids in bl_ids)
                    {
                        tmp_bl[data.i].Add(ids);
                    }
                }
                catch
                {
                    if(blocklist.Count>0)
                        tmp_bl.Add(new List<long>(blocklist[data.i]));
                    else
                        tmp_bl.Add(new List<long>());

                }

            }
            foreach(var data in blocklist)
            {
                data.Clear();
            }
            blocklist.Clear();
            blocklist = new List<List<long>>(tmp_bl);



            int stream_from = setdata.d.stream_from-1;//0-99:from token account -1:public
            
            check_num.Limit_Set(setdata.d.limit_num);
            check_num.Num_Reset();
            System.Reactive.Subjects.IConnectableObservable<StreamingMessage> stream;
            if (stream_from >= 0)
                stream = this.tokens[stream_from].token.Streaming.UserAsObservable(StreamingType.User).Publish();//user stream
            else
                stream = this.tokens[0].token.Streaming.SampleAsObservable().Publish();//like Public stream


            //Restart if errored 
            stream.Catch(
                stream.DelaySubscription(TimeSpan.FromSeconds(10)).Retry()
                )
                .Repeat();
            //Stream
            stream.OfType<StatusMessage>()
            .Subscribe(x => Get_Timeline(x), onCompleted: () => Console.WriteLine("completed"));


            this.disposable = stream.Connect();
            this.timer = new Timer(new TimerCallback(RT_Go), null, interval, interval);

        }

        public void Stop()
        {
            this.disposable.Dispose();
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            rt_data.Clear();

            if (setdata.d.propaganda && check_num.NUM!=0)
            {
                foreach (var token in tokens)
                {
                    if (token.IsEnabled)
                    {
                        try
                        {
                            string text = check_num.NUM+Properties.Resources.tweet_propaganda;
                            CoreTweet.Status Status = token.token.Statuses.Update(new Dictionary<string, object>() { { "status", text } });
                            Console.WriteLine(text);
                        }
                        catch
                        {

                        }

                    }
                }
            }

            //Console.WriteLine("---------------RT End--------------------");
        }

        public Twitter(List<Tokens> fromtokens,int frominterval,SetData fromsetdata,NG_Data fromngdatas)
        {
            setdata     = fromsetdata;
            interval    = frominterval*1000;
            tokens      = fromtokens;
            ng_datas    = fromngdatas;
            check_num.RT_Limited_Event += new Check_Num.RT_Limited_EventHandler(RT_Limited);

        }


        public void  Make_Regex(Key_Erea key_erea) 
        {
            foreach (var key_data in key_erea.key)
            {
                if (key_data.IsEnabled == true)
                {   
                    string regex_str= "^";
                    foreach (var keyword_data in key_data.keyword)
                    {
                        string text = keyword_data.kw_text.Text;
                        if ((text != "") && (!(new Regex("^\\s+$").IsMatch(text))))
                        {
                            if (keyword_data.IsReversal == true)
                                regex_str += "(?!.*" + text + ")";
                            else
                                regex_str += "(?=.*" + text + ")";
                        }
                    }
                    regex_str += ".*$";
                    if(regex_str!="^.*$")
                        regex.Add(new Regex(regex_str));

                }

            }
            WithHeart = key_erea.WithHeart;
        }

    }
}
