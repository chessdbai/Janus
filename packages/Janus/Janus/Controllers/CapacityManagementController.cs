//-----------------------------------------------------------------------
// <copyright file="CapacityManagementController.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
//-----------------------------------------------------------------------

namespace Janus.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Janus.Capacity.Containers;
    using Janus.Capacity.Servers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A controller for managing capacity.
    /// </summary>
    [ApiController]
    public class CapacityManagementController : ControllerBase
    {
        private readonly IServerManager serverManager;
        private readonly IContainerManager containerManager;
        private readonly ILogger<CapacityManagementController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapacityManagementController"/> class.
        /// </summary>
        /// <param name="serverManager">The server launcher.</param>
        /// <param name="containerManager">The container launcher.</param>
        /// <param name="logger">The logger.</param>
        public CapacityManagementController(
            IServerManager serverManager,
            IContainerManager containerManager,
            ILogger<CapacityManagementController> logger)
        {
            this.serverManager = serverManager;
            this.containerManager = containerManager;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the weather forecasts.
        /// </summary>
        /// <returns>The list of weather forecasts.</returns>
        [HttpPost]
        [Route("/capacity")]
        public async Task<IEnumerable<LaunchedCapacity>> PostAsync() => await this.serverManager
            .LaunchServersAsync(
                ServerType.CPU,
                ServerSize.Small,
                1);

        /// <summary>
        /// Gets the weather forecasts.
        /// </summary>
        /// <param name="instanceId">The instance id.</param>
        /// <returns>The list of weather forecasts.</returns>
        [HttpPut]
        [Route("/capacity/{instanceId}")]
        public async Task<string> PutAsync([FromRoute] string instanceId) => await this.containerManager
            .LaunchEngineContainerAsync(
                new ContainerScale()
                {
                    Type = ContainerType.CPU,
                    Cores = 1,
                    GPUs = 1,
                    MemoryMegabytes = 1024,
                },
                "3.137.191.148");
    }
}