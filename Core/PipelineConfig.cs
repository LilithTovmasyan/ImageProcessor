using System.Collections.Generic;
using System.IO;
using System.Text.Json;


namespace ImageProcessor.Core
{
    public sealed class EffectInvocation
    {
        public string Key { get; set; } = string.Empty;
        public Dictionary<string, JsonElement>? Params { get; set; }
    }


    public sealed class ImageWorkItem
    {
        public string Id { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public List<EffectInvocation> Effects { get; set; } = new();


        public ImageDocument ToImage() => new ImageDocument(Id, Width, Height);
    }


    public sealed class PipelineConfig
    {
        public List<ImageWorkItem> Images { get; set; } = new();


        public static PipelineConfig LoadFromFile(string path)
        {
            using var fs = File.OpenRead(path);
            var cfg = JsonSerializer.Deserialize<PipelineConfig>(fs, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return cfg ?? new PipelineConfig();
        }


        public static PipelineConfig LoadFromJson(string json)
        {
            var cfg = JsonSerializer.Deserialize<PipelineConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return cfg ?? new PipelineConfig();
        }
    }
}