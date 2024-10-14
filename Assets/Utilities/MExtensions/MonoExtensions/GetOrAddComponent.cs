using UnityEngine;

public static class MMonoBehaviourComponentExtensions
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var comp = gameObject.GetComponent<T>();
        return (comp == null) ? gameObject.AddComponent<T>() : comp;
    }
}    
