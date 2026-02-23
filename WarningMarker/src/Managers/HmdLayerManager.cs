using System;
using UnityEngine;
using Zenject;
using WarningMarker.Configuration;

namespace WarningMarker.Managers
{
    internal class HmdLayerManager : IInitializable, IDisposable
    {
        private const int HmdOnlyLayer = 17;
        private const int HmdOnlyMask = 1 << HmdOnlyLayer;
        private const int InverseHmdOnlyMask = ~HmdOnlyMask;

        public void Initialize()
        {
            Camera.onPreCull += OnCameraPreCull;
        }

        public void Dispose()
        {
            Camera.onPreCull -= OnCameraPreCull;
        }

        private void OnCameraPreCull(Camera camera)
        {
            if (!PluginConfig.HmdOnly) return;

            if (camera.stereoEnabled)
            {
                if ((camera.cullingMask & HmdOnlyMask) == 0)
                {
                    camera.cullingMask |= HmdOnlyMask;
                }
            }
            else
            {
                if ((camera.cullingMask & HmdOnlyMask) != 0)
                {
                    camera.cullingMask &= InverseHmdOnlyMask;
                }
            }
        }
    }
}