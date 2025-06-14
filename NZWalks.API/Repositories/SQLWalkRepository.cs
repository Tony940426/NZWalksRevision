using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbcontext;

        public SQLWalkRepository(NZWalksDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbcontext.Walks.AddAsync(walk);
            await dbcontext.SaveChangesAsync();
            return walk;

        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            //It will get the relevant navigation property
             //return await dbcontext.Walks
             //   .Include("Difficulty")
             //   .Include("Region")
             //   .ToListAsync();

            var walks = dbcontext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            if (string.IsNullOrWhiteSpace(sortBy) == false) 
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            return await walks.ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbcontext.Walks.Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = await  GetByIdAsync(id);

            if(walkDomainModel == null)
            {
                return null;
            };

            walkDomainModel.Name = updateWalkRequestDto.Name;
            walkDomainModel.Description  = updateWalkRequestDto.Description;
            walkDomainModel.WalkImageUrl = updateWalkRequestDto.WalkImageUrl;
            walkDomainModel.DifficultyId = updateWalkRequestDto.DifficultyId;
            walkDomainModel.WalkImageUrl = updateWalkRequestDto.WalkImageUrl;
            walkDomainModel.RegionId = updateWalkRequestDto.RegionId;

            dbcontext.SaveChanges();
            return walkDomainModel;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkDomainModel = await GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return null;
            }

            dbcontext.Walks.Remove(walkDomainModel);
            await dbcontext.SaveChangesAsync();
            return walkDomainModel;
        }
    }
}   
