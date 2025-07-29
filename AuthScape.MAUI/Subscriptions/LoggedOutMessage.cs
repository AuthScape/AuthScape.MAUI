using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AuthScape.MAUI.Subscriptions
{
    public class LoggedOutMessage : ValueChangedMessage<bool>
    {
        public LoggedOutMessage(bool value) : base(value) { }
    }
}