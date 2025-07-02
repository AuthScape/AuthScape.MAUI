namespace AuthScape.MAUI.DeepLink
{
    public class URI
    {
        public static IDictionary<string, object> ParseQueryString(string query)
        {
            var result = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(query))
                return result;

            // Trim the leading "?" if it exists
            if (query.StartsWith("?"))
                query = query.Substring(1);

            var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                var kvp = pair.Split('=', 2);
                if (kvp.Length == 2)
                {
                    var key = Uri.UnescapeDataString(kvp[0]);
                    var value = Uri.UnescapeDataString(kvp[1]);
                    result[key] = value;
                }
            }

            return result;
        }

    }
}
