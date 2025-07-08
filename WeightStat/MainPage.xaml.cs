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
    private bool _isChartInitialized = false;

    private DateTime _selectedRecordDate;
    private RecordTime _selectedRecordTime = RecordTime.Undefined;
    private string[] _times = Enum.GetNames(typeof(RecordTime));

    private struct Axes
    {
        public List<string> xValues;
        public List<float> yValues;
    }

    public DatabaseService DbService;

    private List<WeightRecord> _records = new List<WeightRecord>();

    public MainPage()
	{
		InitializeComponent();
        this.Appearing += OnPageAppearing;

        DbService = new DatabaseService();

        _selectedRecordDate = DateTime.Today;
	}

    private void OnPageAppearing(object sender, EventArgs e)
    {
        if (!_isChartInitialized)
        {
            //CreateLineChart();
            _isChartInitialized = true;
        }
    }

    private Axes GetAxes()
    {
        List<string> dates = new List<string>();
        List<float> weights = new List<float>();

        foreach (var record in _records)
        {
            dates.Add(record.Date.ToString("dd.MM"));
            weights.Add(record.Weight);
        }

        return new Axes
        {
            xValues = dates,
            yValues = weights
        };
    }

    private void BuildPlot()
    {

        Axes axes = GetAxes();

        var model = new PlotModel { Title = "WeightPlot" };

        var xAxes = new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Date",
            Key = "Dates",
            MajorGridlineStyle = LineStyle.LongDash,
            MajorGridlineColor = OxyColors.Gray,
            Angle = 90
        };

        foreach (var date in axes.xValues)
        {
            xAxes.Labels.Add(date);
        }

        model.Axes.Add(xAxes);

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Weight",
            MajorGridlineStyle = LineStyle.LongDash,
            MajorGridlineColor = OxyColors.Gray
        });

        var series = new LineSeries
        {
            Color = OxyColors.Blue,
            StrokeThickness = 2,
            XAxisKey = "Dates",
            MarkerType = MarkerType.Circle
        };

        for (int i = 0; i < axes.xValues.Count; i++)
        {
            series.Points.Add(new DataPoint(i, axes.yValues[i]));
        }

        model.Series.Add(series);

        plotView.Model = model;
    }


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

    private void DateSelected(object sender, EventArgs e)
    {
        _selectedRecordDate = datePicker.Date;
    }

    private void timePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        _selectedRecordTime = (RecordTime)timePicker.SelectedIndex;
    }

    private void addRecordBtn_OnClicked(object sender, EventArgs e)
    {
        if (_selectedRecordTime != RecordTime.Undefined && weightEntry.Text is not null)
        {
            float.TryParse(weightEntry.Text, out float weight);
            _records.Add(new WeightRecord()
            {
                Id = 1,
                Date = _selectedRecordDate,
                Weight = weight,
                RecTime = _selectedRecordTime
            });
            try
            {
                BuildPlot();
            }
            catch (Exception ex)
            {
                var asdasd = ex;
            }
            
        }
        else
        {
            infoLabel.Text = "Please enter all data";
        }

    }
}

