using UnityEngine;

namespace Hide.Events
{
    [CreateAssetMenu(fileName = "StringGameEvent", menuName = "ScriptableObject/Event/StringGameEvent", order = 0)]
    public class StringGameEvent : ParameterizedGameEvent<string>
    {
        
    }
}