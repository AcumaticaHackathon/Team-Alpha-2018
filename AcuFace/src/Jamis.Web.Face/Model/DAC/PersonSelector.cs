using PX.Data;
using System.Linq;

namespace Jamis.Web.Face
{
    public class PersonSelectorAttribute : PXCustomSelectorAttribute
    {
        private IFaceApi Api;

        public PXCache Cache { get; private set; }

        public PersonSelectorAttribute() : base(typeof(Person.name))
        {
        }

        public override void CacheAttached(PXCache sender)
        {
            Api = sender.GetFaceApi();

            Cache = sender;

            base.CacheAttached(sender);
        }

        public override void FieldVerifying(PXCache sender, PXFieldVerifyingEventArgs e)
        {
            // do not check against db
        }

        protected virtual System.Collections.IEnumerable GetRecords()
        {
            if (Api != null)
            {
                var groupName = (string)Cache.GetValue(Cache.Current, nameof(Person.groupName));
                if (groupName != null)
                {
                    return Api.GetPersons(groupName).Where(x => string.IsNullOrEmpty(x.Name) == false);
                }
            }

            return Enumerable.Empty<Person>();
        }
    }
}