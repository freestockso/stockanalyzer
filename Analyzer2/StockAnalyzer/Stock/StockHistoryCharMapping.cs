﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stock.Common.Data;
using FinanceAnalyzer.Utility;

namespace FinanceAnalyzer.Stock
{
    /// <summary>
    /// Convert Stock history to a string for find
    /// </summary>
    class StockHistoryCharMapping
    {
        /// <summary>
        /// Init String map
        /// </summary>
        public StockHistoryCharMapping()
        {
            Init();
        }

        void Init()
        {
            List<string> chars = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                for (char j = 'a'; j < 'z'; j++)
                {
                    string s = string.Format("{0}{1}", j.ToString(), i);
                    chars.Add(s);
                }
            }

            for (int i = -101; i < 102; i++)
            {
                RatioToString_.Add(i, chars[i + 101]);
            }
        }

        string GetRatioString(double ratio)
        {
            int r = (int)(ratio * 1000) + 101;

            if (r > MAXRATIO)
            {
                return RatioToString_[MAXRATIO];
            }
            else if (r < MINRATIO)
            {
                return RatioToString_[MINRATIO];
            }
            else
            {
                return RatioToString_[r];
            }
        }
        
        public string GetCharMapping(IStockHistory hist)
        {
            string totalMapping = "";
            DateTime startDate = hist.MinDate;
            DateTime endDate = hist.MaxDate;

            while (startDate < endDate)
            {
                IStockData todayData = hist.GetStock(startDate);
                IStockData pervData = hist.GetPrevDayStock(startDate);

                if (todayData == null)
                {
                    startDate = DateFunc.GetNextWorkday(startDate);
                    continue;
                }

                string s = ParseChars(pervData, todayData);

                if (!string.IsNullOrEmpty(s))
                {
                    totalMapping += s + "|";
                }

                startDate = DateFunc.GetNextWorkday(startDate);
            }

            return totalMapping;
        }

        public string ParseChars(IStockData prevData, IStockData todayData)
        {
            if (todayData == null)
            {
                throw new ArgumentNullException();
            }

            double priseRatio = 0.0;
            if (prevData != null)
            {
                priseRatio = (todayData.StartPrice - prevData.EndPrice) / prevData.EndPrice;
            }
            double upRatio = (todayData.MaxPrice - todayData.StartPrice) / todayData.StartPrice;
            double downRatio = (todayData.MinPrice - todayData.StartPrice) / todayData.StartPrice;
            double endRatio = (todayData.EndPrice - todayData.StartPrice) / todayData.StartPrice;

            string todayMapping = GetRatioString(priseRatio) + GetRatioString(upRatio)
                + GetRatioString(downRatio) + GetRatioString(endRatio);

            return todayMapping;
        }

        const int MINRATIO = -101;
        const int MAXRATIO = 101;

        Dictionary<int, string> RatioToString_ = new Dictionary<int, string>();
    }
}
