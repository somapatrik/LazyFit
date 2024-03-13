using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models.Drinks;

namespace LazyFit.Messages
{
    public class NewDrinkMessage : ValueChangedMessage<Drink>
    {
        public NewDrinkMessage(Drink value) : base(value)
        {
        }
    }
}
