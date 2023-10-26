
using ETicaretAPI.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class StorageService : IStorageServices
    {
        readonly IStorage _storage;
        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public string StorageName { get => _storage.GetType().Name; }

        public Task DeleteAsync(string pathOrContainerName, string fileName)
        => _storage.DeleteAsync(pathOrContainerName, fileName);

        public List<string> GetFiles(string pathOrContainerName)
        => _storage.GetFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName)
        => _storage.HasFile(pathOrContainerName, fileName);

        public Task<List<(string fileName, string pathOrContainerName)>> UplaodAsync(string pathOrContainerName, IFormFileCollection files)
        => _storage.UplaodAsync(pathOrContainerName, files);
    }
}
