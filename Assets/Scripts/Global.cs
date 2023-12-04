using GRP.Unity;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Global
{
    public static class Layers
    {
        public const int Default         = 0;
        public const int TransparentFX   = 1;
        public const int IgnoreRaycast   = 2;
        public const int Paddle          = 6;
        public const int Goal            = 7;
    }

    public static class Data
    {
        public const string NameP1   = "P1";
        public const string NameP2   = "P2";
        public const string NameCPU1 = "CPU1";
        public const string NameCPU2 = "CPU2";

        public static T FindFirstMonoSingle<T>()
            where T : MonoSingle<T>
            => GameObject
                .FindFirstObjectByType(typeof(GameManager))
                .GetComponent<T>();
    }

}
