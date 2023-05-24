using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RoadsideAssistance.Api.Data.DomianEntities;
using System.Linq.Expressions;

namespace RoadsideAssistance.Api.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : DomainEntity
    {
        private readonly DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public DbSet<TEntity> Entity
        {
            get
            {
                return _dataContext.Set<TEntity>();
            }
        }
        public IQueryable<TEntity> Queryable()
        {
            return _dataContext.Set<TEntity>().AsQueryable();
        }

        //Find a Specific Record with Id
        public async Task<TEntity?> FindAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dataContext.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        
        /// <summary>
        /// Get All Data with expression filter
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default)
        {
            var entityQuery = _dataContext.Set<TEntity>().AsQueryable();
            if(expression != null)
            {
                entityQuery = entityQuery.Where(expression);
            }
            return await entityQuery.ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<TEntity>> GetAllFromSql(string sqlQuery, CancellationToken cancellationToken = default, params SqlParameter[] sqlParameters)
        {
            var entity = _dataContext.Set<TEntity>();
            if (sqlParameters != null && sqlParameters.Any())
            {
                //var paramNames = sqlParameters.Select(p=>p.ParameterName).ToArray();
                //sqlQuery +=$"{string.Join(",",paramNames)}";

                return await entity.FromSqlRaw(sqlQuery, sqlParameters).ToListAsync();
            }

            return await entity.FromSqlRaw(sqlQuery).ToListAsync();
        }
        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var createdEntity = _dataContext.Set<TEntity>().Add(entity).Entity;
            await _dataContext.SaveChangesAsync(cancellationToken);
            return createdEntity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync(cancellationToken);
            return _dataContext.Entry(entity).Entity;
        }

        public async  Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dataContext.Entry(entity).State = EntityState.Deleted;
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

    }
}
