using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SmartMirror.Messengers.Google;
namespace SmartMirror.Common
{
    class ContentDataTemplateSelector: DataTemplateSelector
    {
        public DataTemplate IdleDataTemplate { get; set; }
        public DataTemplate OpenMessageDataTemplate { get; set; }
        public DataTemplate ListMessageDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate selectedTemplate = IdleDataTemplate;

            if (item is GmailListMessage_ViewModel)
                selectedTemplate = ListMessageDataTemplate;
            else if (item is GmailOpenMessage_ViewModel)
                selectedTemplate = OpenMessageDataTemplate; 

            return selectedTemplate; ;
        }
    }
}
