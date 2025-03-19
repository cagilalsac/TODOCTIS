using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.TODO.Domain
{
    public class Category : Entity
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public List<Todo> Todos { get; set; } = new List<Todo>();
    }
}
