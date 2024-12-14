namespace TheOneStudio.HyperCasual.Utils
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using TheOneStudio.HyperCasual.Extensions;
    using UnityEngine;
    using UnityEngine.Networking;

    public class WebRequestUtils
    {
        public async UniTask DownloadCSVRawString(string url, Action<string> OnWebRequestSuccess = null, Action<string> OnWebRequestFail = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                Debug.Log($"Get from url: {url}");
                await www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    OnWebRequestSuccess?.Invoke(www.error);
                    Debug.LogError(www.error);
                }

                string tsvRawString = www.downloadHandler.text;
                OnWebRequestSuccess?.Invoke(tsvRawString);
            }
        }

        public async UniTask<AudioClip> DownloadAudio(string url)
        {
            Debug.Log($"Download audio from url: {url}");
            #if FETCH_FROM_DRIVE
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url.ViewUrlToDownloadUrl(), AudioType.MPEG))
                #else
                        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
                #endif
            {
                await www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    return null;
                }

                return DownloadHandlerAudioClip.GetContent(www);
            }
        }
        public async UniTask<AudioClip> DownloadAudio(string url, CancellationToken token)
        {
            Debug.Log($"Download audio from url: {url}");
            #if FETCH_FROM_DRIVE
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url.ViewUrlToDownloadUrl(), AudioType.MPEG))
                #else
                        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
                #endif
            {
                await www.SendWebRequest().WithCancellation(token);

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                    return null;
                }

                return DownloadHandlerAudioClip.GetContent(www);
            }
        }
        public async UniTask<byte[]> DownloadMidiFile(string url)
        {
            #if FETCH_FROM_DRIVE
            using (UnityWebRequest www = UnityWebRequest.Get(url.ViewUrlToDownloadUrl()))
                #else
            using (UnityWebRequest www = UnityWebRequest.Get(url))
                #endif
            {
                await www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                }

                return www.downloadHandler.data;
            }
        }

        public async UniTask<Sprite> DownloadSprite(string url)
        {
            try
            {
                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url.SpriteUrlToDownloadUrl()))
                {
                    await request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Error downloading image: {request.error}");
                        return null;
                    }
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);

                    Sprite sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );

                    return sprite;
                }
            }
            catch (Exception)
            {
                Debug.LogError($"Error from download sprite: {url.SpriteUrlToDownloadUrl()}");
                return null;
            }
        }
    }
}