namespace CORE.APP.Domain
{
    public abstract class Entity
    {
        public int Id { get; set; }

        protected Entity()
        {
        }

        protected Entity(int id)
        {
            Id = id;
        }
    }
}
