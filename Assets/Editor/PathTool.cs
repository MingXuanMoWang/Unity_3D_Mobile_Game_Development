using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathTool : ScriptableObject
{
    private static PathNode m_parent = null;

    [MenuItem("PathTool/Create Path Node")]
    static void CreatePathNode()
    {
        GameObject go = new GameObject();
        go.AddComponent<PathNode>();
        go.name = "pathnode";
        go.tag = "pathnode";
        Selection.activeTransform = go.transform;
    }

    [MenuItem("PathTool/Set Parent %q")]
    static void SetParent()
    {
        if (!Selection.activeGameObject || Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1)
        {
            Debug.LogError("Please select only one PathNode to set as parent.");
            return;
        }

        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            m_parent = Selection.activeGameObject.GetComponent<PathNode>();
        }
    }

    [MenuItem("PathTool/Set Next %w")]
    static void SetNextChild()
    {
        if (!Selection.activeGameObject || m_parent == null ||
            Selection.GetTransforms(SelectionMode.Unfiltered).Length > 1)
        {
            return;
        }

        if (Selection.activeGameObject.tag.CompareTo("pathnode") == 0)
        {
            m_parent.SetNext(Selection.activeGameObject.GetComponent<PathNode>());
            m_parent = null;
        }
    }
}
