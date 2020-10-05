using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using RestSharp;

namespace LibmanToUpdate
{
    static class CDNService
    {
        private const string cdnjsEndpoint = "https://api.cdnjs.com/libraries/";
        private const string jsDelivrEndpoint = "https://data.jsdelivr.com/v1/package/npm/";
        private const string unpkgEndpoint = "https://unpkg.com/";

        public static LibraryUpdateData CheckLibrary(string provider, string library) {
            var client = new RestClient();
            client.ThrowOnAnyError = true;
            string endpoint = string.Empty;

            if (provider.Equals("cdnjs")) {
                endpoint = cdnjsEndpoint + library;
            }
            else if (provider.Equals("jsdelivr")) {
                endpoint = jsDelivrEndpoint + library;
            }
            else if (provider.Equals("unpkg")) {
                endpoint = unpkgEndpoint + library;
            }

            try {
                var request = new RestRequest(endpoint, Method.GET);
                var response = client.Execute(request);

                if (!response.IsSuccessful) {
                    return null;
                }

                string version = string.Empty;

                if (!provider.Equals("unpkg")) {
                    // all providers with an API
                    var parsedResponse = SimpleJson.DeserializeObject(response.Content);
                    JsonObject jsonObject = (JsonObject)parsedResponse;
                    version = string.Empty;

                    if (provider.Equals("cdnjs")) {
                        version = jsonObject["version"].ToString();
                    }
                    else if (provider.Equals("jsdelivr")) {
                        JsonObject tags = (JsonObject)jsonObject["tags"];
                        version = tags["latest"].ToString();
                    }
                }
                else if (provider.Equals("unpkg")) {
                    Uri responseUri = response.ResponseUri;
                    string[] versionTempArray = responseUri.Segments[1].Split("@");
                    string versionTemp = versionTempArray[1].ToString();
                    version = versionTemp.Remove(versionTemp.Length-1);
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
