using System;
using System.Collections.Generic;
using UnityEngine;


public class SendWebhook : MonoBehaviour
{
    private async void Awake()
    {
        Uri URL = new("https://cdn.discordapp.com/attachments/856270086816923648/908435073542545428/vectorstock_20239429.png?ex=6757ead7&is=67569957&hm=8b4cbe0f6403d41435d08df5ef0b65935a0862c5821bcc42eda7345a409a4b08&");
        Uri webhook_url = new("https://discord.com/api/webhooks/1315373704586203227/T-bYVN8wJksmWDGNnUGtmViW7-_QOzSmuFjTWAWV3UzA4SYrjhnS_rwsjbzvS0m1wnSm");

        Webhook hook = new(webhook_url);
        hook.SetContent("This is the content field");
        hook.SetUsername("Username for the hook");

        Webhook.Embed embed = new();
        embed.SetTitle("Title (with url)");
        embed.SetColor(Webhook.Colors.Red);
        embed.AddField(new()
        {
            name = "this is a field title",
            value = "this is a field value"
        });
        hook.AddEmbed(embed);

        hook.AddEmbed(new()
        {
            title = "Title (with url)",
            thumbnail = new()
            {
                url = URL
            },
            description = "description",
            url = new("https://www.github.com"),
            color = Webhook.Colors.Orange,
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
            fields = new List<Webhook.Embed.Field>() { new Webhook.Embed.Field() {
                name = "field 1",
                value = "field 1",
                inline = true
            },
            new Webhook.Embed.Field() {
                name = "field 2",
                value = "field 2",
                inline = true
            },
            new Webhook.Embed.Field() {
                name = "field 3",
                value = "field 3",
                inline = true
            }}
        });

        Debug.Log(hook.ToJson());
        var (result, responseCode) = await hook.Send();

        print($"Result: {result}, Response code: {responseCode}");

    }
}
