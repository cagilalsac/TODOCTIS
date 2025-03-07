using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.TODO.Features.Todos
{
    public class TodoUpdateRequest : Request, IRequest<CommandResponse> // DTO: Data transfering object
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
    }

    public class TodoUpdateHandler : TodoDbHandler, IRequestHandler<TodoUpdateRequest, CommandResponse>
    {
        public TodoUpdateHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TodoUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Todos.AnyAsync(t => t.Id != request.Id && t.Title.ToUpper() == request.Title.ToUpper().Trim() && t.CompletePercentage != 1, cancellationToken))
                return Error("Not completed todo with the same title exists!");

            // Way 1: not recommended
            //var entity = new Todo()
            //{
            //    Id = request.Id,
            //    CompletePercentage = request.CompletePercentage,
            //    Description = request.Description?.Trim(),
            //    DueDate = request.DueDate,
            //    Title = request.Title.Trim(),
            //    TopicIds = request.TopicIds
            //};
            // Way 2:
            //var entity = await _db.Todos.FindAsync(request.Id, cancellationToken);
            // Way 3:
            var entity = await _db.Todos.Include(t => t.TodoTopics).SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Todo doesn't exists!");

            _db.TodoTopics.RemoveRange(entity.TodoTopics);

            entity.CompletePercentage = request.CompletePercentage;
            entity.Description = request.Description?.Trim();
            entity.DueDate = request.DueDate;
            entity.Title = request.Title.Trim();
            entity.TopicIds = request.TopicIds;

            _db.Todos.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Todo updated successfully.", entity.Id);
        }
    }
}
