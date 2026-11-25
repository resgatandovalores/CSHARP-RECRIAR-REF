namespace Recriar.Application.Dtos
{
    public class FamilyMemberDto
    {

        private int _id;
        private int _relationshipTypeId;
        private string _otherRelationship;
        private string _name;
        private string _age;

        // Inicializa os campos do cadastro.
        public FamilyMemberDto()
        {
            _id = 0;
            _relationshipTypeId = 0;
            _otherRelationship = string.Empty;
            _name = string.Empty;
            _age = string.Empty;
        }

        public int getId()
        {
            return _id;
        }

        public void setId(int value)
        {
            _id = value;
        }

        public int getRelationshipTypeId()
        {
            return _relationshipTypeId;
        }

        public void setRelationshipTypeId(int value)
        {
            _relationshipTypeId = value;
        }


        public string getOtherRelationship()
        {
            return _otherRelationship;
        }

        public void setOtherRelationship(string value)
        {
            _otherRelationship = value;
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

    }
}
