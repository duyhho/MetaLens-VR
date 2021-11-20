using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class XRGUIToggle : MonoBehaviour
{   
    [SerializeField]
    Toggle autoToggle;
    [SerializeField]
    GameObject droneManager;

    // Start is called before the first frame update
    void Start()
    {
        autoToggle.onValueChanged.AddListener(ToggleActivation);
    }
    void ToggleActivation(bool isOn) {
        droneManager.GetComponent<DroneManager>().ToggleActivation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
