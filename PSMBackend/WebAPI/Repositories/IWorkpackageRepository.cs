using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Response;

namespace PSMWebAPI.Repositories;

public interface IWorkpackageRepository
{
    Task<PageResponse<WorkPackage>> GetWorkPackages(ComplainPaging complainPaging);
    Task<WorkPackage> GetByIdAsync(int id);
    Task<WorkPackage> AddWorkpackageAsync(WorkPackage workPackage);
    Task<WorkPackage> AddComplainAsync(LightPostComplint workPackage);
    Task<IEnumerable<WorkPackage>> GetWorkPackagesByTicketId(int ticketId);
    Task DeleteWorkpackageMappingByTicketId(int ticketId, int workpackageId);
    Task AddWorkpackageMappingByTicketId(int ticketId, int workpackageId);


    #region General Complains

    Task<WorkPackage> AddGeneralComplainAsync(GeneralComplain generalComplain);
    Task<IEnumerable<GeneralComplain>> GetGeneralComplain(GeneralComplainGetPagingRequest request);

    #endregion

    #region Light Post Complains

    Task<IEnumerable<LightPost>> GetLightPostsByCenterPoint(double latitude, double longitude);
    Task<IEnumerable<LightPostComplainDetail>> GetDetailLightPostComplintsByPostIdAndName(string postNo, string name);
    Task<IEnumerable<LightpostComplainSummary>> GetSummaryLightPostComplintsByPostId(string postNo);

    #endregion
}
