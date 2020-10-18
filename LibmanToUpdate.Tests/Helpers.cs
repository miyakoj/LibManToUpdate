using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibmanToUpdate.Tests
{
    internal class Helpers
    {
        public static string LibmanJson { 
            get {
                return GetLibmanJson();
            }
        }

        public static string InvalidJson { 
            get {
                return GetInvalidJson();
            }
        }

        public static string InvalidLibmanJson { 
            get {
                return GetInvalidLibmanJson();
            }
        }

        private static string GetLibmanJson() {
            return File.ReadAllText("TestLibman.json");
        }

        private static string GetInvalidJson() {
            return File.ReadAllText("InvalidTestJson.json");
        }

        private static string GetInvalidLibmanJson() {
            return File.ReadAllText("InvalidTestLibman.json");
        }
    }
}
