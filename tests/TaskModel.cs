using FHSDK.Sync;
using Newtonsoft.Json;

namespace tests
{
    public class TaskModel : IFHSyncModel
    {
        [JsonProperty("taskName")]
        public string TaksName { set; get; }

        [JsonProperty("completed")]
        public bool Completed { set; get; }

        [JsonIgnore]
        public string UID { set; get; }

        public override string ToString()
        {
            return $"[TaskModel: UID={UID}, TaksName={TaksName}, Completed={Completed}]";
        }

        protected bool Equals(TaskModel other)
        {
            return string.Equals(UID, other.UID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TaskModel) obj);
        }

        public override int GetHashCode()
        {
            return UID?.GetHashCode() ?? 0;
        }
    }
}