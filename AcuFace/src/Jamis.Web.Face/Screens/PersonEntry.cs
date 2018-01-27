using PX.Data;
using System.Collections;

namespace Jamis.Web.Face.Screens
{
    public class PersonEntry : PXGraph<PersonEntry, Person>
    {
        [PXVirtualDAC]
        public PXSelect<Person> Persons;      

        private IFaceApi Api;

        public PersonEntry()
        {
            this.Api = this.GetFaceApi();
        }

        public IEnumerable persons()
        {
            foreach (var group in Api.GetGroups())
            {
                foreach (var person in Api.GetPersons(group.Name))
                {
                    yield return person;
                }
            }
        }
    }
}
