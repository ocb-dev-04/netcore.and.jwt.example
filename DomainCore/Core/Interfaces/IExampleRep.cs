using System.Threading.Tasks;
using System.Collections.Generic;
using DomainCore.Core.EntitiesDTO.App.Example;

namespace DomainCore.Core.Interfaces
{
    public interface IExampleRep
    {
        #region Methods

        #region Get's

        Task<IEnumerable<ExampleDTO>> GetAll();
        Task<ExampleDTO> GetById(int id);

        #endregion

        #region CRUD

        Task<ExampleDTO> CreateAsync(CreateExampleDTO create);
        Task<bool> UpdateAsync(UpdateExampleDTO update);
        Task<bool> DeleteAsync(int id);

        #endregion

        #endregion
    }
}
