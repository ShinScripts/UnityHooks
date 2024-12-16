using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendWebhook : MonoBehaviour
{
    public TMP_Text text_field;

    public async void Send()
    {
        Uri URL = new("add your image URL here");
        Uri webhook_url = new("add you discord webhook token here");

        Webhook hook = new(webhook_url);

        hook.AddEmbed(new()
        {
            title = "Title (with url)",
            thumbnail = new()
            {
                url = URL
            },
            description = "description",
            url = new("https://www.github.com"),
            color = Colors.Orange,
            footer = new()
            {
                text = "footer",
                icon_url = URL
            },
            image = new()
            {
                url = URL
            },
            author = new()
            {
                name = "author name",
                url = URL,
                icon_url = URL
            },
            timestamp = DateTime.UtcNow.ToString(),
            fields = new List<Embed.Field>() { new Embed.Field() {
                name = "field 1",
                value = "field 1",
                inline = true
            },
            new Embed.Field() {
                name = "field 2",
                value = "field 2",
                inline = true
            },
            new Embed.Field() {
                name = "field 3",
                value = "field 3",
                inline = true
            }}
        });

        var (result, responseCode, error) = await hook.Send();
        string log_message = $"Result: {result}, Response code: {responseCode}, Error: {error}";
        text_field.text = log_message;

        Debug.Log(hook.ToJson());
        Debug.Log(log_message);
    }
}
