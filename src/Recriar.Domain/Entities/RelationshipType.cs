namespace Recriar.Domain.Entities
{
    public class RelationshipType
    {

        private int otherId = 9;

        public int getOtherId()
        {
            return otherId;
        }

        private int _id;
        private string _name;

        // Inicializa os campos do cadastro.
        public RelationshipType()
        {
            _id = 0;
            _name = string.Empty;
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

    }
}
