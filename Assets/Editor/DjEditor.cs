using Audio;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    [CustomEditor(typeof(Dj))]
    public class DjEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var dj = (Dj)target;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300));
            
            var songStyle = new GUIStyle(EditorStyles.textField)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white, background = Texture2D.blackTexture },
                fontSize = 14
            };
            
            GUILayout.Space(10);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            var time = "00:00";
            var length = "00:00";
            var song = "#game is not running#";
            if (Application.isPlaying)
            {
                time = $"{(int)(dj.CurrentSongTime / 60):00}:{(int)(dj.CurrentSongTime % 60):00}";
                length = $"{(int)(dj.CurrentSongLength / 60):00}:{(int)(dj.CurrentSongLength % 60):00}";
                song = dj.CurrentSongName;
            }
            GUILayout.Label($"{song}\n{time}/{length}", songStyle, GUILayout.Height(40));
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);
            if (Application.isPlaying)
            {
                EditorGUI.BeginChangeCheck();
                var newProgress = EditorGUILayout.Slider(dj.Progress, 0f, 0.98f);
                if (EditorGUI.EndChangeCheck())
                {
                    dj.Progress = newProgress;
                }
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(dj.IsPaused ? "Resume" : "Pause", GUILayout.Width(80)) && Application.isPlaying)
            {
                dj.IsPaused = !dj.IsPaused;
            }
            if (GUILayout.Button("Next", GUILayout.Width(80)) && Application.isPlaying)
            {
                dj.Next();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();

            if (Application.isPlaying)
            {
                Repaint();
            }
        }
    }
}