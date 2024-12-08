using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
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
            embeds.Add(embed);
            return this;
        }

        public async void Send(string webhook_url)
        {
            if (content == null && embeds.Count == 0)
            {
                throw new Exception("Content and embeds cannot be null, you need to provide at least one of them");
            }

            if (embeds.Count > 10)
            {
                throw new Exception("You can only have up to 10 embeds per discords limits");
            }

            UnityWebRequest request = new(webhook_url, "POST");

            string json = ToJson();
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();

            print($"Result: {request.result}, Response code: {request.responseCode}");
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public string username = null;
        public string content = null;
        public List<Embed> embeds = new();
    }

    class Embed
    {
        // public class Footer
        // {
        //     public string text;
        // }

#nullable enable
        public string? title = null;
        public string? description = null;
        // public string url;
        // public Footer footer;
    }

    void Start()
    {
        Hook hook = new();
        // hook.AddEmbed(new()
        // {
        //     title = "title",
        //     description = "description"
        // });

        Debug.Log(hook.ToJson());
        hook.Send(webhook_url);
    }
}
