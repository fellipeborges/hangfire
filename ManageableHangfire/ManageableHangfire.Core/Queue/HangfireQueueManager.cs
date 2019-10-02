using Hangfire;
using ManageableHangfire.Core.Settings;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ManageableHangfire.Core.Queue
{
    public static class HangfireQueueManager
    {
        private static SettingsModel _lastReadSettings;

        public static string CreateJob(Expression<Action> methodCall, string queueName)
        {
            var queue = new Hangfire.States.EnqueuedState(queueName);
            var job = Hangfire.Common.Job.FromExpression(methodCall);
            var jobId = new BackgroundJobClient().Create(job, queue);

            return jobId;
        }

        //[DisplayName("Managing Hangfire Queues")]
        public static void ManageHangfire()
        {
            var settings = GetCurrentSettings();

            // Stop job servers
            if (settings.MustStopWorkers && HangfireConfig.IsRunning)
            {
                HangfireConfig.DisposeQueueJobServers();
            }

            // Start job servers
            settings = GetCurrentSettings();
            if (settings.MustStartWorkers && !HangfireConfig.IsRunning)
            {
                HangfireConfig.RegisterQueueJobServers();
            }
        }

        private static SettingsModel GetCurrentSettings()
        {
            var currentSettings = SettingsManager.CurrentSettings;
            if (_lastReadSettings != null)
            {
                if (currentSettings.ToComparableString() != _lastReadSettings.ToComparableString())
                {
                    currentSettings.HasChangedSinceLastRead = true;
                }
            }

            _lastReadSettings = currentSettings;

            return currentSettings;
        }

        private static bool HasSettingsChanged(SettingsModel currentSettings)
        {
            if (_lastReadSettings != null)
            {
                if (currentSettings.ToJson() != _lastReadSettings.ToJson())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
