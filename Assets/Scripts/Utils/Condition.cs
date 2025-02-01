using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Scripts.Utils
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] Disjunction[] and;

        public bool Check(IEnumerable<IConditionEvaluator> conditions)
        {
            return and.All(dis => dis.Check(conditions));
        }
        
        [System.Serializable]
        class Disjunction
        {
            [SerializeField] private Predicate[] or;

            public bool Check(IEnumerable<IConditionEvaluator> evaluators)
            {
                return or.Any(predicate => predicate.Check(evaluators));
            }
            
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] string predicate;
            [SerializeField] string[] parameters;
            [SerializeField] bool negate;

            public bool Check(IEnumerable<IConditionEvaluator> evaluators)
            {
                return evaluators
                    .Select(evaluator => evaluator.Evaluate(predicate, parameters))
                    .Where(result => result != null)
                    .All(result => result != negate);
            }
        }
    }
}