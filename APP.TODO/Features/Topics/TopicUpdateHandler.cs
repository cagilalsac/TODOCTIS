using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.TODO.Features.Topics
{
    public class TopicUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }

    public class TopicUpdateHandler : TodoDbHandler, IRequestHandler<TopicUpdateRequest, CommandResponse>
    {
        public TopicUpdateHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TopicUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Topics.AnyAsync(t => t.Id != request.Id && t.Name.ToLower() == request.Name.ToLower().Trim(), cancellationToken))
                return Error("Topic with the same name exitsts!");

            // Way 1: not recommended
            //var entity = new Topic()
            //{
            //    Id = request.Id,
            //    Name = request.Name.Trim()
            //};
            // Way 2:
            //var entity = await _db.Topics.SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            // Way 3:
            var entity = await _db.Topics.FindAsync(request.Id, cancellationToken);

            if (entity is null)
                return Error("Topic not found!");

            entity.Name = request.Name.Trim();

            // Way 1:
            //_db.Update(entity);
            // Way 2:
            _db.Topics.Update(entity);

            await _db.SaveChangesAsync(cancellationToken);
            return Success("Topic updated successfully.", entity.Id);
        }
    }
}
