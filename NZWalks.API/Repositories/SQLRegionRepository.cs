using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region); 
            await dbContext.SaveChangesAsync(); 
            return region;
        }

        public async Task<Region?> Delete(Guid Id)
        {
            var existingRegion = await GetByIdAsync(Id);

            if (existingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;

        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }


        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);   

        }

        public async Task<Region?> UpdateAsync(Guid Id, UpdateRegionRequestDto updateRegionRequest)

        {
            var regionDomainModel = await GetByIdAsync(Id);

            if (regionDomainModel == null)
            {
                return null;
            }

            regionDomainModel.Name = updateRegionRequest.Name;
            regionDomainModel.Code = updateRegionRequest.Code;
            regionDomainModel.RegionImageUrl = updateRegionRequest.RegionImageUrl;

             await dbContext.SaveChangesAsync();

            return regionDomainModel;
        }
    }
}
