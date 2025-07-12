using System;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface IRegionRepository
{
    Task<IEnumerable<Region>> GetRegions();
}
