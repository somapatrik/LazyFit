using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Foods;

namespace LazyFit.Messages
{
    public class FoodRefreshMessage : ValueChangedMessage<Food>
    {
        public FoodRefreshMessage(Food value) : base(value)
        {
        }
    }
}
