using System;

namespace Jamis.Web.Face.Scripts
{
    public static class Module
    {
        static public readonly Type BaseKey = typeof(Module);

        private const string path = "Jamis.Web.Face.Scripts.";

        public const string WebType = "application/x-javascript";

        public const string Key = "Jamis.Web.Face";

        public const string Script = path + "module.js";

        static public readonly string[] Files = new string[] { Script };
    }
}