using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public static class ClipTransform {

	[MenuItem("GameObject/Clip Transform/Copy PosRotScale", false, 0)]
	public static void CopyPRS()
	{
		var builder = new StringBuilder();
		var gameObject = Selection.activeGameObject;


		builder.AppendFormat("Vector3{0}\tVector3{1}\tVector3{2}"
			, gameObject.transform.localPosition.ToString()
			, gameObject.transform.localEulerAngles.ToString()
			, gameObject.transform.localScale.ToString()
			);

		EditorGUIUtility.systemCopyBuffer = builder.ToString();
	}
	[MenuItem("GameObject/Clip Transform/Paste PosRotScale", false, 0)]
	public static void PastePRS()
	{
		var gameObject = Selection.activeGameObject;

		Undo.RecordObject(gameObject.transform, "Paste PosRotScale " + gameObject.name);

		var fields = EditorGUIUtility.systemCopyBuffer.Split('\t');
		for (int i = 0; i < 3; i++) {
			var match = regexVector3.Match(fields[i]);
			if (!match.Success) {
				continue;
			}
			var values = match.Groups[1].Value.Split(',');
			var vector = new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
			switch(i) {
			case 0:
				gameObject.transform.localPosition = vector;
				break;
			case 1:
				gameObject.transform.localEulerAngles = vector;
				break;
			case 2:
				gameObject.transform.localScale = vector;
				break;
			}
		}
	}
	static Regex regexVector3 = new Regex(@"Vector3\((.+)\)");
}
