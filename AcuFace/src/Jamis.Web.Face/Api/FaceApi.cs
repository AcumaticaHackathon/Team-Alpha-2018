using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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


        public static IEnumerable<Candidate> IdentifyCandidates(this IFaceApi api, string imageDataUrl, string groupName)
        {
            if (imageDataUrl != null)
            {
                var strData = imageDataUrl.Split(',').Skip(1).FirstOrDefault();
                if (strData != null)
                {
                    var binData = Convert.FromBase64String(strData);
                    if (binData != null)
                    {
                        var faceIDs = api.Detect(binData);
                        if (faceIDs != null && faceIDs.Length > 0)
                        {
                            return api.Identify(groupName, faceIDs);
                        }
                    }
                }
            }

            return Enumerable.Empty<Candidate>();
        }

        public static Person IdentifyPerson(this IFaceApi api, string imageDataUrl, string groupName, double confidenceThreshold = 0.6)
        {
            var candidate = api.IdentifyCandidates(imageDataUrl, groupName).FirstOrDefault();

            if (candidate?.Confidence > confidenceThreshold)
            {
                return (Person)api.GetPersons(groupName).Where(x => x.Id == candidate.PersonId).FirstOrDefault();
            }

            return null;
        }
    }
}
