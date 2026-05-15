using System;

namespace EmployeeApp.Models
{
    /// <summary>
    /// Сообщение чата, передаваемое между отделами через Named Pipes.
    /// </summary>
    public class ChatMessage
    {
        public string From { get; set; } = string.Empty;
        public string FromDepartment { get; set; } = string.Empty;
        public string ToDepartment { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.Now;

        public override string ToString()
            => $"[{Time:HH:mm:ss}] {From} ({FromDepartment} → {ToDepartment}): {Text}";
    }
}
