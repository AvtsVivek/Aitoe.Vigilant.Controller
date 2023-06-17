using System;

namespace Aitoe.Vigilant.Controller.BL.Infra.Events
{
    public class ReadOnlyEventArgs<T> : EventArgs
    {
        public T Parameter { get; private set; }

        public ReadOnlyEventArgs(T input)
        {
            Parameter = input;
        }
    }
}
