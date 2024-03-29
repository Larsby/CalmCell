using UnityEngine;
using UnityEditor;

public static class DebugMenu
{

	[MenuItem("Debug/Print Global Transforms")]
	public static void PrintGlobaTransforms()
	{
		if (Selection.activeGameObject != null)
		{
			Debug.Log(Selection.activeGameObject.name + ":\nPosition: " + Selection.activeGameObject.transform.position + "        Rotation: " + Selection.activeGameObject.transform.rotation + "        Scale: " + Selection.activeGameObject.transform.lossyScale);
		}
	}
}
