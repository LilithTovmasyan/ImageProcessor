using System;
using System.Collections.Generic;


namespace ImageProcessor.Core
{
    public sealed class ImageDocument
    {
        public string Id { get; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<string, object?> Tags { get; } = new();
        public List<string> History { get; } = new();


        public ImageDocument(string id, int width, int height)
        {
            Id = id;
            Width = width;
            Height = height;
            History.Add($"Loaded '{id}' as {width}x{height}");
        }


        public override string ToString() => $"{Id}: {Width}x{Height}";
    }
}