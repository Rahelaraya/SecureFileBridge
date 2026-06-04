using Application_Layer.Interfaces;
using Domain_Layer.Entities;
using Infrastructure_Layer.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure_Layer.Repositories;

/// <summary>
/// Repository för att läsa filer från filsystemet.
/// </summary>
public class FileRepository : IFileRepository
{
    private readonly FilePollingSettings _settings;
    private readonly ILogger<FileRepository> _logger;

    public FileRepository(
        IOptions<FilePollingSettings> options,
        ILogger<FileRepository> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// Hämtar alla filer från source path.
    /// </summary>
    public async Task<IEnumerable<FileEntity>> GetAllAsync()
    {
        
        _logger.LogInformation(
            "Reading files from path: {Path}",
            _settings.SourcePath);

        if (string.IsNullOrWhiteSpace(_settings.SourcePath))
        {
            _logger.LogWarning("SourcePath is empty");
            return [];
        }


        if (!Directory.Exists(_settings.SourcePath))
        {
            _logger.LogWarning(
                "Directory does not exist: {Path}",
                _settings.SourcePath);

            return [];
        }

      
        var files = Directory.GetFiles(
            _settings.SourcePath,
            _settings.FilePattern ?? "*.*");

     
        _logger.LogInformation(
            "Physical files found: {Count}",
            files.Length);

        var result = new List<FileEntity>();

        // Loopar igenom alla filer
        foreach (var filePath in files)
        {
            var fileInfo = new FileInfo(filePath);

            _logger.LogInformation(
                "Physical file: {File}",
                filePath);

            // Skapar FileEntity för varje fil
            result.Add(new FileEntity
            {
                FileName = fileInfo.Name,
                FilePath = fileInfo.FullName,
                FileSize = fileInfo.Length,

                // Konverterar filinnehåll till Base64
                Content = Convert.ToBase64String(
                    await File.ReadAllBytesAsync(filePath)),

                CreatedAt = fileInfo.CreationTimeUtc,
                ProcessedAt = null,
                ErrorMessage = null
            });
        }

        return result;
    }

    /// <summary>
    /// Hämtar en specifik fil baserat på filnamn.
    /// </summary>
    public async Task<FileEntity?> GetByNameAsync(string fileName)
    {
        var filePath = Path.Combine(
            _settings.SourcePath,
            fileName);

        // Kontrollera om filen finns
        if (!File.Exists(filePath))
        {
            _logger.LogWarning(
                "File not found: {FilePath}",
                filePath);

            return null;
        }

        try
        {
            var fileInfo = new FileInfo(filePath);

            var entity = new FileEntity
            {
                FileName = fileInfo.Name,
                FilePath = fileInfo.FullName,
                FileSize = fileInfo.Length,

                Content = Convert.ToBase64String(
                    await File.ReadAllBytesAsync(filePath)),

                CreatedAt = fileInfo.CreationTimeUtc
            };

            _logger.LogInformation(
                "File retrieved: {FileName}",
                fileName);

            return entity;
        }
        catch (Exception ex)
        {
            // Loggar fel vid läsning av fil
            _logger.LogError(
                ex,
                "Error reading file: {FileName}",
                fileName);

            return null;
        }
    }
}