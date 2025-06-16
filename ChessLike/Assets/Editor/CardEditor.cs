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
		SerializedProperty isZone = serializedObject.FindProperty("isZone");
		SerializedProperty requireZoneCnt = serializedObject.FindProperty("RequireZoneCnt");
		SerializedProperty maxZoneCnt = serializedObject.FindProperty("MaxZoneCnt");

		SerializedProperty ispiece = serializedObject.FindProperty("isPiece");
		SerializedProperty requirePieceCnt = serializedObject.FindProperty("RequirePieceCnt");
		SerializedProperty maxPieceCnt = serializedObject.FindProperty("MaxPieceCount");
		SerializedProperty piece = serializedObject.FindProperty("pieces");
		SerializedProperty tier = serializedObject.FindProperty("cardTier");
		SerializedProperty image = serializedObject.FindProperty("cardImage");


		EditorGUILayout.PropertyField(title);
		EditorGUILayout.PropertyField(description);
		EditorGUILayout.PropertyField(isOwn);

		if (isOwn.boolValue)
		{
			EditorGUILayout.PropertyField(piece);
		}

		EditorGUILayout.PropertyField(isZone);

		if (isZone.boolValue)
		{
			EditorGUILayout.PropertyField(requireZoneCnt);
			EditorGUILayout.PropertyField(maxZoneCnt);
		}

		EditorGUILayout.PropertyField(ispiece);

		if (ispiece.boolValue)
		{
			EditorGUILayout.PropertyField(requirePieceCnt);
			EditorGUILayout.PropertyField(maxPieceCnt);
		}

		EditorGUILayout.PropertyField(tier);
		EditorGUILayout.PropertyField(image);
		serializedObject.ApplyModifiedProperties();
	}
}
