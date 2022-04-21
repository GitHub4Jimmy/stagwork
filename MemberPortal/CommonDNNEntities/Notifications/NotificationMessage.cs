using DotNetNuke.Entities.Modules.Communications;
using System.Security.Policy;

namespace StagwellTech.SEIU.CommonDNNEntities.Notifications
{
    public enum NotificationColor
    {
        ERROR, WARNING, SUCCESS, PENDING
    }
    public enum NotificationIcon
    {
        NONE, ERROR, WARNING, SUCCESS, FLAG, HOURGLASS, STOPWATCH
    }
    public enum NotificationHeader
    {
        NONE, REMINDER, STATUS, REQUIRED, UPCOMING, ATTENTION
    }



    public class NotificationMessage : ModuleCommunicationEventArgs
    {
        public NotificationMessage() { }
        public NotificationMessage(NotificationColor color, NotificationIcon icon, NotificationHeader header, string textBody, string textHighlighted, string textLink, string urlLink, string notifId = null, string linkClass = null) 
        {
            this.color = color;
            this.icon = icon;
            this.header = header;
            this.textBody = textBody;
            this.textHighlighted = textHighlighted;
            this.textLink = textLink;
            this.urlLink = urlLink;
            
            this.NotificationId = notifId;
            this.linkClass = linkClass;
        }

        public string NotificationId { get; set; }

        public NotificationColor color { get; set; }
        public NotificationIcon icon { get; set; }
        public NotificationHeader header { get; set; }

        public string textBody { get; set; }
        public string textHighlighted { get; set; }
        public string textLink { get; set; }
        public string urlLink { get; set; }
        public string linkClass { get; set; }
    }
}
