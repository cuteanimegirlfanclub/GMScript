
namespace GMEngine.StringExtension
{
    public static class StringExtension
    {
        public static string SOToGO(this string str)
        {
            string result = str.Replace("SO", "GO");
            return result;
        }

        public static string SceneToConfigure(this string str)
        {
            string result = str.Replace("Scene", "Configure");
            return result;
        }

    }
}

