using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LazyFit.Messages
{
    class ShowPageMessage: ValueChangedMessage<int>
    {
        public ShowPageMessage(int value) : base(value)
        {

        }
    }
}
