namespace EF.Learning.Storage.Entities
{
    public class Entity2
    {
        public string Entity2Id { get; set; } = string.Empty;
        public string? Payload { get; set; }

        public ICollection<DataLog> DataLogs { get; set; } = new List<DataLog>();
    }
}
