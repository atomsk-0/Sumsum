using System.Text;

namespace Proton.Util;

public class TextScanner : IDisposable
{
    private const char Separator = '\n';
    
    private readonly Dictionary<string, object> _values = new();
    
    private bool _disposed;
    
    public TextScanner() {}

    public TextScanner(string str) => Load(str);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public string Build()
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var val in _values)
        {
            if (!string.IsNullOrEmpty(val.Key))
            {
                stringBuilder.Append(val.Key);
                stringBuilder.Append('|');
                stringBuilder.Append(val.Value);
                stringBuilder.Append('\n');
            }
        }

        return stringBuilder.ToString();
    }

    public void Remove(string key)
    {
        _values.Remove(key);
    }

    public bool Has(string key)
    {
        return _values.ContainsKey(key);
    }

    public void Set(string key, object value)
    {
        if (Has(key))
        {
            _values[key] = value;
        }
        else
        {
            _values.Add(key, value);
        }
    }

    public T Get<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Key can't be null or whitespace", nameof(key));
        }

        try
        {
            if (!_values.TryGetValue(key, out object? value))
            {
                return (T)Convert.ChangeType(0, typeof(T));
            }

            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(value.ToString()!.Trim(), typeof(T));
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (InvalidCastException)
        {
            throw new InvalidOperationException($"Cannot convert the value of key '{key}' to type '{typeof(T)}'.");
        }
    }

    private void Load(string str)
    {
        var lines = str.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            var split = line.Split('|');
            if (split.Length < 2) continue;
            var key = split[0];
            var value = split[1];
            _values.TryAdd(key, value);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _values.Clear();
        }

        _disposed = true;
    }

    ~TextScanner() => _values.Clear();
}