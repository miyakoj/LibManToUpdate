using System.Collections.Generic;

namespace LibmanToUpdate
{
    class LibManConfig
    {
        public string Version {get; set;}
        public string DefaultProvider {get; set;}
        public string DefaultDestination {get; set;}
        public List<LibraryModel> Libraries {get; set;}
    }

    class LibraryModel {
        public string Library {get; set;}
        public string Provider {get; set;}
        public string Destination {get; set;}
        public List<string> Files {get; set;}
    }

    class LibraryUpdateData {
        public string Library {get; set;}
        public string CurrentVersion {get; set;}
        public string MostRecentVersion {get; set;}
    }
}
