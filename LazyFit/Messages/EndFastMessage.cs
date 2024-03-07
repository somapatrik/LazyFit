using CommunityToolkit.Mvvm.Messaging.Messages;
using LazyFit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyFit.Messages
{
    public class EndFastMessage : ValueChangedMessage<Fast>
    {
        public EndFastMessage(Fast value) : base(value)
        {

        }
    }
}
