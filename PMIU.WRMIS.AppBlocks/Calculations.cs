using PMIU.WRMIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMIU.WRMIS.AppBlocks
{
    public class Calculations
    {
        /// <summary>
        /// This function calculates the coefficient of discharge for Bed Level guages.
        /// Created On 13-11-2015
        /// </summary>
        /// <param name="_ExponentValue"></param>
        /// <param name="_MeanDepth"></param>
        /// <param name="_ObservedDischarge"></param>
        /// <param name="_GaugeCorrectionType"></param>
        /// <param name="_GaugeValueCorrection"></param>
        /// <returns>double</returns>
        public static double GetBedCoefficientOfDischarge(double _ExponentValue, double _MeanDepth, double _ObservedDischarge, bool? _GaugeCorrectionType, double? _GaugeValueCorrection)
        {
            //if (_GaugeCorrectionType != null)
            //{
            //    if (_GaugeCorrectionType == Constants.GaugeCorrectionSiltedType)
            //    {
            //        _MeanDepth = _MeanDepth - Convert.ToDouble(_GaugeValueCorrection);
            //    }
            //    else
            //    {
            //        _MeanDepth = _MeanDepth + Convert.ToDouble(_GaugeValueCorrection);
            //    }
            //}

            return Math.Round((_ObservedDischarge / (Math.Pow(_MeanDepth, _ExponentValue))), 3);
        }

        /// <summary>
        /// This function calculates the coefficient of discharge for Crest Level guages.
        /// Created On 18-11-2015
        /// </summary>
        /// <param name="_FallBreath"></param>
        /// <param name="_HeadAboveCrest"></param>
        /// <param name="_ObservedDischarge"></param>
        /// <returns>double</returns>
        public static double GetCrestCoefficientOfDischarge(double _FallBreath, double _HeadAboveCrest, double _ObservedDischarge)
        {
            return Math.Round((_ObservedDischarge / (_FallBreath * (Math.Pow(_HeadAboveCrest, 1.5)))), 3);
        }
        /// <summary>
        /// This function calculate total RDs from left and right RD
        /// </summary>
        /// <param name="_TotalRDLeft"></param>
        /// <param name="_TotalRDRight"></param>
        /// <returns>Total RD</returns>
        public static int CalculateTotalRDs(string _TotalRDLeft, string _TotalRDRight)
        {
            return (Cast.ToInt(_TotalRDLeft) * 1000) + Cast.ToInt(_TotalRDRight);
        }
        /// <summary>
        /// This function split total RD into RD Text 
        /// </summary>
        /// <param name="_TotalRD"></param>
        /// <returns>RD Text</returns>
        public static string GetRDText(double? _TotalRD)
        {
            if (_TotalRD.HasValue)
            {
                double TailRD = _TotalRD.Value / 1000;
                double TailRDMOD = _TotalRD.Value % 1000;

                string TotalRD = Convert.ToString(Math.Truncate(TailRD)) + "+" + Convert.ToString(Math.Truncate(TailRDMOD).ToString("000"));
                return TotalRD;
            }
            else
                return "-";
        }
        /// <summary>
        /// This function return splited Total RDs into two parts
        /// </summary>
        /// <param name="_TotalRD"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetRDValues(double? _TotalRD)
        {
            double totalRD = _TotalRD.HasValue ? _TotalRD.Value : 0;
            double TailRD = totalRD / 1000;
            double TailRDMOD = totalRD % 1000;

            Tuple<string, string> tuple = Tuple.Create(Convert.ToString(Math.Truncate(TailRD)), Convert.ToString(Math.Truncate(TailRDMOD)));
            return tuple;
        }

        public static string CalculateStorageToFill(double? _ELStr, double? _ELInitialStr, double? _StrFillPercent)
        {
            return String.Format("{0:0.000}", ((_ELStr - _ELInitialStr) * (double)(_StrFillPercent / 100)));
        }

        public static string CalculateStorageRelease(double? _StrDepretion, double? _ELStr, double? _ELInitialStr)
        {
            return String.Format("{0:0.000}", (((_ELStr * (1 - (_StrDepretion / 100))) - _ELInitialStr) * -1));
        }

    }
}
