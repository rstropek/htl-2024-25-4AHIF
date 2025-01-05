// We disable CA1416 because we use file locks that are not available on macOS.
// However, in this app, we do not need to support macOS.
#pragma warning disable CA1416 // Validate platform compatibility

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Registration.Api.DataAccess;

public interface IJsonFileRepository
{
    /// <summary>
    /// Creates a new entity in the repository.
    /// </summary>
    /// <typeparam name="T">The type of entity to create.</typeparam>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="entity">The entity to persist.</param>
    /// <returns>A new <see cref="Item"/> containing the generated ID and file path.</returns>
    Task<Item> Create<T>(Guid id, T entity);

    /// <summary>
    /// Enumerates all items in the repository.
    /// </summary>
    /// <returns>An enumerable of <see cref="Item"/> representing all stored entities.</returns>
    /// <remarks>
    /// This method is not async because .NET does not offer async methods
    /// to enumerate files in a directory. Such methods do not exist because the underlying
    /// operating systems do not provide them.
    /// </remarks>
    IEnumerable<Item> EnumerateAll();

    /// <summary>
    /// Opens a file stream for an entity with exclusive locking.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    /// A locked <see cref="FileStream"/> if the entity exists; otherwise, null.
    /// The caller is responsible for disposing the stream. <see cref="Update"/> 
    /// will dispose the stream after the update.
    /// </returns>
    FileStream? Open(Guid id);

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <typeparam name="T">The type of entity to retrieve.</typeparam>
    /// <param name="stream">The file stream obtained from <see cref="Open"/>.</param>
    /// <remarks>
    /// The stream must be obtained using the <see cref="Open"/> method to ensure proper locking.
    /// </remarks>
    /// <returns>
    /// The entity stored in the stream.
    /// </returns>
    Task<T?> Get<T>(FileStream stream);

    /// <summary>
    /// Updates an entity in the repository using the provided file stream.
    /// </summary>
    /// <typeparam name="T">The type of entity to update.</typeparam>
    /// <param name="stream">The file stream obtained from <see cref="Open"/>.</param>
    /// <param name="entity">The updated entity data.</param>
    /// <remarks>
    /// The stream must be obtained using the <see cref="Open"/> method to ensure proper locking.
    /// </remarks>
    Task Update<T>(FileStream stream, T entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    void Delete(Guid id);
}

/// <summary>
/// Provides file-based persistence using JSON files, with one file per entity.
/// Each entity is stored in a separate JSON file named with its GUID.
/// </summary>
/// <param name="folder">The directory path where JSON files will be stored.</param>
public class JsonFileRepository(string folder) : IJsonFileRepository
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <inheritdoc />
    public async Task<Item> Create<T>(Guid id, T entity)
    {
        var json = JsonSerializer.Serialize(entity, jsonSerializerOptions);
        var filePath = Path.Combine(folder, $"{id:N}.json"); // Use Guid as the file name
        await File.WriteAllTextAsync(filePath, json);
        return new Item(id, filePath);
    }

    /// <inheritdoc />
    public IEnumerable<Item> EnumerateAll()
        => Directory.EnumerateFiles(folder)
            .Select(file => new Item(Guid.ParseExact(Path.GetFileNameWithoutExtension(file), "N"), file));

    /// <inheritdoc />
    public FileStream? Open(Guid id)
    {
        var filePath = Path.Combine(folder, $"{id:N}.json");
        try
        {
            var stream = new FileStream(filePath, FileMode.Open);
            stream.Lock(0, stream.Length);
            return stream;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public async Task<T?> Get<T>(FileStream stream)
    {
        stream.Position = 0;
        return await JsonSerializer.DeserializeAsync<T>(stream, jsonSerializerOptions);
    }

    /// <inheritdoc />
    public async Task Update<T>(FileStream stream, T entity)
    {
        stream.Position = 0;
        stream.SetLength(0);
        await JsonSerializer.SerializeAsync(stream, entity, jsonSerializerOptions);
    }

    /// <inheritdoc />
    public void Delete(Guid id)
    {
        // Regarding async: Same problem as above in EnumerateAll.
        var filePath = Path.Combine(folder, $"{id:N}.json");
        File.Delete(filePath);
    }
}

/// <summary>
/// Represents a stored item in the repository with its identifier and file location.
/// </summary>
/// <param name="Id">The unique identifier of the item.</param>
/// <param name="FilePath">The full file path where the item is stored.</param>
public record Item(Guid Id, string FilePath);
