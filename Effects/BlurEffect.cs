using ImageProcessor.Core;


namespace ImageProcessor.Effects
{
    public sealed class BlurEffect : IImageEffect
    {
        public string Key => "blur";
        public ParameterDefinition? Parameter => new("radius", System.TypeCode.Int32, Required: false, DefaultValue: 1, Description: "Blur radius in px");


        public void Apply(ImageDocument image, EffectParameters parameters, EffectContext ctx)
        {
            int r = parameters.Get<int>("radius", 1);
            image.Tags["blur.radius"] = r;
            ctx.Log($"[{image.Id}] blur radius={r}");
        }
    }
}