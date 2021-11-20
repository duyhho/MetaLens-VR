using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Newtonsoft.Json;
using System;

public class PopUpWindow : MonoBehaviour
{
    public Texture2D[] allPeopleImagesArray;
    public GameObject[] emotionEffects;
    public GameObject currentTarget;
    GameObject currentEffect = null;
    public RawImage avatar;
    [SerializeField]
    RectTransform rectTransform;
    Texture2D currentImage;
    [SerializeField]
    Text[] likelihoodTexts;

    [Serializable]
    public class PredictionResult {
        [JsonProperty(PropertyName = "Sentiment")]
        public string sentiment;
        [JsonProperty(PropertyName = "Age")]
        public string age;
        [JsonProperty(PropertyName = "Ethnicity")]
        public string ethnicity;
        [JsonProperty(PropertyName = "Gender")]
        public string gender;
        [JsonProperty(PropertyName = "Age_Estimate")]
        public int ageEstimate;
    }
    // Start is called before the first frame update
    void Start()
    {
        // avatar = transform.Find("RawImage").gameObject.GetComponent<RawImage>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PredictEmotion() {
        StartCoroutine(ProcessRequest("https://hackaroo.ngrok.io/uploader", currentImage));
    }
    public void AssignTextureBasedOnName(GameObject target) {
        string targetName = target.name.ToLower();
        Debug.Log(target.name);
        currentTarget = target;
        currentImage = AssignPersonImage("random");

        //Assigns a photo//
        if (targetName.Contains("female")){
            
            currentImage = AssignPersonImage("female");
        }
        else if (targetName.Contains("male")) {
            currentImage = AssignPersonImage("male");
        }
        float scaleXtoY = (float) currentImage.width/(float) currentImage.height;
        
        avatar.texture = currentImage;
        rectTransform.localScale  = new Vector3(scaleXtoY * 2f, 2f, 0f );
    }
    Texture2D AssignPersonImage(string gender) {
        Texture2D imageToAssign = allPeopleImagesArray[UnityEngine.Random.Range (0, allPeopleImagesArray.Length)];
        string[] imageInfo = imageToAssign.name.Split('_');
        Debug.Log(gender);
        
        if (gender == "female") {
            Debug.Log("female");
            Debug.Log(imageInfo[1]);

            while (imageInfo[1] != "1" ) {
                imageToAssign = allPeopleImagesArray[UnityEngine.Random.Range(0, allPeopleImagesArray.Length)];
                imageInfo = imageToAssign.name.Split('_');
            }  
        }
        else if (gender == "male") {

            while (imageInfo[1] != "0" ) {
                imageToAssign = allPeopleImagesArray[UnityEngine.Random.Range (0, allPeopleImagesArray.Length)];
                imageInfo = imageToAssign.name.Split('_');

            }
        }
        Debug.Log(imageToAssign.name);
        return imageToAssign;
    }
    public void AssignRandomEmotion() {
        int r = UnityEngine.Random.Range(0, emotionEffects.Length);
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
        GameObject effectToAssign = new GameObject();
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
        // effectToAssign = emotionEffects[1];
        if (currentEffect != null) {
            GameObject.Destroy(currentEffect);
            currentEffect = null;
        }
        currentEffect = (GameObject) GameObject.Instantiate(effectToAssign, go.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        currentEffect.transform.parent = go.transform;
    }
    public PredictionResult GetPredictionResult(string jsonResponse)
    {
        //Valid: "https://dl.dropbox.com/s/4z4bzprj1pud3tq/Assets.json?dl=0"
        //Sample: https://dl.dropbox.com/s/fbh6jbyzrf86g0x/Assets-sample.json?dl=0

        PredictionResult info = JsonConvert.DeserializeObject<PredictionResult>(jsonResponse);
        // Debug.Log(info.payload.scannedCount);
        return info;
    }
    private IEnumerator ProcessRequest(string apiURL, Texture2D image)
    {

			// texture2D.Apply(false); // Not required. Because we do not need to be uploaded it to GPU
		Texture2D decompressed = DeCompress(image);
        byte[] jpg = decompressed.EncodeToJPG();
		List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("file", jpg, "upload.jpg" , "image/jpg"));

        using (UnityWebRequest request = UnityWebRequest.Post(apiURL, formData))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(jsonResponse);
                PredictionResult serverPredictionResult = GetPredictionResult(jsonResponse);
                DisplayText(serverPredictionResult);

                // Debug.Log(sentimentInfo.gender);
                AssignEmotion(currentTarget, serverPredictionResult.sentiment);
            }
            // TurnOffPopup();
        }
    }
    void DisplayText(PredictionResult result) {
        likelihoodTexts[0].text = result.sentiment;
        if (result.sentiment.ToLower() == "happy") {
            likelihoodTexts[0].color = Color.green;
        }
        else if (result.sentiment.ToLower() == "sad") {
            likelihoodTexts[0].color = Color.blue;
        }
        else if (result.sentiment.ToLower() == "angry") {
            likelihoodTexts[0].color = Color.red;
        }
        else if (result.sentiment.ToLower() == "neutral") {
            likelihoodTexts[0].color = new Color(135f/255f,206f/255f,235f/255f);
            // likelihoodTexts[0].color = Color.Red;
        }
        likelihoodTexts[1].text = result.ageEstimate.ToString();
        likelihoodTexts[2].text = result.gender;
        if (result.gender.ToLower() == "female") {
            likelihoodTexts[2].color = new Color(255f/255f,182f/255f,193f/255f);
        }
        else if (result.gender.ToLower() == "male") {
            likelihoodTexts[2].color = Color.blue;
        }
        likelihoodTexts[3].text = result.ethnicity;

    }
    public void ResetText(){
        likelihoodTexts[0].text = "N/A";
        likelihoodTexts[0].color = Color.black;

        likelihoodTexts[1].text =  "N/A";
        likelihoodTexts[1].color = Color.black;

        likelihoodTexts[2].text =  "N/A";
        likelihoodTexts[2].color = Color.black;

        likelihoodTexts[3].text =  "N/A";
        likelihoodTexts[3].color = Color.black;

    } 
    public Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
