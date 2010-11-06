using System;
using System.Collections.Generic;
using System.Text;
using FinanceAnalyzer.DB;

namespace FinanceAnalyzer.Strategy.Rise
{
    public class RiseJudger : IStockJudger
    {
        // ��ʱ�������������죬�ж��Ƿ���������
        // Day1��ǰ��Day2��һЩ. 
        public bool FulFil(StockData day1, StockData day2, StockData day3)
        {
            // ����������������
            return (StockJudger.IsRise(day3, day2) && StockJudger.IsRise(day2, day1));
        }

        // ��ʱ�������������죬�ж��Ƿ������෴������
        // Day1��ǰ��Day2��һЩ. 
        public bool ReverseFulFil(StockData day1, StockData day2, StockData day3)
        {
            // ���������µ�����
            return (!StockJudger.IsRise(day3, day2) && !StockJudger.IsRise(day2, day1));
        }

        public string Name
        {
            get
            {
                return "Compare prev";
            }
        }
    }
}
