using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.tender;

namespace PSMWebAPI.Repositories;

public interface ITenderRepository
{
    Task<Tender> AddAsync(Tender tender);
    Task<Tender> UpdateAsync(Tender tender);
    Task<Tender?> GetByIdAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Tender>> GetTendersByProjectIdAsync(int projectId);
    Task<Tender?> GetAwardedTenderByProjectId(int id);
    Task<Tender?> GetTenderByProjectIdAndContractorIdAsync(int projectId, string contractorId);
}