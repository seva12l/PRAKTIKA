using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApp.Services
{
    public class ScheduleService : IDisposable
    {
        private const string MapName = "EmployeeAppScheduleNotice";
        private const int MapSize = 4096;

        public event Action<string>? NoticeReceived;

        private readonly CancellationTokenSource cts = new();
        private readonly Task watchTask;
        private string lastSeen = string.Empty;

        public ScheduleService()
        {
            watchTask = Task.Run(() => WatchLoopAsync(cts.Token));
        }

        public void Publish(string message)
        {
            try
            {
                using var mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize);
                using var view = mmf.CreateViewAccessor();
                var stamped = $"{DateTime.Now:HH:mm:ss}|{message}";
                var bytes = Encoding.UTF8.GetBytes(stamped);
                view.Write(0, bytes.Length);
                view.WriteArray(4, bytes, 0, Math.Min(bytes.Length, MapSize - 4));
            }
            catch (Exception ex)
            {
                App.Log("ScheduleService.Publish failed: " + ex.Message);
            }
        }

        private async Task WatchLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using var mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize);
                    using var view = mmf.CreateViewAccessor();
                    int len = view.ReadInt32(0);
                    if (len > 0 && len < MapSize - 4)
                    {
                        var bytes = new byte[len];
                        view.ReadArray(4, bytes, 0, len);
                        var text = Encoding.UTF8.GetString(bytes);
                        if (text != lastSeen)
                        {
                            lastSeen = text;
                            NoticeReceived?.Invoke(text);
                        }
                    }
                }
                catch { }
                try { await Task.Delay(800, token); } catch { return; }
            }
        }

        public void Dispose()
        {
            try { cts.Cancel(); } catch { }
        }
    }
}
