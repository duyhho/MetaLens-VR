using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.Events.UnityAction;

public class XRPersonInteractable : MonoBehaviour
{
    public GameObject[] emotionEffects;
    public GameObject currentTarget;
    // List<int> scannedPeople;
    Dictionary<int, GameObject> scannedPeople = new Dictionary<int, GameObject>();
    Dictionary<string, int> scanInfo = new Dictionary<string, int>();
    GameObject currentEffect = null;
    [SerializeField]
    XRSimpleInteractable simpleInteractable;
    // [SerializeField]
    // XRGrabInteractable m_GrabInteractable;
    private XRBaseInteractor  m_MyFirstAction;
    bool m_Held;
    // Start is called before the first frame update
    void Start()
    {
        currentTarget = transform.gameObject;

        // m_GrabInteractable = GetComponent<XRGrabInteractable>();
        // m_GrabInteractable.firstHoverEntered.AddListener(OnFirstHoverEntered);
        // m_GrabInteractable.lastHoverExited.AddListener(OnLastHoverExited);
        // m_GrabInteractable.selectEntered.AddListener(OnSelectEntered);
        // m_GrabInteractable.selectExited.AddListener(OnSelectExited);
        //  m_MyFirstAction += AssignRandomEmotion;
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        simpleInteractable.selectEntered.AddListener(OnSelectEntered);
    }   

    // Update is called once per frame
    void Update()
    {
    }
    protected virtual void OnSelectEntered(SelectEnterEventArgs args)
    {
        // m_MeshRenderer.material.color = s_UnityCyan;
        m_Held = true;
        Debug.Log("Selected");

    }

    protected virtual void OnSelectExited(SelectExitEventArgs args)
    {
        // m_MeshRenderer.material.color = Color.white;
        m_Held = false;
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
    public void AssignRandomEmotion() {
        int r = Random.Range(0, emotionEffects.Length);
        string randomEmotion = "happy";
        if (r == 0) {
            randomEmotion = "happy";
        }
        else if (r == 1) {
            randomEmotion = "sad";
        }
        else if (r == 2) {
            randomEmotion = "angry";
        }
        else if (r == 3) {
            randomEmotion = "neutral";
        }
        AssignEmotion(currentTarget, randomEmotion);
        // TurnOffPopup();

    }
    void AssignEmotion(GameObject go, string emotion) {
        GameObject effectToAssign = null;
        if (!scannedPeople.ContainsKey(go.GetInstanceID())) {
            Debug.Log(go.name + " is " + emotion );

            if (emotion.ToLower() == "happy") {
            effectToAssign = emotionEffects[0];
            }
            else if (emotion.ToLower() == "sad") {
                effectToAssign = emotionEffects[1];
            }
            else if (emotion.ToLower() == "angry") {
                effectToAssign = emotionEffects[2];
            }
            else if (emotion.ToLower() == "neutral") {
                effectToAssign = emotionEffects[3];
            }
            // if (currentEffect != null) {
            //     GameObject.Destroy(currentEffect);
            //     currentEffect = null;
            // }
            // effectToAssign = emotionEffects[2];
            currentEffect = (GameObject) GameObject.Instantiate(effectToAssign, go.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            currentEffect.transform.parent = go.transform;
            // scannedPeople.Add(currentTarget.GetInstanceID(), currentEffect);
            // scanInfo[emotion] += 1;
            // Debug.Log("Number of Scanned People: " + scannedPeople.Count);
        }
    }



}
