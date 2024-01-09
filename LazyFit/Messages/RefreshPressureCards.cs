using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LazyFit.Messages
{
    internal class RefreshPressureCards : ValueChangedMessage<bool>
    {
        public RefreshPressureCards(bool value) : base(value)
        {
        }
    }
}
