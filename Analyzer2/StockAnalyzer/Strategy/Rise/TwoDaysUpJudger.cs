using System;
using System.Collections.Generic;
using System.Text;
using FinanceAnalyzer.DB;

namespace FinanceAnalyzer.Strategy.Rise
{
    class TwoDaysUpJudger : IStockJudger
    {
        // ��ʱ�������������죬�ж��Ƿ���������
        // Day1��ǰ��Day2��һЩ. 
        public bool FulFil(StockData day1, StockData day2, StockData day3)
        {
            // ����������������
            return StockJudger.IsUp(day1) && StockJudger.IsUp(day2);
        }

        // ��ʱ�������������죬�ж��Ƿ������෴������
        // Day1��ǰ��Day2��һЩ. 
        public bool ReverseFulFil(StockData day1, StockData day2, StockData day3)
        {
            // ���������µ�����
            return !StockJudger.IsUp(day1) && !StockJudger.IsUp(day2);
        }

        public string Name
        {
            get
            {
                return "2days up";
            }
        }
    }
}
