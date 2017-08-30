﻿//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodríguez Orozco & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Linq;
using LiveCharts.Data;
using LiveCharts.Definitions.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Helpers;

namespace LiveCharts.Charts
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Charts.ChartCore" />
    public class PieChartCore : ChartCore
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PieChartCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="updater">The updater.</param>
        public PieChartCore(IChart2DView view, ChartUpdater updater) : base(view, updater)
        {
        }

        #endregion

        #region Publics

        /// <summary>
        /// Prepares the axes.
        /// </summary>
        internal override void PrepareAxes()
        {
            if (View.ActualSeries.Any(x => !(x.Core is IPieSeries)))
            {
                throw new LiveChartsException(ExceptionReason.NotAPieSeries);
            }

            foreach (var xAxis in View.FirstDimension)
            {
                var xi = xAxis.Core;

                xi.S = 1;
                xi.BotLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).XLimit.Min)
                    .DefaultIfEmpty(0).Min();
                xi.TopLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).XLimit.Max)
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(xi.BotLimit - xi.TopLimit) < xi.S * .01)
                {
                    xi.BotLimit -= xi.S;
                    xi.TopLimit += xi.S;
                }
            }

            foreach (var yAxis in View.SecondDimension)
            {
                var yi = yAxis.Core;

                //yi.CalculateSeparator(this, AxisTags.X);
                yi.BotLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).YLimit.Min)
                    .DefaultIfEmpty(0).Min();
                yi.TopLimit = View.ActualSeries.Select(x => x.Values.GetTracker(x).YLimit.Max)
                    .DefaultIfEmpty(0).Max();

                if (Math.Abs(yi.BotLimit - yi.TopLimit) < yi.S * .01)
                {
                    yi.BotLimit -= yi.S;
                    yi.TopLimit += yi.S;
                }
            }

            StackPoints(View.ActualSeries, AxisOrientation.Y, 0);

            var cs = View.ControlSize;

            var curSize = new RectangleData(0, 0, cs.Width, cs.Height);

            curSize = PlaceLegend(curSize);

            View.DrawMarginTop = curSize.Top;
            View.DrawMarginLeft = curSize.Left;
            View.DrawMarginWidth = curSize.Width;
            View.DrawMarginHeight = curSize.Height;
        }

        #endregion

    }
}
