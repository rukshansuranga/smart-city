using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSMModel.Models;
using PSMWebAPI.DTOs.tender;
using PSMWebAPI.DTOs.Common;
using PSMWebAPI.Extensions;
using PSMWebAPI.Repositories;
using PSMWebAPI.Utils;

namespace PSMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenderController : ControllerBase
    {
        private readonly ITenderRepository _tenderRepository;
        private readonly IMapper _mapper;
        public TenderController(ITenderRepository tenderRepository, IMapper mapper)
        {
            _tenderRepository = tenderRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _tenderRepository.GetByIdAsync(id); // Calls service to fetch product by ID
                if (ticket == null)
                {
                    return this.ApiFailure<Tender>("Tender not found");
                }
                return this.ApiSuccess(ticket, "Tender retrieved successfully");
            }
            catch (KeyNotFoundException)
            {
                return this.ApiFailure<Tender>("Tender not found");
            }
        }

        [HttpGet("awardedTender/{id}")]
        public async Task<IActionResult> GetAwardedTenderByProjectId(int id)
        {
            try
            {
                var awardedTender = await _tenderRepository.GetAwardedTenderByProjectId(id); // Calls service to fetch product by ID
                if (awardedTender == null)
                {
                    return this.ApiFailure("Awarded tender not found");
                }
                return this.ApiSuccess(awardedTender, "Awarded tender retrieved successfully");
            }
            catch (KeyNotFoundException)
            {
                return this.ApiFailure("Awarded tender not found");
            }
        }

        [HttpGet("project/{projectId}")]
        // [Route("project")]
        public async Task<IActionResult> Get(int projectId)
        {
            if (projectId <= 0)
            {
                return this.ApiFailure("Invalid project ID");
            }

            var response = await _tenderRepository.GetTendersByProjectIdAsync(projectId); // Calls service to fetch product by ID
            return this.ApiSuccess(response, "Tenders retrieved successfully");
        }
        
        //generate action for create project
        [HttpPost]
        public async Task<IActionResult> Add(TenderPostRequest request)
        {
            if (request == null)
            {
                return this.ApiFailure<Tender>("Invalid request data");
            }

            try
            {
                //Check if tender already exists for this project and contractor
                var existingTender = await _tenderRepository.GetTenderByProjectIdAndContractorIdAsync(request.ProjectId, request.ContractorId);
                if (existingTender != null)
                {
                    return this.ApiFailure<Tender>("Tender for this project and contractor already exists");
                }

                Tender tender = _mapper.Map<Tender>(request);
                tender.SubmittedDate = PSMDateTime.Now; // Set the submitted date to current time

                var createdTender = await _tenderRepository.AddAsync(tender);
                return this.ApiSuccess(createdTender, "Tender created successfully");
            }
            catch (Exception ex)
            {
                return this.ApiFailure<Tender>("An error occurred while creating the tender", ex.Message);
            }
            
        }

        //generate action for update project
        [HttpPut("{id}")]       
        public async Task<IActionResult> Update(int id, TenderPostRequest request)
        {
            if (request == null || id <= 0)
            {
                return this.ApiFailure<Tender>("Invalid request data or ID");
            }

            var existingTender = await _tenderRepository.GetByIdAsync(id);
            if (existingTender == null)
            {
                return this.ApiFailure<Tender>("Tender not found");
            }

            _mapper.Map<TenderPostRequest,Tender>(request,existingTender);
            
            //_mapper.Map<Project, Project>(mapObject, existingProject);

            var updatedTender = await _tenderRepository.UpdateAsync(existingTender);
            return this.ApiSuccess(updatedTender, "Tender updated successfully");
        }

        //generate action for delete tender
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return this.ApiFailure("Invalid tender ID");
            }

            try
            {
                var existingTender = await _tenderRepository.GetByIdAsync(id);
                if (existingTender == null)
                {
                    return this.ApiFailure("Tender not found");
                }

                var isDeleted = await _tenderRepository.DeleteAsync(id);
                if (isDeleted)
                {
                    return this.ApiSuccess(true, "Tender deleted successfully");
                }
                else
                {
                    return this.ApiFailure("Failed to delete tender");
                }
            }
            catch (Exception ex)
            {
                return this.ApiFailure("An error occurred while deleting the tender", ex.Message);
            }
        }

    }
}
