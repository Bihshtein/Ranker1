using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Graders.MealGraders
{
    abstract class TasteMealGrader<T> : MealGrader
    {
        abstract protected void InitDataStructures(Meal meal);

        protected override double InternalGrade(Meal meal)
        {
            InitDataStructures(meal);
            if (flavorDict == null)
            {
                return 0.5;
            }
            if (objectList == null)
            {
                return 1;
            }

            double totalFlavorGrade = 0;
            
            foreach (var obj in objectList)
            {
                if (flavorDict.ContainsKey(obj))
                {
                    totalFlavorGrade += flavorDict[obj];
                }
            }

            int objNum = objectList.Count;
            double scaledTotalFlavorGrade = totalFlavorGrade + objNum;
            double maxFlavorGrade = 2 * objNum;

            return scaledTotalFlavorGrade / maxFlavorGrade;
        }

        protected Dictionary<T, double> flavorDict;
        protected List<T> objectList;
    }
}
