using App.Data.Sqlite.Context;
using Microsoft.Extensions.Logging;

namespace App.Data.Sqlite.Repositories;

/// <summary>
/// Base class for all repositories.
/// </summary>
/// <param name="context"> The database context. </param>
/// <param name="logger"> The logger. </param>
/// <typeparam name="T"> The type of the repository. </typeparam>
public class RepositoryBase<T>(ISqliteDbContext context, ILogger<T> logger)
{
    /// <summary>
    /// Saves changes asynchronously to the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous save operation. The task result is a boolean value indicating whether the save operation was successful (true) or not (false).
    /// </returns>
    protected async Task<bool> SaveChangesAsync()
    {
        try
        {
            await context.Context.SaveChangesAsync();
        }catch (Exception e)
        {
            logger.LogError(e, "An error occurred while saving changes to the database");
            return false;
        }

        return true;
    }
}