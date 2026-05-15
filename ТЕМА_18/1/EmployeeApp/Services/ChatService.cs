using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EmployeeApp.Models;

namespace EmployeeApp.Services
{
    public class ChatService : IDisposable
    {
        private const string PipePrefix = "EmployeeAppChat_";

        public string MyLogin { get; }
        public string MyDepartment { get; }

        public event Action<ChatMessage>? MessageReceived;

        private readonly CancellationTokenSource cts = new();
        private readonly Task listenTask;

        public ChatService(string login, string department)
        {
            MyLogin = login ?? string.Empty;
            MyDepartment = department ?? string.Empty;
            listenTask = Task.Run(() => ListenLoopAsync(cts.Token));
        }

        private async Task ListenLoopAsync(CancellationToken token)
        {
            var pipeName = PipePrefix + MyDepartment;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using var server = new NamedPipeServerStream(
                        pipeName, PipeDirection.In, NamedPipeServerStream.MaxAllowedServerInstances,
                        PipeTransmissionMode.Message, PipeOptions.Asynchronous);

                    await server.WaitForConnectionAsync(token);
                    using var reader = new StreamReader(server);
                    var json = await reader.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(json)) continue;
                    var msg = JsonSerializer.Deserialize<ChatMessage>(json);
                    if (msg != null) MessageReceived?.Invoke(msg);
                }
                catch (OperationCanceledException) { return; }
                catch (Exception ex)
                {
                    App.Log("ChatService listen error: " + ex.Message);
                    await Task.Delay(300, token);
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

            var pipeName = PipePrefix + toDepartment;
            try
            {
                using var client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out);
                await client.ConnectAsync(500);
                using var writer = new StreamWriter(client) { AutoFlush = true };
                writer.Write(JsonSerializer.Serialize(msg));
                writer.Flush();
                MessageReceived?.Invoke(msg);
                return true;
            }
            catch (TimeoutException) { return false; }
            catch (Exception ex)
            {
                App.Log("ChatService send error: " + ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            try { cts.Cancel(); } catch { }
            try
            {
                using var client = new NamedPipeClientStream(".", PipePrefix + MyDepartment, PipeDirection.Out);
                client.Connect(100);
            }
            catch { }
        }
    }
}
