using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApp.Services
{
    /// <summary>
    /// Уведомления об изменениях в расписании через Memory-Mapped Files.
    /// Все запущенные экземпляры приложения открывают общий MMF "EmployeeApp_Schedule"
    /// размером 4 КБ. Опубликованное уведомление содержит счётчик версии (long)
    /// и UTF-8 строку. Подписчики опрашивают MMF и поднимают событие при смене версии.
    /// </summary>
    public class NotificationService : IDisposable
    {
        private const string MmfName = "EmployeeApp_Schedule";
        private const int Capacity = 4096;
        private const int HeaderSize = sizeof(long) + sizeof(int); // version + length

        private readonly MemoryMappedFile mmf;
        private readonly CancellationTokenSource cts = new();
        private long lastSeenVersion;

        public event Action<string>? NotificationReceived;

        public NotificationService()
        {
            mmf = MemoryMappedFile.CreateOrOpen(MmfName, Capacity);
            using var accessor = mmf.CreateViewAccessor();
            lastSeenVersion = accessor.ReadInt64(0);
        }

        public void StartListening()
        {
            Task.Run(() => PollLoop(cts.Token));
        }

        private async Task PollLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using var accessor = mmf.CreateViewAccessor();
                    long version = accessor.ReadInt64(0);
                    if (version != lastSeenVersion && version > 0)
                    {
                        lastSeenVersion = version;
                        int len = accessor.ReadInt32(sizeof(long));
                        if (len > 0 && len < Capacity - HeaderSize)
                        {
                            var buf = new byte[len];
                            accessor.ReadArray(HeaderSize, buf, 0, len);
                            var text = Encoding.UTF8.GetString(buf);
                            NotificationReceived?.Invoke(text);
                        }
                    }
                }
                catch { /* игнорируем единичный сбой и продолжаем опрос */ }

                try { await Task.Delay(500, token); }
                catch (TaskCanceledException) { break; }
            }
        }

        public void Publish(string text)
        {
            using var accessor = mmf.CreateViewAccessor();
            long newVersion = accessor.ReadInt64(0) + 1;
            var buf = Encoding.UTF8.GetBytes(text);
            int len = Math.Min(buf.Length, Capacity - HeaderSize);
            accessor.WriteArray(HeaderSize, buf, 0, len);
            accessor.Write(sizeof(long), len);
            accessor.Write(0, newVersion);
            lastSeenVersion = newVersion; // не показываем эхо себе же
        }

        public void Dispose()
        {
            cts.Cancel();
            mmf.Dispose();
        }
    }
}
