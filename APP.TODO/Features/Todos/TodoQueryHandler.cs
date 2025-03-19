using APP.TODO.Domain;
using APP.TODO.Features.Categories;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace APP.TODO.Features.Todos
{
    public class TodoQueryRequest : Request, IRequest<IQueryable<TodoQueryResponse>>
    {
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public List<int> TopicIds { get; set; }
        public DateTime? DueDateStart { get; set; }
        public DateTime? DueDateEnd { get; set; }
        public double? CompletePercentageStart { get; set; }
        public double? CompletePercentageEnd { get; set; }

        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }
    }

    public class TodoQueryResponse : QueryResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public string DueDateF { get; set; }
        public double CompletePercentage { get; set; }
        public string CompletePercatageF { get; set; }
        public List<int> TopicIds { get; set; }

        public string TopicNames { get; set; }

        public CategoryQueryResponse Category { get; set; }
    }

    public class TodoQueryHandler : TodoDbHandler, IRequestHandler<TodoQueryRequest, IQueryable<TodoQueryResponse>>
    {
        public TodoQueryHandler(TodoDb db) : base(db)
        {
        }

        public Task<IQueryable<TodoQueryResponse>> Handle(TodoQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = _db.Todos.Include(todo => todo.TodoTopics).ThenInclude(todotopic => todotopic.Topic).Include(todo => todo.Category)
                .OrderBy(t => t.CompletePercentage).ThenByDescending(t => t.DueDate).ThenBy(t => t.Title).AsQueryable();

            // filtering:
            if (!string.IsNullOrWhiteSpace(request.Title))
                entityQuery = entityQuery.Where(t => t.Title.ToUpper().Contains(request.Title.ToUpper().Trim()));
            if (request.CategoryId.HasValue)
                entityQuery = entityQuery.Where(t => t.CategoryId == request.CategoryId.Value);

            // Way 1:
            //if (request.TopicIds is not null && request.TopicIds.Any())
            //    entityQuery = entityQuery.Where(todo => todo.TodoTopics.Any(tt => request.TopicIds.Contains(tt.TopicId)));
            // Way 2:
            if (request.TopicIds is not null && request.TopicIds.Any())
                entityQuery = entityQuery.Where(t => t.TopicIds.Any(topicId => request.TopicIds.Contains(topicId)));

            if (request.DueDateStart.HasValue)
                entityQuery = entityQuery.Where(t => t.DueDate >= request.DueDateStart.Value);
            if (request.DueDateEnd.HasValue)
                entityQuery = entityQuery.Where(t => t.DueDate <= request.DueDateEnd.Value);
            if (request.CompletePercentageStart.HasValue)
                entityQuery = entityQuery.Where(t => t.CompletePercentage >= request.CompletePercentageStart);
            if (request.CompletePercentageEnd.HasValue)
                entityQuery = entityQuery.Where(t => t.CompletePercentage <= request.CompletePercentageEnd);
            
            var query = entityQuery.Select(t => new TodoQueryResponse()
                {
                    Id = t.Id,
                    CompletePercentage = t.CompletePercentage,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Title = t.Title,
                    TopicIds = t.TopicIds,
                    DueDateF = t.DueDate.HasValue ? t.DueDate.Value.ToString("MM/dd/yyyy HH:mm:ss") : string.Empty,
                    CompletePercatageF = t.CompletePercentage.ToString("N2"),

                    TopicNames = string.Join(", ", t.TodoTopics.Select(todotopic => todotopic.Topic.Name)),

                    Category = t.Category == null ? null : new CategoryQueryResponse()
                    {
                        Name = t.Category.Name,
                        Id = t.Category.Id
                    }
                });
            return Task.FromResult(query);
        }
    }
}
