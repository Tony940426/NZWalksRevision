using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using System.Numerics;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>>GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);

        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> UpdateAsync(Guid id, UpdateWalkRequestDto updateWalkRequestDto);

        Task<Walk?> DeleteAsync(Guid id);

    };
}
