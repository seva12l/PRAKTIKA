using System;
using System.IO;
using System.Threading;

Console.Write("Введите путь к папке: ");
string dir = Console.ReadLine();

if (!Directory.Exists(dir))
{
    Directory.CreateDirectory(dir);
    Console.WriteLine("Папка создана: " + dir);
}

FileWatcher watcher = new FileWatcher(dir);
watcher.Start();

Console.WriteLine("Отслеживание запущено. Создайте .tmp файл в папке.");
Console.WriteLine("Для выхода нажмите Enter.");
Console.ReadLine();

watcher.Stop();


class FileWatcher
{
    private FileSystemWatcher watcher;
    private string logFile = "watcher.log";

    public FileWatcher(string path)
    {
        watcher = new FileSystemWatcher();
        watcher.Path = path;
        watcher.NotifyFilter = NotifyFilters.FileName;

        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Changed += OnChanged;
        watcher.Renamed += OnRenamed;
    }

    public void Start()
    {
        watcher.EnableRaisingEvents = true;
        Log("Старт: " + watcher.Path);
    }

    public void Stop()
    {
        watcher.EnableRaisingEvents = false;
        watcher.Dispose();
        Log("Стоп");
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        Log("Создан: " + e.Name);

        if (Path.GetExtension(e.Name) == ".tmp")
        {
            Thread.Sleep(200);
            try
            {
                File.Delete(e.FullPath);
                Log("Удалён .tmp: " + e.Name);
            }
            catch (Exception ex)
            {
                Log("Ошибка: " + ex.Message);
            }
        }
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        Log("Удалён: " + e.Name);
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Log("Изменён: " + e.Name);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        Log("Переименован: " + e.OldName + " -> " + e.Name);
    }

    private void Log(string message)
    {
        string line = DateTime.Now.ToString("HH:mm:ss") + " | " + message;
        Console.WriteLine(line);
        File.AppendAllText(logFile, line + Environment.NewLine);
    }
}
