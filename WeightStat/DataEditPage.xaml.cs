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

        deleteSelectedBtn.IsEnabled = false;
        cancelSelectionBtn.IsEnabled = false;
    }

    private void UpdateRecords()
    {
        List<WeightRecord> newRecords = dbService.GetAllRecords().OrderByDescending(r => r.Date).ToList();
        var setNewRecords = new HashSet<WeightRecord>(newRecords);

        // Удаление лишних элементов
        for (int i = WeightRecords.Count - 1; i >= 0; i--)
        {
            if (!setNewRecords.Contains(WeightRecords[i]))
                WeightRecords.RemoveAt(i);
        }

        // Добавление новых элементов
        foreach (var record in setNewRecords.OrderByDescending(r => r.Date))
        {
            if (!WeightRecords.Contains(record))
                WeightRecords.Add(record);
        }
        amountInfoLabel.Text = $"Total: {WeightRecords.Count} records";
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
            deleteSelectedBtn.IsEnabled = true;
            cancelSelectionBtn.IsEnabled = true;
            SelectedRecordInfoLabel.Text = $"Selected: {selectedRecord.RecTime.ToString()} {selectedRecord.Date.ToString("dd.MM")} - {selectedRecord.Weight}kg";
        }
        else
        {
            deleteSelectedBtn.IsEnabled = false;
            cancelSelectionBtn.IsEnabled = false;
            SelectedRecordInfoLabel.Text = "Nothing selected";
        }
    }
}