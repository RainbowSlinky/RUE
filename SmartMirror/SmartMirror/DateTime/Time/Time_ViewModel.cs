using SmartMirror.Common;
using System;
using System.Threading.Tasks;
namespace SmartMirror.DateTime.Time
{
    public class Time_ViewModel : ViewModelBase
    {
        
        private string _digitalTimer;
        public string DigitalTimer
        {
            get { return _digitalTimer; }
            set { SetProperty(ref _digitalTimer, value); }
        }

        private int hr, min, sec;
        public Time_ViewModel()
        {
            displayTime();
        }

        public async void displayTime()
        {
            while (true)
            {
                hr = System.DateTime.Now.Hour;
                min = System.DateTime.Now.Minute;
                sec = System.DateTime.Now.Second;

                if (hr > 12)
                {
                    hr -= 12;
                }
                if (sec % 2 == 0)
                {
                    DigitalTimer = hr + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
                }
                else
                {
                    DigitalTimer = hr + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
                }
                await Task.Delay(TimeSpan.FromMilliseconds(950));
            }
        }

    }
}

