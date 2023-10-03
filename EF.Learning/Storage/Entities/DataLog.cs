namespace EF.Learning.Storage.Entities
{
    public class DataLog
    {
        public Guid MainId { get; set; }
        public long PartitionId { get; set; }
        public string? Payload { get; set; }

        public long? Entity1Id { get; set; }
        public string? Entity2Id { get; set; }

        public virtual Entity1? Entity1 { get; set; }
        public virtual Entity2? Entity2 { get; set; }
    }
}
