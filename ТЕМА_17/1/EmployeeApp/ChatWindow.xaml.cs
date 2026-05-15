using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using EmployeeApp.Models;
using EmployeeApp.Services;

namespace EmployeeApp
{
    public partial class ChatWindow : Window
    {
        private readonly ChatService chat;

        public ChatWindow(ChatService chat, IEnumerable<string> departments)
        {
            InitializeComponent();
            this.chat = chat;

            foreach (var d in departments)
                if (d != chat.MyDepartment)
                    DepartmentBox.Items.Add(d);
            if (DepartmentBox.Items.Count > 0) DepartmentBox.SelectedIndex = 0;

            chat.MessageReceived += OnMessageReceived;
            Closed += (_, _) => chat.MessageReceived -= OnMessageReceived;

            Title = $"Чат — {chat.MyLogin} ({chat.MyDepartment})";
        }

        private void OnMessageReceived(ChatMessage msg)
        {
            Dispatcher.Invoke(() =>
            {
                if (msg.ToDepartment == chat.MyDepartment || msg.FromDepartment == chat.MyDepartment)
                    MessagesList.Items.Add(msg.ToString());
            });
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var to = DepartmentBox.SelectedItem as string;
            var text = MessageBox.Text.Trim();
            if (string.IsNullOrEmpty(to) || string.IsNullOrEmpty(text)) return;

            var ok = await chat.SendAsync(to, text);
            if (!ok)
            {
                MessagesList.Items.Add($"[!] Отдел \"{to}\" не в сети (нет слушателя на канале).");
            }
            MessageBox.Clear();
        }

        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendButton_Click(sender, e);
        }
    }
}
