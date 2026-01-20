using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.FSharp.Core;
using System.Collections;

namespace WebsiteFirstDraft.Components.Pages
{
    public partial class Graphs
    {
        private GraphCategories? GraphCategory;
        
        private enum GraphCategories 
        {
            BodyweightProgress,
            CalorieTracking,
            Macros,
            Hypertrophy,
            Exercise,
            Consistency
        }

        private enum GraphName
        {
            BodyweightoverTime,
            WeightChangeperWeek,
            DailyCalorieIntakevsTarget,
            Dailycaloriesurplusordeficit,
            AverageMacroDistribution,
            DailyMacroIntake,
            CaloriesBurntThroughExercise,
            ExerciseTypeFrequency,
        }


        string errorMessage = String.Empty;

        // Reference to the chart component instance (initialised later)
        private LineChart? lineChart = default!;
        private BarChart? barChart = default!;
        private PieChart? pieChart = default!;

        // Configuration options for the charts
        private LineChartOptions? lineChartOptions = default!;
        private BarChartOptions? barChartOptions = default!;
        private PieChartOptions? pieChartOptions = default!;

        // Data model (labels + datasets) for the chart
        private ChartData chartData = default!;

        // Counter for how many datasets have been created
        private int datasetsCount;

        // Counter for how many labels (data points) have been created
        private int labelsCount;

        // Random number generator used to create sample data
        private Random random = new();

        private bool baseline = true;

        private int selectedGraph;

        private int previousGraph = -1;

        // ✅ Track whether a graph has been loaded
        private bool isGraphLoaded = false;

        // Lifecycle method: initialize component state before rendering
        protected override void OnInitialized()
        {
            selectedGraph = (int)GraphName.BodyweightoverTime;
            // ✅ Don't initialize graph on page load - wait for user selection
            // InitializeSelectedGraph();
        }

        // Lifecycle method: runs after component has rendered
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // ✅ Only initialize if graph has been explicitly loaded
            if (isGraphLoaded && (firstRender || previousGraph != selectedGraph))
            {
                previousGraph = selectedGraph;
                await InitializeChart();
            }
            
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnGraphSelected()
        {
            await InitializeSelectedGraph();
            isGraphLoaded = true; // ✅ Mark graph as loaded
            StateHasChanged();
            
            // Wait for the next render cycle to complete before updating
            await Task.Delay(100);
            await UpdateChart();
        }

        private async Task InitializeSelectedGraph()
        {
            // Reset counters
            labelsCount = 0;
            datasetsCount = 0;

            switch (selectedGraph)
            {
                case 0:
                    await InitialiseBodyweightOverTimeGraph();
                    break;
                case 1:
                    await InitialiseWeightChangePerWeekGraph();
                    break;
                case 2:
                    await InitialiseDailyCalorieIntakeVsTargetGraph();
                    break;
                case 3:
                    await InitialiseDailyCalorieSurplusorDeficitGraph();
                    break;
                case 4:
                    await InitialiseMacroDistributionGraph();
                    break;
                case 5:
                    await InitialiseDailyMacroIntakeGraph();
                    break;
                case 6:
                    await InitialiseCaloriesBurntThroughExerciseGraph();
                    break;
                case 7:
                    InitialiseExerciseTypeFrequencyGraph();
                    break;
                default:
                    await InitialiseWeightChangePerWeekGraph();
                    break;
            }
        }

        // Helper method to determine if the selected graph is a bar chart
        private bool IsBarChart() => selectedGraph == 3 || selectedGraph == 5 || selectedGraph == 7;

        // Helper method to determine if the selected graph is a pie chart
        private bool IsPieChart() => selectedGraph == 4;

        // Method to initialize the chart based on the selected graph type
        private async Task InitializeChart()
        {

            // Initialize the appropriate chart type
            if (IsBarChart())
            {
                if (barChart != null)
                {
                    await barChart.InitializeAsync(chartData, barChartOptions);
                }
            }
            else if (IsPieChart())
            {
                if (pieChart != null)
                {
                    await pieChart.InitializeAsync(chartData, pieChartOptions);
                }
            }
            else
            {
                if (lineChart != null)
                {
                    await lineChart.InitializeAsync(chartData, lineChartOptions);
                }
            }
        }

        // Method to update the chart with new data
        private async Task UpdateChart()
        {
            // Update the appropriate chart type
            if (IsBarChart())
            {
                if (barChart != null)
                {
                    await barChart.UpdateAsync(chartData, barChartOptions);
                }
            }
            else if (IsPieChart())
            {
                if (pieChart != null)
                {
                    await pieChart.UpdateAsync(chartData, pieChartOptions);
                }
            }
            else
            {
                if (lineChart != null)
                {
                    await lineChart.UpdateAsync(chartData, lineChartOptions);
                }
            }
        }

