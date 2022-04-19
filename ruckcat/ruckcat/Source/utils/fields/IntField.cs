using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ruckcat
{
    [Serializable]
    public class IntField : BaseField
    {
        [HorizontalGroup("1")] [HideLabel]
        public int defaultValue;

         [HorizontalGroup("1")]
        [BoxGroup("1/NoTitle", false)] [HideLabel]  public IntFloatValueType ValueType = IntFloatValueType.Constant;

        [ShowIfGroup("1/NoTitle/Curve/ValueType", Value = IntFloatValueType.Curve)] [BoxGroup("1/NoTitle/Curve", false)]
        public AnimationCurve animationCurve = AnimationCurve.Constant(0, 1, 0);

        [ShowIfGroup("1/NoTitle/MinMax/ValueType", Value = IntFloatValueType.Random)] [BoxGroup("1/NoTitle/MinMax", false)]
        public int minValue;

        [ShowIfGroup("1/NoTitle/MinMax/ValueType", Value = IntFloatValueType.Random)] [BoxGroup("1/NoTitle/MinMax", false)]
        public int maxValue;

        [ShowIfGroup("1/NoTitle/LevelMultiple/ValueType", Value = IntFloatValueType.LevelMultiple)]
        [BoxGroup("1/NoTitle/LevelMultiple", false)]
        public int multipleFactor;

        [ShowIfGroup("1/NoTitle/LevelAddition/ValueType", Value = IntFloatValueType.LevelAddition)]
        [BoxGroup("1/NoTitle/LevelAddition", false)]
        public int additionFactor;

        public int GetValue(float time = 1)
        {
            int levelNumber =  GetLevel();
            switch (ValueType)
            {
                case IntFloatValueType.Curve:
                    return (int) animationCurve.Evaluate(time);
                case IntFloatValueType.Constant:
                    return defaultValue;
                case IntFloatValueType.Random:
                    return RandomMinMaxValue();
                case IntFloatValueType.LevelMultiple:
                    return levelNumber == 1 ? defaultValue : (levelNumber - 1) * multipleFactor * defaultValue;
                case IntFloatValueType.LevelAddition:
                    return levelNumber == 1 ? defaultValue : (levelNumber - 1) * additionFactor + defaultValue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int RandomMinMaxValue()
        {
            return Random.Range(minValue, maxValue);
        }
    }
}