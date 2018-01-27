using PX.Data;
using System;

namespace Jamis.Web.Face
{
    [PXVirtual]
    [Serializable]
    public class PersonGroup : IBqlTable
    {
        #region Name
        public abstract class name : IBqlField
        {
        }

        [PXDefault]
        [PersonGroupSelector]
        [PXUIField(DisplayName = "Name", Required = true)]
        [PXString(50, IsUnicode = true, IsKey = true)]
        public string Name { get; set; }
        #endregion

        #region UserData
        public abstract class userData : IBqlField { }

        [PXUIField(DisplayName = "Description", Visibility = PXUIVisibility.SelectorVisible)]
        [PXString(256, IsUnicode = true)]
        public string UserData { get; set; }
        #endregion
    }
}
