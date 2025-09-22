using System;
using System.Collections.Generic;


namespace ImageProcessor.Core
{
    public sealed class ImagePipeline
    {
        private readonly PluginRegistry _registry;
        public ImagePipeline(PluginRegistry registry) => _registry = registry;


        public IReadOnlyList<ImageDocument> Execute(PipelineConfig config, Action<string>? log = null)
        {
            var logger = log ?? Console.WriteLine;
            var ctx = new EffectContext(logger);
            var output = new List<ImageDocument>();


            foreach (var item in config.Images)
            {
                var img = item.ToImage();

                foreach (var call in item.Effects)
                {
                    if (!_registry.TryCreate(call.Key, out var effect) || effect is null)
                        throw new KeyNotFoundException($"Effect key '{call.Key}' is not registered.");

                    var pars = new EffectParameters();
                    var def = effect.Parameter;
                    if (def.HasValue)
                    {
                        var pd = def.Value;
                        object? value = pd.DefaultValue;
                        if (call.Params != null && call.Params.TryGetValue(pd.Name, out var elem))
                        {
                            value = elem.ValueKind switch
                            {
                                System.Text.Json.JsonValueKind.String => elem.GetString(),
                                System.Text.Json.JsonValueKind.Number when pd.Type == TypeCode.Int32 && elem.TryGetInt32(out var i) => i,
                                System.Text.Json.JsonValueKind.Number when pd.Type == TypeCode.Double && elem.TryGetDouble(out var d) => d,
                                System.Text.Json.JsonValueKind.True => true,
                                System.Text.Json.JsonValueKind.False => false,
                                _ => value
                            };
                        }
                        if (pd.Required && value is null)
                            throw new ArgumentException($"Missing required parameter '{pd.Name}' for '{effect.Key}'.");
                        if (value is not null)
                            pars.Set(pd.Name, value);
                    }


                    effect.Apply(img, pars, ctx);
                    img.History.Add($"{effect.Key}({pars})");
                }


                logger($"Finished {img}");
                output.Add(img);
            }
            return output;
        }
    }
}