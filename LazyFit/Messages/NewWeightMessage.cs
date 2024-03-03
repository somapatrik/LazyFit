using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.WeightModels;

namespace LazyFit.Messages
{
    public class NewWeightMessage : ValueChangedMessage<Weight>
    {
        public NewWeightMessage(Weight value) : base(value)
        {

        }
    }
}