        private async Task InitialiseWeightChangePerWeekGraph()
        {
            try
            {
                // Query real data from database
                var user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Username == Session.UserSession.Username);

                if (user == null)
                {
                    errorMessage = "User not found. Please log in.";
                    InitializeEmptyLineChart("Weight Change (kg)");
                    return;
                }

                // Get all weight logs for the user
                var weightLogs = await Db.Weight_Logs
                    .Where(w => w.UserId == user.User_id)
                    .OrderBy(w => w.LogDate)
                    .ToListAsync();

                // Check if we have any data
                if (weightLogs.Count == 0)
                {
                    errorMessage = "No weight data available. Start logging your weight to see weekly trends!";
                    InitializeEmptyLineChart("Weight Change (kg)");
                    return;
                }

                // Number of weeks to analyze (configurable)
                const int numberOfWeeks = 7;

                // Calculate week start dates (going back from today)
                var weeklyData = new List<(string WeekLabel, double AverageWeight)>();
                var today = DateTime.Today;

                for (int weekOffset = numberOfWeeks - 1; weekOffset >= 0; weekOffset--)
                {
                    // Calculate the start and end of each week
                    var weekStart = today.AddDays(-7 * weekOffset - 6);
                    var weekEnd = today.AddDays(-7 * weekOffset);

                    // Get all weight logs within this week
                    var logsInWeek = weightLogs
                        .Where(w => w.LogDate.Date >= weekStart && w.LogDate.Date <= weekEnd)
                        .ToList();

                    // If there are entries for this week, calculate the mean
                    if (logsInWeek.Count != 0)
                    {
                        var averageWeight = logsInWeek.Average(w => w.Weight);
                        var weekLabel = $"Week {numberOfWeeks - weekOffset}";
                        weeklyData.Add((weekLabel, averageWeight));
                    }
                }

                if (weeklyData.Count == 0)
                {
                    errorMessage = "Not enough weight data to calculate weekly changes.";
                    InitializeEmptyLineChart("Weight Change (kg)");
                    return;
                }

                // Calculate week-to-week changes
                var labels = new List<string>();
                var weightChanges = new List<double?>();


                for (int i = 1; i < weeklyData.Count; i++)
                {
                    var change = weeklyData[i].AverageWeight - weeklyData[i - 1].AverageWeight;
                    labels.Add(weeklyData[i].WeekLabel);
                    weightChanges.Add(change);
                }

                // Handle case where we only have one week of data
                if (weightChanges.Count == 0)
                {
                    errorMessage = "Need at least 2 weeks of data to show weight changes.";
                    InitializeEmptyLineChart("Weight Change (kg)");
                    return;
                }

                // Calculate safe min/max for the Y-axis
                var minChange = weightChanges.Where(d => d.HasValue).Select(d => d!.Value).DefaultIfEmpty(0).Min();
                var maxChange = weightChanges.Where(d => d.HasValue).Select(d => d!.Value).DefaultIfEmpty(0).Max();
                var yAxisPadding = Math.Max(0.5, Math.Abs(Math.Max(Math.Abs(minChange), Math.Abs(maxChange))) * 0.2);

                // Create baseline data (all zeros) matching the number of labels
                var baselineData = new List<double?>();
                for (int i = 0; i < labels.Count; i++)
                {
                    baselineData.Add(0);
                }

                chartData = new ChartData
                {
                    Labels = labels,
                    Datasets =
                    [
                        new LineChartDataset
                        {
                            Label = "Weight Change (kg)",
                            Data = weightChanges,
                            BackgroundColor = ColorUtility.CategoricalTwelveColors[2],
                            BorderColor = ColorUtility.CategoricalTwelveColors[2],
                            PointRadius = [5],
                            PointHoverRadius = [8]
                        },
                        new LineChartDataset
                        {
                            Label = "Baseline",
                            Data = baselineData,
                            BackgroundColor = ColorUtility.CategoricalTwelveColors[3],
                            BorderColor = ColorUtility.CategoricalTwelveColors[3],
                            PointRadius = [0],
                            PointHoverRadius = [0],
                            BorderDash = [5, 5]
                        }
                    ]
                };

                lineChartOptions = new LineChartOptions
                {
                    IndexAxis = "x",
                    Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                    Responsive = true,
                    Scales = new Scales
                    {
                        Y = new()
                        {
                            BeginAtZero = true,
                            Min = minChange - yAxisPadding,
                            Max = maxChange + yAxisPadding
                        }
                    }
                };

                // Clear error message on success
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading weight change data: {ex.Message}";
                System.Console.WriteLine($"Error in InitialiseWeightChangePerWeekGraph: {ex.Message}");
                InitializeEmptyLineChart("Weight Change (kg)");
            }
        }

