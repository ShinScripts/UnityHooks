using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;



public class WebHook : MonoBehaviour
{
    string webhook_url = "https://discord.com/api/webhooks/1315373704586203227/T-bYVN8wJksmWDGNnUGtmViW7-_QOzSmuFjTWAWV3UzA4SYrjhnS_rwsjbzvS0m1wnSm";

    private enum Colors : int
    {
        Default = 0,
        Aqua = 1752220,
        DarkAqua = 1146986,
        Green = 5763719,
        DarkGreen = 2067276,
        Blue = 3447003,
        DarkBlue = 2123412,
        Purple = 10181046,
        DarkPurple = 7419530,
        LuminousVividPink = 15277667,
        DarkVividPink = 11342935,
        Gold = 15844367,
        DarkGold = 12745742,
        Orange = 15105570,
        DarkOrange = 11027200,
        Red = 15548997,
        DarkRed = 10038562,
        Grey = 9807270,
        DarkGrey = 9936031,
        DarkerGrey = 8359053,
        LightGrey = 12370112,
        Navy = 3426654,
        DarkNavy = 2899536,
        Yellow = 16776960,
        White = 16777215,
        Greyple = 10070709,
        Black = 2303786,
        DarkButNotBlack = 2895667,
        NotQuiteBlack = 2303786,
        Blurple = 5793266,
        Fuchsia = 15418782,
    }

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
        public class Footer
        {
            public string text;
        }

#nullable enable
        public string? title = null;
        public string? description = null;
        public Colors? color = null;
        public Uri? url = null;
        public Footer? footer = null;
        public string? timestamp
        {
            get => _timestamp;
            set
            {
                if (DateTime.TryParse(value, out DateTime dateTime))
                {
                    _timestamp = dateTime.ToString("o", CultureInfo.InvariantCulture); // ISO 8601 format
                }
                else
                {
                    throw new Exception("The input string is not a valid DateTime format");
                }
            }
        }
        private string? _timestamp = null;
    }

    void Start()
    {
        Hook hook = new();
        hook.AddEmbed(new()
        {
            title = "title",
            description = "description",
            url = new Uri("https://www.github.com"),
            color = Colors.Orange,
            footer = new()
            {
                text = "footer"
            }
        });

        Debug.Log(hook.ToJson());
        hook.Send(webhook_url);
    }
}
