using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreatePost : MonoBehaviour
{
    [SerializeField] private string postPropEndpoint = "http://127.0.0.1:8080/api/proposition/create/";
    [SerializeField] private string updatePropEndpoint = "http://127.0.0.1:8080/api/proposition/update/";


    [Header("POST UI")]
    public GameObject PostUI;
    public GameObject UploadBTN;

    [Header("Texts")]
    [SerializeField] private TMP_InputField titleField;
    [SerializeField] private TMP_InputField descriptionField;



    [Header("Others")]
    public GameObject propositionPrefab;
    public RawImage chosenPhoto;


    

    #region OnPost
    public void OnPost()
    {
        TryAddPostToActivity(PropositionManager.Instance.AddPostToActivity);
    }

    private void TryAddPostToActivity(UnityAction<GameObject> callback)
    {

        var title = titleField.text;
        // var desc = descriptionField.text;

        var propositionElement = Instantiate(propositionPrefab);
        var activityElementConstructor = propositionElement.GetComponent<PropositionElement>();

        if (activityElementConstructor != null)
            activityElementConstructor.Init(title);

        StartCoroutine(UploadProposition(title));
        activityElementConstructor.photo.texture = chosenPhoto.texture;
        LoadProposition.Instance.Count++;

        callback.Invoke(propositionElement);
    }

    IEnumerator UploadProposition(string title)
    {
        string token_user = PlayerPrefs.GetString("token_user");


        WWWForm form = new WWWForm();
        form.AddField("name", title);

        UnityWebRequest request = UnityWebRequest.Post(postPropEndpoint + token_user, form);
        var handler = request.SendWebRequest();

        float startTime = 0.0f;
        while (!handler.isDone)
        {
            startTime += Time.deltaTime;

            if (startTime > 10.0f)
            {
                break;
            }

            yield return null;
        }

        if (request.result == UnityWebRequest.Result.Success)
        {

            PropositionData.responseCreateProp response = JsonUtility.FromJson<PropositionData.responseCreateProp>(request.downloadHandler.text);
            var prop_id = response._id;

            print(updatePropEndpoint + prop_id + "/" + token_user);

            UploadImageToServer
        .Initialize()
        .SetUrl(updatePropEndpoint + prop_id + "/" + token_user)
        .SetTexture((Texture2D)chosenPhoto.texture)
        .SetFieldName("image")
        .SetFileName("image")
        .SetType(ImageType.JPG)
        .OnError(error => Debug.Log(error))
        .OnComplete(text => Debug.Log(text))
        .Upload();

            yield return null;  
        }
    }
    #endregion


    #region OnPhotos 
    public void OnPhotos()
    {
        PickImage(512);
    }


    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                //add chosen image and disable uploadBTN
                chosenPhoto.texture = texture;
                chosenPhoto.texture.name = Path.GetFileName(path);
                UploadBTN.SetActive(false);

            }
        });

        Debug.Log("Permission result: " + permission);
    }

    #endregion OnPhotos 


}









