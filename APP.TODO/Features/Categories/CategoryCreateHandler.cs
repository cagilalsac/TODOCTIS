using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APP.TODO.Features.Categories
{
    public class CategoryCreateRequest : Request, IRequest<CommandResponse>
    {
        [JsonIgnore]
        public override int Id { get => base.Id; set => base.Id = value; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class CategoryCreateHandler : TodoDbHandler, IRequestHandler<CategoryCreateRequest, CommandResponse>
    {
        public CategoryCreateHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Categories.AnyAsync(c => c.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Category with the same name exists!");
            var entity = new Category()
            {
                Name = request.Name?.Trim()
            };
            _db.Categories.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Category created successfully.", entity.Id);
        }
    }
}
