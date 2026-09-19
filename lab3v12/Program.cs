using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab3V12;

public class BluetoothConnection : IDisposable
{
    private readonly string _deviceName = string.Empty;
    private bool _isPaired;
    private bool _disposed;

    public string DeviceName => _deviceName;
    public bool IsPaired => _isPaired;
    public bool IsDisposed => _disposed;

    public BluetoothConnection(string deviceName)
    {
        if (string.IsNullOrWhiteSpace(deviceName))
        {
            throw new ArgumentException(
                "Назва пристрою не може бути порожньою.", nameof(deviceName));
        }

        _deviceName = deviceName;
        _isPaired = true; // Імітація виділення некерованого ресурсу.
        Console.WriteLine($"[{_deviceName}] Bluetooth-з'єднання встановлено.");
    }

    public void TransferData(byte[] data)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(BluetoothConnection));

        if (data is null)
            throw new ArgumentNullException(nameof(data));

        Console.WriteLine(
            $"[{_deviceName}] Передано байтів: {data.Length}. Дані: " +
            BitConverter.ToString(data));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return; // Повторний виклик не звільняє ресурс ще раз.

        if (disposing)
        {
            // Тут звільняють власні керовані IDisposable-ресурси.
            // У цій імітації таких ресурсів немає.
            Console.WriteLine(
                $"[{_deviceName}] Dispose(true): явне звільнення ресурсу.");
        }

        // Імітований некерований ресурс звільняємо в обох сценаріях.
        if (_isPaired)
        {
            _isPaired = false;
            Console.WriteLine($"[{_deviceName}] Bluetooth-з'єднання розірвано.");
        }

        _disposed = true;
    }

    ~BluetoothConnection()
    {
        // Вивід у фіналізаторі потрібний лише для навчальної демонстрації.
        Console.WriteLine($"[{_deviceName}] Фіналізатор: Dispose(false).");
        Dispose(false);
    }
}

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Лабораторна робота №3. Варіант 12.");

        Console.WriteLine("\n1. Використання using");
        using (var connection = new BluetoothConnection("Навушники"))
        {
            connection.TransferData(new byte[] { 10, 20, 30 });
        }
        Console.WriteLine("Блок using завершено. Ресурс звільнено.");

        Console.WriteLine("\n2. Явний виклик Dispose()");
        var phone = new BluetoothConnection("Смартфон");
        try
        {
            phone.TransferData(new byte[] { 40, 50, 60 });
        }
        finally
        {
            phone.Dispose();
        }
        Console.WriteLine($"IsPaired після Dispose(): {phone.IsPaired}");
        phone.Dispose(); // Перевірка безпечного повторного виклику.
        Console.WriteLine("Повторний Dispose() завершено без повторного закриття.");

        Console.WriteLine("\n3. Без Dispose(): демонстрація фіналізатора");
        CreateConnectionWithoutDispose();
        Console.WriteLine("Запускаємо GC та очікуємо завершення фіналізаторів.");
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Console.WriteLine("Збирання сміття завершено.");
        Console.WriteLine("Усі три сценарії виконано.");
    }

    // Окремий метод усуває сильне посилання до виклику GC у Main.
    // NoInlining забороняє JIT вбудувати цей метод у Main.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void CreateConnectionWithoutDispose()
    {
        var connection = new BluetoothConnection("Колонка");
        connection.TransferData(new byte[] { 70, 80, 90 });
        // Навмисно не викликаємо Dispose() для третього сценарію.
    }
}
