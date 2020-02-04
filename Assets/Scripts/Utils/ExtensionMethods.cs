using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class ExtensionMethods
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            int count = enumerable.Count();
            if (count > 0)
            {
                int index = Random.Range(0, enumerable.Count());
                return enumerable.ElementAt(index);
            }

            return default(T);
        }

        public static float UnclampedEval(this AnimationCurve curve, float time)
        {
            if (curve.keys.Length > 0)
            {
                Keyframe firstKeyframe = curve.keys.First();
                Keyframe lastKeyframe = curve.keys.Last();
                if (time < firstKeyframe.time)
                {
                    float offset = firstKeyframe.time - time;
                    Vector2 firstKeyframePos = new Vector2(firstKeyframe.time, firstKeyframe.value);
                    Vector2 offsetVector = -new Vector2(Mathf.Acos(firstKeyframe.inTangent), Mathf.Asin(firstKeyframe.inTangent)).normalized;
                    return (firstKeyframePos + offsetVector * offset).y;
                }
                else if (time <= lastKeyframe.time)
                {
                    return curve.Evaluate(time);
                }
                else
                {
                    float offset = time - lastKeyframe.time;
                    Vector2 lastKeyframePos = new Vector2(lastKeyframe.time, lastKeyframe.value);
                    Vector2 offsetVector = new Vector2(Mathf.Acos(lastKeyframe.outTangent), Mathf.Asin(lastKeyframe.outTangent)).normalized;
                    return (lastKeyframePos + offsetVector * offset).y;
                }
            }

            return 0;
        }

        public static void SwitchItems<T>(this List<T> list, int first, int second)
        {
            T firstValue = list[first];
            list.RemoveAt(first);
            list.Insert(second, firstValue);
            //            T secondValue = list[second];
            //            list[second] = list[first];
            //            list[first] = secondValue;
        }
    }
}