using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jamis.Web.Face
{
    public class FaceApiOxfordClient : IFaceApi, IDisposable
    {
        private FaceServiceClient Api = new FaceServiceClient(FaceApi.SubscriptionID, FaceApi.ServiceUrl);
      
        public Guid[] Detect(byte[] imageData)
        {
            try
            {
                using (var stream = new MemoryStream(imageData))
                {
                    return Task.Run(() => Api.DetectAsync(stream)).GetAwaiter().GetResult().Select(x => x.FaceId).ToArray();
                }
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public void Train(string groupName)
        {
            try
            {
                Task.Run(() => Api.TrainPersonGroupAsync(groupName.ToLowerInvariant())).GetAwaiter().GetResult();
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public IEnumerable<PersonGroup> GetGroups()
        {
            try
            {
                return Task.Run(() => Api.GetPersonGroupsAsync()).GetAwaiter().GetResult().Select(x => new PersonGroup
                {
                    Name = x.Name,
                    UserData = x.UserData
                });
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public PersonGroup CreateGroup(PersonGroup item)
        {
            if (item != null)
            {
                try
                {
                    Task.Run(() => Api.CreatePersonGroupAsync(item.Name.ToLowerInvariant(), item.Name, item.UserData)).GetAwaiter().GetResult();
                }
                catch (FaceAPIException ex)
                {
                    throw new FaceApiException(ex.ErrorMessage);
                }
            }

            return item;
        }

        public void DeleteGroup(PersonGroup group)
        {
            if (group != null)
            {
                try
                {
                    Task.Run(() => Api.DeletePersonGroupAsync(group.Name.ToLowerInvariant())).GetAwaiter().GetResult();
                }
                catch (FaceAPIException ex)
                {
                    throw new FaceApiException(ex.ErrorMessage);
                }
            }
        }

        public IEnumerable<Person> GetPersons(string groupName)
        {
            try
            {
                return Task.Run(() => Api.GetPersonsAsync(groupName.ToLowerInvariant())).GetAwaiter().GetResult().Select(x => new Person
                {
                    Name = x.Name,
                    Id = x.PersonId,
                    GroupName = groupName,
                    UserData = x.UserData
                });
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public IEnumerable<Guid> GetPersonFaces(Person person)
        {
            try
            {
                return Task.Run(() => Api.GetPersonAsync(person.GroupName.ToLowerInvariant(), person.Id.Value)).GetAwaiter().GetResult().PersistedFaceIds;
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public Person CreatePerson(Person item)
        {
            try
            {
                var dto = Task.Run(() => Api.CreatePersonAsync(item?.GroupName.ToLowerInvariant(), item?.Name, item.UserData)).GetAwaiter().GetResult();

                if (dto?.PersonId != null)
                {
                    var person = new Person
                    {
                        Name = item.Name,
                        Id = dto.PersonId,
                        GroupName = item.GroupName,
                        UserData = item.UserData
                    };

                    return person;
                }
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }

            return null;
        }


        public void UpdatePerson(Person person)
        {
            try
            {
                Task.Run(() => Api.UpdatePersonAsync(person.GroupName.ToLowerInvariant(), person.Id.Value, person.Name, person.UserData)).GetAwaiter().GetResult();
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public void DeletePerson(Person person)
        {
            try
            {
                Task.Run(() => Api.DeletePersonAsync(person?.GroupName.ToLowerInvariant(), person?.Id ?? System.Guid.Empty)).GetAwaiter().GetResult();
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public Guid AddPersonFace(Person person, byte[] imageData)
        {
            try
            {
                using (var stream = new MemoryStream(imageData))
                {
                    return Task.Run(() => Api.AddPersonFaceAsync(person.GroupName.ToLowerInvariant(), person.Id.Value, stream)).GetAwaiter().GetResult().PersistedFaceId;
                }
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        public void DeletePersonFace(Person person, Guid faceId)
        {
            try
            {
                Task.Run(() => Api.DeletePersonFaceAsync(person.GroupName.ToLowerInvariant(), person.Id.Value, faceId)).GetAwaiter().GetResult();
            }
            catch (FaceAPIException ex)
            {
                throw new FaceApiException(ex.ErrorMessage);
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Api != null)
                    {
                        Api.Dispose();
                        Api = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        public void Close()
        {
            Api.Dispose();
        }
    }
}