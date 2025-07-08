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

        public int AddRecord(WeightRecord record) => GET(db =>
        {
            return db.Insert(record);
        });

    }
}
