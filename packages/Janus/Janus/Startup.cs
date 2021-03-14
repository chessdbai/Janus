//-----------------------------------------------------------------------
// <copyright file="Startup.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
//-----------------------------------------------------------------------

namespace Janus
{
    using Amazon;
    using Amazon.EC2;
    using Amazon.ECR;
    using Amazon.Extensions.NETCore.Setup;
    using Amazon.Runtime;
    using Amazon.Runtime.CredentialManagement;
    using Janus.Capacity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services required for dependency injection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultAWSOptions(CreateAWSOptions());
            services.AddAWSService<IAmazonEC2>();
            services.AddAWSService<IAmazonECR>();

            services.AddSingleton<IServerLauncher, EC2ServerLauncher>();
            services.AddSingleton<IContainerLauncher, DockerContainerLauncher>();

            services.AddControllers();
        }

        /// <summary>
        /// Set up the configured services.
        /// </summary>
        /// <param name="app">The app environment.</param>
        /// <param name="env">The web host environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static AWSOptions CreateAWSOptions()
        {
            AWSCredentials credentials;

            var chain = new CredentialProfileStoreChain();
            AWSCredentials awsCredentials;
            if (chain.TryGetAWSCredentials("chessdb-prod", out awsCredentials))
            {
                credentials = awsCredentials;
            }
            else
            {
                credentials = FallbackCredentialsFactory.GetCredentials();
            }

            return new AWSOptions()
            {
                Credentials = credentials,
                Region = RegionEndpoint.USEast2,
            };
        }
    }
}