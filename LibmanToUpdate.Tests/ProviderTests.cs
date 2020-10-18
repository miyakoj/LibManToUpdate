using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibmanToUpdate.Tests
{
    [TestClass]
    public class ProviderTests
    {
        private const string testLibrary = "jquery";

        [TestMethod]
        public void CanDownloadFromCdnjs()
        {
            LibraryUpdateData result = CDNService.CheckLibrary("cdnjs", testLibrary);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanDownloadFromJsDeliver()
        {
            LibraryUpdateData result = CDNService.CheckLibrary("jsdelivr", testLibrary);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanDownloadFromUnpkg()
        {
            LibraryUpdateData result = CDNService.CheckLibrary("unpkg", testLibrary);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ThrowsErrorForUnknownProviders()
        {
            ArgumentException exception = null;

            try {
                CDNService.CheckLibrary("unknown", testLibrary);
            }
            catch (ArgumentException e) {
                exception = e;
            }
            
            Assert.IsNotNull(exception);
        }
    }
}
