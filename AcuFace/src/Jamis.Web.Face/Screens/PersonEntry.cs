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

        protected virtual void Person_RowDeleted(PXCache sedner, PXRowDeletedEventArgs e)
        {
            Persons.Cache.IsDirty = true;
        }

        protected virtual void Person_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            var person = (Person)e.Row;

            if (string.IsNullOrEmpty(person?.Name) == false)
            {
                if (string.IsNullOrEmpty(person?.GroupName) == false)
                {
                    switch (e.Operation)
                    {
                        case PXDBOperation.Delete:
                            Api.DeletePerson(person);
                            this.Clear();
                            break;
                        case PXDBOperation.Insert:
                            Api.CreatePerson(person);
                            this.Clear();
                            break;
                        case PXDBOperation.Update:
                            Api.UpdatePerson(person);
                            break;
                    }

                    e.Cancel = true;
                }
            }
        }

        public override void Unload()
        {
            Api.Close();
            base.Unload();
        }
    }
}