namespace Recriar.Domain.Entities
{
    public class EmergencyContact
    {

        private int _id;
        private string _name;
        private string _phone;
        private int _childId;
        private Child? _child;

        // Inicializa os campos do cadastro.
        public EmergencyContact()
        {
            _id = 0;
            _name = string.Empty;
            _phone = string.Empty;
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

        public string getName()
        {
            return _name;
        }

        public void setName(string value)
        {
            _name = value;
        }


        public string getPhone()
        {
            return _phone;
        }

        public void setPhone(string value)
        {
            _phone = value;
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
