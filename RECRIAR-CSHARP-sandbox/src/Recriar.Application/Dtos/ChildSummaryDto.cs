namespace Recriar.Application.Dtos
{
    public class ChildSummaryDto
    {

        private int _id;
        private string _name;
        private string _cpf;
        private string _phone;

        // Inicializa os campos do cadastro.
        public ChildSummaryDto()
        {
            _id = 0;
            _name = string.Empty;
            _cpf = string.Empty;
            _phone = string.Empty;
        }

        public int getId()
        {
            return _id;
        }

        public void setId(int value)
        {
            _id = value;
        }

        public string getName()
        {
            return _name;
        }

        public void setName(string value)
        {
            _name = value;
        }


        public string getCpf()
        {
            return _cpf;
        }

        public void setCpf(string value)
        {
            _cpf = value;
        }


        public string getPhone()
        {
            return _phone;
        }

        public void setPhone(string value)
        {
            _phone = value;
        }

    }
}
