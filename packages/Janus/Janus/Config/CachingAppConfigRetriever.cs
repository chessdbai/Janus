// -----------------------------------------------------------------------
// <copyright file="CachingAppConfigRetriever.cs" company="ChessDB.AI">
// MIT Licensed.
// </copyright>
// -----------------------------------------------------------------------

namespace Janus.Config
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Amazon.AppConfig;
    using Amazon.AppConfig.Model;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// A CachingAppConfigRetriever class.
    /// </summary>
    public class CachingAppConfigRetriever : IConfigRetriever
    {
        private const string ConfigAppName = "Janus";
        private const string ConfigCacheKey = "config";
        private readonly IMemoryCache cache;
        private readonly IAmazonAppConfig appConfig;
        private readonly string stage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingAppConfigRetriever"/> class.
        /// </summary>
        /// <param name="cache">The memory cache.</param>
        /// <param name="appConfig">The AWS AppConfig client.</param>
        /// <param name="cloudConfig">The CloudConfig.</param>
        public CachingAppConfigRetriever(
            IMemoryCache cache,
            IAmazonAppConfig appConfig,
            CloudConfig cloudConfig)
        {
            this.cache = cache;
            this.appConfig = appConfig;
            this.stage = cloudConfig.Stage;
        }

        /// <inheritdoc cref="IConfigRetriever"/>
        public async Task<JanusConfig> RetrieveConfigAsync()
        {
            JanusConfig config;
            if (!this.cache.TryGetValue(ConfigCacheKey, out config))
            {
                config = await this.LoadConfigFromCloudAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(1));
                this.cache.Set(ConfigCacheKey, config, cacheEntryOptions);
            }

            return config;
        }

        private async Task<JanusConfig> LoadConfigFromCloudAsync()
        {
            var get = new GetConfigurationRequest()
            {
                Application = ConfigAppName,
                Environment = this.stage,
                Configuration = "OperationalSettings",
            };
            var response = await this.appConfig.GetConfigurationAsync(get);
            using var content = response.Content;
            using var reader = new StreamReader(content);
            string json = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<JanusConfig>(json);
        }
    }
}