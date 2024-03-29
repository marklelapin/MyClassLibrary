﻿using MyClassLibrary.Extensions;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MyClassLibrary.ChartJs
{
    public class CartesianAxisBuilder
    {
        private CartesianAxis _axis;

        public CartesianAxisBuilder(string? type = null)
        {
            _axis = new CartesianAxis();
            _axis.type = type;
        }

        public CartesianAxisBuilder AddTitle(string title, string? color = null, Font? font = null, string align = "center")
        {
            _axis.title = new Title(title, align, color, font);
            return this;
        }


        public CartesianAxisBuilder AddType(string type)
        {
            _axis.type = type
             ; return this;
        }

        public CartesianAxisBuilder AddAbsoluteScaleLimits(double? min, double? max)
        {
            _axis.min = min;
            _axis.max = max;
            return this;
        }

        //public CartesianAxisBuilder AddAbsoluteScaleLimits(string? min, string? max)
        //{
        //    _axis.min = min;
        //    _axis.max = max;
        //    return this;
        //}
        public CartesianAxisBuilder AddSuggestedScaleLimts(double? suggestedMin, double? suggestedMax)
        {
            _axis.suggestedMin = suggestedMin.ToString();
            _axis.suggestedMax = suggestedMax.ToString();
            return this;
        }
        public CartesianAxisBuilder AddSuggestedScaleLimts(string? suggestedMin, string? suggestedMax)
        {
            _axis.suggestedMin = suggestedMin;
            _axis.suggestedMax = suggestedMax;
            return this;
        }

        public CartesianAxisBuilder Stacked(string stacked)
        {
            _axis.stacked = stacked;
            return this;
        }

        public CartesianAxisBuilder AddLabels(List<string> labels)
        {
            _axis.labels = labels.ToArray();
            return this;
        }

        public CartesianAxisBuilder SetTickRotationBetween(int? minDegrees, int? maxDegrees)
        {
            _axis.ticks.minRotation = minDegrees;
            _axis.ticks.maxRotation = maxDegrees;
            return this;
        }

        public CartesianAxisBuilder SetTickRotation(int degrees)
        {
            return SetTickRotationBetween(degrees, degrees);
        }


        /// <summary>
        /// The unit of time to use in the chart.
        /// </summary>
        /// <remarks>
        /// millisecond <br/>
        /// second <br/>
        /// minute <br/>
        /// hour <br/>
        /// day <br/>
        /// week <br/>
        /// month <br/>
        /// quarter <br/>
        /// year
        /// </remarks>
        public CartesianAxisBuilder SetTimeUnit(string timeUnit)
        {
            _axis.time.unit = timeUnit;
            return this;
        }

        public CartesianAxisBuilder AddTickCategoryLabels(Dictionary<string, int> labelDictionary)
        {
            string dp = string.Empty;
            var labels = labelDictionary.ToList();

            for (int i = 0; i < labels.Count; i++)
            {
                dp = dp + Regex.Unescape($@"case {labels[i].Value}: return '{labels[i].Key}'; break; ");
            }

            _axis.ticks.callback = $"callbackfunction.UseTickLabels({dp}).";

            return this;
        }


        public CartesianAxisBuilder ConvertTickToDateTime(string dateTimeFormat)
        {
            _axis.ticks.callback = $@"callbackfunction.ConvertTickToDateTime(""{dateTimeFormat}"").";
            return this;
        }

        public CartesianAxisBuilder ConvertLabelToDateTime(string dateTimeFormat)
        {
            _axis.ticks.callback = $@"callbackfunction.ConvertLabelToDateTime(""{dateTimeFormat}"").";
            return this;
        }


        public CartesianAxisBuilder AutoSkipTicks(bool autoSkip, int? autoSkipPadding = null)
        {
            _axis.autoSkip = autoSkip;
            _axis.autoSkipPadding = autoSkipPadding;
            return this;

        }


        public CartesianAxisBuilder OverrideTickValues(List<double> values)
        {
            var valuesString = "[";

            foreach (double value in values)
            {
                valuesString += $@"{{ value: {value}}},";
            }

            valuesString = valuesString.TrimEnd(',') + "]";


            valuesString = _axis.afterBuildTicks = $@"callbackfunction.OverrideTickValues({valuesString}).";
            return this;
        }


        public CartesianAxisBuilder TickColor(Color color)
        {
            _axis.ticks.color = color.ToHex();
            return this;
        }

        public CartesianAxisBuilder TickColor(string color)
        {
            _axis.ticks.color = color;
            return this;
        }




        public CartesianAxisBuilder AddGrid()
        {
            throw new NotImplementedException();
        }


        public CartesianAxisBuilder AddTicks()
        {
            throw new NotImplementedException();
        }


        public CartesianAxis Build()
        {
            return _axis;
        }

    }




}
