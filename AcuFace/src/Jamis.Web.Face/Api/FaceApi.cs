using PX.Data;

namespace Jamis.Web.Face
{
    public static class FaceApi
    {
        public const string SubscriptionID = "993090911ad44e028fa4f5ed47f80fea";

        public const string ServiceUrl = "https://eastus.api.cognitive.microsoft.com/face/v1.0/";

        public static IFaceApi GetFaceApi(this PXCache cache)
        {
            return GetFaceApi(cache.Graph);
        }

        public static IFaceApi GetFaceApi(this PXGraph graph)
        {
            return new FaceApiClient(new FaceApiOxfordClient(), new FaceApiStore(graph));
        }
    }
}
