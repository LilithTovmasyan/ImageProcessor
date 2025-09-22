using System;

namespace ImageProcessor.Core
{
    public sealed class EffectContext
    {
        public Action<string> Log { get; }
        public EffectContext(Action<string> log) => Log = log;
    }
}