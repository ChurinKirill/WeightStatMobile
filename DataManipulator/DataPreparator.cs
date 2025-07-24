namespace DataManipulator
{
    public class DataPreparator
    {
        public static List<IntervalInfo> GetIntervalInfos(List<WeightRecord> records)
        {
            var res = new List<IntervalInfo>();

            var intervals = from record in records
                            group record by new { MonthNum = record.Date.Month, YearNum = record.Date.Year };

            foreach (var interval in intervals)
            {
                float minWeight = 200, maxWeight = 0;

                foreach (var intervalInfo in interval)
                {
                    if (intervalInfo.Weight > maxWeight)
                        maxWeight = intervalInfo.Weight;
                    if (intervalInfo.Weight < minWeight)
                        minWeight = intervalInfo.Weight;
                }
                res.Add(new IntervalInfo
                {
                    YearNum = interval.Key.YearNum,
                    MonthNum = interval.Key.MonthNum,
                    MinWeight = minWeight,
                    MaxWeight = maxWeight
                });
            }

            return res
                .OrderBy(r => r.YearNum)
                .OrderBy(r => r.MonthNum)
                .ToList();

        }

        public static List<Axis> GetAxisByRecordTime(List<WeightRecord> records)
        {
            var result = new List<Axis>();

            foreach (int i in Enum.GetValues(typeof(RecordTime)))
            {
                var dates = new List<DateTime>();
                var weights = new List<float>();

                foreach (var record in records)
                {
                    if ((RecordTime)i == record.RecTime)
                    {
                        dates.Add(record.Date);
                        weights.Add(record.Weight);
                    }

                }

                result.Add(new Axis
                {
                    xValues = dates,
                    yValues = weights,
                    type = (RecordTime)i
                });
            }
            return result;
        }
    }
}
