using SmartMirror.Common;
using System.Collections.ObjectModel;

namespace SmartMirror.Messengers.Google
{
    public class GmailListMessage_ViewModel : ViewModelBase
    {
        private ObservableCollection<GmailMessage> _messageList;
        private GmailMessage _gmailMessage;

        public GmailMessage GmailMessage
        {
            get { return _gmailMessage; }
            set { SetProperty(ref _gmailMessage, value); }
        }

        public ObservableCollection<GmailMessage> MessageList
        {
            get { return _messageList; }
            set { SetProperty(ref _messageList, value); }
        }

        public GmailListMessage_ViewModel()
        {

        }
    }
}
