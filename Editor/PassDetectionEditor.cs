using Balltracking.Scripts;
using UnityEditor;
using UnityEngine;

namespace Balltracking
{
	[CustomEditor(typeof(PassDetectionController))]
	public class CameraControllerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			_avatarMeshController = (PassDetectionController)target;

			EditorGUILayout.Space(10);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(" ");
			var isButtonPressed = GUILayout.Button("Calibrate Space");
			EditorGUILayout.EndHorizontal();

			if (isButtonPressed)
				_avatarMeshController.CalibrateOrigin();
		}

		PassDetectionController _avatarMeshController;
	}
}