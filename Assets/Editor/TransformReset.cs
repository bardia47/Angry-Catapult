using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
[CustomEditor(typeof(Transform)),CanEditMultipleObjects]
public class TransformReset : DecoratorEditor
{

    bool unfoled = false;
    static Vector3 resetPosition = Vector3.zero;
    static Vector3 resetRotation= Vector3.zero;
    static Vector3 resetScale = Vector3.one;

    SerializedProperty p;
    SerializedProperty r;
    SerializedProperty s;

    public TransformReset() : base("TransformInspector") { }
    private void OnEnable()
    {
        p = serializedObject.FindProperty("m_LocalPosition");
        r = serializedObject.FindProperty("m_LocalRotation");
        s = serializedObject.FindProperty("m_LocalScale");
        if (EditorPrefs.HasKey("CustomOriginResetPosition")) {
            resetPosition = StringToVector3(EditorPrefs.GetString("CustomOriginResetPosition"));
        }
        if (EditorPrefs.HasKey("CustomOriginResetRotation"))
        {
            resetRotation = StringToVector3(EditorPrefs.GetString("CustomOriginResetRotation"));
        }
        if (EditorPrefs.HasKey("CustomOriginResetScale"))
        {
            resetScale = StringToVector3(EditorPrefs.GetString("CustomOriginResetScale"));
        }

    }

    public override void OnInspectorGUI()
    {
        //Draw's defualt Inspector
        base.OnInspectorGUI();
        //Add reset for position , rotation and scale button
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset All", EditorStyles.miniButton))
        {
            s.vector3Value = resetScale;
            p.vector3Value = resetPosition;
            r.quaternionValue = Quaternion.Euler(resetRotation);
            serializedObject.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Position", EditorStyles.miniButtonLeft))
        {
            p.vector3Value = resetPosition;
            serializedObject.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Rotation", EditorStyles.miniButtonMid))
        {

            r.quaternionValue = Quaternion.Euler(resetRotation);
            serializedObject.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }
        if (GUILayout.Button("Scale", EditorStyles.miniButtonRight))
            {
                s.vector3Value = resetScale;
                serializedObject.ApplyModifiedProperties();
                GUI.FocusControl(null);
            }
        EditorGUILayout.EndHorizontal();
        string originalLabel;
        if (resetPosition != Vector3.zero || resetRotation != Vector3.zero || resetScale != Vector3.zero)
        {
            originalLabel = "Set Origin [Custom]";
        }
        else {
            originalLabel = "Set Origin [Default]";

        }

        unfoled = EditorGUILayout.Foldout(unfoled, originalLabel);
        if (unfoled) {
            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Clear Custom Origin", EditorStyles.miniButton))
            {
                resetPosition = Vector3.zero;
                resetRotation = Vector3.zero;
                resetScale = Vector3.one;

            }
            resetPosition = EditorGUILayout.Vector3Field("Position", resetPosition);
            resetRotation = EditorGUILayout.Vector3Field("Rotation", resetRotation);
            resetScale = EditorGUILayout.Vector3Field("Scale", resetScale);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString("CustomOriginResetPosition",resetPosition.ToString());
                EditorPrefs.SetString("CustomOriginResetRotation",resetRotation.ToString());
                EditorPrefs.SetString("CustomOriginResetScale",resetScale.ToString());

            }
        }

    }   
    private Vector3 StringToVector3(string vector)
    {
        if (vector.StartsWith("(") && vector.EndsWith(")"))
        {
            vector = vector.Substring(1, vector.Length - 2);
        }
        string[] array = vector.Split(",");
        Vector3 result = new Vector3(
            float.Parse(array[0]),
            float.Parse(array[1]),
            float.Parse(array[2]));
        return result;

    }
}

public abstract class DecoratorEditor : Editor
{
    private static readonly object[] EMPTY_ARRAY = new object[0];
  
    #region Editor Fields
    private System.Type decoratorEditorType;
    private System.Type editorObjectType;
    private Editor editorInstance;
    #endregion

    private static Dictionary<string, MethodInfo> decoratedMethods = new Dictionary<string, MethodInfo>();
   
    private static Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));

    protected Editor EditorInstance {
        get
        {
            if (editorInstance == null && targets != null && targets.Length > 0)
            {
                editorInstance = Editor.CreateEditor(targets, decoratorEditorType);
            }
            if (editorInstance == null)
            {

                Debug.LogError("Could not create editor!");
            }
            return editorInstance;
        }

    }
    public DecoratorEditor(string editorTypeName) {
        this.decoratorEditorType = editorAssembly.GetTypes()
            .Where(t => t.Name == editorTypeName).FirstOrDefault();

        Init();

        var originalEditorTypeName = GetCustomEditorType(decoratorEditorType);
        if (originalEditorTypeName != editorObjectType)
        {
            throw new System.ArgumentException($"Type {editorObjectType} does not match the editor {editorTypeName}");
        }
    }

    private void Init()
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance;
        var attributes = GetType().GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
        var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();
        editorObjectType = field.GetValue(attributes[0]) as System.Type;
    
    }

    private System.Type GetCustomEditorType(System.Type type)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Instance;
        var attributes = type.GetCustomAttributes(typeof(CustomEditor), true) as CustomEditor[];
        var field = attributes.Select(editor => editor.GetType().GetField("m_InspectedType", flags)).First();
        return field.GetValue(attributes[0]) as System.Type;
    }

    private void OnDisable()
    {
        if (editorInstance != null)
        {
            DestroyImmediate(editorInstance);

        }
    }
    protected void CallInspectorMethod(string methodName)
    {
        MethodInfo method = null;
        //add methodInfo to cache
        if (!decoratedMethods.ContainsKey(methodName))
        {
            var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            method = decoratorEditorType.GetMethod(methodName, flags);
            if (method != null)
            {
            method =   decoratedMethods[methodName];
            }
            else
            {
                Debug.LogError($"Could not find method {methodName}");

            }
        }
        if (method != null)
        {
            method.Invoke(EditorInstance, EMPTY_ARRAY);
        }

    }

    protected override void OnHeaderGUI()
    {
        CallInspectorMethod("OnHeaderGUI");
    }

    public override void OnInspectorGUI()
    {
        EditorInstance.OnInspectorGUI();
    }
    public override void DrawPreview(Rect previewArea)
    {
        EditorInstance.DrawPreview(previewArea);
    }

    public override string GetInfoString()
    {
        return EditorInstance.GetInfoString();
    }
    public override GUIContent GetPreviewTitle()
    {
        return EditorInstance.GetPreviewTitle();
    }

    public override bool HasPreviewGUI()
    {
        return EditorInstance.HasPreviewGUI();
    }
    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        EditorInstance.OnInteractivePreviewGUI(r, background);
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        EditorInstance.OnPreviewGUI(r, background);
    }

    public override void OnPreviewSettings()
    {
        EditorInstance.OnPreviewSettings();
    }

    public override void ReloadPreviewInstances()
    {
        EditorInstance.ReloadPreviewInstances();
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        return EditorInstance.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    public override bool RequiresConstantRepaint()
    {
        return EditorInstance.RequiresConstantRepaint();
    }

    public override bool UseDefaultMargins()
    {
        return EditorInstance.UseDefaultMargins();
    }
}