using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_Layer.Entities;


namespace Application_Layer.Interfaces;

public interface IFileRepository
{
    Task<IEnumerable<FileEntity>> GetAllAsync();
    Task<FileEntity?> GetByNameAsync(string fileName);
}