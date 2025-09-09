using System;
using PSMModel.Models;

namespace PSMWebAPI.Repositories;

public interface IMiscRepository
{
    Task<IEnumerable<Contractor>> GetContractors();
    Task<Contractor?> GetContractorByIdAsync(string id);
}
