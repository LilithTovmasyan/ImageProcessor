using System;
using ImageProcessor.Core;


namespace ImageProcessor.Effects
{
    public sealed class ResizeEffect : IImageEffect
    {
        public string Key => "resize";
        public ParameterDefinition? Parameter => new("width", TypeCode.Int32, Required: true, Description: "Target width in px");


        public void Apply(ImageDocument image, EffectParameters parameters, EffectContext ctx)
        {
            int w = parameters.Get<int>("width");
            int oldW = image.Width, oldH = image.Height;
            double ratio = (double)w / Math.Max(1, oldW);
            image.Width = w;
            image.Height = Math.Max(1, (int)Math.Round(oldH * ratio));
            ctx.Log($"[{image.Id}] resize {oldW}x{oldH} -> {image.Width}x{image.Height}");
        }
    }
}