using Aitoe.Vigilant.Controller.BL.ExceptionDefs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aitoe.Vigilant.Controller.BL.Infra
{
    //http://stackoverflow.com/questions/9314172/getting-all-messages-from-innerexceptions
    public static class ExceptionExtensions
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source, Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }
        

        public static string GetaAllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return String.Join(Environment.NewLine, messages);
        }

        public static string GetaAllAitoeBaseExceptionMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return String.Join(Environment.NewLine, messages);
        }

        public static IEnumerable<AitoeBaseException> GetaAllAitoeBaseExceptions(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException).
                Where(ex => ex is AitoeBaseException).
                Select(ex => ex as AitoeBaseException);
            return messages;
        }

        public static AitoeBaseException GetSummaryAitoeBaseException(this Exception exception)
        {
            var aitoeExceptions = exception.GetaAllAitoeBaseExceptions();
            if (aitoeExceptions.Count() == 0)
            {
                var ae = new AitoeBaseException(exception.GetaAllMessages(), AitoeErrorCodes.OtherFailure);
                return ae;
            }
            else if (aitoeExceptions.Count() == 1)
            {
                return aitoeExceptions.FirstOrDefault();
            }
            else
            {
                var ae = new AitoeBaseException(exception.GetaAllAitoeBaseExceptionMessages(), AitoeErrorCodes.MultipleFailures);
                return ae;
            }
        }
    }
}
