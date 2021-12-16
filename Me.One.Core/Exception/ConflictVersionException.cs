using System;
using Newtonsoft.Json;

namespace Me.One.Core.Exception
{
    [Serializable]
    public class ConflictVersionException : System.Exception
    {
        public ConflictVersionException()
        {
        }

        public ConflictVersionException(string message)
            : base(message)
        {
        }

        public ConflictVersionException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        public ConflictVersionException(
            string message,
            System.Exception inner,
            ulong currentVersion,
            ulong savingVersion)
            : base(message, inner)
        {
            CurrentVersion = currentVersion;
            SavingVersion = savingVersion;
        }

        public ulong CurrentVersion { get; set; }

        public ulong SavingVersion { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}