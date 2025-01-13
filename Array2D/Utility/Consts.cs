using System;
using System.Collections.Generic;
using UnityEngine;

namespace BBExtensions.Array2D
{
    internal static class Consts
    {
        internal static readonly Dictionary<Type, int> TypeWidths = new()
        {
            { typeof(bool), 16 },
            { typeof(int), 32 },
            { typeof(float), 32 },
            { typeof(double), 32 },
            { typeof(string), 128 },
            { typeof(Vector2), 64 },
            { typeof(Vector3), 96 },
            { typeof(Vector4), 128 },
            { typeof(Color), 64 },
            { typeof(Rect), 128 },
            { typeof(Quaternion), 96 },
            { typeof(Matrix4x4), 256 },
            { typeof(Bounds), 128 },
            { typeof(AnimationCurve), 128 },
            { typeof(LayerMask), 32 },
            { typeof(Gradient), 128 },
            { typeof(RectOffset), 64 },
            { typeof(Vector2Int), 64 },
            { typeof(Vector3Int), 96 },
            { typeof(RectInt), 128 },
            { typeof(BoundsInt), 128 }
        };
    }
}