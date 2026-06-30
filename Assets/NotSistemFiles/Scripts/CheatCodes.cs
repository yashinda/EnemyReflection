using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    public GameObject mirror;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            DestroyMirror();
    }
    
    private void DestroyMirror()
    {
        mirror.SetActive(false);
    }
}
