using System;
using System.Collections.Generic;
using UnityEngine;

namespace BBExtensions.Array2D
{
    internal static class Consts
    {
        internal static readonly Dictionary<Type, float> TypeWidths = new()
        {
            { typeof(bool), 16f },
            { typeof(int), 32f },
            { typeof(float), 32f },
            { typeof(double), 32f },
            { typeof(string), 128f },
            { typeof(Vector2), 64f },
            { typeof(Vector3), 96f },
            { typeof(Vector4), 128f },
            { typeof(Color), 64f },
            { typeof(Rect), 128f },
            { typeof(Quaternion), 96f },
            { typeof(Matrix4x4), 256f },
            { typeof(Bounds), 128f },
            { typeof(AnimationCurve), 128f },
            { typeof(LayerMask), 32f },
            { typeof(Gradient), 128f },
            { typeof(RectOffset), 64f },
            { typeof(Vector2Int), 64f },
            { typeof(Vector3Int), 96f },
            { typeof(RectInt), 128f },
            { typeof(BoundsInt), 128f }
        };
    }
}