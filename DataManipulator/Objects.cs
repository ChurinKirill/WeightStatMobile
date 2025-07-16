using SQLite;

namespace DataManipulator
{
    public enum RecordTime
    {
        Morning,
        Evening
    }

    public enum ErrorCode
    {
        Ok,
        IncorrectInput,
        InternalError
    }

    [Table("records")]
    public class WeightRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Column("Date"), MaxLength(10)]
        public DateTime Date { get; set; }

        public float Weight { get; set; }

        public RecordTime RecTime { get; set; }
    }
}
