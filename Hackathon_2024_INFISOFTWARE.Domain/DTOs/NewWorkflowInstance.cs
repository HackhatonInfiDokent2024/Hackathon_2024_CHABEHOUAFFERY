using Hackathon_2024_INFISOFTWARE.Domain.Models;

namespace Hackathon_2024_INFISOFTWARE.Domain.DTOs
{
    public class NewWorkflowInstance
    {
        public string Name { get; set; }
        public Dictionary<string, Etape> ProcessusDemandeFormation { get; set; }
    }
}
