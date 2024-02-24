using System;
using System.Net.Http;
using UnityEngine;
using UnityEngine.UI;

public class GetAvatar : MonoBehaviour
{
    public static async void GetAvatarURL(string url, Image _avatar, Image _imageDefault)
    {
        try
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    var result = await response.Content.ReadAsByteArrayAsync();
                    Texture2D texture = new Texture2D(1, 1);
                    texture.LoadImage(result);
                    _avatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
        catch (Exception ex)
        {
            _avatar.sprite = _imageDefault.sprite;
            Debug.Log("Error: " + ex.Message.ToString());
        }
    }
}
