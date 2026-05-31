using Naseejk.core.Common;

namespace Naseejk.Infrastructure.Services;

public class SnowflakeIdGenerator : ISnowflakeIdGenerator
{
    private readonly long _workerId;
    private readonly DateTime _epoch = new DateTime(2026, 1, 1);
    private long _lastTimestamp = -1L;
    private long _sequence = 0L;
    private readonly object _lock = new();

    public SnowflakeIdGenerator(long workerId)
    {
        if (workerId < 0 || workerId > 1023)
            throw new ArgumentException("Worker ID must be between 0 and 1023");
        _workerId = workerId;
    }

    public long CreateId()
    {
        lock (_lock)
        {
            var timestamp = GetTimestamp();

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & 4095;
                if (_sequence == 0) timestamp = WaitNextMillis(_lastTimestamp);
            }
            else
            {
                _sequence = 0L;
            }

            _lastTimestamp = timestamp;

            return ((timestamp - (long)(_epoch - DateTime.UnixEpoch).TotalMilliseconds) << 22)
                   | (_workerId << 12)
                   | _sequence;
        }
    }

    private long GetTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    private long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = GetTimestamp();
        while (timestamp <= lastTimestamp) timestamp = GetTimestamp();
        return timestamp;
    }


}