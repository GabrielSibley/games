using System.IO;
using System.Text;
using Type = System.Type;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

public static class GeneratePrefabManager
{

	private static string path = "Assets/Scripts/PrefabManager.cs";
	private static string format = 
@"public static {0} {0} {{	get	{{ return instance.{1}; }} }} [SerializeField] private {0} {1};";

	[MenuItem("Generate/PrefabManager")]
	public static void Generate()
	{
		EditorApplication.LockReloadAssemblies();
		try
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("// ----- AUTO GENERATED CODE ----- //");
			builder.AppendLine("using UnityEngine;");
			builder.AppendLine("public class PrefabManager : MonoBehaviour");
			builder.AppendLine("{");
			builder.AppendLine("private static PrefabManager instance;");
			builder.AppendLine("private void Awake(){instance = this; }");
			foreach(System.Type type in GetPrefabManagerAttributeTypes())
			{
				builder.AppendLine( string.Format(format, type.Name, "_" + type.Name));
			}
			
			builder.AppendLine("}");
			File.WriteAllText( path, builder.ToString() );
		}
		finally{
			EditorApplication.UnlockReloadAssemblies();
		}
		AssetDatabase.Refresh();
	}

	static IEnumerable<Type> GetPrefabManagerAttributeTypes() {
		Assembly assembly = Assembly.GetAssembly(typeof(PrefabManagerAttribute));
		foreach(Type type in assembly.GetTypes()) {
			if (type.GetCustomAttributes(typeof(PrefabManagerAttribute), true).Length > 0) {
				Debug.Log (type.ToString());
				yield return type;
			}
		}
	}
}
