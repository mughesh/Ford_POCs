using UnityEngine;

public static class GameObjectUtils
{
    /// <summary>
    /// Creates a completely empty GameObject with only a Transform.
    /// </summary>
    public static GameObject CreateEmpty(GameObject dupe = null, string name = "Empty")
    {
        if (dupe == null)
            dupe = new GameObject(name);
        else
        {
            dupe = GameObject.Instantiate(dupe);
            dupe.name = name;
        }

#if UNITY_EDITOR
        void StripOrDisable(Transform t)
        {
            foreach (var comp in t.GetComponents<Component>())
            {
                if (comp is Transform || comp is MeshRenderer || comp is Renderer || comp is MeshFilter)
                    continue; // Keep Transform and Renderer

                // Try to destroy component
                try
                {
                    Object.DestroyImmediate(comp);
                }
                catch
                {
                    // If Unity prevents removal, disable it
                    if (comp is Behaviour behaviour)
                        behaviour.enabled = false;
                    else if (comp is Collider collider)
                        collider.enabled = false;
                    // You can add other types as needed
                }
            }

            foreach (Transform child in t)
            {
                StripOrDisable(child);
            }
        }

        StripOrDisable(dupe.transform);
        StripOrDisable(dupe.transform);
#endif
        return dupe;
    }
}
