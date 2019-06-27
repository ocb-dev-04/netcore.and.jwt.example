using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainCore.Core.Interfaces;
using DomainCore.Data.Entities.App;
using DomainCore.Data.DataBaseContext;
using DomainCore.Core.EntitiesDTO.App.Example;

namespace DomainCore.Core.Reps.App
{
    public class ExampleRep : IExampleRep
    {
        #region Properties

        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Construct

        public ExampleRep(
            AppDbContext appDbContext,
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        #region Get's

        public async Task<IEnumerable<ExampleDTO>> GetAll()
            => await _appDbContext
                        .Example
                        .ProjectTo<ExampleDTO>(_mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<ExampleDTO> GetById(int id)
            => await _appDbContext
                        .Example
                        .ProjectTo<ExampleDTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(p => p.Id == id);

        #endregion

        #region CRUD

        public async Task<ExampleDTO> CreateAsync(CreateExampleDTO create)
        {
            #region Confirm

            // confirm if some register exist
            var confirm = await _appDbContext
                                    .Example
                                    .FirstOrDefaultAsync(p =>
                                        p.ProductoName == create.ProductoName &&
                                        p.Description == create.Description);
            if (confirm != null)
                throw new ArgumentNullException(nameof(confirm));

            #endregion

            var map = _mapper.Map<Example>(create);
            var add = await _appDbContext.Example.AddAsync(map);
            if(add == null)
                throw new ArgumentNullException(nameof(add));

            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<ExampleDTO>(add.Entity);
        }

        public async Task<bool> UpdateAsync(UpdateExampleDTO update)
        {
            #region ConfirmId

            // confirm if id exist
            var confirm = await _appDbContext
                                    .Example
                                    .FindAsync(update.Id);
            if (confirm == null)
                return false;

            #endregion

            // mapping and convert update (DTO) to confirm (Register)
            var map = _mapper.Map(update, confirm);

            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            #region ConfirmId

            // confirm if id exist
            var confirm = await _appDbContext
                                    .Example
                                    .FindAsync(id);
            if (confirm == null)
                return false;

            #endregion

            var delete = _appDbContext.Example.Remove(confirm);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        #endregion

        #endregion
    }
}
