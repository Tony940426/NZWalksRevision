using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? IsAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModels = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, IsAscending ?? true, pageNumber, pageSize);

            var walksDto = mapper.Map<List<WalkDto>>(walksDomainModels);

            return Ok(walksDto);
        }

        [HttpGet("{id}", Name = "GetWalkByIdAsync")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateAsync([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            var walkDto = mapper.Map<WalkDto>(walkDomainModel);

            return CreatedAtRoute("GetWalkByIdAsync", new { id = walkDto.Id }, walkDto);
        }

        [HttpPut]
        [Route("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = await walkRepository.UpdateAsync(id, updateWalkRequestDto);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete (Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDomainDto = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDomainDto);
        }
    }
}
