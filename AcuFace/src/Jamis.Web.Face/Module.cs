using Autofac;
using PX.Web.UI;

namespace Jamis.Web.Face
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new JSMainFrameScriptBatch(Scripts.Module.BaseKey, Scripts.Module.Key, Scripts.Module.Files, PX.Web.UI.JS.BaseKey));
        }
    }
}
