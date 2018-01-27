using System;
using System.Collections.Generic;

namespace Jamis.Web.Face
{
    public class FaceApiFakeClient : IFaceApi
    {
        private static Guid Id1 = new Guid("3D6DEBC8-770F-4DE2-97FE-390B9F17FEA2");
        private static Guid Id2 = new Guid("198538EC-66CD-45F7-ADE2-FEB76F91C092");
        private static Guid Id3 = new Guid("D13F2DD1-2F0D-4A09-A955-06638AC04474");

        public void Close()
        {
            
        }

        public Guid AddPersonFace(Person person, byte[] data)
        {
            return Guid.NewGuid();
        }

        public PersonGroup CreateGroup(PersonGroup group)
        {
            return group;
        }

        public Person CreatePerson(Person person)
        {
            return person;
        }

        public void DeleteGroup(PersonGroup group)
        {
           
        }

        public void DeletePerson(Person person)
        {
           
        }

        public void DeletePersonFace(Person person, Guid faceId)
        {

        }

        public IEnumerable<PersonGroup> GetGroups()
        {
            return new PersonGroup[]
            {
                new PersonGroup { Name = "First" },
                new PersonGroup { Name = "Second" },
                new PersonGroup { Name = "Third" }
            };
        }

        public IEnumerable<Person> GetPersons(string groupName)
        {
            return new Person[]
            {
                new Person { Id = Id1, GroupName = groupName, Name = "First" },
                new Person { Id = Id2, GroupName = groupName, Name = "Second" },
                new Person { Id = Id3, GroupName = groupName, Name = "Third" }
            };
        }

        public void UpdatePerson(Person person)
        {
           
        }
    }
}
