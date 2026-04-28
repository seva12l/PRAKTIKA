using System;
using System.IO;

string workDir = Directory.GetCurrentDirectory();
string filePath = Path.Combine(workDir, "zhivaga.vv");
string copyPath = Path.Combine(workDir, "zhivaga_copy.vv");
string moveDir = Path.Combine(workDir, "moved");
string movedPath = Path.Combine(moveDir, "zhivaga.vv");
string renamedPath = Path.Combine(workDir, "zhivaga.io");
string patternDir = Path.Combine(workDir, "pattern_test");

FileManager manager = new FileManager();
FileInfoProvider info = new FileInfoProvider();

Console.WriteLine("1) Создание и чтение файла");
manager.CreateWithText(filePath, "Привет! Это файл zhivaga.vv");
Console.WriteLine(File.ReadAllText(filePath));
Console.WriteLine();

Console.WriteLine("2) Проверка перед удалением");
string tempPath = Path.Combine(workDir, "temp.vv");
manager.CreateWithText(tempPath, "временный");
manager.DeleteIfExists(tempPath);
manager.DeleteIfExists(tempPath);
Console.WriteLine();

Console.WriteLine("3) Информация о файле");
info.PrintInfo(filePath);
Console.WriteLine();

Console.WriteLine("4) Копирование");
manager.CopyFile(filePath, copyPath);
Console.WriteLine("Копия существует: " + File.Exists(copyPath));
Console.WriteLine();

Console.WriteLine("5) Перемещение");
Directory.CreateDirectory(moveDir);
manager.MoveFile(copyPath, movedPath);
Console.WriteLine();

Console.WriteLine("6) Переименование");
manager.RenameFile(filePath, renamedPath);
Console.WriteLine();

Console.WriteLine("7) Удаление несуществующего");
manager.DeleteIfExists(Path.Combine(workDir, "noexist.vv"));
Console.WriteLine();

Console.WriteLine("8) Сравнение по размеру");
string fileA = Path.Combine(workDir, "a.vv");
string fileB = Path.Combine(workDir, "b.vv");
manager.CreateWithText(fileA, "короткий");
manager.CreateWithText(fileB, "этот файл длиннее");
info.CompareBySize(fileA, fileB);
Console.WriteLine();

Console.WriteLine("9) Удаление по шаблону *.vv");
Directory.CreateDirectory(patternDir);
File.WriteAllText(Path.Combine(patternDir, "1.vv"), "a");
File.WriteAllText(Path.Combine(patternDir, "2.vv"), "b");
File.WriteAllText(Path.Combine(patternDir, "keep.txt"), "c");
manager.DeleteByPattern(patternDir, "*.vv");
Console.WriteLine();

Console.WriteLine("10) Список файлов");
info.ListFiles(workDir);
Console.WriteLine();

Console.WriteLine("11) Запрет записи");
manager.CreateWithText(renamedPath, "содержимое");
manager.SetReadOnly(renamedPath, true);
manager.TryWrite(renamedPath, "попытка");
manager.SetReadOnly(renamedPath, false);
Console.WriteLine();

Console.WriteLine("12) Проверка прав доступа");
info.CheckAccess(renamedPath);

File.Delete(renamedPath);
File.Delete(fileA);
File.Delete(fileB);
if (Directory.Exists(moveDir)) Directory.Delete(moveDir, true);
if (Directory.Exists(patternDir)) Directory.Delete(patternDir, true);


class FileManager
{
    public void CreateWithText(string path, string text)
    {
        File.WriteAllText(path, text);
        Console.WriteLine("Создан: " + Path.GetFileName(path));
    }

    public void DeleteIfExists(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("Файл не найден: " + Path.GetFileName(path));
            return;
        }
        File.Delete(path);
        Console.WriteLine("Удалён: " + Path.GetFileName(path));
    }

    public void CopyFile(string source, string dest)
    {
        File.Copy(source, dest, true);
        Console.WriteLine("Скопирован в " + Path.GetFileName(dest));
    }

    public void MoveFile(string source, string dest)
    {
        File.Move(source, dest);
        Console.WriteLine("Перемещён в " + dest);
    }

    public void RenameFile(string source, string dest)
    {
        File.Move(source, dest);
        Console.WriteLine("Переименован в " + Path.GetFileName(dest));
    }

    public void DeleteByPattern(string dir, string pattern)
    {
        string[] files = Directory.GetFiles(dir, pattern);
        Console.WriteLine("Найдено: " + files.Length);
        foreach (string f in files)
        {
            File.Delete(f);
            Console.WriteLine("Удалён: " + Path.GetFileName(f));
        }
    }

    public void SetReadOnly(string path, bool readOnly)
    {
        FileInfo fi = new FileInfo(path);
        if (readOnly)
            fi.Attributes = fi.Attributes | FileAttributes.ReadOnly;
        else
            fi.Attributes = fi.Attributes & ~FileAttributes.ReadOnly;
        Console.WriteLine("ReadOnly = " + readOnly);
    }

    public void TryWrite(string path, string text)
    {
        try
        {
            File.AppendAllText(path, text);
            Console.WriteLine("Запись прошла");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Ошибка: нет прав на запись");
        }
    }
}

class FileInfoProvider
{
    public void PrintInfo(string path)
    {
        FileInfo fi = new FileInfo(path);
        Console.WriteLine("Имя: " + fi.Name);
        Console.WriteLine("Размер: " + fi.Length + " байт");
        Console.WriteLine("Создан: " + fi.CreationTime.ToString("dd.MM.yyyy HH:mm:ss"));
        Console.WriteLine("Изменён: " + fi.LastWriteTime.ToString("dd.MM.yyyy HH:mm:ss"));
    }

    public void CompareBySize(string pathA, string pathB)
    {
        FileInfo a = new FileInfo(pathA);
        FileInfo b = new FileInfo(pathB);
        Console.WriteLine(a.Name + " = " + a.Length + " байт");
        Console.WriteLine(b.Name + " = " + b.Length + " байт");
        if (a.Length > b.Length) Console.WriteLine(a.Name + " больше");
        else if (a.Length < b.Length) Console.WriteLine(b.Name + " больше");
        else Console.WriteLine("Одинаковые");
    }

    public void ListFiles(string dir)
    {
        string[] files = Directory.GetFiles(dir);
        Console.WriteLine("Всего файлов: " + files.Length);
        foreach (string f in files)
        {
            FileInfo fi = new FileInfo(f);
            Console.WriteLine(fi.Name + " - " + fi.Length + " байт");
        }
    }

    public void CheckAccess(string path)
    {
        FileInfo fi = new FileInfo(path);
        bool readOnly = (fi.Attributes & FileAttributes.ReadOnly) != 0;
        bool hidden = (fi.Attributes & FileAttributes.Hidden) != 0;
        Console.WriteLine("Чтение: да");
        Console.WriteLine("Запись: " + (!readOnly ? "да" : "нет"));
        Console.WriteLine("Скрытый: " + hidden);
    }
}
