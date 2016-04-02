using System;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace LINQPadFoo
{
    public static class Charting
    {
        class SummaryStatistics
        {
            public double Mean, Median, Variance;
            public override string ToString() => $"mean: {Mean}, median: {Median}, variance {Variance}";
        }

        static SummaryStatistics GetSummary(Chart chart, string seriesName) => new SummaryStatistics
        {
            Mean = chart.DataManipulator.Statistics.Mean(seriesName),
            Median = chart.DataManipulator.Statistics.Median(seriesName),
            Variance = chart.DataManipulator.Statistics.Variance(seriesName, sampleVariance: true)
        };

        public static Chart LinePlot<Y>(DateTime[] xs, Y[] ys)
        {
            var chart1 = CreateEmptyChart();

            var series1 = new Series();
            series1.Name = "Series1";
            series1.ChartType = SeriesChartType.Line;
            series1.Points.DataBindXY(xs, ys);
            chart1.Series.Add(series1);

            return chart1;
        }

        public static Chart ScatterPlot<X, Y>(X[] xs, Y[] ys)
        {
            var chart1 = CreateEmptyChart();
            var series1 = new Series();

            series1.Name = "Series1";
            series1.ChartType = SeriesChartType.Point;
            series1.Points.DataBindXY(xs, ys);

            chart1.Series.Add(series1);
            return chart1;
        }

        public static Chart HistogramPlot<Y>(Y[] ys, int segmentIntervalNumber = 10, double segmentIntervalWidth = double.NaN, string plotName = "")
        {

            var histogramHelper = new HistogramChartHelper();
            histogramHelper.SegmentIntervalNumber = segmentIntervalNumber;
            histogramHelper.SegmentIntervalWidth = segmentIntervalWidth;

            var chartArea1 = new ChartArea();
            chartArea1.Name = "Default";
            chartArea1.AlignWithChartArea = "HistogramArea";
            var chartArea2 = new ChartArea();
            chartArea2.Name = "HistogramArea";
            var series1 = new Series();
            var series2 = new Series();
            var series3 = new Series();
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series1.ChartArea = "Default";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(252)))), ((int)(((byte)(180)))), ((int)(((byte)(65)))));
            series1.Enabled = false;

            series1.Legend = "Default";
            series1.MarkerSize = 9;
            series1.Name = "RawData";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));

            series2.ChartArea = "Default";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(252)))), ((int)(((byte)(180)))), ((int)(((byte)(65)))));
            series2.Legend = "Default";
            series2.MarkerSize = 8;
            series2.Name = "DataDistribution";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series3.ChartArea = "HistogramArea";

            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(64)))), ((int)(((byte)(10)))));
            series3.IsValueShownAsLabel = true;

            series3.Legend = "Default";
            series3.Name = "Histogram";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series3.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

            var chart1 = new Chart();
            chart1.ChartAreas.Add(chartArea1);
            chart1.ChartAreas.Add(chartArea2);
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.Series.Add(series3);
            chart1.Series["RawData"].Points.DataBindY(ys);

            // NOTE: Interval width may be specified instead of interval number

            //histogramHelper.SegmentIntervalWidth = 15;

            histogramHelper.CreateHistogram(chart1, "RawData", "Histogram");

            // Set same X axis scale and interval in the single axis data distribution 

            // chart area as in the histogram chart area.

            chart1.ChartAreas["Default"].AxisX.Minimum = chart1.ChartAreas["HistogramArea"].AxisX.Minimum;
            chart1.ChartAreas["Default"].AxisX.Maximum = chart1.ChartAreas["HistogramArea"].AxisX.Maximum;
            chart1.ChartAreas["Default"].AxisX.Interval = chart1.ChartAreas["HistogramArea"].AxisX.Interval;

            var summary = GetSummary(chart1, "RawData");
            var annotation = new TextAnnotation { Text = plotName + ", " + summary.ToString() };
            annotation.ForeColor = Color.Black;
            annotation.Font = new Font("Arial", 12);
            annotation.AnchorX = 30;
            annotation.AnchorY = 25;

            chart1.Annotations.Add(annotation);
            return chart1;
        }

        static Chart CreateEmptyChart()
        {
            var chartArea1 = new ChartArea();
            chartArea1.Name = "Default";
            var chart1 = new Chart();
            chart1.ChartAreas.Add(chartArea1);

            return chart1;
        }
    }

    internal class HistogramChartHelper
    {

        #region Fields




        /// <summary>

        /// Number of class intervals the data range is devided in.

        /// This property only has affect when "SegmentIntervalWidth" is 

        /// set to double.NaN.

        /// </summary>

        public int SegmentIntervalNumber = 10;




        /// <summary>

        /// Histogram class interval width. Setting this value to "double.NaN"

        /// will result in automatic width calculation based on the data range

        /// and number of required interval specified in "SegmentIntervalNumber".

        /// </summary>

        public double SegmentIntervalWidth = double.NaN;




        /// <summary>

        /// Indicates that percent frequency should be shown on the right axis

        /// </summary>

        public bool ShowPercentOnSecondaryYAxis = false;


        #endregion // Fields




        #region Methods




        /// <summary>

        /// Creates a histogram chart.

        /// </summary>

        /// <param name="chartControl">Chart control reference.</param>

        /// <param name="dataSeriesName">Name of the series which stores the original data.</param>

        /// <param name="histogramSeriesName">Name of the histogram series.</param>

        public void CreateHistogram(

            Chart chartControl,

            string dataSeriesName,

            string histogramSeriesName)

        {

            // Validate input

            if (chartControl == null)

            {

                throw (new ArgumentNullException("chartControl"));

            }

            if (chartControl.Series.IndexOf(dataSeriesName) < 0)

            {

                throw (new ArgumentException("Series with name'" + dataSeriesName + "' was not found.", "dataSeriesName"));

            }




            // Make data series invisible

            chartControl.Series[dataSeriesName].Enabled = false;




            // Check if histogram series exsists

            Series histogramSeries = null;

            if (chartControl.Series.IndexOf(histogramSeriesName) < 0)

            {

                // Add new series

                histogramSeries = chartControl.Series.Add(histogramSeriesName);




                // Set new series chart type and other attributes

                histogramSeries.ChartType = SeriesChartType.Column;

                histogramSeries.BorderColor = Color.Black;

                histogramSeries.BorderWidth = 1;

                histogramSeries.BorderDashStyle = ChartDashStyle.Solid;

            }

            else

            {

                histogramSeries = chartControl.Series[histogramSeriesName];

                histogramSeries.Points.Clear();

            }




            // Get data series minimum and maximum values

            double minValue = double.MaxValue;

            double maxValue = double.MinValue;

            int pointCount = 0;

            foreach (DataPoint dataPoint in chartControl.Series[dataSeriesName].Points)

            {

                // Process only non-empty data points

                if (!dataPoint.IsEmpty)

                {

                    if (dataPoint.YValues[0] > maxValue)

                    {

                        maxValue = dataPoint.YValues[0];

                    }

                    if (dataPoint.YValues[0] < minValue)

                    {

                        minValue = dataPoint.YValues[0];

                    }

                    ++pointCount;

                }

            }




            // Calculate interval width if it's not set

            if (double.IsNaN(this.SegmentIntervalWidth))

            {

                this.SegmentIntervalWidth = (maxValue - minValue) / SegmentIntervalNumber;

                this.SegmentIntervalWidth = RoundInterval(this.SegmentIntervalWidth);

            }




            // Round minimum and maximum values

            minValue = Math.Floor(minValue / this.SegmentIntervalWidth) * this.SegmentIntervalWidth;

            maxValue = Math.Ceiling(maxValue / this.SegmentIntervalWidth) * this.SegmentIntervalWidth;




            // Create histogram series points

            double currentPosition = minValue;

            for (currentPosition = minValue; currentPosition <= maxValue; currentPosition += this.SegmentIntervalWidth)

            {

                // Count all points from data series that are in current interval

                int count = 0;

                foreach (DataPoint dataPoint in chartControl.Series[dataSeriesName].Points)

                {

                    if (!dataPoint.IsEmpty)

                    {

                        double endPosition = currentPosition + this.SegmentIntervalWidth;

                        if (dataPoint.YValues[0] >= currentPosition &&

                            dataPoint.YValues[0] < endPosition)

                        {

                            ++count;

                        }




                        // Last segment includes point values on both segment boundaries

                        else if (endPosition >= maxValue)

                        {

                            if (dataPoint.YValues[0] >= currentPosition &&

                                dataPoint.YValues[0] <= endPosition)

                            {

                                ++count;

                            }

                        }

                    }

                }







                // Add data point into the histogram series

                histogramSeries.Points.AddXY(currentPosition + this.SegmentIntervalWidth / 2.0, count);

            }




            // Adjust series attributes

            histogramSeries["PointWidth"] = "1";




            // Adjust chart area

            ChartArea chartArea = chartControl.ChartAreas[histogramSeries.ChartArea];

            chartArea.AxisY.Title = "Frequency";

            chartArea.AxisX.Minimum = minValue;

            chartArea.AxisX.Maximum = maxValue;




            // Set axis interval based on the histogram class interval

            // and do not allow more than 10 labels on the axis.

            double axisInterval = this.SegmentIntervalWidth;

            while ((maxValue - minValue) / axisInterval > 10.0)

            {

                axisInterval *= 2.0;

            }

            chartArea.AxisX.Interval = axisInterval;




            // Set chart area secondary Y axis

            chartArea.AxisY2.Enabled = AxisEnabled.Auto;

            if (this.ShowPercentOnSecondaryYAxis)

            {

                chartArea.RecalculateAxesScale();




                chartArea.AxisY2.Enabled = AxisEnabled.True;

                chartArea.AxisY2.LabelStyle.Format = "P0";

                chartArea.AxisY2.MajorGrid.Enabled = false;

                chartArea.AxisY2.Title = "Percent of Total";




                chartArea.AxisY2.Minimum = 0;

                chartArea.AxisY2.Maximum = chartArea.AxisY.Maximum / (pointCount / 100.0);

                double minStep = (chartArea.AxisY2.Maximum > 20.0) ? 5.0 : 1.0;

                chartArea.AxisY2.Interval = Math.Ceiling((chartArea.AxisY2.Maximum / 5.0 / minStep)) * minStep;




            }

        }




        /// <summary>

        /// Helper method which rounds specified axsi interval.

        /// </summary>

        /// <param name="interval">Calculated axis interval.</param>

        /// <returns>Rounded axis interval.</returns>

        internal double RoundInterval(double interval)

        {

            // If the interval is zero return error

            if (interval == 0.0)

            {

                throw (new ArgumentOutOfRangeException("interval", "Interval can not be zero."));

            }




            // If the real interval is > 1.0

            double step = -1;

            double tempValue = interval;

            while (tempValue > 1.0)

            {

                step++;

                tempValue = tempValue / 10.0;

                if (step > 1000)

                {

                    throw (new InvalidOperationException("Auto interval error due to invalid point values or axis minimum/maximum."));

                }

            }




            // If the real interval is < 1.0

            tempValue = interval;

            if (tempValue < 1.0)

            {

                step = 0;

            }




            while (tempValue < 1.0)

            {

                step--;

                tempValue = tempValue * 10.0;

                if (step < -1000)

                {

                    throw (new InvalidOperationException("Auto interval error due to invalid point values or axis minimum/maximum."));

                }

            }




            double tempDiff = interval / Math.Pow(10.0, step);

            if (tempDiff < 3.0)

            {

                tempDiff = 2.0;

            }

            else if (tempDiff < 7.0)

            {

                tempDiff = 5.0;

            }

            else

            {

                tempDiff = 10.0;

            }




            // Make a correction of the real interval

            return tempDiff * Math.Pow(10.0, step);

        }




        #endregion // Methods

    }
}

