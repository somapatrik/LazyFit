using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Foods;

namespace LazyFit.Messages
{
    public class NewFoodMessage : ValueChangedMessage<Food>
    {
        public NewFoodMessage(Food value) : base(value)
        {
        }
    }
}
