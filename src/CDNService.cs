using System;
using System.IO;
using RestSharp;

namespace LibmanToUpdate
{
    static class CDNService
    {
        private const string cdnjsEndpoint = "https://api.cdnjs.com/libraries/{0}";
        private const string jsDelivrEndpoint = "https://data.jsdelivr.com/v1/package/npm/{0}";
        private const string unpkgEndpoint = "https://unpkg.com/{0}";
        private static RestClient client;

        static CDNService () {
            client = new RestClient();
            client.ThrowOnAnyError = true;
        }

        public static LibraryUpdateData CheckLibrary(string provider, string library) {
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
                File.AppendAllText(App.LogFile, "Unknown provider " + provider + Environment.NewLine);
                throw new ArgumentException("Unknown provider " + provider);
            }

            try {
                var request = new RestRequest(endpoint, Method.GET);
                var response = client.Execute(request);

                if (!response.IsSuccessful) {
                    throw new System.Net.Http.HttpRequestException(response.ErrorMessage);
                }

                string version = string.Empty;
                object parsedResponse;
                bool parsedFlag = SimpleJson.TryDeserializeObject(response.Content, out parsedResponse);

                if (parsedFlag) {
                    // all providers with an API
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
                File.AppendAllText(App.LogFile, err.ToString()+ Environment.NewLine);
                return null;
            }
        }
    }
}
