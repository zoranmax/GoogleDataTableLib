using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Google.DataTable.Net.Wrapper
{
    public class ColumnRole
    {
        /// <summary>
        /// Text to display on the chart near the associated data point. 
        /// The text displays without any user interaction. 
        /// Annotations and annotation text can be assigned to both 
        /// data points and categories (axis labels).
        /// </summary>
        public static string Annotation
        {
            get { return "annotation"; }
        }

        /// <summary>
        /// Extended text to display when the user hovers 
        /// over the associated annotation        
        /// </summary>
        public static string AnnotationText
        {
            get { return "annotationText"; }
        }

        /// <summary>
        /// ndicates whether a data point is certain or not. 
        /// How this is displayed depends on the chart type—for 
        /// example, it might be indicated by dashed lines or a striped fill.
        /// </summary>
        public static string Certainty
        {
            get { return "certainty"; }
        }

        /// <summary>
        /// Emphasizes specified chart data points. Displayed as a 
        /// thick line and/or large point.
        /// For line and area charts, the segment 
        /// between two data points is emphasized if and only if
        /// both data points are emphasized.
        /// </summary>
        public static string Emphasis
        {
            get { return "emphasis"; }
        }

        /// <summary>
        /// ndicates potential data range for a specific point. 
        /// Intervals are usually displayed as I-bar style range indicators. 
        /// Interval columns are numeric columns; add interval columns in 
        /// pairs, marking the low and high value of the bar. Interval 
        /// values should be added in increasing value, from left to right.
        /// </summary>
        public static string Interval
        {
            get { return "interval"; }
        }

        /// <summary>
        /// Indicates whether a point is in or out of scope. 
        /// If a point is out of scope, it is visually de-emphasized.
        /// For line and area charts, the segment between two data points 
        /// is in scope if either endpoint is in scope.
        /// </summary>
        public static string Scope
        {
            get { return "scope"; }
        }

        /// <summary>
        /// Text to display when the user hovers over the data point 
        /// associated with this row.
        /// </summary>
        public static string Tooltip
        {
            get { return "tooltip"; }
        }

        /// <summary>
        /// You should not need to assign this role explicitly unless designing 
        /// a multi-domain chart (shown here); the basic 
        /// format of the data table enables the charting engine to infer which
        /// columns are domain columns. However, you should be aware of 
        /// which columns are domain columns so that you know which other 
        /// columns can modify it.
        /// Domain columns specify labels along the major axis of the chart. 
        /// Multiple domain columns can sometimes be used to support multiple 
        /// scales within the same chart.
        /// </summary>
        public static string Domain
        {
            get { return "domain"; }
        }

        /// <summary>
        /// You should not need to assign this role explicitly; the basic 
        /// format of the data table enables the charting engine to 
        /// infer which columns are domain columns. However, you sould be 
        /// aware of which columns are data columns so that you know which 
        /// other columns can modify it.
        /// 
        /// Data role columns specify series data to render in the chart. 
        /// For most charts, one column = one series, but this can vary 
        /// by chart type (for example, scatter charts require two data 
        /// columns for the first series, and an additional one for each 
        /// additional series; candlestick charts require four data columns 
        /// for each series).
        /// </summary>
        public static string Data
        {
            get { return "data"; }
        }
    }
}
