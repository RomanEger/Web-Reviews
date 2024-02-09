using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Repository;
using Service.Contracts;
using Service.Helpers;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VideoStatusesService : GenericService<Videostatus>, IVideoStatusesService
    {
        private readonly IGenericService<Videostatus>.CheckEntityAndGetIfItExist checkVideoStatusAndGet;
        public VideoStatusesService(IRepositoryManager repositoryManager, IMapper mapper, EntityChecker entityChecker)
            : base(repositoryManager, mapper)
        {
            checkVideoStatusAndGet = entityChecker.CheckVideoStatusAndGetIfItExist;
        }

        public async Task<ReferenceDTO> CreateVideoStatusAsync(ReferenceForManipulationDTO manipulationDTO) =>
            await CreateAsync<ReferenceForManipulationDTO, ReferenceDTO>(manipulationDTO);

        public async Task DeleteVideoStatusAsync(Guid videoStatusId, bool trackChanges) =>
            await DeleteAsync(videoStatusId, trackChanges, checkVideoStatusAndGet);

        public async Task<ReferenceDTO> GetVideoStatusByIdAsync(Guid videoStatusId, bool trackChanges) =>
            await GetByIdAsync<ReferenceDTO>(videoStatusId, trackChanges, checkVideoStatusAndGet);

        public async Task<IEnumerable<ReferenceDTO>> GetVideoStatusesAsync(bool trackChanges) =>
            await GetAllAsync<ReferenceDTO>(trackChanges);

        public async Task<ReferenceDTO> UpdateVideoStatus(Guid videoStatusId, ReferenceForManipulationDTO manipulationDTO, bool trackChanges) =>
            await UpdateAsync<ReferenceForManipulationDTO, ReferenceDTO>(videoStatusId, manipulationDTO, trackChanges, checkVideoStatusAndGet);

    }
}
