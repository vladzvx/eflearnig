namespace EF.Learning.Storage.Entities
{
    public class Entity1
    {
        public long Entity1Id { get; set; }
        public string? Payload { get; set; }

        public ICollection<DataLog> DataLogs { get; set; } = new List<DataLog>();
    }
}
