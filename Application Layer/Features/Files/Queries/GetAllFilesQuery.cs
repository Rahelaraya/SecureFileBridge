using Domain_Layer.Entities;
using MediatR;

namespace Application_Layer.Features.Files.Queries;

public record GetAllFilesQuery 
    
    
    : IRequest<IEnumerable<FileEntity>>;



