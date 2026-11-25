namespace Recriar.Domain.Entities
{
    public class Child
    {

        private int _id;
        private string _name;
        private int _age;
        private string _cpf;
        private string _rg;
        private string _address;
        private string _neighborhood;
        private string _city;
        private string _school;
        private string _motherName;
        private string _fatherName;
        private string _phone;
        private DateTime _registrationDate;
        private string _healthCenter;
        private string _fileNumber;
        private string _allergy;
        private Guardian? _guardian;
        private EmergencyContact? _emergencyContact;
        private List<FamilyMember> _familyMembers;

        // Inicializa os campos do cadastro.
        public Child()
        {
            _id = 0;
            _name = string.Empty;
            _age = 0;
            _cpf = string.Empty;
            _rg = string.Empty;
            _address = string.Empty;
            _neighborhood = string.Empty;
            _city = string.Empty;
            _school = string.Empty;
            _motherName = string.Empty;
            _fatherName = string.Empty;
            _phone = string.Empty;
            _registrationDate = new DateTime();
            _healthCenter = string.Empty;
            _fileNumber = string.Empty;
            _allergy = string.Empty;
            _guardian = null;
            _emergencyContact = null;
            _familyMembers = new List<FamilyMember>();
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


        public int getAge()
        {
            return _age;
        }

        public void setAge(int value)
        {
            _age = value;
        }


        public string getCpf()
        {
            return _cpf;
        }

        public void setCpf(string value)
        {
            _cpf = value;
        }


        public string getRg()
        {
            return _rg;
        }

        public void setRg(string value)
        {
            _rg = value;
        }


        public string getAddress()
        {
            return _address;
        }

        public void setAddress(string value)
        {
            _address = value;
        }


        public string getNeighborhood()
        {
            return _neighborhood;
        }

        public void setNeighborhood(string value)
        {
            _neighborhood = value;
        }


        public string getCity()
        {
            return _city;
        }

        public void setCity(string value)
        {
            _city = value;
        }


        public string getSchool()
        {
            return _school;
        }

        public void setSchool(string value)
        {
            _school = value;
        }


        public string getMotherName()
        {
            return _motherName;
        }

        public void setMotherName(string value)
        {
            _motherName = value;
        }


        public string getFatherName()
        {
            return _fatherName;
        }

        public void setFatherName(string value)
        {
            _fatherName = value;
        }


        public string getPhone()
        {
            return _phone;
        }

        public void setPhone(string value)
        {
            _phone = value;
        }


        public DateTime getRegistrationDate()
        {
            return _registrationDate;
        }

        public void setRegistrationDate(DateTime value)
        {
            _registrationDate = value;
        }


        public string getHealthCenter()
        {
            return _healthCenter;
        }

        public void setHealthCenter(string value)
        {
            _healthCenter = value;
        }


        public string getFileNumber()
        {
            return _fileNumber;
        }

        public void setFileNumber(string value)
        {
            _fileNumber = value;
        }


        public string getAllergy()
        {
            return _allergy;
        }

        public void setAllergy(string value)
        {
            _allergy = value;
        }


        public Guardian? getGuardian()
        {
            return _guardian;
        }

        public void setGuardian(Guardian? value)
        {
            _guardian = value;
        }


        public EmergencyContact? getEmergencyContact()
        {
            return _emergencyContact;
        }

        public void setEmergencyContact(EmergencyContact? value)
        {
            _emergencyContact = value;
        }


        public List<FamilyMember> getFamilyMembers()
        {
            return _familyMembers;
        }

        public void setFamilyMembers(List<FamilyMember> value)
        {
            _familyMembers = value;
        }

    }
}
