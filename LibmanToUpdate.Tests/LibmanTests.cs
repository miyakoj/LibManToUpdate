using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace LibmanToUpdate.Tests
{
    [TestClass]
    public class LibmanTests
    {
        [TestMethod]
        public void CanParseLibmanJson()
        {
            string json = Helpers.LibmanJson;
            LibManConfig libman = JsonConvert.DeserializeObject<LibManConfig>(json);
            Assert.IsNotNull(libman);
        }

        [TestMethod]
        public void ThrowsExceptionForInvalidJson()
        {
            JsonSerializationException exception = null;
            string json = Helpers.InvalidJson;
            
            try {
                LibManConfig libman = JsonConvert.DeserializeObject<LibManConfig>(json);
            }
            catch(JsonSerializationException e) {
                exception = e;
            }

            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void ThrowsExceptionForInvalidLibmanJson()
        {
            JsonReaderException exception = null;
            string json = Helpers.InvalidLibmanJson;
            
            try {
                LibManConfig libman = JsonConvert.DeserializeObject<LibManConfig>(json);
            }
            catch(JsonReaderException e) {
                exception = e;
            }

            Assert.IsNotNull(exception);
        }
    }
}
