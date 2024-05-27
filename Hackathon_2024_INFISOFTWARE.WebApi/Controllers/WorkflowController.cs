using Hackathon_2024_INFISOFTWARE.Domain.DTOs;
using Hackathon_2024_INFISOFTWARE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon_2024_INFISOFTWARE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;


        public WorkflowsController(IWorkflowService workflowService, IEmailService emailService, IConfiguration configuration, ILogger<WorkflowsController> logger)
        {
            _workflowService = workflowService;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Loads a workflow from a file.
        /// </summary>
        [HttpGet("load")]
        public async Task<IActionResult> LoadWorkflow()
        {
            try
            {
                // Récupération du chemin du fichier à partir de la configuration
                var fileName = _configuration["WorkflowFilePath"];

                // Préparation du chemin complet du fichier
                var directory = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(directory, fileName);

                // Appel à la méthode LoadWorkflowAsync avec IConfiguration
                var workflow = await _workflowService.LoadWorkflowAsync(_configuration);

                return Ok(workflow);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        /// <summary>
        /// Creates a new workflow instance from JSON data.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> LoadWorkflowFromJson([FromBody] NewWorkflowInstance workflowInstance)
        {
            var result = await _workflowService.CreateWorkflowFromJson(workflowInstance);
            if (result)
            {
                return Ok("Workflow created successfully");
            }
            return BadRequest("Failed to create workflow");
        }

        /// <summary>
        /// Retrieves the state of a workflow by its ID.
        /// </summary>
        [HttpGet("state/{id}")]
        public async Task<IActionResult> GetWorkflowState(string id)
        {
            try
            {
                var workflow = await _workflowService.GetWorkflowStateById(id);
                return Ok(workflow);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing workflow.
        /// </summary>
        [HttpPost("update")]
        public async Task<IActionResult> UpdateWorkflow([FromBody] UpdateWorkflowRequest updateRequest)
        {
            try
            {
                var updated = await _workflowService.UpdateWorkflowAsync(updateRequest.WorkflowId, updateRequest);
                if (updated)
                {
                    return Ok("Workflow updated successfully");
                }
                return BadRequest("Failed to update workflow");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
