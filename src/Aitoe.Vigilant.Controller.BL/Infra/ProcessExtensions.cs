using System;
using System.Diagnostics;

namespace Aitoe.Vigilant.Controller.BL.Infra
{
    public static class ProcessExtensions
    {
        public static bool IsRunning(this Process process)
        {
            try { Process.GetProcessById(process.Id); }
            catch (InvalidOperationException) { return false; }
            catch (ArgumentException argEx)
            {
                var message = argEx.Message;
                return false;
            }
            return true;
        }
    }
}
