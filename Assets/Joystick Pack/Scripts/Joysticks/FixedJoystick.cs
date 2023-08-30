using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private bool isActive = true;
    public int a = 55;
    public static FixedJoystick instance;
    private void Awake()
    {
        instance = this;
    }
    public void ActivateJoystick()
    {
        isActive = true;
    }

    public void DeactivateJoystick()
    {
        isActive = false;
    }

    public bool IsActive()
    {
        return isActive;
    }
   
    public void OnEndDrag(PointerEventData eventData)
    {
       DeactivateJoystick();
        Debug.Log("Deactivate");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ActivateJoystick();
        Debug.Log("Activate");
    }
}