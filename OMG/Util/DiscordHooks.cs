using System.Threading.Tasks;
using Discord;
using Discord.Webhook;

namespace OMG.Util
{
    public static class DiscordHooks
    {
        // TODO: Use deserialization instead

        private static DiscordWebhookClient StatusWebhookClient { get; } = new DiscordWebhookClient(
            797912179923746856,
            "6AnkNpb2Ju4ijGHlQ8xeF8EPXXQCOFaEyYozQ72rQ-DiP8qBYiBRi_PqJ-p_ILZRDBhq"
        );

        public static async Task Status()
        {
            var embed = new EmbedBuilder
            {
                Title = "Test Embed",
                Description = "Test Description"
            };

            // Webhooks are able to send multiple embeds per message
            // As such, your embeds must be passed as a collection.
            await StatusWebhookClient.SendMessageAsync("Send a message to this webhook!",
                embeds: new[] {embed.Build()});
        }
    }
}