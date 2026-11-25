namespace Recriar.Application.Helpers
{
    public class TextFormatter
    {

        public string ToLowerCase(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.ToLowerInvariant();
        }

        public string CapitalizeWords(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            char[] characters = ToLowerCase(text).ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {

                bool isFirstLetter = i == 0;
                bool followsSpace = i > 0 && characters[i - 1] == ' ';

                if (isFirstLetter || followsSpace)
                {
                    characters[i] = char.ToUpperInvariant(characters[i]);
                }
            }

            return new string(characters);
        }
    }
}
