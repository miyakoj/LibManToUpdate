using System;
using System.Runtime.CompilerServices;
using RestSharp;

[assembly: InternalsVisibleTo("LibmanToUpdate.Tests")]
namespace LibmanToUpdate
{
    internal static class CDNService
    {
        private const string cdnjsEndpoint = "https://api.cdnjs.com/libraries/{0}";
        private const string jsDelivrEndpoint = "https://data.jsdelivr.com/v1/package/npm/{0}";
        private const string unpkgEndpoint = "https://unpkg.com/{0}";
        private static RestClient client;

        static CDNService () {
            client = new RestClient();
            client.ThrowOnAnyError = true;
        }

        internal static LibraryUpdateData CheckLibrary(string provider, string library) {
            string endpoint;

            if (provider.Equals("cdnjs")) {
                endpoint = string.Format(cdnjsEndpoint, library);
            }
            else if (provider.Equals("jsdelivr")) {
                endpoint = string.Format(jsDelivrEndpoint, library);
            }
            else if (provider.Equals("unpkg")) {
                endpoint = string.Format(unpkgEndpoint, library);
            }
            else {
                System.IO.File.WriteAllText(App.LogFile, "Unknown provider " + provider);
                throw new ArgumentException("Unknown provider.");
            }

            try {
                var request = new RestRequest(endpoint, Method.GET);
                var response = client.Execute(request);

                if (!response.IsSuccessful) {
                    throw new System.Net.Http.HttpRequestException(response.ErrorMessage);
                }

                string version = string.Empty;

                if (!provider.Equals("unpkg")) {
                    // all providers with an API
                    var parsedResponse = SimpleJson.DeserializeObject(response.Content);
                    JsonObject jsonObject = (JsonObject)parsedResponse;

                    if (provider.Equals("cdnjs")) {
                        version = jsonObject["version"].ToString();
                    }
                    else if (provider.Equals("jsdelivr")) {
                        JsonObject tags = (JsonObject)jsonObject["tags"];
                        version = tags["latest"].ToString();
                    }
                }
                else {
                    // all providers without an API
                    if (provider.Equals("unpkg")) {
                        Uri responseUri = response.ResponseUri;
                        string[] versionTempArray = responseUri.Segments[1].Split("@");
                        string versionTemp = versionTempArray[1].ToString();
                        version = versionTemp.Remove(versionTemp.Length-1);
                    }
                }

                return new LibraryUpdateData {
                    Library = library,
                    MostRecentVersion = version
                };
            }
            catch (Exception err) {
                System.IO.File.WriteAllText(App.LogFile, err.ToString());
                return null;
            }
        }
    }
}
