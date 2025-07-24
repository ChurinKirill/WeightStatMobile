using DataManipulator;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace WeightStat;

public partial class StatisticPage : ContentPage
{
	DatabaseService dbService = new DatabaseService();

	List<WeightRecord> Records;

    public StatisticPage()
	{
		InitializeComponent();
		UpdateRecords();
		BuildMainBlot();
		BuildMonthStatPlot();

    }

	private void UpdateRecords()
	{
		Records = dbService.GetAllRecords()
			.OrderBy(r => r.Date.Year)
			.OrderBy(r => r.Date.Month)
			.ToList();
    }

	private void BuildMainBlot()
	{
		var axis = DataPreparator.GetAxisByRecordTime(Records);
		var xValues = axis[0].xValues.Concat(axis[1].xValues).Distinct().ToList();

        var model = new PlotModel { Title = "WeightPlot" };

        var xAxis = new CategoryAxis
		{
			Position = AxisPosition.Bottom,
			Title = "Date",
			Key = "Dates",
			MajorGridlineStyle = LineStyle.LongDash,
			MajorGridlineColor = OxyColors.Gray,
			Angle = 45,
			FontSize = 16
		};

		foreach (var date in xValues)
		{
			xAxis.Labels.Add(date.ToString("dd.MM.yy"));
		}

		model.Axes.Add(xAxis);

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
			Title = "Morning",
			Color = OxyColors.Blue,
			StrokeThickness = 2,
			XAxisKey = "Dates",
			MarkerType = MarkerType.Circle,
		};

        var series2 = new LineSeries
        {
			Title = "Evening",
            Color = OxyColors.Orange,
            StrokeThickness = 2,
            XAxisKey = "Dates",
            MarkerType = MarkerType.Circle,
        };

		for (int i = 0; i < axis[0].xValues.Count; i++)
			series1.Points.Add(new DataPoint(xValues.FindIndex(x => x == axis[0].xValues[i]), axis[0].yValues[i]));

        for (int i = 0; i < axis[1].xValues.Count; i++)
            series2.Points.Add(new DataPoint(xValues.FindIndex(x => x == axis[1].xValues[i]), axis[1].yValues[i]));

		model.Series.Add(series1);
		model.Series.Add(series2);

		mainPlotView.Model = model;
    }

	private void BuildMonthStatPlot()
	{

		var model = new PlotModel 
		{
			Title = "MothStatPlot",
			IsLegendVisible = true
		};

        var legend = new Legend
        {
            LegendPlacement = LegendPlacement.Inside,
            LegendPosition = LegendPosition.BottomRight,
            LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
            LegendBorder = OxyColors.Black,
            Key = "Legend",
        };

		model.Legends.Add(legend);

        var intervals = DataPreparator.GetIntervalInfos(Records);

		var valueAxis = new LinearAxis
		{
			Position = AxisPosition.Bottom,
			Title = "Weight",
            MajorGridlineStyle = LineStyle.LongDash,
            MajorGridlineColor = OxyColors.Gray,
            FontSize = 16,
			AbsoluteMinimum = 0
        };
		
		var categoryAxis = new CategoryAxis
		{
			Position = AxisPosition.Left,
			Title = "Month",
			Angle = 45,
			FontSize = 16,
            ItemsSource = intervals.Select(i => $"{i.MonthNum}.{i.YearNum}").ToList()
		};

		model.Axes.Add(categoryAxis);
		model.Axes.Add(valueAxis);


		var series1 = new BarSeries
		{
			Title = "Min",
			StrokeColor = OxyColors.Black,
			FillColor = OxyColors.Blue,
			BarWidth = 1,
			StrokeThickness = 0.5,
			LabelPlacement = LabelPlacement.Outside,
			LabelFormatString = "{0:.00}",
			LegendKey = "Legend"
        };

        var series2 = new BarSeries
        {
            Title = "Max",
            StrokeColor = OxyColors.Black,
            FillColor = OxyColors.OrangeRed,
			BarWidth = 1,
            StrokeThickness = 0.5,
            LabelPlacement = LabelPlacement.Outside,
            LabelFormatString = "{0:.00}",
            LegendKey = "Legend"
        };

		foreach (var interval in intervals)
		{
			series1.Items.Add(new BarItem { Value = interval.MinWeight });
            series2.Items.Add(new BarItem { Value = interval.MaxWeight });
        }

		model.Series.Add(series1);
		model.Series.Add(series2);

		monthStatPlotView.Model = model;

    }
}