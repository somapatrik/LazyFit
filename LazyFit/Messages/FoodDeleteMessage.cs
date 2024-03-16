using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Foods;

namespace LazyFit.Messages
{
    public class FoodDeleteMessage : ValueChangedMessage<Food>
    {
        public FoodDeleteMessage(Food value) : base(value)
        {
        }
    }
}
