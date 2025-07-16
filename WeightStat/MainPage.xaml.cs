using DataManipulator;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;


namespace WeightStat;

public partial class MainPage : ContentPage
{

    private DateTime _selectedRecordDate;
    //private RecordTime _selectedRecordTime = RecordTime.Undefined;
    private bool _doDelete = false;
    private string[] _times = Enum.GetNames(typeof(RecordTime));

    private struct Axes
    {
        public List<string> xValues;
        public List<float> yValues;
        public RecordTime type;
    }

    public DatabaseService DbService;

    private List<WeightRecord> _records = new List<WeightRecord>();

    public MainPage()
	{
		InitializeComponent();

        DbService = new DatabaseService();

        _selectedRecordDate = DateTime.Today;
	}

    private async void toDataEditBtn_OnClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DataEditPage());
    }

    private async void toStatisticBtn_OnClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatisticPage());
    }

    //private Axes GetAxes()
    //{
    //    List<string> dates = new List<string>();
    //    List<float> weights = new List<float>();

    //    foreach (var record in _records)
    //    {
    //        dates.Add(record.Date.ToString("dd.MM"));
    //        weights.Add(record.Weight);
    //    }

    //    return new Axes
    //    {
    //        xValues = dates,
    //        yValues = weights,
    //        type = RecordTime.Undefined
    //    };
    //}

    //private List<Axes> GetAxesByRecordTime()
    //{
    //    List<Axes> result = new List<Axes>();
    //    foreach (int i in Enum.GetValues(typeof(RecordTime)))
    //    {
    //        if (i != (int)RecordTime.Undefined)
    //        {
    //            List<string> dates = new List<string>();
    //            List<float> weights = new List<float>();

    //            foreach (var record in _records)
    //            {
    //                dates.Add(record.Date.ToString("dd.MM"));
    //                weights.Add(record.Weight);
    //            }

    //            result.Add(new Axes
    //            {
    //                xValues = dates,
    //                yValues = weights,
    //                type = (RecordTime)i
    //            });
    //        }
    //    }
    //    return result;
    //}

    //private void BuildSimplePlot()
    //{
    //    Axes axes = GetAxes();

    //    var model = new PlotModel { Title = "WeightPlot" };

    //    var xAxes = new CategoryAxis
    //    {
    //        Position = AxisPosition.Bottom,
    //        Title = "Date",
    //        Key = "Dates",
    //        MajorGridlineStyle = LineStyle.LongDash,
    //        MajorGridlineColor = OxyColors.Gray,
    //        Angle = 90,
    //        FontSize = 16
    //    };

    //    foreach (var date in axes.xValues)
    //    {
    //        xAxes.Labels.Add(date);
    //    }

    //    model.Axes.Add(xAxes);

    //    model.Axes.Add(new LinearAxis
    //    {
    //        Position = AxisPosition.Left,
    //        Title = "Weight",
    //        MajorGridlineStyle = LineStyle.LongDash,
    //        MajorGridlineColor = OxyColors.Gray,
    //        FontSize = 16
    //    });

    //    var series = new LineSeries
    //    {
    //        Color = OxyColors.Blue,
    //        StrokeThickness = 2,
    //        XAxisKey = "Dates",
    //        MarkerType = MarkerType.Circle,
    //    };

    //    for (int i = 0; i < axes.xValues.Count; i++)
    //    {
    //        series.Points.Add(new DataPoint(i, axes.yValues[i]));
    //    }

    //    model.Series.Add(series);


    //    //plotView.Model = model;
    //}


    //   private void CreateLineChart()
    //{
    //       if (chartView.Chart is not null)
    //           chartView.Chart = null;

    //       // Принудительное обновление UI
    //       Dispatcher.Dispatch(() => 
    //       {
    //           Axes axes = GetAxes();

    //           var entries = new List<ChartEntry>();
    //           for (int i = 0; i < axes.xValues.Count; i++)
    //           {
    //               entries.Add(new ChartEntry(axes.yValues[i])
    //               {

    //                   Label = axes.xValues[i].ToString("dd.MM.yyyy"),
    //                   ValueLabel = axes.yValues[i].ToString(),
    //                   Color = SKColor.Parse("#3498db") // Синий цвет
    //               });
    //           }
    //           var a = chartView.Chart;

    //           chartView.Chart = new LineChart
    //           {
    //               Entries = entries,
    //               LabelTextSize = 20,
    //               BackgroundColor = SKColors.Transparent,
    //               AnimationDuration = TimeSpan.FromSeconds(0)
    //           };
    //       });

    //   }

    //private void DateSelected(object sender, EventArgs e)
    //{
    //    _selectedRecordDate = datePicker.Date;
    //}

    //private void timePicker_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    _selectedRecordTime = (RecordTime)timePicker.SelectedIndex;
    //}

    //private void recordBtn_OnClicked(object sender, EventArgs e)
    //{
    //    if (_selectedRecordTime != RecordTime.Undefined && weightEntry.Text is not null)
    //    {
    //        if (_doDelete)
    //        {

    //        }
    //        else
    //        {
    //            float.TryParse(weightEntry.Text, out float weight);
    //            _records.Add(new WeightRecord()
    //            {
    //                Id = 1,
    //                Date = _selectedRecordDate,
    //                Weight = weight,
    //                RecTime = _selectedRecordTime
    //            });

    //            // С данными всё хорошо
    //            //infoLabel.Text = "";

    //            BuildSimplePlot();
    //        }
    //    }
    //    //else
    //    //{
    //    //    // Неполные данные
    //    //    infoLabel.Text = "Please enter all data";
    //    //}
    //}

    //private void doDeleteCheckbox_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    _doDelete = doDeleteCheckbox.IsChecked;

    //    recordBtn.Text = _doDelete ? "Delete" : "Add";

    //}
}

