using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using FinanceAnalyzer.DB;

namespace FinanceAnalyzer.Strategy.Impl
{
    public class StrategyBear : IFinanceStrategy
    {
        // ָ��Ϊ���û�й�Ʊ�����տ������룬�����ǵ����ﵽx%������ֹӯֹ����ԣ�
        public override ICollection<StockOper> GetOper(DateTime day, IAccount account)
        {
            IStockData prevStockProp = _StockHistory.GetPrevDayStock(day);
            IStockData curStockProp = _StockHistory.GetStock(day);
            if ((prevStockProp == null) || (curStockProp == null))
            {
                return null;
            }

            ICollection<StockOper> opers = new List<StockOper>();

            if (!_StockHolder.HasStock())
            {                
                int stockCount = Transaction.GetCanBuyStockCount(account.BankRoll,
                        curStockProp.StartPrice); // ���û�й�Ʊ�����쿪������
                if (stockCount > 0)
                {
                    StockOper oper = new StockOper(curStockProp.StartPrice, stockCount, OperType.Buy);
                    opers.Add(oper);
                    return opers; // �����ʣ�Ŀǰ��֧��һ��֮�ڵ�����
                }
            }            
            else
            {
                double unitCost = _StockHolder.UnitPrice;
                if (unitCost > 0)
                {
                    if (curStockProp.MaxPrice >= (unitCost * (1 + winPercent)))
                    {
                        // ֹӯ
                        StockOper oper2 = new StockOper(unitCost * (1 + winPercent), _StockHolder.StockCount(), OperType.Sell);
                        opers.Add(oper2);
                        return opers;
                    }
                    
                    if (curStockProp.MinPrice <= (unitCost * (1 - winPercent)))
                    {
                        // ֹ��
                        StockOper oper1 = new StockOper(unitCost * (1 - winPercent), _StockHolder.StockCount(), OperType.Sell);
                        opers.Add(oper1);
                        return opers;
                    }                    
                }
            }

            return null;
        }

        public override string Name
        {
            get
            {
                return "Bear: " + Profit.ToString("F03", CultureInfo.CurrentCulture);
            }
        }

        // ӯ�������İٷֱ�
        public double Profit
        {
            get
            {
                return winPercent;
            }
            set
            {
                winPercent = value;
            }
        }

        private double winPercent;
    }
}
