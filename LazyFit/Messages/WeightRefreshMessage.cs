using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.WeightModels;

namespace LazyFit.Messages
{
    public class WeightRefreshMessage : ValueChangedMessage<Weight>
    {
        public WeightRefreshMessage(Weight value) : base(value)
        {

        }
    }
}
