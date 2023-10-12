using UnityEngine;

public class ExitApp : MonoBehaviour
{
    public void ExitAPP()
    {
        // Android
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.Quit();
        }
        // Editor
#if UNITY_EDITOR
        else if (Application.platform == RuntimePlatform.WindowsEditor ||
                 Application.platform == RuntimePlatform.OSXEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }
}
