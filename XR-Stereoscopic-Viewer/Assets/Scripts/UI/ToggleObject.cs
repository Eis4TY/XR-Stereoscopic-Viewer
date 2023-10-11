using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject objectToToggle; // The object to be toggled

    public void ToggleActiveState()
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
