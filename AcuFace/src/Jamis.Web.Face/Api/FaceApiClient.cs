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

        public void Close()
        {
            Store.Clear<PersonGroup>();
            Store.Clear<Person>();
            Client.Close();
        }
    }
}