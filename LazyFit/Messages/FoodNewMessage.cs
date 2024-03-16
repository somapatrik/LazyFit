using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Foods;

namespace LazyFit.Messages
{
    public class FoodNewMessage : ValueChangedMessage<Food>
    {
        public FoodNewMessage(Food value) : base(value)
        {
        }
    }
}
