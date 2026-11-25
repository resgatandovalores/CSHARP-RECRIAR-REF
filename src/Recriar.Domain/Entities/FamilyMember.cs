namespace Recriar.Domain.Entities
{
    public class FamilyMember
    {

        private int _id;
        private int _relationshipTypeId;
        private RelationshipType? _relationshipType;
        private string? _otherRelationship;
        private string _name;
        private int _age;
        private int _childId;
        private Child? _child;

        // Inicializa os campos do cadastro.
        public FamilyMember()
        {
            _id = 0;
            _relationshipTypeId = 0;
            _relationshipType = null;
            _otherRelationship = null;
            _name = string.Empty;
            _age = 0;
            _childId = 0;
            _child = null;
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


        public RelationshipType? getRelationshipType()
        {
            return _relationshipType;
        }

        public void setRelationshipType(RelationshipType? value)
        {
            _relationshipType = value;
        }


        public string? getOtherRelationship()
        {
            return _otherRelationship;
        }

        public void setOtherRelationship(string? value)
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


        public int getAge()
        {
            return _age;
        }

        public void setAge(int value)
        {
            _age = value;
        }


        public int getChildId()
        {
            return _childId;
        }

        public void setChildId(int value)
        {
            _childId = value;
        }


        public Child? getChild()
        {
            return _child;
        }

        public void setChild(Child? value)
        {
            _child = value;
        }

    }
}
