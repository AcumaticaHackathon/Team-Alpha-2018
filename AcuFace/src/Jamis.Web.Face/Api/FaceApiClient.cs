using System;
using System.Collections.Generic;

namespace Jamis.Web.Face
{
    public class FaceApiClient : IFaceApi
    {
        private IFaceApi Client;

        private FaceApiStore Store;

        public FaceApiClient(IFaceApi client, FaceApiStore store)
        {
            this.Client = client;

            this.Store = store;
        }

        public IEnumerable<Candidate> Identify(string groupName, Guid[] faceIDs)
        {
            return Client.Identify(groupName, faceIDs);
        }

        public Guid[] Detect(byte[] imageData)
        {
            return Client.Detect(imageData);
        }

        public void Train(string groupName)
        {
            Client.Train(groupName);
        }


        public Guid AddPersonFace(Person person, byte[] data)
        {
            return Client.AddPersonFace(person, data);
        }

        public IEnumerable<PersonGroup> GetGroups()
        {
            return Store.Cache(Client.GetGroups);
        }

        public PersonGroup CreateGroup(PersonGroup item)
        {
            try
            {
                return Client.CreateGroup(item);
            }
            finally
            {
                Store.Clear<PersonGroup>();
            }
        }

        public void DeleteGroup(PersonGroup item)
        {
            Store.Clear<PersonGroup>();
            Client.DeleteGroup(item);
        }

        public IEnumerable<Person> GetPersons(string groupName)
        {
            return Store.Cache(groupName, Client.GetPersons);
        }

        public Person CreatePerson(Person item)
        {
            try
            {
                return Client.CreatePerson(item);
            }
            finally
            {
                Store.Clear<Person>(item.GroupName);
            }
        }

        public void UpdatePerson(Person item)
        {
            Client.UpdatePerson(item);
        }

        public void DeletePerson(Person item)
        {
            Store.Clear<Person>(item.GroupName);
            Client.DeletePerson(item);
        }

        public void DeletePersonFace(Person person, Guid faceId)
        {
            Client.DeletePersonFace(person, faceId);
        }

        public void Close()
        {
            Store.Clear<PersonGroup>();
            Store.Clear<Person>();
            Client.Close();
        }
    }
}