using Hangfire;
using System;
using System.ComponentModel;

namespace ManageableHangfire.Core.Executors
{
    public class Executor
    {
        //[DisplayName("Download of id {0}")]
        public void Download(int id, IJobCancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine($"Downloaded id {id}");
        }

        //[DisplayName("Upload of id {0}")]
        public void Upload(int id)
        {
            System.Threading.Thread.Sleep(5000);
            Console.WriteLine($"Uploaded id {id}");
        }
    }
}
