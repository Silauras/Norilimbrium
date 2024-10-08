namespace Code.Scripts.Utils
{
    public interface IConditionEvaluator
    {
        bool? Evaluate(string predicate, string[] args);
    }
}