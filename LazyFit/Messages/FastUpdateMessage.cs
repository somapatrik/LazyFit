using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;

namespace LazyFit.Messages
{
    public class FastUpdateMessage : ValueChangedMessage<Fast>
    {
        public FastUpdateMessage(Fast value) : base(value)
        {
        }
    }
}
