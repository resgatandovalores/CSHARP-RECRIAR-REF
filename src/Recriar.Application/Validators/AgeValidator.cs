namespace Recriar.Application.Validators
{
    public class AgeValidator
    {

        private int minAge = 0;

        public int getMinAge()
        {
            return minAge;
        }

        private int maxAge = 120;

        public int getMaxAge()
        {
            return maxAge;
        }

        public int ParseAge(string text)
        {
            if (!new TextValidator().HasOnlyDigits(text)) throw new FormatException();
            int age = int.Parse(text);
            if (age < minAge || age > maxAge) throw new ArgumentException("Idade inválida.");
            return age;
        }
    }
}
