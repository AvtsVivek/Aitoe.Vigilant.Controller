using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aitoe.Vigilant.Controller.BL.Infra.Events
{
    public class EventArgs<T> : EventArgs
    {
        public T Parameter { get; set; }

        public EventArgs(T input)
        {
            Parameter = input;
        }
    }
}
