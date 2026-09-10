using System;

class Song
{
    private string _title = "";
    private string _artist = "";
    private int _durationSeconds;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Artist
    {
        get { return _artist; }
        set { _artist = value; }
    }

    public int DurationSeconds
    {
        get { return _durationSeconds; }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException(
                    "Тривалість пісні повинна бути більшою за 0.");
            }

            _durationSeconds = value;
        }
    }

    public Song() : this("Unknown", "Unknown", 180)
    {
    }

    public Song(string title, string artist, int duration)
    {
        Title = title;
        Artist = artist;
        DurationSeconds = duration;
    }

    public void Play()
    {
        Console.WriteLine($"Відтворюється пісня: {Title}");
        Console.WriteLine($"Виконавець: {Artist}");
        Console.WriteLine($"Тривалість: {DurationSeconds} секунд");
        Console.WriteLine();
    }

    ~Song()
    {
        Console.WriteLine(
            $"Деструктор: об'єкт пісні \"{Title}\" знищено.");
    }
}

class Program
{
    static void CreateSongs()
    {
        Song song1 = new Song();
        song1.Play();

        Song song2 = new Song(
            "Believer",
            "Imagine Dragons",
            204);
        song2.Play();

        Song song3 = new Song(
            "Shape of You",
            "Ed Sheeran",
            234);
        song3.Play();

        Console.WriteLine("=== Перевірка властивостей ===");
        Console.WriteLine($"Назва: {song2.Title}");
        Console.WriteLine($"Виконавець: {song2.Artist}");
        Console.WriteLine(
            $"Тривалість: {song2.DurationSeconds} секунд");
        Console.WriteLine();
    }

    static void Main()
    {
        Console.WriteLine("=== Лабораторна робота №2 ===");
        Console.WriteLine("Варіант №12 — Клас Song");
        Console.WriteLine();

        CreateSongs();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine();
        Console.WriteLine("Програма завершила роботу.");
    }
}