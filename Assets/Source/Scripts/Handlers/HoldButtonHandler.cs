using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class HoldButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Inject] private PlayerController controller;
    public float holdDuration = 3f; 
    private bool isHolding = false;
    private float holdStartTime;

    public Button targetButton; 

    private void Update()
    {
        if (isHolding && Time.time - holdStartTime >= holdDuration)
        {
           
            targetButton.onClick.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Hold");
        isHolding = true;
        holdStartTime = Time.time;
    }

    [System.Obsolete]
    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        if (!controller.DistantWeapon.active)
        {
            Debug.Log("DistantWeapon is not active");
            return;
        }
        else controller.DistantWeapon.SetActive(false);
    }
}
