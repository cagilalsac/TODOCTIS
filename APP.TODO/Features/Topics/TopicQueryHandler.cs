using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.TODO.Features.Topics
{
    public class TopicQueryRequest : Request, IRequest<IQueryable<TopicQueryResponse>>
    {
    }

    public class TopicQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    // select Id, Name from Topics
    public class TopicQueryHandler : TodoDbHandler, IRequestHandler<TopicQueryRequest, IQueryable<TopicQueryResponse>>
    {
        public TopicQueryHandler(TodoDb db) : base(db)
        {
        }

        public Task<IQueryable<TopicQueryResponse>> Handle(TopicQueryRequest request, CancellationToken cancellationToken)
        {
            IQueryable<TopicQueryResponse> query = _db.Topics.OrderBy(t => t.Name).Select(t => new TopicQueryResponse()
            {
                Id = t.Id,
                Name = t.Name
            });
            return Task.FromResult(query);
        }
    }
}
