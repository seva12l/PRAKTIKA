using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    /// <summary>
    /// Чат между отделами через Named Pipes. Каждый отдел — отдельный pipe-сервер
    /// с именем "EmployeeApp_Chat_{Department}". Отправка — это короткое
    /// клиентское подключение к pipe-серверу нужного отдела с сериализованным сообщением.
    /// </summary>
    public class ChatService
    {
        public const string PipePrefix = "EmployeeApp_Chat_";

        public string MyDepartment { get; }
        public string MyLogin { get; }

        private readonly CancellationTokenSource cts = new();
        public event Action<ChatMessage>? MessageReceived;

        public ChatService(string login, string department)
        {
            MyLogin = login;
            MyDepartment = department;
        }

        public void Start()
        {
            Task.Run(() => ListenLoop(cts.Token));
        }

        public void Stop() => cts.Cancel();

        private async Task ListenLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using var server = new NamedPipeServerStream(
                        PipePrefix + MyDepartment,
                        PipeDirection.In,
                        NamedPipeServerStream.MaxAllowedServerInstances,
                        PipeTransmissionMode.Byte,
                        PipeOptions.Asynchronous);

                    await server.WaitForConnectionAsync(token);

                    using var reader = new StreamReader(server);
                    var json = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(json)) continue;

                    var msg = JsonSerializer.Deserialize<ChatMessage>(json);
                    if (msg != null) MessageReceived?.Invoke(msg);
                }
                catch (OperationCanceledException) { break; }
                catch
                {
                    // Игнорируем единичные сбои подключения и продолжаем слушать.
                }
            }
        }

        public async Task<bool> SendAsync(string toDepartment, string text)
        {
            var msg = new ChatMessage
            {
                From = MyLogin,
                FromDepartment = MyDepartment,
                ToDepartment = toDepartment,
                Text = text,
                Time = DateTime.Now
            };
            try
            {
                using var client = new NamedPipeClientStream(
                    ".", PipePrefix + toDepartment, PipeDirection.Out, PipeOptions.Asynchronous);
                await client.ConnectAsync(500);
                using var writer = new StreamWriter(client) { AutoFlush = true };
                await writer.WriteAsync(JsonSerializer.Serialize(msg));
            }
            catch
            {
                return false;
            }
            // Эхо-сообщение — чтобы отправитель тоже видел свою реплику в окне чата.
            MessageReceived?.Invoke(msg);
            return true;
        }
    }
}
