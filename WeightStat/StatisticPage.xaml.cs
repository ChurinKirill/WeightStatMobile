using DataManipulator;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace WeightStat;

public partial class StatisticPage : ContentPage
{

    private struct Axes
    {
        public List<string> xValues;
        public List<float> yValues;
        public RecordTime type;
    }

	DatabaseService dbService = new DatabaseService();

    public StatisticPage()
	{
		InitializeComponent();
		BuildBlot();
	}

	private List<Axes> GetAxesByRecordTime()
	{
		List<Axes> result = new List<Axes>();

		List<WeightRecord> records = dbService.GetAllRecords();

		foreach(int i in Enum.GetValues(typeof(RecordTime)))
		{
			List<string> dates = new List<string>();
			List<float> weights = new List<float>();

			foreach (var record in records)
			{
				if ((RecordTime)i == record.RecTime)
				{
					dates.Add(record.Date.ToString("dd.MM"));
					weights.Add(record.Weight);
				}
                
			}

			result.Add(new Axes
			{
				xValues = dates,
				yValues = weights,
				type = (RecordTime)i
			});
        }
		return result;
	}

	private void BuildBlot()
	{
		List<Axes> axes = GetAxesByRecordTime();

        var model = new PlotModel { Title = "WeightPlot" };

        var xAxes = new CategoryAxis
		{
			Position = AxisPosition.Bottom,
			Title = "Date",
			Key = "Dates",
			MajorGridlineStyle = LineStyle.LongDash,
			MajorGridlineColor = OxyColors.Gray,
			Angle = 45,
			FontSize = 16
		};

		foreach (var date in axes[0].xValues)
		{
			xAxes.Labels.Add(date);
		}

		model.Axes.Add(xAxes);

        model.Axes.Add(new LinearAxis
		{
			Position = AxisPosition.Left,
			Title = "Weight",
			MajorGridlineStyle = LineStyle.LongDash,
			MajorGridlineColor = OxyColors.Gray,
			FontSize = 16
		});

		// пока руками
        var series1 = new LineSeries
		{
			Color = OxyColors.Blue,
			StrokeThickness = 2,
			XAxisKey = "Dates",
			MarkerType = MarkerType.Circle,
		};

        var series2 = new LineSeries
        {
            Color = OxyColors.Orange,
            StrokeThickness = 2,
            XAxisKey = "Dates",
            MarkerType = MarkerType.Circle,
        };

		for (int i = 0; i < axes[0].xValues.Count; i++)
			series1.Points.Add(new DataPoint(i, axes[0].yValues[i]));

        for (int i = 0; i < axes[1].xValues.Count; i++)
            series2.Points.Add(new DataPoint(i, axes[1].yValues[i]));

		model.Series.Add(series1);
		model.Series.Add(series2);

		mainPlotWiew.Model = model;
    }

    private async void toHomeBtn_OnClick(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new MainPage());
	}
}