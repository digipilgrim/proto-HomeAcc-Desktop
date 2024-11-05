using System;

namespace PersAcc
{
    public partial class Transaction
    {
        public int Id { get; set; }

        public int? BucketId { get; set; }
        public virtual Bucket Bucket { get; set; }

        public double? Value { get; set; }

        public long? DateTime { get; set; }
        public DateTime? DateTimeFromTicks => new DateTime(DateTime.Value);

        public string? Description { get; set; }
    }
}
