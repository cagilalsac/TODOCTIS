using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.TODO.Features.Todos
{
    public class TodoDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class TodoDeleteHandler : TodoDbHandler, IRequestHandler<TodoDeleteRequest, CommandResponse>
    {
        public TodoDeleteHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TodoDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Todos.Include(t => t.TodoTopics).SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (entity is null)
                return Error("Todo not found!");

            _db.TodoTopics.RemoveRange(entity.TodoTopics);

            _db.Todos.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return Success("Todo deleted successfully.", entity.Id);
        }
    }
}
