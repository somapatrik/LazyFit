using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.WeightModels;

namespace LazyFit.Messages
{
    public class RefreshWeightMessage : ValueChangedMessage<Weight>
    {
        public RefreshWeightMessage(Weight value) : base(value)
        {
        }
    }
}
