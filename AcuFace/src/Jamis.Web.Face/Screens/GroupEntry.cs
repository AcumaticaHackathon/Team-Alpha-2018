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
     
        protected virtual void PersonGroup_RowSelected(PXCache sedner, PXRowSelectedEventArgs e)
        {
            var group = Groups.Current;

            if (group != null)
            {
                var inserted = Groups.Cache.GetStatus(group) == PXEntryStatus.Inserted;

                Persons.AllowDelete = Persons.AllowUpdate = Persons.AllowInsert = !inserted;

                PXUIFieldAttribute.SetEnabled(Groups.Cache, group, inserted);

                PXUIFieldAttribute.SetEnabled<PersonGroup.name>(Groups.Cache, group, true);
            }
        }

        protected virtual void PersonGroup_RowDeleted(PXCache sedner, PXRowDeletedEventArgs e)
        {
            Groups.Cache.IsDirty = true;
        }

        protected virtual void PersonGroup_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            var group = e.Row as PersonGroup;

            if (string.IsNullOrEmpty(group?.Name) == false)
            {
                switch (e.Operation)
                {
                    case PXDBOperation.Delete:
                        Api.DeleteGroup(group);
                        break;
                    case PXDBOperation.Insert:
                        Api.CreateGroup(group);
                        break;
                }

                e.Cancel = true;
            }
        }

        protected virtual void Person_GroupName_FieldDefaulting(PXCache sedner, PXFieldDefaultingEventArgs e)
        {
            var group = Groups.Current;

            if (string.IsNullOrEmpty(group?.Name) == false)
            {
                e.NewValue = group.Name;
                e.Cancel = true;
            }
        }

        protected virtual void Person_RowSelected(PXCache sedner, PXRowSelectedEventArgs e)
        {
            var person = e.Row as Person;
            if (person != null)
            {
                PXUIFieldAttribute.SetEnabled(Persons.Cache, person, Persons.Cache.GetStatus(person) == PXEntryStatus.Inserted);
                PXUIFieldAttribute.SetEnabled<Person.name>(Persons.Cache, person, true);
            }
        }

        protected virtual void Person_RowDeleted(PXCache sedner, PXRowDeletedEventArgs e)
        {
            Persons.Cache.IsDirty = true;
        }

        protected virtual void Person_RowPersisting(PXCache sender, PXRowPersistingEventArgs e)
        {
            var group = Groups.Current;

            if (string.IsNullOrEmpty(group?.Name) == false)
            {
                if (Groups.Cache.GetStatus(group) != PXEntryStatus.Inserted)
                {
                    var person = e.Row as Person;

                    if (string.IsNullOrEmpty(person?.Name) == false)
                    {
                        switch (e.Operation)
                        {
                            case PXDBOperation.Delete:
                                Api.DeletePerson(person);
                                break;
                            case PXDBOperation.Insert:
                                Api.CreatePerson(person);
                                break;
                            case PXDBOperation.Update:
                                Api.UpdatePerson(person);
                                break;
                        }

                        e.Cancel = true;
                    }
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