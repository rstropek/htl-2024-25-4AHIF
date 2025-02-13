using Registration.Api.DataAccess;
using System.Text.Json;

namespace Registration.Tests.DataAccess;

public class JsonFileRepositoryTests : IDisposable
{
    private readonly string testFolder;
    private readonly JsonFileRepository repository;
    private readonly RepositorySettings settings;

    public JsonFileRepositoryTests()
    {
        testFolder = Path.Combine(Path.GetTempPath(), $"JsonFileRepositoryTests_{Guid.NewGuid():N}");
        Directory.CreateDirectory(testFolder);
        settings = new RepositorySettings
        {
            DataFolder = testFolder,
            NumberOfRetries = 3,
            RetryDelayMilliseconds = 5
        };
        repository = new JsonFileRepository(settings);
    }

    public void Dispose()
    {
        if (Directory.Exists(testFolder))
        {
            Directory.Delete(testFolder, recursive: true);
        }
    }

    [Fact]
    public async Task Create_ShouldPersistEntity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var testEntity = new TestEntity { Name = "Test", Value = 42 };

        // Act
        var item = await repository.Create(id, testEntity);

        // Assert
        Assert.Equal(id, item.Id);
        Assert.True(File.Exists(item.FilePath));
        var json = await File.ReadAllTextAsync(item.FilePath);
        var deserializedEntity = JsonSerializer.Deserialize<TestEntity>(json, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        Assert.Equal(testEntity.Name, deserializedEntity?.Name);
        Assert.Equal(testEntity.Value, deserializedEntity?.Value);
    }

    [Fact]
    public async Task Get_ShouldRetrieveEntity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var testEntity = new TestEntity { Name = "Test", Value = 42 };
        await repository.Create(id, testEntity);

        // Act
        await using var stream = await repository.Open(id, false);
        var retrievedEntity = await repository.Get<TestEntity>(stream!);

        // Assert
        Assert.NotNull(retrievedEntity);
        Assert.Equal(testEntity.Name, retrievedEntity.Name);
        Assert.Equal(testEntity.Value, retrievedEntity.Value);
    }

    [Fact]
    public async Task Update_ShouldModifyEntity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var testEntity = new TestEntity { Name = "Test", Value = 42 };
        await repository.Create(id, testEntity);

        // Act
        await using var stream = await repository.Open(id, true);
        var updatedEntity = new TestEntity { Name = "Updated", Value = 100 };
        await repository.Update(stream!, updatedEntity);
        stream!.Close();

        // Verify
        await using var verifyStream = await repository.Open(id, false);
        var retrievedEntity = await repository.Get<TestEntity>(verifyStream!);

        // Assert
        Assert.NotNull(retrievedEntity);
        Assert.Equal(updatedEntity.Name, retrievedEntity.Name);
        Assert.Equal(updatedEntity.Value, retrievedEntity.Value);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntity()
    {
        // Arrange
        var id = Guid.NewGuid();
        var testEntity = new TestEntity { Name = "Test", Value = 42 };
        var item = await repository.Create(id, testEntity);

        // Act
        repository.Delete(id);

        // Assert
        Assert.False(File.Exists(item.FilePath));
    }

    [Fact]
    public async Task EnumerateAll_ShouldListAllEntities()
    {
        // Arrange
        var entities = new List<(Guid Id, TestEntity Entity)>
        {
            (Guid.NewGuid(), new TestEntity { Name = "Test1", Value = 1 }),
            (Guid.NewGuid(), new TestEntity { Name = "Test2", Value = 2 }),
            (Guid.NewGuid(), new TestEntity { Name = "Test3", Value = 3 })
        };

        foreach (var (id, entity) in entities)
        {
            await repository.Create(id, entity);
        }

        // Act
        var items = repository.EnumerateAll().ToList();

        // Assert
        Assert.Equal(entities.Count, items.Count);
        foreach (var (id, _) in entities)
        {
            Assert.Contains(items, item => item.Id == id);
        }
    }

    [Fact]
    public async Task Open_NonexistentEntity_ShouldReturnNull()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var stream = await repository.Open(id, false);

        // Assert
        Assert.Null(stream);
    }

    [Fact]
    public async Task Open_WithWriteLock_ShouldPreventConcurrentAccess()
    {
        // Arrange
        var id = Guid.NewGuid();
        var testEntity = new TestEntity { Name = "Test", Value = 42 };
        await repository.Create(id, testEntity);

        // Act
        await using var firstStream = await repository.Open(id, true);
        var secondStream = await repository.Open(id, true);

        // Assert
        Assert.NotNull(firstStream);
        Assert.Null(secondStream); // Second attempt should fail due to file lock
    }
}

public class TestEntity
{
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
}
