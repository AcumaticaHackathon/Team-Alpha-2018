using System;
using System.Collections.Generic;

namespace Jamis.Web.Face
{
    public interface IFaceApi
    {
        Guid[] Detect(byte[] imageData);

        IEnumerable<Candidate> Identify(string groupName, Guid[] faceIDs);

        void Train(string groupName);

        Guid AddPersonFace(Person person, byte[] imageData);

        PersonGroup CreateGroup(PersonGroup group);

        Person CreatePerson(Person person);

        void UpdatePerson(Person person);

        void DeleteGroup(PersonGroup group);

        void DeletePerson(Person person);

        void DeletePersonFace(Person person, Guid faceId);

        IEnumerable<PersonGroup> GetGroups();

        IEnumerable<Person> GetPersons(string groupName);
     
        void Close();
    }
}
