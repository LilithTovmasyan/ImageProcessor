using System;
using ImageProcessor.Core;


namespace ImageProcessor
{
    public static class Program
    {
        public static void Main()
        {
            var registry = PluginRegistry.FromFile("Config/plugins.json");
            var config = PipelineConfig.LoadFromFile("Config/pipeline.json");


            var pipeline = new ImagePipeline(registry);
            var results = pipeline.Execute(config, Console.WriteLine);


            Console.WriteLine(" Summary: ");
            foreach (var img in results)
            {
                Console.WriteLine($"- {img}");
                foreach (var step in img.History)
                    Console.WriteLine(" * " + step);
            }
        }
    }
}