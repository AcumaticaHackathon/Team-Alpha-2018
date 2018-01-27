using PX.Data;

namespace Jamis.Web.Face
{
    public static class FaceApi
    {
        public static IFaceApi GetFaceApi(this PXCache cache)
        {
            return GetFaceApi(cache.Graph);
        }

        public static IFaceApi GetFaceApi(this PXGraph graph)
        {
            return new FaceApiFakeClient();
        }
    }
}