        private async Task InitialiseBodyweightOverTimeGraph()
        {
            try
            {
                // Query real data from database
                var user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Username == Session.UserSession.Username);

                if (user == null)
                {
                    errorMessage = "User not found. Please log in.";
                    // Initialize with empty/default data to prevent null reference
                    InitializeEmptyLineChart("Body Weight (kg)");
                    return;
                }

                // Get historical weight data
                var weightLogs = await Db.Weight_Logs
                    .Where(w => w.UserId == user.User_id)
                    .GroupBy(w => w.LogDate.Date)

                    // Ensures only 1 log per day is selected, the latest one
                    .Select(g => new 
                    { 
                        LogDate = g.Key,
                        g.OrderByDescending(x => x.LogDate).First().Weight 
                    })

                    .OrderBy(w => w.LogDate)
                    .Take(7)  // Last 7 days
                    .ToListAsync();

                // Check if we have any data
                if (weightLogs == null || weightLogs.Count == 0)
                {
                    errorMessage = "No weight data available. Start logging your weight to see trends!";
                    // Initialize with empty/default data
                    InitializeEmptyLineChart("Body Weight (kg)");
                    return;
                }

                // Extract data safely
                var labels = weightLogs.Select(w => w.LogDate.ToString("MM/dd")).ToList();
                var data = weightLogs.Select(w => (double?)w.Weight).ToList();

                // Calculate safe min/max with fallbacks
                var minWeight = data.Where(d => d.HasValue).Select(d => d!.Value).DefaultIfEmpty(0).Min();
                var maxWeight = data.Where(d => d.HasValue).Select(d => d!.Value).DefaultIfEmpty(100).Max();

                chartData = new ChartData
                {
                    Labels = labels,
                    Datasets = new List<IChartDataset>
                    {
                        new LineChartDataset
                        {
                            Label = "Body Weight (kg)",
                            Data = data,
                            BackgroundColor = ColorUtility.CategoricalTwelveColors[0],
                            BorderColor = ColorUtility.CategoricalTwelveColors[0],
                            PointRadius = new List<double> { 5 },
                            PointHoverRadius = new List<double> { 8 }
                        }
                    }
                };

                lineChartOptions = new LineChartOptions
                {
                    IndexAxis = "x",
                    Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                    Responsive = true,
                    Scales = new Scales
                    {
                        Y = new()
                        {
                            BeginAtZero = false,
                            Min = Math.Max(0, minWeight - 5),
                            Max = maxWeight + 5
                        }
                    }
                };

                // Clear error message on success
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading weight data: {ex.Message}";
                System.Console.WriteLine($"Error in InitialiseBodyweightOverTimeGraph: {ex.Message}");
                //Initialize with empty data to prevent crashes
                InitializeEmptyLineChart("Body Weight (kg)");
            }
        }

        /// <summary>
        /// Initializes an empty line chart with default/placeholder data
        /// </summary>
        private void InitializeEmptyLineChart(string label)
        {
            chartData = new ChartData
            {
                Labels = new List<string> { "No Data" },
                Datasets = new List<IChartDataset>
                {
                    new LineChartDataset
                    {
                        Label = label,
                        Data = new List<double?> { 0 },
                        BackgroundColor = ColorUtility.CategoricalTwelveColors[0],
                        BorderColor = ColorUtility.CategoricalTwelveColors[0],
                        PointRadius = new List<double> { 5 },
                        PointHoverRadius = new List<double> { 8 }
                    }
                }
            };

            lineChartOptions = new LineChartOptions
            {
                IndexAxis = "x",
                Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                Responsive = true,
                Scales = new Scales
                {
                    Y = new()
                    {
                        BeginAtZero = true,
                        Min = 0,
                        Max = 100
                    }
                }
            };
        }

        private async Task InitialiseDailyCalorieIntakeVsTargetGraph()
        {
            try
            {
                // Query real data from database
                var user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Username == Session.UserSession.Username);

                if (user == null)
                {
                    errorMessage = "User not found. Please log in.";
                    InitializeEmptyLineChart("Daily Calorie Intake");
                    return;
                }

                // Get the user's maintenance calorie target
                var maintenanceTarget = user.Maintenance_Calories;

                if (maintenanceTarget == 0)
                {
                    errorMessage = "No maintenance calorie target set. Please complete the Maintenance Calories Calculator.";
                    InitializeEmptyLineChart("Daily Calorie Intake");
                    return;
                }

                // Get calorie logs for the past 6 days
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-5); // 6 days total including today

                var calorieLogs = await Db.Calorie_Logs

                    // Checks Logs in the given date range
                    .Where(c => c.User_id == user.User_id && c.Log_Date.Date >= startDate && c.Log_Date.Date <= endDate)
                    .GroupBy(c => c.Log_Date.Date)
                    .Select(g => new
                    {
                        LogDate = g.Key,
                        TotalNetCalories = g.Sum(c => c.Net_Calories)
                    })
                    .OrderBy(c => c.LogDate)
                    .ToListAsync();

                // Create labels and data for all 6 days (including days with no logs)
                var labels = new List<string>();
                var actualIntakeData = new List<double?>();
                var targetData = new List<double?>();

                for (int i = 0; i < 6; i++)
                {
                    var currentDate = startDate.AddDays(i);
                    labels.Add(currentDate.ToString("MM/dd"));

                    // Find the log for this date
                    var logForDate = calorieLogs.FirstOrDefault(l => l.LogDate == currentDate);
                    
                    if (logForDate != null)
                    {
                        actualIntakeData.Add((double?)logForDate.TotalNetCalories);
                    }
                    else
                    {
                        actualIntakeData.Add(null); // No data for this day
                    }

                    targetData.Add((double?)maintenanceTarget);
                }

                // Check if we have any actual data
                if (actualIntakeData.All(d => !d.HasValue))
                {
                    errorMessage = "No calorie data available for the past 6 days. Start logging your meals!";
                    InitializeEmptyLineChart("Daily Calorie Intake");
                    return;
                }

                // Calculate safe min/max for the Y-axis
                var allValues = actualIntakeData.Where(d => d.HasValue).Select(d => d!.Value).ToList();
                allValues.Add(maintenanceTarget);

                var minValue = allValues.Min();
                var maxValue = allValues.Max();
                var yAxisPadding = Math.Max(100, (maxValue - minValue) * 0.1);

                chartData = new ChartData
                {
                    Labels = labels,
                    Datasets =
                    [
                        new LineChartDataset
                        {
                            Label = "Daily Calorie Intake",
                            Data = actualIntakeData,
                            BackgroundColor = ColorUtility.CategoricalTwelveColors[0],
                            BorderColor = ColorUtility.CategoricalTwelveColors[0],
                            PointRadius = new List<double> { 5 },
                            PointHoverRadius = new List<double> { 8 },
                            SpanGaps = true // Connect points even when there are null values
                        },
                        new LineChartDataset
                        {
                            Label = "Maintenance Target",
                            Data = targetData,
                            BackgroundColor = ColorUtility.CategoricalTwelveColors[1],
                            BorderColor = ColorUtility.CategoricalTwelveColors[1],
                            PointRadius = new List<double> { 3 },
                            PointHoverRadius = new List<double> { 5 },
                            BorderDash = new List<double> { 5, 5 } // Dashed line for target
                        }
                    ]
                };

                lineChartOptions = new LineChartOptions
                {
                    IndexAxis = "x",
                    Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                    Responsive = true,
                    Scales = new Scales
                    {
                        Y = new()
                        {
                            BeginAtZero = false,
                            Min = minValue - yAxisPadding,
                            Max = maxValue + yAxisPadding
                        }
                    }
                };

                // Clear error message on success
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading calorie intake data: {ex.Message}";
                System.Console.WriteLine($"Error in InitialiseDailyCalorieIntakeVsTargetGraph: {ex.Message}");
                InitializeEmptyLineChart("Daily Calorie Intake");
            }
        }

        // Graph Initialisation Method
        private async Task InitialiseDailyCalorieSurplusorDeficitGraph()
        {
            try
            {
                // Query real data from database
                var user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Username == Session.UserSession.Username);

                if (user == null)
                {
                    errorMessage = "User not found. Please log in.";
                    InitializeEmptyBarChart("Daily Calorie Surplus/Deficit");
                    return;
                }

                // Get the user's maintenance calorie target
                var maintenanceTarget = user.Maintenance_Calories;

                if (maintenanceTarget == 0)
                {
                    errorMessage = "No maintenance calorie target set. Please complete the Maintenance Calories Calculator.";
                    InitializeEmptyBarChart("Daily Calorie Surplus/Deficit");
                    return;
                }

                // Get calorie logs for the past 6 days
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-5); // 6 days total including today

                var calorieLogs = await Db.Calorie_Logs
                    .Where(c => c.User_id == user.User_id && c.Log_Date.Date >= startDate && c.Log_Date.Date <= endDate)
                    .GroupBy(c => c.Log_Date.Date)
                    .Select(g => new
                    {
                        LogDate = g.Key,
                        TotalNetCalories = g.Sum(c => c.Net_Calories)
                    })
                    .OrderBy(c => c.LogDate)
                    .ToListAsync();

                // Create labels and data for all 6 days (including days with no logs)
                var labels = new List<string>();
                var surplusDeficitData = new List<double?>();
                var backgroundColors = new List<string>();
                var borderColors = new List<string>();

                for (int i = 0; i < 6; i++)
                {
                    var currentDate = startDate.AddDays(i);
                    labels.Add(currentDate.ToString("MM/dd"));

                    // Find the log for this date
                    var logForDate = calorieLogs.FirstOrDefault(l => l.LogDate == currentDate);

                    if (logForDate != null)
                    {
                        // Calculate surplus/deficit: Net Calories - Maintenance Calories
                        var surplusDeficit = logForDate.TotalNetCalories - maintenanceTarget;
                        surplusDeficitData.Add((double?)surplusDeficit);

                        // Color code: green for surplus, red for deficit
                        if (surplusDeficit >= 0)
                        {
                            backgroundColors.Add(ColorUtility.CategoricalTwelveColors[10]); // Green/positive color
                            borderColors.Add(ColorUtility.CategoricalTwelveColors[10]);
                        }
                        else
                        {
                            backgroundColors.Add(ColorUtility.CategoricalTwelveColors[3]); // Red/negative color
                            borderColors.Add(ColorUtility.CategoricalTwelveColors[3]);
                        }
                    }
                    else
                    {
                        surplusDeficitData.Add(0); // No data for this day, show as 0
                        backgroundColors.Add(ColorUtility.CategoricalTwelveColors[5]); // Gray for no data
                        borderColors.Add(ColorUtility.CategoricalTwelveColors[5]);
                    }
                }

                // Check if we have any actual data
                var hasData = calorieLogs.Count != 0;
                if (!hasData)
                {
                    errorMessage = "No calorie data available for the past 6 days. Start logging your meals!";
                    InitializeEmptyBarChart("Daily Calorie Surplus/Deficit");
                    return;
                }

                // Create a dataset for daily calorie surplus/deficit
                var dataset1 = new BarChartDataset()
                {
                    Label = "Daily Calorie Surplus/Deficit",
                    Data = surplusDeficitData,
                    BackgroundColor = backgroundColors,
                    BorderColor = borderColors,
                    BorderWidth = new List<double> { 0 },
                };

                chartData = new ChartData { Labels = labels, Datasets = new List<IChartDataset> { dataset1 } };

                barChartOptions = new BarChartOptions
                {
                    Responsive = true,
                    Interaction = new Interaction { Mode = InteractionMode.Index },
                    IndexAxis = "x"
                };

                barChartOptions.Scales.X!.Title = new ChartAxesTitle { Text = "Days", Display = true };
                barChartOptions.Scales.Y!.Title = new ChartAxesTitle { Text = "Calories", Display = true };
                barChartOptions.Scales.Y!.BeginAtZero = true;

                barChartOptions.Plugins.Legend.Display = true;

                // Clear error message on success
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading calorie surplus/deficit data: {ex.Message}";
                System.Console.WriteLine($"Error in InitialiseDailyCalorieSurplusorDeficitGraph: {ex.Message}");
                InitializeEmptyBarChart("Daily Calorie Surplus/Deficit");
            }
        }

        private async Task InitialiseMacroDistributionGraph()
        {
            try
            {
                // Query real data from database
                var user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Username == Session.UserSession.Username);

                if (user == null)
                {
                    errorMessage = "User not found. Please log in.";
                    InitializeEmptyPieChart("Daily Macro Intake (g)");
                    return;
                }

                // Check if we have any macro data
                if (user.Daily_Carbs == 0 && user.Daily_Protein == 0 && user.Daily_Fat == 0)
                {
                    errorMessage = "No macro data available. Start logging your meals!";
                    InitializeEmptyPieChart("Daily Macro Intake (g)");
                    return;
                }

                // Define labels and datasets for the pie chart
                var labels = new List<string> { "Carbs", "Protein", "Fats" };
                var datasets = new List<IChartDataset>();

                // Create a dataset for macro distribution with REAL data
                var dataset1 = new PieChartDataset()
                {
                    Label = "Daily Macro Intake (g)",
                    Data = new List<double?>
            {
                (double?)user.Daily_Carbs,
                (double?)user.Daily_Protein,
                (double?)user.Daily_Fat
            },
                    BackgroundColor = new List<string>
            {
                ColorUtility.CategoricalTwelveColors[0],
                ColorUtility.CategoricalTwelveColors[1],
                ColorUtility.CategoricalTwelveColors[2]
            },
                    BorderColor = new List<string>
            {
                ColorUtility.CategoricalTwelveColors[0],
                ColorUtility.CategoricalTwelveColors[1],
                ColorUtility.CategoricalTwelveColors[2]
            },
                    BorderWidth = new List<double> { 0 },
                };
                datasets.Add(dataset1);

                chartData = new ChartData { Labels = labels, Datasets = datasets };

                pieChartOptions = new PieChartOptions();
                pieChartOptions.Responsive = true;
                pieChartOptions.Plugins.Title!.Text = "Daily Macro Breakdown";
                pieChartOptions.Plugins.Title.Display = true;
                pieChartOptions.Plugins.Legend.Position = "bottom";

                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading macro data: {ex.Message}";
                System.Console.WriteLine($"Error in InitialiseMacroDistributionGraph: {ex.Message}");
                InitializeEmptyPieChart("Daily Macro Intake (g)");
            }
        }

        /// <summary>
        /// Initializes an empty pie chart with default/placeholder data
        /// </summary>
        private void InitializeEmptyPieChart(string label)
        {
            chartData = new ChartData
            {
                Labels = ["No Data"],
                Datasets = new List<IChartDataset>
        {
            new PieChartDataset
            {
                Label = label,
                Data = [1],
                BackgroundColor = [ColorUtility.CategoricalTwelveColors[0]]
            }
        }
            };

            pieChartOptions = new PieChartOptions
            {
                Responsive = true,
                Plugins = new PieChartPlugins
                {
                    Title = new ChartPluginsTitle
                    {
                        Text = label,
                        Display = true
                    },
                    Legend = new ChartPluginsLegend
                    {
                        Position = "bottom"
                    }
                }
            };
        }

        /// <summary>
        /// Initializes an empty bar chart with default/placeholder data
        /// </summary>
        private void InitializeEmptyBarChart(string label)
        {
            chartData = new ChartData
            {
                Labels = new List<string> { "No Data" },
                Datasets = new List<IChartDataset>
                {
                    new BarChartDataset
                    {
                        Label = label,
                        Data = new List<double?> { 0 },
                        BackgroundColor = new List<string> { ColorUtility.CategoricalTwelveColors[0] },
                        BorderColor = new List<string> { ColorUtility.CategoricalTwelveColors[0] },
                        BorderWidth = new List<double> { 0 }
                    }
                }
            };

            barChartOptions = new BarChartOptions
            {
                Responsive = true,
                Interaction = new Interaction { Mode = InteractionMode.Index },
                IndexAxis = "x",
                Scales = new Scales
                {
                    X = new() { Title = new ChartAxesTitle { Text = "Days", Display = true } },
                    Y = new() { Title = new ChartAxesTitle { Text = "Calories", Display = true }, BeginAtZero = true }
                },
                Plugins = new BarChartPlugins
                {
                    Legend = new ChartPluginsLegend { Display = true }
                }
            };
        }


        private async Task InitialiseDailyMacroIntakeGraph()
        {
            try
            {
                // Query real data from database
                var user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Username == Session.UserSession.Username);

                if (user == null)
                {
                    errorMessage = "User not found. Please log in.";
                    InitializeEmptyBarChart("Daily Macro Intake (kcal)");
                    return;
                }

                // Get macro logs for the past 6 days
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-5); // 6 days total including today

                var macroLogs = await Db.Calorie_Logs
                    .Where(c => c.User_id == user.User_id && c.Log_Date.Date >= startDate && c.Log_Date.Date <= endDate)
                    .GroupBy(c => c.Log_Date.Date)
                    .Select(g => new
                    {
                        LogDate = g.Key,
                        TotalCarbs = g.Sum(c => c.Calories_From_Carbs),
                        TotalProtein = g.Sum(c => c.Calories_From_Protein),
                        TotalFat = g.Sum(c => c.Calories_From_Fats)
                    })
                    .OrderBy(c => c.LogDate)
                    .ToListAsync();

                // Check if we have any data
                if (macroLogs.Count == 0)
                {
                    errorMessage = "No macro data available for the past 6 days. Start logging your meals!";
                    InitializeEmptyBarChart("Daily Macro Intake (kcal)");
                    return;
                }

                // Calculate average macros across the days with data
                var avgCarbs = macroLogs.Average(m => m.TotalCarbs);
                var avgProtein = macroLogs.Average(m => m.TotalProtein);
                var avgFat = macroLogs.Average(m => m.TotalFat);

                var labels = new List<string> { "Carbs", "Protein", "Fat" };
                var datasets = new List<IChartDataset>();

                var dataset1 = new BarChartDataset()
                {
                    Label = "Average Daily Macro Intake (kcal) - Past 6 Days",
                    Data =
                    [
                        (double?)avgCarbs, 
                        (double?)avgProtein, 
                        (double?)avgFat 
                    ],
                    BackgroundColor =
                    [
                        ColorUtility.CategoricalTwelveColors[0],
                        ColorUtility.CategoricalTwelveColors[1],
                        ColorUtility.CategoricalTwelveColors[2]
                    ],
                    BorderColor =
                    [
                        ColorUtility.CategoricalTwelveColors[0],
                        ColorUtility.CategoricalTwelveColors[1],
                        ColorUtility.CategoricalTwelveColors[2]
                    ],
                    BorderWidth = [0],
                };
                datasets.Add(dataset1);

                chartData = new ChartData { Labels = labels, Datasets = datasets };

                barChartOptions = new BarChartOptions
                {
                    Responsive = true,
                    Interaction = new Interaction { Mode = InteractionMode.Y },
                    IndexAxis = "y"
                };

                barChartOptions.Scales.X!.Title = new ChartAxesTitle { Text = "Calories", Display = true };
                barChartOptions.Scales.Y!.Title = new ChartAxesTitle { Text = "Macro", Display = true };

                barChartOptions.Plugins.Legend.Display = true;

                // Clear error message on success
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading macro intake data: {ex.Message}";
                System.Console.WriteLine($"Error in InitialiseDailyMacroIntakeGraph: {ex.Message}");
                InitializeEmptyBarChart("Daily Macro Intake (kcal)");
            }
        }

        private async Task InitialiseCaloriesBurntThroughExerciseGraph()
        {
            try
            {
                // Query real data from database
                var user = await Db.Users
                    .FirstOrDefaultAsync(u => u.Username == Session.UserSession.Username);

                if (user == null)
                {
                    errorMessage = "User not found. Please log in.";
                    InitializeEmptyLineChart("Calories Burnt (kcal)");
                    return;
                }

                // Get calorie logs for the past 6 days
                var endDate = DateTime.Today;
                var startDate = endDate.AddDays(-5); // 6 days total including today

                var exerciseLogs = await Db.Calorie_Logs
                    .Where(c => c.User_id == user.User_id && c.Log_Date.Date >= startDate && c.Log_Date.Date <= endDate)
                    .GroupBy(c => c.Log_Date.Date)
                    .Select(g => new
                    {
                        LogDate = g.Key,
                        TotalCaloriesBurned = g.Sum(c => c.Calories_Burned ?? 0)
                    })
                    .OrderBy(c => c.LogDate)
                    .ToListAsync();

                // Create labels and data for all 6 days (including days with no logs)
                var labels = new List<string>();
                var caloriesBurnedData = new List<double?>();

                for (int i = 0; i < 6; i++)
                {
                    var currentDate = startDate.AddDays(i);
                    labels.Add(currentDate.ToString("MM/dd"));

                    // Find the log for this date
                    var logForDate = exerciseLogs.FirstOrDefault(l => l.LogDate == currentDate);

                    if (logForDate != null)
                    {
                        caloriesBurnedData.Add((double?)logForDate.TotalCaloriesBurned);
                    }
                    else
                    {
                        caloriesBurnedData.Add(0); // No data for this day, show as 0
                    }
                }

                // Check if we have any actual data (any day with calories burned > 0)
                if (caloriesBurnedData.All(d => d == 0))
                {
                    errorMessage = "No exercise data available for the past 6 days. Start logging your workouts!";
                    InitializeEmptyLineChart("Calories Burnt (kcal)");
                    return;
                }

                // Calculate safe max for the Y-axis
                var maxCalories = caloriesBurnedData.Max() ?? 0;
                var yAxisMax = Math.Max(100, Math.Ceiling(maxCalories / 100.0) * 100); // Round up to nearest 100

                chartData = new ChartData
                {
                    Labels = labels,
                    Datasets = new List<IChartDataset>
                    {
                        new LineChartDataset
                        {
                            Label = "Calories Burnt (kcal)",
                            Data = caloriesBurnedData,
                            BackgroundColor = ColorUtility.CategoricalTwelveColors[0],
                            BorderColor = ColorUtility.CategoricalTwelveColors[0],
                            PointRadius = new List<double> { 5 },
                            PointHoverRadius = new List<double> { 8 },
                            //Fill = true,
                            //Tension = 0.3 // Smooth line
                        }
                    }
                };

                lineChartOptions = new LineChartOptions
                {
                    IndexAxis = "x",
                    Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                    Responsive = true,
                    Scales = new Scales
                    {
                        Y = new()
                        {
                            BeginAtZero = true,
                            Min = 0,
                            Max = yAxisMax
                        }
                    }
                };

                // Clear error message on success
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error loading exercise calorie data: {ex.Message}";
                System.Console.WriteLine($"Error in InitialiseCaloriesBurntThroughExerciseGraph: {ex.Message}");
                InitializeEmptyLineChart("Calories Burnt (kcal)");
            }
        }

        private void InitialiseExerciseTypeFrequencyGraph()
        {
            // Define labels and datasets for the bar chart
            var labels = new List<string> { "Cardio", "Strength", "Flexibility" };
            var datasets = new List<IChartDataset>();

            // Create a dataset for exercise type frequency
            var dataset1 = new BarChartDataset()
            {
                Label = "Exercise Type Frequency",

                //  Data representing frequency of each exercise type
                Data = [2, 3, 4],
                BackgroundColor =
                [
                    ColorUtility.CategoricalTwelveColors[0],
                    ColorUtility.CategoricalTwelveColors[1],
                    ColorUtility.CategoricalTwelveColors[2]
                ],
                BorderColor =
                [
                    ColorUtility.CategoricalTwelveColors[0],
                    ColorUtility.CategoricalTwelveColors[1],
                    ColorUtility.CategoricalTwelveColors[2]
                ],
                BorderWidth = new List<double> { 0 },
            };
            datasets.Add(dataset1);

            chartData = new ChartData { Labels = labels, Datasets = datasets };

            barChartOptions = new BarChartOptions();
            barChartOptions.Responsive = true;
            barChartOptions.Interaction = new Interaction { Mode = InteractionMode.Y };
            barChartOptions.IndexAxis = "y";

            barChartOptions.Scales.X!.Title = new ChartAxesTitle { Text = "Frequency", Display = true };
            barChartOptions.Scales.Y!.Title = new ChartAxesTitle { Text = "Weeks", Display = true };

            barChartOptions.Plugins.Legend.Display = true;
        }

        #region Data Preparation

        private List<IChartDataset> GetDefaultDataSets(int numberOfDatasets, bool baseline, string label, bool positive, int min = 0, int max = 0)
        {
            var datasets = new List<IChartDataset>();

            for (var index = 0; index < numberOfDatasets; index++)
            {
                datasets.Add(GetRandomLineChartDataset(label, positive, min, max));
            }

            if (baseline)
            {
                datasets.Add(GetBaselineLine());
            }

            return datasets;
        }

        private LineChartDataset GetRandomLineChartDataset(string label, bool positive, int min = 0, int max = 0)
        {
            var c = ColorUtility.CategoricalTwelveColors[datasetsCount].ToColor();

            datasetsCount += 1;

            return new LineChartDataset
            {
                Label = label,
                Data = positive ? GetRandomPositiveData(min, max) : GetRandomData(),
                BackgroundColor = c.ToRgbaString(),
                BorderColor = c.ToRgbString(),
                PointRadius = new List<double> { 5 },
                PointHoverRadius = new List<double> { 8 },
            };
        }

        private LineChartDataset GetBaselineLine()
        {
            var c = ColorUtility.CategoricalTwelveColors[datasetsCount].ToColor();

            return new LineChartDataset
            {
                Label = $"Baseline",
                Data = GetBaseline(),
                BackgroundColor = c.ToRgbaString(),
                BorderColor = c.ToRgbString(),
                PointRadius = new List<double> { 5 },
                PointHoverRadius = new List<double> { 8 },
            };
        }

        private List<double?> GetRandomData()
        {
            var data = new List<double?>();
            for (var index = 0; index < labelsCount; index++)
            {
                data.Add((random.NextDouble() * 2.0) - 1.0);
            }

            return data;
        }

        private List<double?> GetRandomPositiveData(int min, int max)
        {
            var data = new List<double?>();
            for (var index = 0; index < labelsCount; index++)
            {
                data.Add(random.Next(min, max + 1));
            }

            return data;
        }

        private List<double?> GetBaseline()
        {
            var data = new List<double?>();
            for (var index = 0; index < labelsCount; index++)
            {
                data.Add(0);
            }

            return data;
        }

        private List<string> GetDefaultDataLabels(int numberOfLabels, string time)
        {
            var labels = new List<string>();
            for (var index = 0; index < numberOfLabels; index++)
            {
                labels.Add(GetNextDataLabel(time));
            }

            return labels;
        }

        private string GetNextDataLabel(string time)
        {
            labelsCount += 1;
            return $"{time} {labelsCount}";
        }

        #endregion Data Preparation
    }
}