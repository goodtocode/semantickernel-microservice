using Goodtocode.SemanticKernel.Core.Domain.Author;
using Microsoft.Extensions.Logging;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;

public class SemanticKernelContextInitializer(ILogger<SemanticKernelContextInitializer> logger, SemanticKernelContext context)
{
    private readonly SemanticKernelContext _context = context;
    private readonly ILogger<SemanticKernelContextInitializer> _logger = logger;

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer()) await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (!_context.Authors.Any())
        {
            var Author = new AuthorEntity()
            {
                Id = Guid.NewGuid(),
                Name = "John Doe"
            };
            _context.Authors.Add(Author);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}