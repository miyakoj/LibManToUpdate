using Newtonsoft.Json;
using System.Collections.Generic;

namespace LibmanToUpdate
{
    internal class LibManConfig
    {
        public string Version {get; set;}

        public string DefaultProvider {get; set;}

        public string SpecifiedProvider {get; set;}

        public string DefaultDestination {get; set;}

        public string SpecifiedDestination {get; set;}

        [JsonProperty(Required = Required.Always)]
        public List<LibraryModel> Libraries {get; set;}
    }

    internal class LibraryModel {
        [JsonProperty(Required = Required.Always)]
        public string Library {get; set;}

        public string Provider {get; set;}

        public string Destination {get; set;}

        public List<string> Files {get; set;}
    }

    internal class LibraryUpdateData {
        public string Library {get; set;}

        public string CurrentVersion {get; set;}

        public string MostRecentVersion {get; set;}
    }
}
