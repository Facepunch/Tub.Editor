using UnityEngine;

public struct TimeSince
{
    float time;

    public static implicit operator float(TimeSince ts)
    {
        return Time.time - ts.time;
    }

    public static implicit operator TimeSince(float ts)
    {
        return new TimeSince { time = Time.time - ts };
    }
}

public struct TimeUntil
{
    float time;

    public static implicit operator float( TimeUntil ts )
    {
        return ts.time - Time.time;
    }

    public static implicit operator TimeUntil( float ts )
    {
        return new TimeUntil { time = Time.time + ts };
    }
}

public struct RealTimeSince
{
    float time;

    public static implicit operator float( RealTimeSince ts )
    {
        return Time.realtimeSinceStartup - ts.time;
    }

    public static implicit operator RealTimeSince( float ts )
    {
        return new RealTimeSince { time = Time.realtimeSinceStartup - ts };
    }
}