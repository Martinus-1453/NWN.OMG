namespace OMG.Util
{
    public class DiscordHooks
    {
        // TODO: Use deserialization instead
        public static WebHook StatusHook { get; } = new WebHook
        {
            WebHookString = @"https://discord.com/api/webhooks/797912179923746856/6AnkNpb2Ju4ijGHlQ8xeF8EPXXQCOFaEyYozQ72rQ-DiP8qBYiBRi_PqJ-p_ILZRDBhq",
            UserName = "Server Status Bot",
            ProfilePicture = @"https://findicons.com/files/icons/1202/futurama_vol_6_the_movies/128/hedionism_bot.png"
        };
    }
}