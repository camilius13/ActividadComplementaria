
using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectInspectable",menuName ="Inspect/Object") ]
public class ObjectInspectionable : ScriptableObject
{
    public string category;
    public GameObject objectPrefab;
    public string description;
}
