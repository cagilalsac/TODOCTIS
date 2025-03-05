using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CORE.APP.Domain;

namespace APP.TODO.Domain
{
    public class Topic : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<TodoTopic> TodoTopics { get; set; } = new List<TodoTopic>();

        [NotMapped]
        public List<int> TodoIds
        {
            get => TodoTopics.Select(tt => tt.TodoId).ToList();
            set => TodoTopics = value.Select(v => new TodoTopic() { TodoId = v }).ToList();
        }
    }
}
