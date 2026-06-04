using Domain_Layer.Entities;

namespace Application_Layer.Interfaces;

public interface IMessagePublisher
{
 
    Task PublishFileMessageAsync(FileEntity file);
}