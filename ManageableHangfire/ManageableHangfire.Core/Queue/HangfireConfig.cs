using Hangfire;
using Hangfire.SqlServer;
using ManageableHangfire.Core.Settings;
using System;
using System.Collections.Generic;

namespace ManageableHangfire.Core.Queue
{
    public static class HangfireConfig
    {
        private static BackgroundJobServer _managerJobServer;
        private static List<BackgroundJobServer> _backgroundJobServers;

        public static bool IsRunning { get; set; }

        public static void RegisterQueueJobServers()
        {
            var settings = SettingsManager.CurrentSettings;
            JobStorage.Current = new SqlServerStorage(settings.ConnectionString);

            _backgroundJobServers = new List<BackgroundJobServer>();
            settings.Queues
                .ForEach(q =>
                {
                    var options = new BackgroundJobServerOptions { Queues = new string[] { q.Name.ToLower() }, WorkerCount = q.Workers };
                    var bgJobServer = new BackgroundJobServer(options);
                    _backgroundJobServers.Add(bgJobServer);
                });

            IsRunning = true;
        }

        public static void RegisterManagerJobServer()
        {
            JobStorage.Current = new SqlServerStorage(SettingsManager.CurrentSettings.ConnectionString);
            var options = new BackgroundJobServerOptions { Queues = new string[] { QueueConstants.MANAGER }, WorkerCount = 1 };
            _managerJobServer = new BackgroundJobServer(options);

            RecurringJob.AddOrUpdate(() => HangfireQueueManager.ManageHangfire(), Cron.MinuteInterval(1), queue: QueueConstants.MANAGER);

            // Executes for the first time to initialize the job servers if needed
            HangfireQueueManager.ManageHangfire();
        }

        public static void DisposeQueueJobServers()
        {
            _backgroundJobServers.ForEach(j => j.Dispose());
            IsRunning = false;
        }

        public static IGlobalConfiguration<SqlServerStorage> GetServiceConfiguration(IGlobalConfiguration configuration)
        {
            var settings = SettingsManager.CurrentSettings;
            return configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(settings.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                });
        }
    }
}
