using PX.Data;
using System.Linq;

namespace Jamis.Web.Face
{
    public class PersonGroupSelectorAttribute : PXCustomSelectorAttribute
    {
        private IFaceApi Api;

        public PersonGroupSelectorAttribute() : base(typeof(PersonGroup.name))
        {
        }

        public override void CacheAttached(PXCache sender)
        {
            Api = sender.GetFaceApi();

            base.CacheAttached(sender);
        }

        protected virtual System.Collections.IEnumerable GetRecords()
        {
            return (Api?.GetGroups() ?? Enumerable.Empty<PersonGroup>()).Where(x => string.IsNullOrEmpty(x.Name) == false);
        }
    }
}