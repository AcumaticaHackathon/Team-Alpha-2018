using PX.Common;
using PX.Data;
using PX.SM;
using PX.Web.UI;
using System;
using System.Collections;

namespace Jamis.Web.Face.Screens
{
    public class PersonEntry : PXGraph<PersonEntry, Person>
    {
        [PXVirtualDAC]
        public PXSelect<Person> Persons;

        private UploadFileMaintenance fileGraph;

        private IFaceApi Api;

        public PersonEntry()
        {
            this.Api = this.GetFaceApi();
        }

        protected UploadFileMaintenance FileGraph
        {
            get
            {
                if (this.fileGraph == null)
                {
                    this.fileGraph = PXGraph.CreateInstance<UploadFileMaintenance>();
                }

                return this.fileGraph;
            }
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

        protected virtual void Person_RowSelected(PXCache sedner, PXRowSelectedEventArgs e)
        {
            var person = Persons.Current;
            if (person != null)
            {
                uploadFile.SetEnabled(Persons.Cache.GetStatus(person) != PXEntryStatus.Inserted);
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

        public PXSelect<Person> NewFacePanel;
        public PXAction<Person> uploadFile;
        [PXUIField(DisplayName = "Add Face", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select, Visible = true)]
        [PXButton()]
        public virtual IEnumerable UploadFile(PXAdapter adapter)
        {
            this.Save.Press();

            if (this.NewFacePanel.AskExt() == WebDialogResult.OK)
            {
                var person = this.Persons.Current;

                if (person != null)
                {
                    const string PanelSessionKey = "FaceFile";

                    var info = PXContext.SessionTyped<PXSessionStatePXData>().FileInfo[PanelSessionKey] as PX.SM.FileInfo;

                    try
                    {
                        var faceId = Api.AddPersonFace(person, info.BinData);

                        info.UID = faceId;

                        try
                        {
                            info.UID = faceId;

                            SaveFile(info);
                        }
                        catch (Exception ex)
                        {
                            PXTrace.WriteError(ex);

                            Api.DeletePersonFace(person, faceId);
                        }
                    }
                    finally
                    {
                        System.Web.HttpContext.Current.Session.Remove(PanelSessionKey);
                    }
                }
            }

            return adapter.Get();
        }

        protected virtual void SaveFile(FileInfo file)
        {
            const string FilesField = "NoteFiles";

            var person = Persons.Current;

            if (person?.Id != null)
            {
                try
                {
                    PXBlobStorage.SaveContext = new PXBlobStorageContext
                    {
                        NoteID = person.Id,
                        ViewName = Persons.View.Name,
                        DataRow = person,
                        Graph = this
                    };

                    if (this.FileGraph.SaveFile(file, FileExistsAction.CreateVersion))
                    {
                        PXNoteAttribute.ForcePassThrow(Persons.Cache, null);

                        Persons.Cache.SetValueExt(person, FilesField, new Guid[] { file.UID.Value });
                    }
                }
                finally
                {
                    PXBlobStorage.SaveContext = null;
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