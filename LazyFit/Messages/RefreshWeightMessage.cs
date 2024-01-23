using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.WeightModels;

namespace LazyFit.Messages
{
    public class RefreshWeightMessage : ValueChangedMessage<bool>
    {
        public RefreshWeightMessage(bool value) : base(value)
        {
        }
    }
}
