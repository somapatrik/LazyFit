
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LazyFit.Messages
{
    class ActionsReloadMessages : ValueChangedMessage<object>
    {
        public ActionsReloadMessages(object value) : base(value)
        {
        }
    }
}
