using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;

namespace LazyFit.Messages
{
    public class DeleteFastMessage : ValueChangedMessage<Fast>
    {
        public DeleteFastMessage(Fast value) : base(value)
        {
        }
    }
}
