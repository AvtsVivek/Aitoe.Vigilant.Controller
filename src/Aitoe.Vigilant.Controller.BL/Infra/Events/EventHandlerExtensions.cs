using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Infra.Events
{
    public static class EventHandlerExtensions
    {
        public static EventArgs<T> CreateArgs<T>(this EventHandler<EventArgs<T>> _, T argument)
        {
            return new EventArgs<T>(argument);
        }

        public static ReadOnlyEventArgs<T> CreateArgs<T>(this EventHandler<ReadOnlyEventArgs<T>> _, T argument)
        {
            return new ReadOnlyEventArgs<T>(argument);
        }
    }
}
