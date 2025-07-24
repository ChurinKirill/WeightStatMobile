using SQLite;

namespace DataManipulator
{
    public enum RecordTime
    {
        Morning,
        Evening
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

    public struct IntervalInfo
    {
        public int YearNum { get; set; }
        public int MonthNum { get; set; }
        public float MinWeight { get; set; }
        public float MaxWeight { get; set; }
    }

    public struct Axis
    {
        public List<DateTime> xValues;
        public List<float> yValues;
        public RecordTime type;
    }

}
