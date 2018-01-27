using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jamis.Web.Face
{

    [Serializable]
    public class FaceApiException : Exception
    {
        public FaceApiException() : base("Face Api Error.") { }
        public FaceApiException(string message) : base("Face Api Error. " + message) { }
        public FaceApiException(string message, Exception inner) : base("Face Api Error. " + message, inner) { }
        protected FaceApiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}