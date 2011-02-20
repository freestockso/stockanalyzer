﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceAnalyzer.Strategy.Indicator;
using FinanceAnalyzer.Strategy.Impl;

namespace FinanceAnalyzer.Strategy.Factory
{
    class VolumeAdjustFactory : StrategyFactory
    {
        public override void Init()
        {
            for (double i = 0.1; i < 1.2; i += 0.1 )
            {
                for (double j = 0.1; j < 1.2; j += 0.1)
                {
                    AddStrategy(new StrategyIndicator(new VolumeCalc(i, j)));
                }
            }

            AddStrategyByIndicator(new HoldCalc());
        }
    }
}
