using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class SharedInfo
    {
        public static bool initialized = false;
        public static bool InMenu;
        public static Vector3[] MapVertices;
        public static AnimationCurve terrainHeightCurve;
        public static AnimationCurve inverseHeightCurve;
        public static List<ChunkProperties> chunkPropertiesList;
        
    }
}