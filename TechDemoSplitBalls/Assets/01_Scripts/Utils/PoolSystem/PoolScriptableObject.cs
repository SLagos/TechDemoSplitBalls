using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utils.PoolSystem
{
    [CreateAssetMenu(fileName = "PoolSettings", menuName = "Data/Create Pool Settings", order = 0)]
    public class PoolScriptableObject : ScriptableObject, ISerializationCallbackReceiver
    {
        private static string poolsNames = "EPools";
        private static string filePath = "Assets/01_Scripts/Utils/PoolSystem/"; //I'm sure that there is a better way for this
        
        [Serializable]
        public struct PoolDataStruct
        {
            public string PoolName;
            [FormerlySerializedAs("_object")] public GameObject Object;
            public int StartingAmount;
        }

        public List<PoolDataStruct> PoolData;

        public Dictionary<string, PoolDataStruct> Data;

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            //In here i will take the list of data struct and create an enum with that data, to be call later on the
            //pool manager
            List<string> pools = new List<string>();
            Data = new Dictionary<string, PoolDataStruct>();
            foreach (var data in PoolData)
            {
                Data.Add(data.PoolName,data);
                if (!pools.Contains(data.PoolName))
                    pools.Add(data.PoolName);
                else
                    Debug.LogError("A pool with the name: "+data.PoolName+" already exists");
            }

            string path = filePath + poolsNames + ".cs";
            using ( StreamWriter streamWriter = new StreamWriter( path ) )
            {
                streamWriter.WriteLine("namespace Utils.PoolSystem");
                streamWriter.WriteLine( "{" );
                streamWriter.WriteLine( "public enum " + poolsNames );
                streamWriter.WriteLine( "{" );
                foreach (var pool in pools)
                {
                    streamWriter.WriteLine( "\t" +pool + "," );
                }
                streamWriter.WriteLine( "}" );
                streamWriter.WriteLine( "}" );
            }
#endif
        }
    }
}