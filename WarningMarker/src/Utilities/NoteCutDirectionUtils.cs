using System;
using UnityEngine;

namespace WarningMarker.Utilities;

public static class NoteCutDirectionUtils
{
    private static readonly Vector2 UpLeft = new Vector2(-1f, 1f).normalized;
    private static readonly Vector2 UpRight = new Vector2(1f, 1f).normalized;
    private static readonly Vector2 DownLeft = new Vector2(-1f, -1f).normalized;
    private static readonly Vector2 DownRight = new Vector2(1f, -1f).normalized;

    public static Vector2 GetCutDirectionVector(NoteCutDirection cutDirection)
    {
        return cutDirection switch
        {
            NoteCutDirection.Up => Vector2.up,
            NoteCutDirection.Down => Vector2.down,
            NoteCutDirection.Left => Vector2.left,
            NoteCutDirection.Right => Vector2.right,
            NoteCutDirection.UpLeft => UpLeft,
            NoteCutDirection.UpRight => UpRight,
            NoteCutDirection.DownLeft => DownLeft,
            NoteCutDirection.DownRight => DownRight,
            NoteCutDirection.Any => throw new ArgumentOutOfRangeException(),
            NoteCutDirection.None => throw new ArgumentOutOfRangeException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static float GetAngle(this NoteCutDirection cutDirection, NoteCutDirection other)
    {
        var a = GetCutDirectionVector(cutDirection);
        var b = GetCutDirectionVector(other);
        return Vector2.Angle(a, b);
    }

    public static NoteCutDirection GetOppositeCutDirection(this NoteCutDirection cutDirection)
    {
        return cutDirection switch
        {
            NoteCutDirection.Up => NoteCutDirection.Down,
            NoteCutDirection.Down => NoteCutDirection.Up,
            NoteCutDirection.Left => NoteCutDirection.Right,
            NoteCutDirection.Right => NoteCutDirection.Left,
            NoteCutDirection.UpLeft => NoteCutDirection.DownRight,
            NoteCutDirection.UpRight => NoteCutDirection.DownLeft,
            NoteCutDirection.DownLeft => NoteCutDirection.UpRight,
            NoteCutDirection.DownRight => NoteCutDirection.UpLeft,
            NoteCutDirection.Any => NoteCutDirection.Any,
            NoteCutDirection.None => NoteCutDirection.None,
            _ => throw new ArgumentOutOfRangeException(nameof(cutDirection), cutDirection, null)
        };
    }
}