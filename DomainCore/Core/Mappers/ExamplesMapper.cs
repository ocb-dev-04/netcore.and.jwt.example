using AutoMapper;
using DomainCore.Core.EntitiesDTO.App.Example;
using DomainCore.Data.Entities.App;

namespace DomainCore.Core.Mappers
{
    public class ExamplesMapper : Profile
    {
        #region Construct

        public ExamplesMapper()
        {
            CreateMap<CreateExampleDTO, Example>();
            CreateMap<UpdateExampleDTO, Example>();
            CreateMap<Example, ExampleDTO>();
        }

        #endregion
    }
}
