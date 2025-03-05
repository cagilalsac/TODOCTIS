using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.TODO.Domain
{
    public class Todo : Entity
    {
        [Required(ErrorMessage = "{0} is required!")]
        // Title is required!

        // Way 1:
        //[StringLength(250, MinimumLength = 3, ErrorMessage = "{0} must have minimum {2} maximum {1} characters!")]
        // Title must have minimum 3 maximum 250 characters!
        // Way 2:
        //[Length(3, 250, ErrorMessage = "{0} must have minimum {1} and maximum {2} characters!")]
        // Title must have minimum 3 maximum 250 characters!

        // Way 3:
        [MaxLength(250, ErrorMessage = "{0} must have maximum {1} characters!")]
        // Title must have maximum 250 characters!
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        // Title must have minimum 3 characters!
        public string Title { get; set; }

        public string Description { get; set; } // varchar(MAX) SQL Server

        // Way 1:
        //public Nullable<DateTime> DueDate { get; set; }
        // Way 2:
        public DateTime? DueDate { get; set; } // 01/01/0001 00:00:00.000 -> null

        public double CompletePercentage { get; set; } // double, float -> 0, 0.25, 0.5, 0.75, 1

        public List<TodoTopic> TodoTopics { get; set; } = new List<TodoTopic>();
        // TodoId: 1, TopicId: 1
        // TodoId: 1, TopicId: 2
        // TodoId: 2, TopicId: 1

        // Todo with Id 1: [1,2]
        // Todo with Id 2: [1]
        [NotMapped]
        public List<int> TopicIds 
        { 
            get => TodoTopics.Select(tt => tt.TopicId).ToList(); 
            set => TodoTopics = value.Select(v => new TodoTopic() { TopicId = v }).ToList(); 
        }
    }
}
