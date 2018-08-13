using System.IO;
using System.Reflection;
using System.Text;

namespace SetMeta.Tests
{
    public static class StaticResources
    {
        private const string ResourcePath = "SetMeta.Tests.Files";

        public static Stream GetStream(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var stream = assembly.GetManifestResourceStream($"{ResourcePath}.{name}");
            stream?.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static string GetString(string name)
        {
            using (var stream = GetStream(name))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}