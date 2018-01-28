using PX.Data;
using PX.SM;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var settings = new PXSetup<PreferencesSecurity>(graph);

            var subscriptionId = (string)settings.Cache.GetValue(settings.Current, "UsrFaceApiID");

            var sericeUrl = (string)settings.Cache.GetValue(settings.Current, "UsrFaceApiURL");

            return new FaceApiClient(new FaceApiOxfordClient(subscriptionId, sericeUrl), new FaceApiStore(graph));
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
