using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Expressions;
using PSMModel.Models;
using PSMWebAPI.DTOs.Project;
using PSMWebAPI.Repositories;

namespace PSMWebAPI.Controllers
{
    // ...removed [AllowAnonymous] to enforce authentication...
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;
        public ProjectController(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ticket = await _projectRepository.GetByIdAsync(id); // Calls service to fetch product by ID
                return Ok(ticket); // Returns 200 OK response if found
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // Returns 404 Not Found if product does not exist
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProjectPaging paging)
        {
            var response = await _projectRepository.GetPagingAsync(paging); // Calls service to fetch product by ID
            return Ok(response); // Returns 200 OK response if found

        }

        //generate action for create project
        [HttpPost]
        public async Task<IActionResult> Add(ProjectPostRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            Project project = _mapper.Map<Project>(request);

            var createdProject = await _projectRepository.AddAsync(project);
            return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
        }

        //generate action for update project
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjectPostRequest request)
        {
            if (request == null || id <= 0)
            {
                return BadRequest();
            }

            var existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
            {
                return NotFound();
            }

            _mapper.Map<ProjectPostRequest, Project>(request, existingProject);

            //_mapper.Map<Project, Project>(mapObject, existingProject);

            var updatedProject = await _projectRepository.UpdateAsync(existingProject);
            return Ok(updatedProject);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProjects()
        {
            var response = await _projectRepository.GetAllProjects(); // Calls service to fetch product by ID
            return Ok(response); // Returns 200 OK response if found

        }

        [HttpPost]
        [Route("filter")]
        public async Task<IActionResult> GetProjectByTypeAndStatusAndName(ProjectQuery query)
        {
            var response = await _projectRepository.GetProjectByTypeAndStatusAndName(query.Type, query.Status, query.Name, query.City, query.IsRecent);
            return Ok(response); // Returns 200 OK response if found
        }

    }

    public class ProjectQuery
    {
        public ProjectType? Type { get; set; }
        public ProjectStatus? Status { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public bool IsRecent { get; set; }
    }
}
