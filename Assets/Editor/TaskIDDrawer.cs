using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TaskID))]
public class TaskIDDrawer : PropertyDrawer
{
    private static TaskDatabase database;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.serializedObject.targetObjects.Length > 1)
        {
            EditorGUI.LabelField(position, label.text, "(Multiple Object Editing Disabled)");
            return;
        }

        if (database == null)
        {
            database = Resources.Load<TaskDatabase>("TaskDatabase");
        }

        SerializedProperty idProp = property.FindPropertyRelative("ID");

        if (database == null || database.TaskIDs.Count == 0)
        {
            EditorGUI.PropertyField(position, idProp, label);
            return;
        }

        int currentIndex = Mathf.Max(0, database.TaskIDs.IndexOf(idProp.stringValue));
        currentIndex = EditorGUI.Popup(position, label.text, currentIndex, database.TaskIDs.ToArray());
        idProp.stringValue = database.TaskIDs[currentIndex];
    }
}
