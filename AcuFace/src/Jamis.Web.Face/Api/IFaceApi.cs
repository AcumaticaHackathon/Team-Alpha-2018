using System.Collections.Generic;

namespace Jamis.Web.Face
{
    public interface IFaceApi
    {
        PersonGroup CreateGroup(PersonGroup group);

        Person CreatePerson(Person person);

        void UpdatePerson(Person person);

        void DeleteGroup(PersonGroup group);

        void DeletePerson(Person person);

        IEnumerable<PersonGroup> GetGroups();

        IEnumerable<Person> GetPersons(string groupName);
     
        void Close();
    }
}
