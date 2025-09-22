using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ImageProcessor.Core
{
    public sealed class PluginRegistry
    {
        private readonly Dictionary<string, Type> _map = new(StringComparer.OrdinalIgnoreCase);

        public void Register<T>() where T : IImageEffect, new()
        {
            var tmp = new T();
            _map[tmp.Key] = typeof(T);
        }

        public bool TryCreate(string key, out IImageEffect? effect)
        {
            effect = null;
            if (_map.TryGetValue(key, out var t))
            {
                effect = (IImageEffect?)Activator.CreateInstance(t);
                return effect is not null;
            }
            return false;
        }

        public IEnumerable<string> ListKeys() => _map.Keys.OrderBy(x => x);

        public static PluginRegistry FromFile(string path)
        {
            using var fs = File.OpenRead(path);
            using var doc = JsonDocument.Parse(fs, new JsonDocumentOptions { AllowTrailingCommas = true });
            return BuildFromJson(doc);
        }

        public static PluginRegistry FromJson(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return BuildFromJson(doc);
        }

        private static PluginRegistry BuildFromJson(JsonDocument doc)
        {
            var reg = new PluginRegistry();
            if (doc.RootElement.TryGetProperty("plugins", out var plugins))
            {
                foreach (var p in plugins.EnumerateObject())
                {
                    var typeName = p.Value.GetString();
                    if (string.IsNullOrWhiteSpace(typeName)) continue;

                    var t = ResolveType(typeName!);
                    if (t == null)
                        throw new TypeLoadException($"Cannot resolve type '{typeName}' for key '{p.Name}'.");
                    if (!typeof(IImageEffect).IsAssignableFrom(t))
                        throw new InvalidOperationException($"Type '{t.FullName}' does not implement IImageEffect.");

                    reg._map[p.Name] = t;
                }
            }
            return reg;
        }

        private static Type? ResolveType(string typeName)
        {
            var t = Type.GetType(typeName, throwOnError: false, ignoreCase: true);
            if (t is not null) return t;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = asm.GetType(typeName, throwOnError: false, ignoreCase: true);
                if (t is not null) return t;
            }
            return null;
        }
    }
}
