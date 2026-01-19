using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.FSharp.Core;

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
                    InitialiseWeightChangePerWeekGraph();
                    break;
                case 2:
                    InitialiseDailyCalorieIntakeVsTargetGraph();
                    break;
                case 3:
                    InitialiseDailyCalorieSurplusorDeficitGraph();
                    break;
                case 4:
                    InitialiseMacroDistributionGraph();
                    break;
                case 5:
                    InitialiseDailyMacroIntakeGraph();
                    break;
                case 6:
                    InitialiseCaloriesBurntThroughExerciseGraph();
                    break;
                case 7:
                    InitialiseExerciseTypeFrequencyGraph();
                    break;
                default:
                    InitialiseWeightChangePerWeekGraph();
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

        private void InitialiseWeightChangePerWeekGraph()
        {
            chartData = new ChartData { Labels = GetDefaultDataLabels(6, "Week"), Datasets = GetDefaultDataSets(1, true, "Weight Change (kg)", false) };

            lineChartOptions = new()
            {
                IndexAxis = "x",
                Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                Responsive = true,

                Scales = new Scales
                {
                    Y = new()
                    {
                        BeginAtZero = false,
                        Min = -1,
                        Max = 1
                    }
                }
            };
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
                    .Take(30)  // Last 30 days
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

        private void InitialiseDailyCalorieIntakeVsTargetGraph()
        {
            chartData = new ChartData { Labels = GetDefaultDataLabels(6, "Day"), Datasets = GetDefaultDataSets(1, true, "Calories above/below Target", false) };

            lineChartOptions = new()
            {
                IndexAxis = "x",
                Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                Responsive = true,

                Scales = new Scales
                {
                    Y = new()
                    {
                        BeginAtZero = false,
                        Min = -1,
                        Max = 1
                    }
                }
            };
        }

        // Graph Initialisation Method
        private void InitialiseDailyCalorieSurplusorDeficitGraph()
        {
            // Define labels and datasets for the bar chart
            var labels = new List<string> { "Day 1", "Day 2", "Day 3", "Day 4", "Day 5" };
            var datasets = new List<IChartDataset>();

            // Create a dataset for daily calorie surplus/deficit
            var dataset1 = new BarChartDataset()
            {
                Label = "Daily Calorie Surplus/Deficit",

                // Positive values indicate surplus, negative values indicate deficit
                Data = new List<double?> { 250, -180, 70, -120, 310 },
                BackgroundColor = new List<string>
                {
                    ColorUtility.CategoricalTwelveColors[0],
                    ColorUtility.CategoricalTwelveColors[1],
                    ColorUtility.CategoricalTwelveColors[2],
                    ColorUtility.CategoricalTwelveColors[3],
                    ColorUtility.CategoricalTwelveColors[4]
                },
                BorderColor = new List<string>
                {
                    ColorUtility.CategoricalTwelveColors[0],
                    ColorUtility.CategoricalTwelveColors[1],
                    ColorUtility.CategoricalTwelveColors[2],
                    ColorUtility.CategoricalTwelveColors[3],
                    ColorUtility.CategoricalTwelveColors[4]
                },
                BorderWidth = new List<double> { 0 },
            };
            datasets.Add(dataset1);

            chartData = new ChartData { Labels = labels, Datasets = datasets };

            barChartOptions = new BarChartOptions();
            barChartOptions.Responsive = true;
            barChartOptions.Interaction = new Interaction { Mode = InteractionMode.Index };
            barChartOptions.IndexAxis = "x";

            barChartOptions.Scales.X!.Title = new ChartAxesTitle { Text = "Days", Display = true };
            barChartOptions.Scales.Y!.Title = new ChartAxesTitle { Text = "Calories", Display = true };
            barChartOptions.Scales.Y!.BeginAtZero = true;

            barChartOptions.Plugins.Legend.Display = true;
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
                Labels = new List<string> { "No Data" },
                Datasets = new List<IChartDataset>
        {
            new PieChartDataset
            {
                Label = label,
                Data = new List<double?> { 1 },
                BackgroundColor = new List<string> { ColorUtility.CategoricalTwelveColors[0] }
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

        private void InitialiseDailyMacroIntakeGraph()
        {
            var labels = new List<string> { "Carbs", "Protein", "Fat" };
            var datasets = new List<IChartDataset>();

            var dataset1 = new BarChartDataset()
            {
                Label = "Daily Macro Intake (g)",
                Data = new List<double?> { 250, 180, 70 },
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

            barChartOptions = new BarChartOptions();
            barChartOptions.Responsive = true;
            barChartOptions.Interaction = new Interaction { Mode = InteractionMode.Y };
            barChartOptions.IndexAxis = "y";

            barChartOptions.Scales.X!.Title = new ChartAxesTitle { Text = "Grams", Display = true };
            barChartOptions.Scales.Y!.Title = new ChartAxesTitle { Text = "Macro", Display = true };

            barChartOptions.Plugins.Legend.Display = true;
        }

        private void InitialiseCaloriesBurntThroughExerciseGraph()
        {
            // Define labels and datasets for the line chart, data values between 0 and 1000
            chartData = new ChartData { Labels = GetDefaultDataLabels(6, "Day"), Datasets = GetDefaultDataSets(1, false, "Calories Burnt (kcal)", true, 0, 1000) };

            lineChartOptions = new()
            {
                IndexAxis = "x",
                Interaction = new Interaction { Mode = InteractionMode.Index, Intersect = false },
                Responsive = true,

                Scales = new Scales
                {
                    Y = new()
                    {
                        // Y-axis starts at 0 and goes up to 1000
                        BeginAtZero = true,
                        Min = 0,
                        Max = 1000
                    }
                }
            };
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
                Data = new List<double?> { 2, 3, 4 },
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