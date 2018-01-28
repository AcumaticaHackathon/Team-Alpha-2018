using PX.Data;
using System;

namespace Jamis.Web.Face
{
    [PXVirtual]
    [Serializable]
    public class Candidate : IBqlTable
    {
        #region PersonId
        public abstract class personId : IBqlField
        {
        }

        [PXGuid()]
        [PXUIField(DisplayName = "Person ID", Enabled = false)]
        public Guid? PersonId { get; set; }
        #endregion

        #region Confidence
        public abstract class confidence : IBqlField
        {
        }

        [PXDouble()]
        [PXUIField(DisplayName = "Confidence", Required = true)]
        public double? Confidence { get; set; }
        #endregion
    }
}