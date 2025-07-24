using DataManipulator;

namespace WeightStat.Tests
{
    public class DataPreparationTests
    {
        [Fact]
        public void TestGettingIntervalInfos()
        {
            var In = new List<WeightRecord>() 
            {
                new WeightRecord()
                {
                    Id = 1,
                    Date = new DateTime(2025, 6, 1),
                    Weight = 53,
                    RecTime = RecordTime.Morning
                },
                new WeightRecord()
                {
                    Id = 2,
                    Date = new DateTime(2025, 6, 2),
                    Weight = 54,
                    RecTime = RecordTime.Morning
                },
                new WeightRecord()
                {
                    Id = 3,
                    Date = new DateTime(2025, 6, 3),
                    Weight = 55,
                    RecTime = RecordTime.Morning
                },
                new WeightRecord()
                {
                    Id = 4,
                    Date = new DateTime(2025, 7, 1),
                    Weight = 54,
                    RecTime = RecordTime.Morning
                },
                new WeightRecord()
                {
                    Id = 5,
                    Date = new DateTime(2025, 7, 2),
                    Weight = 56,
                    RecTime = RecordTime.Morning
                }
            };

            var Out = new List<IntervalInfo>() 
            {
                new IntervalInfo()
                {
                    YearNum = 2025,
                    MonthNum = 6,
                    MinWeight = 53,
                    MaxWeight = 55
                },
                new IntervalInfo()
                {
                    YearNum = 2025,
                    MonthNum = 7,
                    MinWeight = 54,
                    MaxWeight = 56
                }
            };


            var res = DataPreparator.GetIntervalInfos(In);


            Assert.Equal(res.Count, Out.Count);
            for (int i = 0; i < res.Count; i++)
            {
                Assert.Equal(Out[i].YearNum, res[i].YearNum);
                Assert.Equal(Out[i].MonthNum, res[i].MonthNum);
                Assert.Equal(Out[i].MinWeight, res[i].MinWeight);
                Assert.Equal(Out[i].MaxWeight, res[i].MaxWeight);
            }
        }

        [Fact]
        public void TestGettingAxisByRecordTime()
        {
            var In = new List<WeightRecord>()
            {
                new WeightRecord()
                {
                    Id = 1,
                    Date = new DateTime(2025, 6, 1),
                    Weight = 53,
                    RecTime = RecordTime.Morning
                },
                new WeightRecord()
                {
                    Id = 2,
                    Date = new DateTime(2025, 6, 1),
                    Weight = 54,
                    RecTime = RecordTime.Evening
                },
                new WeightRecord()
                {
                    Id = 3,
                    Date = new DateTime(2025, 6, 2),
                    Weight = 55,
                    RecTime = RecordTime.Morning
                },
                new WeightRecord()
                {
                    Id = 4,
                    Date = new DateTime(2025, 7, 1),
                    Weight = 54,
                    RecTime = RecordTime.Morning
                },
                new WeightRecord()
                {
                    Id = 5,
                    Date = new DateTime(2025, 7, 1),
                    Weight = 56,
                    RecTime = RecordTime.Evening
                }
            };

            var Out = new List<Axis>()
            {
                new Axis()
                {
                    xValues = new() { new DateTime(2025, 6, 1), new DateTime(2025, 6, 2), new DateTime(2025, 7, 1) },
                    yValues = new() { 53, 55, 54 },
                    type = RecordTime.Morning
                },
                new Axis()
                {
                    xValues = new() { new DateTime(2025, 6, 1), new DateTime(2025, 7, 1) },
                    yValues = new() { 54, 56 },
                    type = RecordTime.Evening
                }
            };


            var res = DataPreparator.GetAxisByRecordTime(In);


            Assert.Equal(res.Count, Out.Count);
            for (int i = 0; i < res.Count; i++)
            {
                Assert.Equal(Out[i].xValues, res[i].xValues);
                Assert.Equal(Out[i].yValues, res[i].yValues);
                Assert.Equal(Out[i].type, res[i].type);
            }
        }
    }
}