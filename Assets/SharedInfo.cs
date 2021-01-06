using UnityEngine;

namespace DefaultNamespace
{
    public class SharedInfo
    {
        public static bool InMenu;
        public static Vector3[] MapVertices;
        public static AnimationCurve terrainHeightCurve;
        public static AnimationCurve inverseHeightCurve;
        
        public void exit()
        {
            Application.Quit();
        }
    }
}