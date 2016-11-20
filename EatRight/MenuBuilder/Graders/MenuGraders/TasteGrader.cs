﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MenuGraders
{
    class TasteGrader : PerDayGrader
    {
        public TasteGrader()
        {
            dailyGrader = new TasteDailyGrader();
        }
    }
}