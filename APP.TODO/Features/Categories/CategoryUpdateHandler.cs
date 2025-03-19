using APP.TODO.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.TODO.Features.Categories
{
    public class CategoryUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class CategoryUpdateHandler : TodoDbHandler, IRequestHandler<CategoryUpdateRequest, CommandResponse>
    {
        public CategoryUpdateHandler(TodoDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(CategoryUpdateRequest request, CancellationToken cancellationToken)
        {
            if (await _db.Categories.AnyAsync(c => c.Id != request.Id && c.Name.ToUpper() == request.Name.ToUpper().Trim(), cancellationToken))
                return Error("Category with the same name exists!");
            var entity = await _db.Categories.FindAsync(request.Id, cancellationToken);
            if (entity is null)
                return Error("Category not found!");
            entity.Name = request.Name?.Trim();
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Category updated successfully.", entity.Id);
        }
    }
}
