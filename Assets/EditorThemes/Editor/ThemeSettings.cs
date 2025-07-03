using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ThemesPlugin;
using UnityEditorInternal;
//to do TextColor
//EditorStyles.label.normal.textColor 

namespace ThemesPlugin
{

    public class ThemeSettings : EditorWindow
    {
        public List<string> AllThemes = new List<string>();
        

        [MenuItem("Themes/Select Themes")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ThemeSettings>("Theme Settings");
        }


        

        public string ThemeName;

        Vector2 scrollPosition;


        private void OnGUI()
        {
            
            //window code

            GUILayout.Label("Create & Select Themes", EditorStyles.boldLabel);
            GUILayout.Label("Currently Selected: " + Path.GetFileNameWithoutExtension(ThemesUtility.currentTheme), EditorStyles.boldLabel);



            if (GUILayout.Button("Create new Theme"))
            {
                
                CreateThemeWindow window = (CreateThemeWindow)EditorWindow.GetWindow(typeof(CreateThemeWindow), false, "Create Theme");
                window.Show();
            }
            GUILayout.Label("or Select:", EditorStyles.boldLabel);

            

            List<CustomTheme> DarkThemes = new List<CustomTheme>();
            List<CustomTheme> LightThemes = new List<CustomTheme>();
            List<CustomTheme> BothThemes = new List<CustomTheme>();

            foreach (string s in Directory.GetFiles(ThemesUtility.CustomThemesPath, "*"+ ThemesUtility.Enc))
            {
                
                CustomTheme ct = ThemesUtility.GetCustomThemeFromJson(s);
                switch (ct.unityTheme)
                {
                    case CustomTheme.UnityTheme.Dark:
                        DarkThemes.Add(ct);
                        break;
                    case CustomTheme.UnityTheme.Light:
                        LightThemes.Add(ct);
                        break;
                    case CustomTheme.UnityTheme.Both:
                        BothThemes.Add(ct);
                        break ;
                }
            }
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Dark & Light Themes:");
            foreach (CustomTheme ct in BothThemes)
            {
                DisplayGUIThemeItem(ct);
            }


            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Dark Themes:");
            foreach(CustomTheme ct in DarkThemes)
            {
                DisplayGUIThemeItem(ct);
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Light Themes:");
            foreach (CustomTheme ct in LightThemes)
            {
                DisplayGUIThemeItem(ct);
            }

            EditorGUILayout.EndScrollView();


        }


        void DisplayGUIThemeItem(CustomTheme ct)
        {
            
            string Name = ct.Name;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(Name))
            {

                ThemesUtility.LoadUssFileForTheme(Name);
            }

            if (!ct.IsUnEditable && GUILayout.Button("Edit", GUILayout.Width(70)))
            {

                ThemesUtility.OpenEditTheme(ct);
                //EditThemeWindow.ct = ct;
                //EditThemeWindow window = (EditThemeWindow)EditorWindow.GetWindow(typeof(EditThemeWindow), false, "Edit Theme");

                //window.Show();

            }
            if (!ct.IsUnDeletable && GUILayout.Button("Delete", GUILayout.Width(70)))
            {
                if (EditorUtility.DisplayDialog("Do you want to Delete " + ct.Name + " ?", "Do you want to Permanently Delete the Theme " + ct.Name +" (No undo!)", "Delete", "Cancel") == false)
                {
                    return;
                }


                ThemesUtility.DeleteFileWithMeta(ThemesUtility.GetPathForTheme(Name));

                ThemesUtility.LoadUssFileForTheme("_deafault");

            }

            EditorGUILayout.EndHorizontal();
        }
    


    }
}