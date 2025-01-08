#if UNITY_EDITOR
using System;
using UnityEngine;

namespace MBBExtensions.Utility
{
    public readonly struct AlwaysActive : IDisposable
    {
        private readonly bool originalState;

        public AlwaysActive(bool originalState)
        {
            this.originalState = originalState;
            GUI.enabled = true;
        }

        public void Dispose() => GUI.enabled = originalState;
    }
}
#endif