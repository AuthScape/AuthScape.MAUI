using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AuthScape.MAUI.Subscriptions
{
    public class LoginMessage : ValueChangedMessage<bool>
    {
        public LoginMessage(bool value) : base(value) { }
    }
}
