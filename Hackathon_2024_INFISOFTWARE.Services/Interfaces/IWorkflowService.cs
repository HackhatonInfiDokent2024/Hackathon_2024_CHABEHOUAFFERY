using Hackathon_2024_INFISOFTWARE.Domain.DTOs;
using Hackathon_2024_INFISOFTWARE.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Hackathon_2024_INFISOFTWARE.Services.Interfaces
{
    public interface IWorkflowService
    {
        Task<Dictionary<string, dynamic>> LoadWorkflowAsync(IConfiguration configuration);
        Task<bool> CreateWorkflowFromJson(NewWorkflowInstance workflowInstance);
        Task<Workflow> GetWorkflowStateById(string id);
        Task<bool> UpdateWorkflowAsync(string workflowId, UpdateWorkflowRequest updateRequest);
    }

}
