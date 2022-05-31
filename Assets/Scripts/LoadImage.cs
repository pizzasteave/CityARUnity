using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadImage : Singleton<LoadImage>
{
    public string currentPath;
    public List<Texture2D> tex = new List<Texture2D>();
    public IEnumerator OnLoadImageFromWebButtonClick(string imageURL, RawImage textureImage = default(RawImage), string imageName = default(string))
    {

        if (!File.Exists(currentPath))
        {
            yield return StartCoroutine(LoadTextureFromWeb(imageURL, textureImage, imageName));
        }
        else
            yield return StartCoroutine(LoadImageFromDisk(textureImage,imageName));

    }

    // enumerator to load texture from web URL
    IEnumerator LoadTextureFromWeb(string imageURL, RawImage textureImage = default(RawImage), string imageName = default(string))
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
            // cash rerferences of texture in memory 
            loadedTexture.name = imageName;
            tex.Add(loadedTexture);
            
            if(textureImage != null)
            {
                textureImage.texture = loadedTexture;

                // save it into cash 
                if (textureImage.texture == null)
                {
                    Debug.LogError("No Image to Save!");

                }
                print("ImageLoadedFromServer!");

                WriteImageOnDisk(textureImage);

            }
            
        }
    }

    IEnumerator LoadImageFromDisk(RawImage textureImage,string imageName = default(string))
    {

        byte[] textureBytes = File.ReadAllBytes(currentPath);

        //get texture from bytes
        Texture2D loadedTexture = new Texture2D(0, 0);
        loadedTexture.LoadImage(textureBytes);

        // cash rerferences of texture in memory 
        loadedTexture.name = imageName;
        tex.Add(loadedTexture);

        if (textureImage != null)
        {
            textureImage.texture = loadedTexture;

            print("ImageLoadedFromDisk!");

            yield return textureImage.texture;
            
            // green light for next button

        }
       
    }

    void WriteImageOnDisk(RawImage textureImage)
    {
        Texture2D tex2D = (Texture2D)textureImage.texture;
        byte[] textureBytes = tex2D.EncodeToPNG();
        File.WriteAllBytes(currentPath, textureBytes);


        print("File Written On Disk!");

    }


    public void DestroyAllTextures()
    {
        foreach (Texture2D texture in tex)
        {
            Destroy(texture);
        }
    }
}
