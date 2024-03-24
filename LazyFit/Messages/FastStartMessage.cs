using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;

namespace LazyFit.Messages
{
    public class FastStartMessage : ValueChangedMessage<Fast>
    {
        public FastStartMessage(Fast value) : base(value)
        {
        }
    }
}
