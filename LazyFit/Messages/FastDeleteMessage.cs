using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;

namespace LazyFit.Messages
{
    public class FastDeleteMessage : ValueChangedMessage<Fast>
    {
        public FastDeleteMessage(Fast value) : base(value)
        {
        }
    }
}
