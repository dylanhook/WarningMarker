using System.Collections.Generic;
using UnityEngine;

namespace WarningMarker.Parity
{
    public enum Parity
    {
        Forehand,
        Backhand,
        Undecided
    }

    public enum ResetType
    {
        None,
        Bomb,
        Rebound,
        Squat
    }

    public static class ParityUtils
    {
        public static readonly Dictionary<int, float> ForehandDict = new Dictionary<int, float> {
            { 0, -180 }, { 1, 0 }, { 2, -90 }, { 3, 90 }, { 4, -135 }, { 5, 135 }, { 6, -45 }, { 7, 45 }, { 8, 0 }
        };

        public static readonly Dictionary<int, float> BackhandDict = new Dictionary<int, float> {
            { 0, 0 }, { 1, 180 }, { 2, 90 }, { 3, -90 }, { 4, 45 }, { 5, -45 }, { 6, 135 }, { 7, -135 }, { 8, 0 }
        };

        public static readonly Vector2[] DirectionVectors = {
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(-1, 1),
            new Vector2(1, 1),
            new Vector2(-1, -1),
            new Vector2(1, -1)
        };

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        public const float SwingTimeThreshold = 0.05f;
        public const float ResetAngleThreshold = 90f;
        public const float ReboundAngleThreshold = 270f;
    }
}