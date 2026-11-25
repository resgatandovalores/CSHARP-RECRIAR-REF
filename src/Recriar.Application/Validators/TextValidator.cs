namespace Recriar.Application.Validators
{
    public class TextValidator
    {

        public bool HasOnlyDigits(string text)
        {

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            foreach (char character in text)
            {

                if (character < '0' || character > '9')
                {
                    return false;
                }
            }

            return true;
        }

        public bool HasOnlyLetters(string text)
        {

            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            foreach (char character in text)
            {

                if (character != ' ' && character != '-' && !char.IsLetter(character))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
