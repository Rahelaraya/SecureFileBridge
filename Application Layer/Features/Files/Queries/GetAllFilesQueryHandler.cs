using Application_Layer.Interfaces;
using Domain_Layer.Entities;
using MediatR;

namespace Application_Layer.Features.Files.Queries;

/// <summary>
/// Handler för att hämta alla filer.
/// </summary>
public class GetAllFilesQueryHandler
    : IRequestHandler<GetAllFilesQuery, IEnumerable<FileEntity>>
{
    private readonly IFileRepository _fileRepository;

    public GetAllFilesQueryHandler(
        IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    /// <summary>
    /// Hämtar alla filer från repository.
    /// </summary>
    public Task<IEnumerable<FileEntity>> Handle(
        GetAllFilesQuery request,
        CancellationToken cancellationToken)
    {
        return _fileRepository.GetAllAsync();
    }
}