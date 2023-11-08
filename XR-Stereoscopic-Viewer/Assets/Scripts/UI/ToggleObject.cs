using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject objectToToggle; // The object to be toggled

    public void ToggleSettingsState()
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
        objectToToggle.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }
}
