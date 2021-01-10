using System.Text.RegularExpressions;
using NWN.API;
using OMG.Util;

namespace OMG.Service.Chat
{
    public static class ChatProcessor
    {
        public static string ProcessChatMessage(string message)
        {
            // Remove spaces from end and beginning
            message = message.Trim();

            // Check if message starts with out-of-character sequence "//"
            if (message.StartsWith("//"))
            {
                // Message is out-of-character
                // Make it orange
                message = message.ColorString(Colors.Orange);
            }
            else
            {
                // Message is in-character
                // Emote is always a pair of '*'
                // Find texts between a pair of '*' using regular expression
                var emotes = Regex.Matches(message, @"\*.*?\*");

                // Replace each emote with colored version of string
                foreach (Match emote in emotes)
                {
                    //Color emote text
                    message = message.Replace(emote.Value, emote.Value.ColorString(Colors.NavyBlue));
                }
            }

            return message;
        }
    }
}