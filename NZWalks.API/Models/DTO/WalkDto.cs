﻿using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        //Navigation Property
        public required RegionDto Region { get; set; }

        public required DifficultyDto Difficulty { get; set; }
    }
}
