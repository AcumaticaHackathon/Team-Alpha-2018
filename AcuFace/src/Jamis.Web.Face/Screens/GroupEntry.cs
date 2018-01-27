using PX.Data;
using System.Collections;
using System.Linq;

namespace Jamis.Web.Face.Screens
{
    public class GroupEntry : PXGraph<GroupEntry, PersonGroup>
    {
        [PXVirtualDAC]
        public PXSelect<PersonGroup> Groups;

        [PXVirtualDAC]
        public PXSelect<Person> Persons;

        private IFaceApi Api;

        public GroupEntry()
        {
            this.Api = this.GetFaceApi();
        }

        public IEnumerable groups()
        {
            return Api.GetGroups();
        }

        public IEnumerable persons()
        {
            var group = Groups.Current;

            if (string.IsNullOrEmpty(group?.Name))
            {
                return Enumerable.Empty<Person>();
            }

            if (Groups.Cache.GetStatus(group) == PXEntryStatus.Inserted)
            {
                return Enumerable.Empty<Person>();
            }

            return Api.GetPersons(group.Name);
        }
    }
}
