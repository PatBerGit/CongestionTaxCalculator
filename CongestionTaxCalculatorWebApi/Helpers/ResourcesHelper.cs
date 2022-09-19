using Newtonsoft.Json;
using System.Reflection;

namespace CongestionTaxCalculatorWebApi.Helpers
{
    public static class ResourcesHelper
    {
        private static string GetStringFromResource(Assembly assembly, string fileName)
        {
            var resources = assembly.GetManifestResourceNames();
            if (resources.Length == 0)
            {
                throw new Exception("No Embedded Resources Found.");
            }
            string resource = resources.FirstOrDefault(x => fileName.Equals(x, StringComparison.Ordinal));
            if (resource == null)
            {
                throw new ArgumentNullException(fileName);
            }

            string data = string.Empty;
            using (StreamReader sr = new(assembly.GetManifestResourceStream(resource)))
            {
                data = sr != null
                    ? sr.ReadToEnd()
                    : data;
            }
            return data;
        }

        public static T ReadConfig<T>(string fileName)
        {
            string data = GetStringFromResource(Assembly.GetExecutingAssembly(), fileName);
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
