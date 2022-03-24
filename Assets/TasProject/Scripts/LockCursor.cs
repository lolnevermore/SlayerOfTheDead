using UnityEngine;

public class LockCursor : MonoBehaviour
{

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



}