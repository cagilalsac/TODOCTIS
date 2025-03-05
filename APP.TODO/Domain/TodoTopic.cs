using CORE.APP.Domain;

namespace APP.TODO.Domain
{
    public class TodoTopic : Entity
    {
        public int TodoId { get; set; }
        public Todo Todo { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
