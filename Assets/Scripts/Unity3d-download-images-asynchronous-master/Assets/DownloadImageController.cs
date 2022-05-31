using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static ImageUtilties;

public class DownloadImageController : Singleton<DownloadImageController>
{
    public List<Texture2D> tex2D;
    LoadImage loadImageClass; 

    public void LoadImage(string url, string imageName, RawImage image = default(RawImage))
    {
        _ = DownloadImagesAsync(url,imageName,image);
    }

    public async UniTask DownloadImagesAsync( string url, string imageName, RawImage image = default(RawImage))
    {
         await DownloadJPGImage(url, imageName);
    }

    public async UniTask<Texture2D> DownloadJPGImage(string url , string name)
    {
        Texture2D img = await ImageDownloader.DownloadImage(url, name, FileFormat.JPG);
        img.name = name;
   
          tex2D.Add(img);
        return img;
    }

    public async UniTask<Sprite> DownloadPNGImage(string url, string name)
    {
        Texture2D img = await ImageDownloader.DownloadImage(url, name, FileFormat.PNG);
        return Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));
    }

    public void DestroyAllTextures()
    {
        foreach (Texture2D texture in tex2D)
        {
            Destroy(texture);
        }
        tex2D.Clear();
    }
}
