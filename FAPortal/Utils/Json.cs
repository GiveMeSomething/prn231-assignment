using System;
using System.Text.Json;

namespace FAPortal.Utils
{
	public class CustomJson
	{
        public static string Stringify<T>(T input)
        {
            return JsonSerializer.Serialize<T>(input);
        }

        public static T? Deserialzie<T>(string input)
        {
            return JsonSerializer.Deserialize<T>(input);
        }
    }
}

