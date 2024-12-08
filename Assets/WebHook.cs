using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebHook : MonoBehaviour
{
    string webhook_url = "https://discord.com/api/webhooks/1315373704586203227/T-bYVN8wJksmWDGNnUGtmViW7-_QOzSmuFjTWAWV3UzA4SYrjhnS_rwsjbzvS0m1wnSm";

    class Hook
    {
        public Hook SetContent(string content)
        {
            this.content = content;
            return this;
        }

        public Hook SetUsername(string username)
        {
            this.username = username;
            return this;
        }

        public Hook AddEmbed(Embed embed)
        {
            embeds[0] = embed;
            return this;
        }

        public async void SendHook(string webhook_url)
        {
            if (content == null)
            {
                throw new Exception("Content cannot be null");
            }

            UnityWebRequest request = new(webhook_url, "POST");

            JsonSeria
            string json = JsonUtility.ToJson(this);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            Debug.Log(json);

            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();

            print(request.result);
            print(request.responseCode);
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public string username = "Webhook";
        public string content = null;
        public Embed[] embeds = new Embed[10];
    }

    class Embed
    {
        // public class Footer
        // {
        //     public string text;
        // }

        // public string? title;
        public string description;
        // public string url;
        // public Footer footer;
    }

    void Start()
    {
        Hook hook = new();
        hook.SetContent("content");
        hook.AddEmbed(new()
        {
            description = "description"
        });

        Debug.Log(hook.ToJson());
        // hook.SendHook(webhook_url);
    }
}
