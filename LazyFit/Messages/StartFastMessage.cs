using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;

namespace LazyFit.Messages
{
    public class StartFastMessage : ValueChangedMessage<Fast>
    {
        public StartFastMessage(Fast value) : base(value)
        {
        }
    }
}
