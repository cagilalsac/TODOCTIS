using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.TODO.Features.Topics
{
    public class TopicDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class TopicDeleteHandler : TodoDbHandler, IRequestHandler<TopicDeleteRequest, CommandResponse>
    {
        public TopicDeleteHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TopicDeleteRequest request, CancellationToken cancellationToken)
        {
            var entity = await _db.Topics.FindAsync(request.Id, cancellationToken);

            if (entity is null)
                return Error("Topic not found!");

            // Way 1:
            //_db.Remove(entity);

            // Way 2:
            _db.Topics.Remove(entity);

            await _db.SaveChangesAsync(cancellationToken);

            return Success("Topic deleted successfully.", entity.Id);
        }
    }
}
