namespace Recriar.Application.Validators
{
    public class DateValidator
    {
        public DateTime ParseDate(string text)
        {
            // Separa dia, mes e ano nos dois formatos aceitos pela ficha.
            if (text == null) throw new FormatException();
            string[] parts;
            if (text.Contains("/")) parts = text.Split('/');
            else parts = text.Split('.');
            if (parts.Length != 3) throw new FormatException();
            if (parts[0].Length > 2 || parts[1].Length > 2 || parts[2].Length != 4)
                throw new FormatException();
            TextValidator validator = new TextValidator();
            foreach (string part in parts)
            {
                if (!validator.HasOnlyDigits(part)) throw new FormatException();
            }
            int day = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int year = int.Parse(parts[2]);
            try
            {
                return new DateTime(year, month, day);
            }
            catch (ArgumentException)
            {
                throw new FormatException();
            }
        }
    }
}
