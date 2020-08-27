using UnityEngine;

namespace Android
{
    // ReSharper disable once InconsistentNaming
    public class AndroidCSUtility
    {
        /// <summary>
        /// Reference: https://stackoverflow.com/a/42681889
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static AndroidJavaObject JavaArrayFromCS(string[] values)
        {
            var arrayClass = new AndroidJavaClass("java.lang.reflect.Array");
            var arrayObject = arrayClass.CallStatic<AndroidJavaObject>(
                "newInstance",
                new AndroidJavaClass("java.lang.String"),
                values.Length);
            for (int i = 0; i < values.Length; ++i)
            {
                arrayClass.CallStatic("set",
                    arrayObject, i,
                    new AndroidJavaObject("java.lang.String", values[i]));
            }

            return arrayObject;
        }
    }
}