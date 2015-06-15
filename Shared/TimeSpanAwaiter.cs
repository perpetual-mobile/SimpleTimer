using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PerpetualEngine
{
    public static class TimeSpanAwaiter
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        { 
            return Task.Delay(timeSpan).GetAwaiter(); 
        }
    }
}

