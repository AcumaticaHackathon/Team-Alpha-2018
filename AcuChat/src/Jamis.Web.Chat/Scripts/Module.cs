using System;

namespace Jamis.Web.Chat.Scripts
{
    public static class Module
    {
        static public readonly Type BaseKey = typeof(Module);

        private const string path = "Jamis.Web.Chat.Scripts.";

        public const string WebType = "application/x-javascript";

        public const string Key = "Jamis.Web.Chat";

        public const string Script = path + "module.js";

        static public readonly string[] Files = new string[] { Script };
    }

}