using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameSystem.Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Data/Create Settings", order = 1)]
    public class SettingsScriptableObject : ScriptableObject
    {
        public int SpawnSafeTreshHold;
        public int SpawnsPerFrame;
    }
}

