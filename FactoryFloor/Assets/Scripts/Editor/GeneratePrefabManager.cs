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
	private static string format = "\tpublic static {0} {0} {{ get {{ return instance.{1}; }} }}\n\t[SerializeField] private {0} {1};\n";
	private static string preamble = "// ----- AUTO GENERATED CODE ----- //\n"
		+"using UnityEngine;\n"
		+"public class PrefabManager : MonoBehaviour {\n"
		+"\tprivate static PrefabManager instance;\n"
		+"\tprivate void Awake(){ instance = this; }\n";
	private static string postamble = "}\n";

	[MenuItem("Generate/PrefabManager")]
	public static void Generate()
	{
		EditorApplication.LockReloadAssemblies();
		try
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(preamble);
			foreach(System.Type type in GetPrefabManagerAttributeTypes())
			{
				builder.Append( string.Format(format, type.Name, "_" + type.Name));
			}			
			builder.Append(postamble);
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
				yield return type;
			}
		}
	}
}
