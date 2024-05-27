using Hackathon_2024_INFISOFTWARE.Domain.Models;

namespace Hackathon_2024_INFISOFTWARE.Domain.DTOs
{
    public class UpdateWorkflowRequest
    {
        public string WorkflowId { get; set; }
        public string NewStepName { get; set; }
        public string NewStepDescription { get; set; }
        public Etape NewStep { get; set; }
        public List<ChampRequis> NewStepChampsRequis { get; set; }
    }
}
