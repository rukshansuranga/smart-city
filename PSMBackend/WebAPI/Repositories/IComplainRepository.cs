using System;
using PSMModel.Enums;
using PSMModel.Models;
using PSMWebAPI.DTOs;
using PSMWebAPI.DTOs.Request;
using PSMWebAPI.DTOs.Response;
using PSMWebAPI.DTOs.Complain;
using PSMWebAPI.DTOs.Complain.LightPostComplain;

namespace PSMWebAPI.Repositories;

public interface IComplainRepository
{
    Task<PageResponse<Complain>> GetComplains(ComplainPaging complainPaging);
    Task<T> GetByIdAsync<T>(int id) where T : class;
    Task<T> AddComplainAsync<T>(T complain) where T : Complain;
    Task<T> UpdateComplainAsync<T>(T complain) where T : Complain;
    Task<IEnumerable<Complain>> GetComplainsByTicketId(int ticketId);
    Task DeleteComplainMappingByTicketId(int ticketId, int ComplainId);
    Task AddComplainMappingByTicketId(int ticketId, int ComplainId);

    #region General Complains
    //Task<Complain> UpdateGeneralComplainAsync(GeneralComplain generalComplain);
    Task<IEnumerable<GeneralComplain>> GetGeneralComplain(GeneralComplainGetPagingRequest request);
    #endregion

    #region Light Post Complains
    Task<IEnumerable<LightPost>> GetLightPostsByCenterPoint(double latitude, double longitude);
    Task<IEnumerable<LightPostComplainDetail>> GetDetailLightPostComplintsByPostIdAndName(string postNo, string name, string FirstName);
    Task<IEnumerable<LightpostComplainSummary>> GetSummaryLightPostComplintsByPostId(string postNo);
    Task<IEnumerable<LightPostComplain>> GetLightPostActiveComplainsByMe(string postNo);
    Task<IEnumerable<ActiveLightPostMarkers>> GetActiveLightPostList();
    Task<IEnumerable<ActiveLightPostMarkers>> GetActiveAndAssignedLightPostList();
    Task<IEnumerable<ActiveLightPostMarkers>> GetLightPostListByLocation(double latitude, double longitude, int[] statuses);
    #endregion

    #region Project Complains
    Task<IEnumerable<ProjectComplain>> GetProjectComplainsByProjectId(int projectId);
    Task<ProjectComplain> GetProjectComplainByComplainId(int ComplainId);
    #endregion

}
