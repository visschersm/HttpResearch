using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MTech.HttpResearch.Entities;

namespace MTech.HttpResearch.DataModel
{
    public interface ITodoContext
    {
        DbSet<TodoItem> TodoItems { get; set; }

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}