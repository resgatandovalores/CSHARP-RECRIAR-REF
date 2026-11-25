namespace Recriar.Application.Dtos
{
    public class ChildDto
    {

        private int _id;
        private string _name;
        private string _age;
        private string _cpf;
        private string _rg;
        private string _address;
        private string _neighborhood;
        private string _city;
        private string _school;
        private string _motherName;
        private string _fatherName;
        private string _phone;
        private string _registrationDate;
        private string _guardianName;
        private string _guardianRg;
        private string _guardianCpf;
        private string _emergencyContactName;
        private string _emergencyContactPhone;
        private string _healthCenter;
        private string _fileNumber;
        private string _allergy;
        private List<FamilyMemberDto> _familyMembers;

        // Inicializa os campos do cadastro.
        public ChildDto()
        {
            _id = 0;
            _name = string.Empty;
            _age = string.Empty;
            _cpf = string.Empty;
            _rg = string.Empty;
            _address = string.Empty;
            _neighborhood = string.Empty;
            _city = string.Empty;
            _school = string.Empty;
            _motherName = string.Empty;
            _fatherName = string.Empty;
            _phone = string.Empty;
            _registrationDate = string.Empty;
            _guardianName = string.Empty;
            _guardianRg = string.Empty;
            _guardianCpf = string.Empty;
            _emergencyContactName = string.Empty;
            _emergencyContactPhone = string.Empty;
            _healthCenter = string.Empty;
            _fileNumber = string.Empty;
            _allergy = string.Empty;
            _familyMembers = new List<FamilyMemberDto>();
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


        public string getAge()
        {
            return _age;
        }

        public void setAge(string value)
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


        public string getRegistrationDate()
        {
            return _registrationDate;
        }

        public void setRegistrationDate(string value)
        {
            _registrationDate = value;
        }


        public string getGuardianName()
        {
            return _guardianName;
        }

        public void setGuardianName(string value)
        {
            _guardianName = value;
        }


        public string getGuardianRg()
        {
            return _guardianRg;
        }

        public void setGuardianRg(string value)
        {
            _guardianRg = value;
        }


        public string getGuardianCpf()
        {
            return _guardianCpf;
        }

        public void setGuardianCpf(string value)
        {
            _guardianCpf = value;
        }


        public string getEmergencyContactName()
        {
            return _emergencyContactName;
        }

        public void setEmergencyContactName(string value)
        {
            _emergencyContactName = value;
        }


        public string getEmergencyContactPhone()
        {
            return _emergencyContactPhone;
        }

        public void setEmergencyContactPhone(string value)
        {
            _emergencyContactPhone = value;
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


        public List<FamilyMemberDto> getFamilyMembers()
        {
            return _familyMembers;
        }

        public void setFamilyMembers(List<FamilyMemberDto> value)
        {
            _familyMembers = value;
        }

    }
}
