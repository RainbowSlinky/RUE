using SmartMirror.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirror.Messengers.Google
{
    class GmailOpenMessage_ViewModel: ViewModelBase
    {
        public GmailOpenMessage_ViewModel()
        {
            From = "";
            Subject = "";
            Body = "";
        }

        public GmailOpenMessage_ViewModel(GmailMessage _message)
        {
            From = _message.FromMessage;
            Subject = _message.HeadlineMessage;
            Body = _message.BodyMessage;
        }


        public void OpenMessage(GmailMessage _message)
        {
            From = _message.FromMessage;
            Subject = _message.HeadlineMessage;
            Body = _message.BodyMessage;
        }

        public String From { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }

    }
}
