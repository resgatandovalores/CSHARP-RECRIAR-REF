using Recriar.Application.Validators;

namespace Recriar.Application.Exceptions
{
    public class ErrorMessages
    {

        private string unexpected = "Ocorreu um erro inesperado. Entre em contato com o suporte.";

        public string getUnexpected()
        {
            return unexpected;
        }

        private string childNotFound = "Cadastro não encontrado.";

        public string getChildNotFound()
        {
            return childNotFound;
        }

        private string invalidRegistrationDate = "Data de cadastro inválida. Use o formato dd/mm/aaaa ou dd.mm.aaaa e uma data que exista.";

        public string getInvalidRegistrationDate()
        {
            return invalidRegistrationDate;
        }

        public string TooManyFamilyMembers()
        {
            return
            $"A ficha pode ter no máximo {new ChildValidator().getMaxFamilyMembers()} familiares.";
        }

        public string FieldRequired(string field)
        {
            return $"O campo {field} é obrigatório.";
        }

        public string OnlyLetters(string field)
        {
            return $"O campo {field} deve conter apenas letras, espaços e hífen.";
        }

        public string OnlyDigits(string field)
        {
            return $"O campo {field} deve conter apenas números.";
        }

        public string InvalidAge(string field)
        {
            return $"O campo {field} é inválido. Informe um número de {new AgeValidator().getMinAge()} a {new AgeValidator().getMaxAge()}.";
        }

        public string FieldTooLong(string field, int maxLength)
        {
            return $"O campo {field} aceita no máximo {maxLength} caracteres.";
        }

        private string exportFailed = "Não foi possível salvar a ficha. Feche a ficha se ela estiver aberta e tente de novo. Se continuar, entre em contato com o suporte.";

        public string getExportFailed()
        {
            return exportFailed;
        }

        public string InvalidRelationshipType(string field)
        {
            return $"O campo {field} é inválido. Escolha uma das opções da lista.";
        }

        public string OtherRelationshipRequired(string field)
        {
            return $"O campo {field} é obrigatório quando o parentesco escolhido é \"Outro\".";
        }
    }
}
