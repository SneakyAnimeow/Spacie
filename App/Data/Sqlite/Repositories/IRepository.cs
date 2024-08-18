namespace App.Data.Sqlite.Repositories;

/// <summary>
/// Represents a generic repository for CRUD operations.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="entity">The entity to be added.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task representing the asynchronous update operation. The task result is the updated entity.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity asynchronously from the database.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the asynchronous delete operation. The task result is a boolean value indicating whether the delete operation was successful (true) or not (false).</returns>
    Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// Retrieves an entity by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the entity with the specified ID.</returns>
    ValueTask<T?> GetByIdAsync(int id);
    
    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all entities.</returns>
    Task<List<T>> GetAllAsync();
}