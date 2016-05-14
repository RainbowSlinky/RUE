﻿using System;

namespace SmartMirror.Messengers.Google
{
    public class GmailMessage
    {
        public String FromMessage { get; set; }
        public String HeadlineMessage { get; set; }
        public String BodyMessage { get; set; }
        public String DateMessage { get; set; }

        public GmailMessage(string fromMessage, string headlineMessage, string bodyMessage, string dateMessage)
        {
            this.FromMessage = fromMessage;
            this.HeadlineMessage = headlineMessage;
            this.BodyMessage = bodyMessage;
            this.DateMessage = dateMessage;
        }

        public  GmailMessage()
        {

        }

    }
}
