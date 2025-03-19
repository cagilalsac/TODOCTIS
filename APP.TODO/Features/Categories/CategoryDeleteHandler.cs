using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.TODO.Features.Categories
{
    public class CategoryDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class CategoryDeleteHandler : TodoDbHandler, IRequestHandler<CategoryDeleteRequest, CommandResponse>
    {
        public CategoryDeleteHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(CategoryDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Categories.Include(c => c.Todos).SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (entity is null)
                return Error("Category not found!");
            if (entity.Todos.Any())
                return Error("Category can't be deleted because it has relational todos!");
            _db.Categories.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Category deleted successfully.", entity.Id);
        }
    }
}
