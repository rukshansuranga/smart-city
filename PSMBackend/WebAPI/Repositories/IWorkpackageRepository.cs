using System;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Response;

namespace PSMWebAPI.Repositories;

public interface IWorkpackageRepository
{
    Task<PageResponse<Workpackage>> GetWorkpackages(ComplainPaging complainPaging);
    Task<T> GetByIdAsync<T>(int id) where T : class;
    Task<T> AddWorkpackageAsync<T>(T workPackage) where T : class;
    Task<T> UpdateWorkpackageAsync<T>(T workpackage) where T : class;
    Task<IEnumerable<Workpackage>> GetWorkpackagesByTicketId(int ticketId);
    Task DeleteWorkpackageMappingByTicketId(int ticketId, int WorkpackageId);
    Task AddWorkpackageMappingByTicketId(int ticketId, int WorkpackageId);


    #region General Complains

    
    //Task<Workpackage> UpdateGeneralComplainAsync(GeneralComplain generalComplain);
    Task<IEnumerable<GeneralComplain>> GetGeneralComplain(GeneralComplainGetPagingRequest request);

    #endregion

    #region Light Post Complains

    Task<IEnumerable<LightPost>> GetLightPostsByCenterPoint(double latitude, double longitude);
    Task<IEnumerable<LightPostComplainDetail>> GetDetailLightPostComplintsByPostIdAndName(string postNo, string name);
    Task<IEnumerable<LightpostComplainSummary>> GetSummaryLightPostComplintsByPostId(string postNo);

    #endregion

    #region Project Complains

    Task<IEnumerable<ProjectComplain>> GetProjectComplainsByProjectId(int projectId);
    
    Task<ProjectComplain> GetProjectComplainByWorkpackageId(int WorkpackageId);

    #endregion

}
