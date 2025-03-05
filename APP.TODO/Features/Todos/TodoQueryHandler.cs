using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.TODO.Features.Todos
{
    public class TodoQueryRequest : Request, IRequest<IQueryable<TodoQueryResponse>>
    {
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
    }

    public class TodoQueryHandler : TodoDbHandler, IRequestHandler<TodoQueryRequest, IQueryable<TodoQueryResponse>>
    {
        public TodoQueryHandler(TodoDb db) : base(db)
        {
        }

        public Task<IQueryable<TodoQueryResponse>> Handle(TodoQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Todos.OrderBy(t => t.CompletePercentage).ThenByDescending(t => t.DueDate).ThenBy(t => t.Title)
                .Select(t => new TodoQueryResponse()
                {
                    Id = t.Id,
                    CompletePercentage = t.CompletePercentage,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Title = t.Title,
                    TopicIds = t.TopicIds,
                    DueDateF = t.DueDate.HasValue ? t.DueDate.Value.ToString("MM/dd/yyyy HH:mm:ss") : string.Empty,
                    CompletePercatageF = t.CompletePercentage.ToString("N2")
                });
            return Task.FromResult(query);
        }
    }
}
