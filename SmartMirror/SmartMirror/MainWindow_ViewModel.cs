using SmartMirror.Auxileriers.Speech;
using SmartMirror.Common;
using SmartMirror.DateTime.Time;
using SmartMirror.Messengers.Google;
using SmartMirror.NewsFeed;
using SmartMirror.Weather;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror
{
    class MainWindow_ViewModel: ViewModelBase
    {
        /*Private instance variables for Common*/
        private ViewModelBase _currentContent;

        /*Private instance variables for Gmail*/
        private Gmail_ViewModel _gmail;
        private GmailListMessage_ViewModel _listMessage;
        private GmailOpenMessage_ViewModel _openMessage;

        /*Private instance variable for NewsFeed*/
        private News_ViewModel _newsFeed;

        /*Private instance variable for Weather*/
        private Weather_ViewModel _weather;

        /*Private instance variable for Time*/
        private Time_ViewModel _time;

        private SpeechComponent speech;

        /*Constructor*/
        public MainWindow_ViewModel()
        {
            /*Gmail relevant initialization*/
            GmailModule = new Gmail_ViewModel();
            GmailModule.OnListChanged += OnListChanged;
            GmailModule.OnOpenEmailRequest += OnOpenEmail;
            _listMessage = new GmailListMessage_ViewModel();
            _openMessage = new GmailOpenMessage_ViewModel();

            /*News relevant initialization*/
            NewsFeedModule = new News_ViewModel();

            /*Weather relevant initialization*/
            WeatherModule = new Weather_ViewModel();

            /*Time relevant initialization*/
            TimeModule = new Time_ViewModel();

           speech = new SpeechComponent();
            speech.sessionsExpired += Speech_sessionsExpired;
            speech.commandsGenerated += commandRecorded;
            speech.startSession();
        }

        private void Speech_sessionsExpired()
        {
            speech.startSession();
        }

        /*Events subscriptions*/
        public void OnListChanged(List<GmailMessage> _messageList)
        {
            _listMessage.MessageList = new ObservableCollection<GmailMessage>(_messageList);
            CurrentModule = _listMessage;
        }

        public void OnOpenEmail(GmailMessage _message)
        {
            _openMessage.OpenMessage(_message);
            CurrentModule = _openMessage;
        }


        /*Properties*/
        public Gmail_ViewModel GmailModule
        {
            get { return _gmail; }
            set { SetProperty(ref _gmail, value);
}
        }

        public ViewModelBase CurrentModule
        {
            get { return _currentContent; }
            set { SetProperty(ref _currentContent, value); }
        }

        public News_ViewModel NewsFeedModule
        {
            get { return _newsFeed; }
            set { SetProperty(ref _newsFeed, value); }
        }

        public Weather_ViewModel WeatherModule
        {
            get { return _weather; }
            set { SetProperty(ref _weather, value); }
        }

        public Time_ViewModel TimeModule
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        public void commandRecorded(string command, string param)
        {
            //try
            //{
                switch (command)
                    {
                    case "showMailList":
                    {
                        GmailModule.OnSignIn();
                        GmailModule.OnListMessages();
                        break;
                    }
                    case "showMails": GmailModule.OnOpenEmailMessage(int.Parse(param)); break;
                    case "closeMails": break;//TDB
                    case "showWeather": break;
                    case "closeWeather": break;
                    case "openNews": break;
                    case "openSpecificNews": break;
                    case "closeNews": break;
                    case "closeSpecificNews": break;
                    case "closeCalender": break;
                    case "openCalender": break;
                }
            //}
            //catch(Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("command " + command + " with parameter " + param + " could not be executed");
            //}
            

            System.Diagnostics.Debug.WriteLine("command was: " + command + " param was: " + param);
        }
    }
}
