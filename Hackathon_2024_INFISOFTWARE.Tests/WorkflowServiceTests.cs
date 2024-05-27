using Hackathon_2024_INFISOFTWARE.Domain.Models;
using Hackathon_2024_INFISOFTWARE.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using Newtonsoft.Json;

namespace Hackathon_2024_INFISOFTWARE.Tests
{
    [TestClass]
    public class WorkflowServiceTests
    {
        private WorkflowService _workflowService;
        private Mock<IMongoCollection<Workflow>> _mockCollection;
        private Mock<IMongoDatabase> _mockDatabase;
        private IConfiguration _configuration;

        [TestInitialize]
        public void Initialize()
        {
            _mockCollection = new Mock<IMongoCollection<Workflow>>();
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockDatabase.Setup(x => x.GetCollection<Workflow>("Workflows", null)).Returns(_mockCollection.Object);

            var configuration = new ConfigurationBuilder()
              .AddInMemoryCollection(new[]
               {
                   new KeyValuePair<string, string>("WorkflowFilePathBase", "TestBasePath"),
                   new KeyValuePair<string, string>("WorkflowFilePath", "TestFilePath")
               })
              .Build();
            _configuration = configuration;

            var loggerMock = new Mock<ILogger<WorkflowService>>();
            _workflowService = new WorkflowService(_mockDatabase.Object, loggerMock.Object);
        }

        [TestMethod]
        public async Task LoadWorkflowAsync_Should_ReturnWorkflowDictionary()
        {
            // Arrange
            var jsonContent = "{\"WorkflowId\": \"123\", \"Name\": \"Test Workflow\"}";
            var expectedWorkflow = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonContent);

            // Act
            var result = await _workflowService.LoadWorkflowAsync(_configuration);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedWorkflow["WorkflowId"], result["WorkflowId"]);
            Assert.AreEqual(expectedWorkflow["Name"], result["Name"]);
        }
    }
}
