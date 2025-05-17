using UnityEditor;

[CustomEditor(typeof(CardData))]
public class CardEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		SerializedProperty title = serializedObject.FindProperty("Title");
		SerializedProperty description = serializedObject.FindProperty("Description");
		SerializedProperty isOwn = serializedObject.FindProperty("isOwn");
		SerializedProperty piece = serializedObject.FindProperty("pieces");

		EditorGUILayout.PropertyField(title);
		EditorGUILayout.PropertyField(description);
		EditorGUILayout.PropertyField(isOwn);

		if (isOwn.boolValue)
		{
			EditorGUILayout.PropertyField(piece);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
