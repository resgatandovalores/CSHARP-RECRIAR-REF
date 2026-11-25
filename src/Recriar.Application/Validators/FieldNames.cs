namespace Recriar.Application.Validators
{
    public class FieldNames
    {

        private string name = "Nome";

        public string getName()
        {
            return name;
        }

        private string age = "Idade";

        public string getAge()
        {
            return age;
        }

        private string cpf = "CPF";

        public string getCpf()
        {
            return cpf;
        }

        private string rg = "RG";

        public string getRg()
        {
            return rg;
        }

        private string address = "Endereço";

        public string getAddress()
        {
            return address;
        }

        private string neighborhood = "Bairro";

        public string getNeighborhood()
        {
            return neighborhood;
        }

        private string city = "Cidade";

        public string getCity()
        {
            return city;
        }

        private string school = "Escola";

        public string getSchool()
        {
            return school;
        }

        private string motherName = "Nome da mãe";

        public string getMotherName()
        {
            return motherName;
        }

        private string fatherName = "Nome do pai";

        public string getFatherName()
        {
            return fatherName;
        }

        private string phone = "Telefone";

        public string getPhone()
        {
            return phone;
        }

        private string registrationDate = "Data de cadastro";

        public string getRegistrationDate()
        {
            return registrationDate;
        }

        private string guardianName = "Nome do responsável";

        public string getGuardianName()
        {
            return guardianName;
        }

        private string guardianRg = "RG do responsável";

        public string getGuardianRg()
        {
            return guardianRg;
        }

        private string guardianCpf = "CPF do responsável";

        public string getGuardianCpf()
        {
            return guardianCpf;
        }

        private string emergencyContactName = "Nome do contato de emergência";

        public string getEmergencyContactName()
        {
            return emergencyContactName;
        }

        private string emergencyContactPhone = "Telefone de emergência";

        public string getEmergencyContactPhone()
        {
            return emergencyContactPhone;
        }

        private string healthCenter = "Centro de saúde";

        public string getHealthCenter()
        {
            return healthCenter;
        }

        private string fileNumber = "Número da ficha";

        public string getFileNumber()
        {
            return fileNumber;
        }

        private string allergy = "Alergia";

        public string getAllergy()
        {
            return allergy;
        }

        public string FamilyMemberRelationship(int number)
        {
            return $"Parentesco do familiar {number}";
        }

        public string FamilyMemberOtherRelationship(int number)
        {
            return $"Outro parentesco do familiar {number}";
        }

        public string FamilyMemberName(int number)
        {
            return $"Nome do familiar {number}";
        }

        public string FamilyMemberAge(int number)
        {
            return $"Idade do familiar {number}";
        }
    }
}
