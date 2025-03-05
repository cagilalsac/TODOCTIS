using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.TODO.Features.Topics
{
    public class TopicCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }

    public class TopicCreateHandler : TodoDbHandler, IRequestHandler<TopicCreateRequest, CommandResponse>
    {
        public TopicCreateHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TopicCreateRequest request, CancellationToken cancellationToken)
        {
            // çağıl, ÇAĞIL, Çağıl, Çağıl     
            //if (_db.Topics.Any(t => t.Name.ToUpper() == request.Name.ToUpper().Trim()))
            if (await _db.Topics.AnyAsync(t => t.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Topic with the same name exists!");
            var entity = new Topic()
            {
                Name = request.Name.Trim()
            };
            _db.Topics.Add(entity);
            await _db.SaveChangesAsync(cancellationToken); // unit of work
            return Success("Topic created successfully.", entity.Id);
        }
    }
}
