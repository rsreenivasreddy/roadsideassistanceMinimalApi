using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RoadsideAssistance.Api.Data.DomianEntities;
using System.Linq.Expressions;

namespace RoadsideAssistance.Api.Data
{
    public interface IRepository<TEntity> where TEntity : DomainEntity
    {
        DbSet<TEntity> Entity { get; }
        IQueryable<TEntity> Queryable();
        Task<TEntity?> FindAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> entity, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> GetAllFromSql(string sqlQuery, CancellationToken cancellationToken = default, params SqlParameter[] sqlParameters);
        Task<TEntity> CreateAsync(TEntity entity,CancellationToken cancellationToken=default);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    }
}
