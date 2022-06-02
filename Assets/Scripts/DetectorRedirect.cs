using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorRedirect : MonoBehaviour
{
    public delegate void OnTriggerEvent(Collider2D colllider, DetectorRedirect detector);

    public event OnTriggerEvent onDetected;
    public event OnTriggerEvent onLeave;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (onDetected != null) onDetected(collider, this);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (onLeave != null) onLeave(collider, this);
    }
}
