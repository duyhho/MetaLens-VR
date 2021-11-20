using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// [RequireComponent(typeof(Rigidbody))]
public class CharacterControlVR : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpSpeed = 8f;
    public float turnSpeed = 90f;
    public float gravityValue = -9.81f;
    public float sensitivity = 8f;
    public float vSpeed = 0f;
    [SerializeField] private InputActionReference changeModeReference;
    [SerializeField] private InputActionReference flyUpReference;
    [SerializeField] private InputActionReference flyDownReference;

    // Start is called before the first frame update
    void Start()
    {
        // _body = GetComponent<Rigidbody>();
        changeModeReference.action.performed += OnSwitchMode;
        flyUpReference.action.performed += OnFlyUpMode;
        flyDownReference.action.performed += OnFlyDownMode;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnSwitchMode(InputAction.CallbackContext obj) {
        Debug.Log("Switch Mode!");
    }
    void OnFlyUpMode(InputAction.CallbackContext obj) {
        Debug.Log("Fly Up Mode!");
        // vSpeed -= jumpHeight * -0.1f * gravityValue;
    }
    void OnFlyDownMode(InputAction.CallbackContext obj) {
        Debug.Log("Fly Down Mode!");
    }
}
