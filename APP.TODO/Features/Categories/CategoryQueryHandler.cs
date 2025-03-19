using APP.TODO.Domain;
using APP.TODO.Features.Todos;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.TODO.Features.Categories
{
    public class CategoryQueryRequest : Request, IRequest<IQueryable<CategoryQueryResponse>>
    {

    }

    public class CategoryQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string TodoTitles { get; set; }
        public List<string> TodoTitleList { get; set; }
        public int TodoCount { get; set; }
        public List<TodoQueryResponse> Todos { get; set; }
    }

    public class CategoryQueryHandler : TodoDbHandler, IRequestHandler<CategoryQueryRequest, IQueryable<CategoryQueryResponse>>
    {
        public CategoryQueryHandler(TodoDb db) : base(db)
        {
        }

        public Task<IQueryable<CategoryQueryResponse>> Handle(CategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Categories.Include(c => c.Todos).OrderBy(c => c.Name).Select(c => new CategoryQueryResponse()
            {
                Id = c.Id,
                Name = c.Name,
                TodoCount = c.Todos.Count,
                TodoTitles = string.Join(", ", c.Todos.Select(t => t.Title)),
                TodoTitleList = c.Todos.Select(t => t.Title).ToList(),
                Todos = c.Todos.Select(t => new TodoQueryResponse()
                {
                    Id = t.Id,
                    Title = t.Title
                }).ToList()
            });
            return Task.FromResult(query);
        }
    }
}
