using SQLite;

using Environment = System.Environment;

namespace DataManipulator
{
    public class DatabaseService
    {
        // Путь к бд в папке приложения
        string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "userdata.db3"
        );

        public DatabaseService()
        {
            InitializeDatabase();
        }

        private T GET<T>(Func<SQLiteConnection, T> func)
        {
            var connection = new SQLiteConnection(dbPath);

            return func(connection);

        }

        private void GET(Action<SQLiteConnection> func)
        {
            var connection = new SQLiteConnection(dbPath);

            func(connection);
        }

        public void InitializeDatabase() => GET(db =>
        {
            // Создаёт таблицу, если её нет
            db.CreateTable<WeightRecord>();
        });

        public List<WeightRecord> GetAllRecords() => GET(db =>
        {
            return db.Table<WeightRecord>().ToList();
        });

        public int DeleteRecord(WeightRecord record) => GET(db =>
        {
            return db.Delete<WeightRecord>(record.Id);
        });

        private List<int> GetAllPrimaryKeys() => GET(db =>
        {
            return db.Query<int>("SELECT id FROM records");
        });

        public int AddRecord(DateTime date, float weight, RecordTime recordTime) => GET(db =>
        {
            var record = FormRecord(date, weight, recordTime);
            return db.Insert(record);
        });

        private WeightRecord FormRecord(DateTime date, float weight, RecordTime recordTime)
        {
            List<int> ids = GetAllPrimaryKeys();
            int id = ids.Count > 0 ? ids.Max() + 1 : 1;
            return new WeightRecord
            {
                Id = id,
                Date = date,
                Weight = weight,
                RecTime = recordTime
            };
        }

    }
}
