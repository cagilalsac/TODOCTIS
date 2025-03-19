using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.TODO.Features.Todos
{
    public class TodoCreateRequest : Request, IRequest<CommandResponse> // DTO: Data transfering object
    {
        // MVC Fluent Validation: third party validation library

        [Required(ErrorMessage = "{0} is required!")]
        [MaxLength(250, ErrorMessage = "{0} must have maximum {1} characters!")]
        [MinLength(3, ErrorMessage = "{0} must have minimum {1} characters!")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "{0} must have maximum {1} characters!")]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        [Range(0, 1, ErrorMessage = "{0} must be between {1} and {2}!")]
        public double CompletePercentage { get; set; } // between 0 and 1

        public List<int> TopicIds { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }

        public int? CategoryId { get; set; }
    }

    public class TodoCreateHandler : TodoDbHandler, IRequestHandler<TodoCreateRequest, CommandResponse>
    {
        public TodoCreateHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TodoCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Todos.AnyAsync(t => t.Title.ToUpper() == request.Title.ToUpper().Trim() && t.CompletePercentage != 1, cancellationToken))
                return Error("Not completed todo with the same title exists!");

            var entity = new Todo()
            {
                CompletePercentage = request.CompletePercentage,
                Description = request.Description?.Trim(),
                DueDate = request.DueDate,
                Title = request.Title.Trim(),
                TopicIds = request.TopicIds,
                CategoryId = request.CategoryId
            };

            _db.Todos.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Todo created successfully.", entity.Id);
        }
    }
}
