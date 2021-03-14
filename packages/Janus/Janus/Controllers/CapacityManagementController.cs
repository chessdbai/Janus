//-----------------------------------------------------------------------
// <copyright file="CapacityManagementController.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
//-----------------------------------------------------------------------

namespace Janus.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Janus.Capacity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A controller for managing capacity.
    /// </summary>
    [ApiController]
    public class CapacityManagementController : ControllerBase
    {
        private readonly IServerLauncher serverLauncher;
        private readonly IContainerLauncher containerLauncher;
        private readonly ILogger<CapacityManagementController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapacityManagementController"/> class.
        /// </summary>
        /// <param name="serverLauncher">The server launcher.</param>
        /// <param name="containerLauncher">The container launcher.</param>
        /// <param name="logger">The logger.</param>
        public CapacityManagementController(
            IServerLauncher serverLauncher,
            IContainerLauncher containerLauncher,
            ILogger<CapacityManagementController> logger)
        {
            this.serverLauncher = serverLauncher;
            this.containerLauncher = containerLauncher;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the weather forecasts.
        /// </summary>
        /// <returns>The list of weather forecasts.</returns>
        [HttpPost]
        [Route("/capacity")]
        public async Task<IEnumerable<LaunchedCapacity>> PostAsync() => await this.serverLauncher
            .LaunchServersAsync(
                ServerType.CPU,
                ServerSize.Small,
                1);

        /// <summary>
        /// Gets the weather forecasts.
        /// </summary>
        /// <returns>The list of weather forecasts.</returns>
        [HttpPut]
        [Route("/capacity")]
        public async Task<string> PutAsync() => await this.containerLauncher
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