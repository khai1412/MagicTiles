namespace TheOneStudio.HyperCasual.Extensions
{
    using System.Text.RegularExpressions;

    public static class GoogleSheetExtensions
    {
        private static string downloadUrl = "https://drive.usercontent.google.com/uc?id=";
        private static string downloadImageUrl = "https://drive.usercontent.google.com/u/1/uc?id=";
        private static string downloadType = "&export=download";
        public static string ConvertTSVtoCSV(this string tsvData)
        {
            // Split the TSV data into rows
            string[] rows = tsvData.Split('\n');

            // Convert each row to a CSV row
            string[] csvRows = new string[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split('\t');
                csvRows[i] = string.Join(",", columns);
            }

            // Join the CSV rows back into a single string
            return string.Join("\n", csvRows);
        }
        public static string ViewUrlToDownloadUrl(this string url)
        {
            string pattern = @"file/d/([^/]+)/";
            Match match   = Regex.Match(url, pattern);

            if (match.Success)
            {
                return $"{downloadUrl}{match.Groups[1].Value}{downloadType}";
            }
            return url;
        }
        public static string SpriteUrlToDownloadUrl(this string url)
        {
            string pattern = @"file/d/([^/]+)/";
            Match match   = Regex.Match(url, pattern);

            if (match.Success)
            {
                return $"{downloadImageUrl}{match.Groups[1].Value}{downloadType}";
            }
            return url;
        }
        public static bool IsLink(this string input)
        {
            // Regular expression pattern for a URL
            string pattern = @"^(https?:\/\/)?([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,}(\/[^\s]*)?$";

            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
    }
}