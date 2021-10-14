using UnityEditor;
using UnityEngine;
using VROOM.Scripts;

namespace VROOM
{
    [CustomEditor(typeof(PassDetection))]
    public class CameraControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            _avatarMeshController = (PassDetection)target;

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            var isButtonPressed = GUILayout.Button("Calibrate Space");
            EditorGUILayout.EndHorizontal();
            
            if(isButtonPressed)
                _avatarMeshController.CalibrateOrigin();
        }

        PassDetection _avatarMeshController;
    }
}
