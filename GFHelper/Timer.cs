using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GFHelper.Models;
using System.Threading;

namespace GFHelper
{


    class Timer
    {

        private List<TimerDelegate> timer;
        private InstanceManager im;
        private bool work;

        public Timer(InstanceManager im)
        {
            this.im = im;
            this.timer = new List<TimerDelegate>();
            this.work = false;
        }

        public void DeleteTimerWithTextBlock(TextBlock tb)
        {
            foreach(var item in timer)
            {
                if (item.textBlock == tb)
                {
                    this.timer.Remove(item);
                    return;
                }
            }
        }
        public void AddTimerText(TextBlock tb, int start, int last)
        {
            TimerDelegate tt = new TimerDelegate();
            tt.textBlock = tb;
            tt.start = start;
            tt.last = last;
            this.AddTimer(tt);
        }


        public void AddTimerDelegate(CommonModels.TimerDelegeFunction function, object a, int start, int last)
        {
            TimerDelegate td = new TimerDelegate();
            td.a = a;
            td.function = function;
            td.start = start;
            td.last = last;
            this.AddTimer(td);
        }

        public void AddTimer(int start, int last, CommonModels.TimerDelegeFunction function = null, object a = null, TextBlock tb = null)
        {
            TimerDelegate td = new TimerDelegate();
            td.a = a;
            td.function = function;
            td.start = start;
            td.last = last;
            td.textBlock = tb;
            this.AddTimer(td);
        }

        public void AddTimer(TimerDelegate td)
        {
            this.timer.Add(td);
        }

        public void Start()
        {
            if (this.work) return;
            this.work = true;
            Task.Run(() =>
            {
                Update();
            });
        }

        public void Stop()
        {
            this.work = false;
        }

        public void Update()
        {

            while (work)
            {
                if (timer.Count != 0)
                    foreach (var item in timer.ToArray())
                    {
                        int duration = (item.start + item.last) - CommonHelper.ConvertDateTimeInt(System.DateTime.Now);

                        if (item.textBlock != null)
                        {
                            im.uiHelper.setTextBlockText(item.textBlock, CommonHelper.formatDuration(duration));
                        }

                        if (duration <= 0)
                        {
                            this.timer.Remove(item);
                            if (item.function != null)
                            {
                                item.function(item.a);
                            }

                            if(item.textBlock != null)
                            {
                                im.uiHelper.setTextBlockText(item.textBlock, CommonHelper.formatDuration(0));
                            }
                            
                        }
                    }
                Thread.Sleep(500);
            }
            
        }

    }
}
