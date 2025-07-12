using System;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface IGarbageRepository
{
    Task<IEnumerable<GCShedule>> GetGCSheduleByRegion(string regionNo);
}
