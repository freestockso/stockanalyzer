﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinanceAnalyzer.DB;
using Stock.Common.Data;
using FinanceAnalyzer.Stock;

namespace FinanceAnalyzer.Business.Shape
{
    interface ITripleShapeScanner
    {
        OperType Analyse(IStockData prevStock, IStockData stock, IStockData nextStock);
    }
}
