namespace EF.Learning.Storage.Entities
{
    public class DataLog
    {
        public Guid MainId { get; set; }
        public Guid? ParentMainId { get; set; }
        public long PartitionId { get; set; }
        public long? ParentPartitionId { get; set; }
        public string? Payload { get; set; }

        public long? Entity1Id { get; set; }
        public string? Entity2Id { get; set; }

        public virtual DataLog? Parent { get; set; }
        public virtual ICollection<DataLog>? Children { get; set; }
        public virtual Entity1? Entity1 { get; set; }
        public virtual Entity2? Entity2 { get; set; }
    }
}
