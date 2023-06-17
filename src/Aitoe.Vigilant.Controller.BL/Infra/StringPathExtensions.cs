using System;

namespace Aitoe.Vigilant.Controller.BL.Infra
{
    public static class StringPathExtensions
    {
        public static string TrimFwdSlashes(this string path)
        {
            return path.Replace("\\", "/").Trim(new Char[] { ' ', '/' });
        }
    }
}
