using UnityEngine;

[CreateAssetMenu(fileName = "MR", menuName = "Mr Control", order = 1)]
public class Settings : ScriptableObject
{
    public bool isMrOn;
    public float chairHeight;
    public float tableHeight;
}
