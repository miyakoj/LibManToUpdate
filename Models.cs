using System;
using System.Collections.Generic;
using System.Text;

namespace LibmanToUpdate
{
    class LibManEssentials
    {
        public string DefaultProvider {get; set;}
        public List<Library> Libraries {get; set;}
    }

    class Library {
        private string Name {get;}
        private string Version {get;}
        private string Provider {get;}

        public Library(string name, string version, string provider=null) {
            Name = name;
            Version = version;
            Provider = provider;
        }
    }
}
