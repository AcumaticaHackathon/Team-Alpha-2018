using PX.Data;
using System;

namespace Jamis.Web.Face
{
    [PXVirtual]
    [Serializable]
    public class Person : IBqlTable
    {
        #region Id
        public abstract class id : IBqlField
        {
        }

        [PXGuid()]
        [PXUIField(DisplayName = "ID", Enabled = false)]
        public Guid? Id { get; set; }
        #endregion

        #region Name
        public abstract class name : IBqlField
        {
        }

        [PXDefault]
        [PXUIField(DisplayName = "Name", Required = true)]
        [PXString(50, IsUnicode = true, IsKey = true)]
        public string Name { get; set; }
        #endregion

        #region GroupName
        public abstract class groupName : IBqlField
        {
        }

        [PXDefault]
        [PXUIField(DisplayName = "Group", Required = true)]
        [PXString(50, IsUnicode = true, IsKey = true)]
        public string GroupName { get; set; }
        #endregion

        #region UserData
        public abstract class userData : IBqlField { }

        [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible)]
        [PXString(256, IsUnicode = true)]
        public string UserData { get; set; }
        #endregion

        #region NoteID

        public abstract class noteID : PX.Data.IBqlField
        {
        }

        [PXNote(BqlField = typeof(id))]
        public virtual Guid? NoteID
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }

        #endregion NoteID
    }
}
