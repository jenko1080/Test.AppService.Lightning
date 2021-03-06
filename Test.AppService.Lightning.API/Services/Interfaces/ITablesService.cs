using Test.AppService.Lightning.API.Models;

namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface ITablesService
    {
        Task<bool> AddToTable<T>(T entity) where T : TableEntityBase;
    }
}