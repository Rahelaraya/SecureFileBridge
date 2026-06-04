using Application_Layer.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Service för att läsa filer från nätverksmapp.
/// </summary>
public class FileReaderService : IFileReaderService
{
    private readonly ILogger<FileReaderService> _logger;

    public FileReaderService(
        ILogger<FileReaderService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Läser alla filer från nätverksmappen
    /// och returnerar innehållet.
    /// </summary>
    public async Task<List<string>> ReadFilesAsync()
    {
        var files = new List<string>();

        try
        {
            // Path till nätverksmappen
            var networkPath = @"\\secure-network\shared";

            // Kontrollera om mappen finns
            if (!Directory.Exists(networkPath))
            {
                _logger.LogWarning(
                    "Network path does not exist: {NetworkPath}",
                    networkPath);

                return files;
            }

            // Hämtar alla filer från mappen
            var fileEntries = Directory.GetFiles(networkPath);

            // Loopar igenom alla filer
            foreach (var file in fileEntries)
            {
                try
                {
                    var fileInfo = new FileInfo(file);

                    // Läser innehållet från filen
                    var content =
                        await File.ReadAllTextAsync(file);

                    // Lägger till filinnehållet i listan
                    files.Add(content);

                    _logger.LogInformation(
                        "File read successfully: {FileName} ({Size} bytes)",
                        fileInfo.Name,
                        fileInfo.Length);
                }
                catch (Exception ex)
                {
                    // Loggar fel för specifik fil
                    _logger.LogError(
                        ex,
                        "Error reading file: {FilePath}",
                        file);
                }
            }
        }
        catch (Exception ex)
        {
            // Loggar generella fel vid läsning
            _logger.LogError(
                ex,
                "Error reading files from network");
        }

        return files;
    }
}