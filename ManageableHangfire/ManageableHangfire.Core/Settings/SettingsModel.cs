using System.Collections.Generic;

namespace ManageableHangfire.Core.Settings
{
    public class SettingsModel
    {
        public List<QueueModel> Queues { get; set; } = new List<QueueModel>();

        public bool IsActive { get; set; }

        public string ConnectionString { get; set; }

        public string HangfireUrl { get; set; }

        public bool HasChangedSinceLastRead { get; set; }

        public bool MustStartWorkers => IsActive || HasChangedSinceLastRead;

        public bool MustStopWorkers => !IsActive || HasChangedSinceLastRead;

        public string ToComparableString()
        {
            var obj = new
            {
                IsActive,
                Queues
            };
            return obj.ToJson();
        }
    }
}
