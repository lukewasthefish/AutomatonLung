using UnityEditor;
using System;

public static class IMGUIDebugger
{

	static Type type = Type.GetType("UnityEditor.GUIViewDebuggerWindow,UnityEditor");

	[MenuItem("Window/IMGUI Debugger")]
	public static void Open() => EditorWindow.GetWindow(type).Show();
	
	
}