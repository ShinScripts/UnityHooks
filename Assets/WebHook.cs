using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Webhook
{
    private readonly Uri webhook_url = null;

    public Webhook(Uri webhook_url)
    {
        this.webhook_url = webhook_url;
    }

    public Webhook SetContent(string content)
    {
        this.content = content;
        return this;
    }

    public Webhook SetUsername(string username)
    {
        this.username = username;
        return this;
    }

    public Webhook AddEmbed(Embed embed)
    {
        embeds.Add(embed);
        return this;
    }

    public async Task<(UnityWebRequest.Result result, long responseCode, string error)> Send()
    {
        if (content == null && embeds.Count == 0)
        {
            throw new Exception("Content and embeds cannot be null, you need to provide at least one of them");
        }

        CheckEmbedLimits(embeds);

        UnityWebRequest request = new(webhook_url, "POST");

        string json = ToJson();
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(bytes);
        request.downloadHandler = new DownloadHandlerBuffer();

        await request.SendWebRequest();

        return (request.result, request.responseCode, request.downloadHandler.text);
    }

    private void CheckEmbedLimits(List<Embed> embeds)
    {
        if (embeds.Count > 10)
        {
            throw new Exception("You can only have up to 10 embeds per discords limits");
        }

        foreach (var embed in embeds)
        {
            if (embed.title?.Length > 256)
            {
                throw new Exception("Embed titles may only be up to 256 characters");
            }
            if (embed.description?.Length > 4096)
            {
                throw new Exception("Embed descriptions may only be up to 4096 characters");
            }
            if (embed.footer?.text.Length > 2048)
            {
                throw new Exception("Embed footer texts may only be up to 2048 characters");
            }
            if (embed.author?.name.Length > 256)
            {
                throw new Exception("Embed author names may only be up to 256 characters");
            }
            if (embed.fields.Count > 25)
            {
                throw new Exception("Embeds may only have up to 25 fields");
            }

            foreach (var field in embed.fields)
            {
                if (field.name.Length > 256)
                {
                    throw new Exception("Embed field names may only be up to 256 characters");
                }
                if (field.value.Length > 1024)
                {
                    throw new Exception("Embed field values may only be up to 1024 characters");
                }
            }
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
        });
    }

    public string username = null;
    public string content = null;
    public List<Embed> embeds = new();
}

public class Embed
{
    public Embed SetTitle(string title)
    {
        this.title = title;
        return this;
    }

    public Embed SetDescription(string description)
    {
        this.description = description;
        return this;
    }

    public Embed SetURL(Uri url)
    {
        this.url = url;
        return this;
    }

    public Embed SetTimestamp(string timestamp)
    {
        this.timestamp = timestamp;
        return this;
    }

    public Embed SetColor(Colors color)
    {
        this.color = color;
        return this;
    }

    public Embed SetFooter(string text, Uri icon_url = null)
    {
        footer = new()
        {
            text = text,
            icon_url = icon_url
        };
        return this;
    }

    public Embed SetImage(Uri url)
    {
        image = new()
        {
            url = url
        };
        return this;
    }

    public Embed SetThumbnail(Uri url)
    {
        thumbnail = new()
        {
            url = url
        };
        return this;
    }

    public Embed SetAuthor(string name, Uri url = null, Uri icon_url = null)
    {
        author = new()
        {
            name = name,
            url = url,
            icon_url = icon_url
        };
        return this;
    }

    public Embed AddField(string name, string value, bool inline = false)
    {
        fields.Add(new()
        {
            name = name,
            value = value,
            inline = inline
        });
        return this;
    }

    public string title = null;
    public string description = null;
    public Uri url = null;
    public string timestamp
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
    public Colors color = Colors.Default;
    public Footer footer = null;
    public Image image = null;
    public Thumbnail thumbnail = null;
    public Author author = null;
    public List<Field> fields = new();

    // Helpers
    public class Field
    {
        public string name = null;
        public string value = null;
        public bool inline = false;
    }

    public class Author
    {
        public string name = null;
        public Uri url = null;
        public Uri icon_url = null;
    }

    public class Image
    {
        public Uri url;
    }
    public class Thumbnail : Image { }
    public class Video : Image { }

    public class Footer
    {
        public string text;
        public Uri icon_url = null;
    }
    private string _timestamp = null;
}

public enum Colors : int
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