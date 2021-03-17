using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using Newtonsoft.Json;
using OMG.Service;
using static OMG.Service.Log;

namespace OMG.Util
{
    public static class DiscordHooks
    {
        static DiscordHooks()
        {
            Tuple<ulong, string> credentials = null;
            // Try if credential deserialization goes well
            try
            {
                // Deserialize credentials -> TODO: Change to more complex type after more hooks are introduced
                credentials = JsonConvert.DeserializeObject<Tuple<ulong, string>>(
                    File.ReadAllText(
                        Path.Join(SerializerPaths.ServerFolderPath, "discord" + SerializerPaths.FileFormat))
                    );
            }
            catch (Exception e)
            {
                // No credentials - disable functionality
                Logger.Error(e, $"Deserialization error for discord credentials");
            }

            if (credentials != null)
            {
                // Init WebhookClient
                StatusWebhookClient = new DiscordWebhookClient(
                    credentials.Item1,
                    credentials.Item2
                );
            }
        }
        // TODO: Use deserialization instead

        private static DiscordWebhookClient StatusWebhookClient { get; }

        public static async Task Status()
        {
            var embed = new EmbedBuilder
            {
                Title = "Test Embed",
                Description = "Test Description"
            };

            // Webhooks are able to send multiple embeds per message
            // As such, your embeds must be passed as a collection.
            if (StatusWebhookClient != null)
            {
                await StatusWebhookClient.SendMessageAsync("Send a message to this webhook!",
                    embeds: new[] {embed.Build()});
            }
        }
    }
}