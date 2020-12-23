using UnityEngine;

namespace DefaultNamespace
{
    public class SharedInfo
    {
        public static bool inMenu;

        public void exit()
        {
            Application.Quit();
        }
    }
}