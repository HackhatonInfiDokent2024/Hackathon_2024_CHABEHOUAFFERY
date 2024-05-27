using Hackathon_2024_INFISOFTWARE.Domain.DTOs;
using Hackathon_2024_INFISOFTWARE.Domain.Models;
using Hackathon_2024_INFISOFTWARE.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Hackathon_2024_INFISOFTWARE.Services.Implementations
{
    public class WorkflowService : IWorkflowService
    {

        private readonly IMongoCollection<Workflow> _workflows;
        private readonly ILogger<WorkflowService> _logger;

        public WorkflowService(IMongoDatabase database, ILogger<WorkflowService> logger)
        {
            _workflows = database.GetCollection<Workflow>("Workflows");
            _logger = logger;
        }

        public async Task<Dictionary<string, dynamic>> LoadWorkflowAsync(IConfiguration configuration)
        {
            // Récupération des valeurs de configuration directement depuis IConfiguration
            var workflowFilePathBase = configuration["WorkflowFilePathBase"];
            var fileName = configuration["WorkflowFilePath"];

            // Construction du chemin complet du fichier
            var directory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(directory, workflowFilePathBase, fileName);

            try
            {
                // Lecture du contenu du fichier
                var jsonContent = await File.ReadAllTextAsync(filePath);

                // Désérialisation du contenu JSON en Dictionary
                var workflow = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonContent);

                return workflow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la lecture du fichier JSON de workflow.");
                throw;
            }
        }

        public async Task<bool> CreateWorkflowFromJson(NewWorkflowInstance workflowInstance)
        {
            try
            {
                var workflow = new Workflow
                {
                    Name = workflowInstance.Name,
                    ProcessusDemandeFormation = workflowInstance.ProcessusDemandeFormation
                };

                await _workflows.InsertOneAsync(workflow);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating workflow from JSON");
                return false;
            }
        }

        public async Task<Workflow> GetWorkflowStateById(string id)
        {
            var filter = Builders<Workflow>.Filter.Eq(w => w.Id, id);
            var workflow = await _workflows.Find(filter).FirstOrDefaultAsync();

            if (workflow == null)
            {
                throw new Exception($"Workflow with ID {id} not found.");
            }

            return workflow;
        }


        public async Task<bool> UpdateWorkflowAsync(string workflowId, UpdateWorkflowRequest updateRequest)
        {
            try
            {
                var filter = Builders<Workflow>.Filter.Eq(w => w.Id, workflowId);

                // Supposons que "newStepName" est unique et représente l'identifiant de l'étape à ajouter ou mettre à jour
                var stepKey = updateRequest.NewStepName;

                // Construire l'objet étape à insérer ou mettre à jour
                var newStep = new Etape
                {
                    Nom = updateRequest.NewStep.Nom,
                    Description = updateRequest.NewStep.Description,
                    ChampsRequis = updateRequest.NewStepChampsRequis,
                    Participants = updateRequest.NewStep.Participants,
                    ActionsPossibles = updateRequest.NewStep.ActionsPossibles
                };

                // Utiliser $set pour ajouter ou mettre à jour l'étape spécifiée
                var updateOperation = Builders<Workflow>.Update
                   .Set(w => w.ProcessusDemandeFormation[stepKey], newStep);

                var result = await _workflows.UpdateOneAsync(filter, updateOperation);

                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow");
                return false;
            }
        }

    }
}
