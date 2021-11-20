using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.Events.UnityAction;

public class XRPersonInteractable : MonoBehaviour
{
    [SerializeField]
    XRSimpleInteractable simpleInteractable;
    [SerializeField]
    GameObject popupWindow;
    PopUpWindow popupWindowManager;
    // [SerializeField]
    // XRGrabInteractable m_GrabInteractable;
    private XRBaseInteractor  m_MyFirstAction;
    bool m_Held;
    // Start is called before the first frame update
    void Start()
    {
        popupWindowManager = popupWindow.GetComponent<PopUpWindow>();
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        simpleInteractable.selectEntered.AddListener(OnSelectEntered);
        simpleInteractable.firstHoverEntered.AddListener(OnFirstHoverEntered);

    }   

    // Update is called once per frame
    void Update()
    {
    }
    public void TurnOffPopup() {
        if (popupWindow) 
        {
            popupWindow.SetActive(false);
        }
    }
    public void TurnOnPopup() {
        if (popupWindow) 
        {
            popupWindow.SetActive(true);
        }
    }
    protected virtual void OnSelectEntered(SelectEnterEventArgs args)
    {
        // m_MeshRenderer.material.color = s_UnityCyan;
        Debug.Log("My person is clicked by VR");
        
        popupWindowManager.AssignTextureBasedOnName(transform.gameObject);
        TurnOnPopup();
    }

    protected virtual void OnSelectExited(SelectExitEventArgs args)
    {
        // m_MeshRenderer.material.color = Color.white;
        m_Held = false;
        // TurnOnPopup();

    }

    protected virtual void OnLastHoverExited(HoverExitEventArgs args)
    {
        // if (!m_Held)
        // {
        //     m_MeshRenderer.material.color = Color.white;
        // }
    }

    protected virtual void OnFirstHoverEntered(HoverEnterEventArgs args)
    {
        // if (!m_Held)
        // {
        //     m_MeshRenderer.material.color = s_UnityMagenta;
        // }
        Debug.Log("Hovered");
    }
}
