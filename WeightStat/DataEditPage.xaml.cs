using DataManipulator;
using System.Collections.ObjectModel;

namespace WeightStat;

public partial class DataEditPage : ContentPage
{

    private DatabaseService dbService;
    public ObservableCollection<WeightRecord> WeightRecords;
    public List<string> recordTimes = new() { "morning", "evening" };

	public DataEditPage()
	{
        InitializeComponent();

        dbService = new DatabaseService();
        WeightRecords = new ObservableCollection<WeightRecord>();
        UpdateRecords();

        foreach (var time in  recordTimes)
            timePicker.Items.Add(time);

        listView.ItemsSource = WeightRecords;
        BindingContext = this;
    }

    private void UpdateRecords()
    {
        List<WeightRecord> newRecords = dbService.GetAllRecords();
        var setNewRecords = new HashSet<WeightRecord>(newRecords);

        // Удаление лишних элементов
        for (int i = WeightRecords.Count - 1; i >= 0; i--)
        {
            if (!setNewRecords.Contains(WeightRecords[i]))
                WeightRecords.RemoveAt(i);
        }

        // Добавление новых элементов
        foreach (var record in setNewRecords)
        {
            if (!WeightRecords.Contains(record))
                WeightRecords.Add(record);
        }
    }

    //private async void toHomeBtn_OnClick(object sender, EventArgs e)
    //{
    //    await Navigation.PushAsync(new MainPage());
    //}

    private void addRecordBtn_OnClick(object sender, EventArgs e)
    {
        DateTime date = datePicker.Date;
        bool convSucces = float.TryParse(weightEntry.Text, out float weight);
        int timeIdx = timePicker.SelectedIndex;

        if (convSucces && timeIdx != -1)
            dbService.AddRecord(date, weight, (RecordTime)timeIdx);

        UpdateRecords();
    }

    private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        UpdateSelectedRecordInfo();
    }

    private void cancelSelectionBtn_OnClick(object sender, EventArgs e)
    {
        listView.SelectedItem = null;
        UpdateSelectedRecordInfo();
    }

    private void deleteSelectedBtn_OnClick(object sender, EventArgs e)
    {
        if (listView.SelectedItem != null)
        {
            dbService.DeleteRecord(listView.SelectedItem as WeightRecord);
            listView.SelectedItem = null;
            UpdateRecords();
            UpdateSelectedRecordInfo();
        }
        
    }

    private void UpdateSelectedRecordInfo()
    {
        WeightRecord selectedRecord = listView.SelectedItem as WeightRecord;

        if (selectedRecord != null)
        {
            SelectedRecordInfoLabel.Text = $"Selected: {selectedRecord.RecTime.ToString()} {selectedRecord.Date.ToString("dd.MM")} - {selectedRecord.Weight}kg";
        }
        else
        {
            SelectedRecordInfoLabel.Text = "Nothing selected";
        }
    }
}