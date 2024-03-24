using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;

namespace LazyFit.Messages
{
    public class FastEndMessage : ValueChangedMessage<Fast>
    {
        public FastEndMessage(Fast value) : base(value)
        {

        }
    }
}
