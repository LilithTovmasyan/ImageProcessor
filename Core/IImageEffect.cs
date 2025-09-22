namespace ImageProcessor.Core
{
    public interface IImageEffect
    {
        string Key { get; } 
        ParameterDefinition? Parameter { get; } 
        void Apply(ImageDocument image, EffectParameters parameters, EffectContext ctx);
    }
}