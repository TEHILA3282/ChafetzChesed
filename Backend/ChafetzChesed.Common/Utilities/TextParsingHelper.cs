using System.Text.RegularExpressions;

namespace ChafetzChesed.Common.Utilities
{
    public static class TextParsingHelper
    {
        public static decimal ExtractAmount(string perut)
        {
            if (string.IsNullOrWhiteSpace(perut))
                return 0;

            var matches = Regex.Matches(perut, @"-?\d{1,3}(,\d{3})*(\.\d+)?|-?\d+(\.\d+)?");
            decimal max = 0;

            foreach (Match match in matches)
            {
                var value = match.Value.Replace(",", "");
                if (decimal.TryParse(value, out var result))
                {
                    if (Math.Abs(result) > Math.Abs(max))
                        max = result;
                }
            }

            if (max == 0)
                Console.WriteLine($"❗ לא זוהה סכום מתוך: '{perut}'");

            return max;
        }
    }
}
