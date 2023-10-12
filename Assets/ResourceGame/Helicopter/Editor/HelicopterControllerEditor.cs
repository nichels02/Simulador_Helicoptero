using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEditor;
//[CustomEditor(typeof(HelicopterControllerPlayer))]
//public class HelicopterControllerEditor : Editor
//{
//    public Texture2D logoTexture;
//    private Rect m_controlRect;

//    HelicopterControllerPlayer m_target;

//    public List<SerializedProperty> Rotores = new List<SerializedProperty>();
    
//    private void OnEnable()
//    {
//        m_target = (HelicopterControllerPlayer)target;
//       // m_target.Rotores = new List<SerializedProperty>();
//        //Rotores = serializedObject.FindProperty("Rotores");
//    }
//    public override void OnInspectorGUI()
//    {
        
//        EditorGUILayout.Space();
//        EditorGUILayout.Space();
//        EditorGUI.BeginChangeCheck();

//        logoTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/ResourceGame/Helicopter/Editor/UI/mi24.png", typeof(Texture2D));
        
//        // Logo
//        m_controlRect = EditorGUILayout.GetControlRect(GUILayout.Height(85));
//        EditorGUI.LabelField(m_controlRect, "", "", "selectionRect");
//        if (logoTexture) GUI.DrawTexture(new Rect(m_controlRect.x, m_controlRect.y, 261, 56), logoTexture);
//        EditorGUILayout.Space(-18);
//        GUILayout.Label(" Version 1.0.0", EditorStyles.miniLabel);
        
//        for (int i = 0; i < 5; i++)
//        {
//            EditorGUILayout.Space();
//        }
//        serializedObject.Update();

       
//        serializedObject.ApplyModifiedProperties();

//        //EditorGUILayout.PropertyField(m_pivotRayHeigth);

//        EditorGUI.BeginChangeCheck();
//        base.OnInspectorGUI();
//    }
//}
