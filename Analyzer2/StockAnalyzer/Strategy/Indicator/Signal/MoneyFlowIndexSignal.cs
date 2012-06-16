﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stock.Common.Data;
using FinanceAnalyzer.DB;
using System.Diagnostics;

namespace FinanceAnalyzer.Strategy.Indicator.Signal
{
    class MoneyFlowIndexSignal : ISignalCalculator
    {
        class PriceUpDown
        {
            public double Price
            {
                get;
                set;
            }
            public bool IsUp
            {
                get;
                set;
            }

            public PriceUpDown(double price, bool isUp)
            {
                Price = price;
                IsUp = isUp;
            }
        };

        #region ISignalCalculator Members

        public bool AddStock(IStockData sd)
        {
            if (sd == null)
            {
                return false;
            }

            prevStock = currentStock;
            currentStock = sd;

            if ((prevStock == null) || (currentStock == null))
            {
                return false;
            }

            double typicalPrice = CalcTypicalPrice(currentStock);
            bool isUp = StockJudger.IsRise(currentStock, prevStock);
            AddTodayPrice(typicalPrice * currentStock.Amount, isUp); // 直接保存Money Flow, 即Typical Price * Volume 

            if (PriceUpDowns_.Count < MFICALCDAYS)
            {
                return false;
            }

            CalculateMFI();
            return true;
        }

        private void CalculateMFI()
        {
            Debug.Assert(PriceUpDowns_.Count == MFICALCDAYS);

            double upSum = 0;
            double allSum = 0;
            for (int i = 0; i < PriceUpDowns_.Count; i++)
            {
                allSum += PriceUpDowns_[i].Price;

                if (PriceUpDowns_[i].IsUp)
                {
                    upSum += PriceUpDowns_[i].Price;
                }
            }

            currentMFI_ = 100 * (upSum / allSum);
        }

        private void AddTodayPrice(double price, bool isUp)
        {
            PriceUpDowns_.Add(new PriceUpDown(price, isUp));
            if (PriceUpDowns_.Count > MFICALCDAYS)
            {
                PriceUpDowns_.RemoveAt(0);
            }
        }

        public OperType GetSignal()
        {
            if (currentMFI_ < MFIBUYMARGIN)
            {
                return OperType.Buy;
            }
            else if (currentMFI_ > MFISELLMARGIN)
            {
                return OperType.Sell;
            }
            else
            {
                return OperType.NoOper;
            }
        }

        public string GetName()
        {
            return "MFI Signal";
        }

        #endregion

        private static double CalcTypicalPrice(IStockData val)
        {
            return (val.MaxPrice + val.MinPrice + val.EndPrice) / 3;
        }

        IStockData prevStock = null;
        IStockData currentStock = null;
        double currentMFI_ = double.NaN;

        List<PriceUpDown> PriceUpDowns_ = new List<PriceUpDown>();
        
        private const int MFICALCDAYS = 14; // MFI计算周期 
        private const double MFIBUYMARGIN = 30; // 买入门限
        private const double MFISELLMARGIN = 70; // 卖出门限
    }
}