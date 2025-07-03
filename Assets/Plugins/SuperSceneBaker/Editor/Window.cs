using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

#if UNITY_5_5_OR_NEWER
using NavMeshBuilderClass = UnityEditor.AI.NavMeshBuilder;
#else
using NavMeshBuilderClass = UnityEditor.NavMeshBuilder;
#endif

namespace SuperSceneBaker
{
    public class Window : EditorWindow
    {
        const string kKeyPrefix = "SSB_";
        const float kVersion = 1.2f;

        private Vector2 scrollPos = new Vector2(0f, 0.5f);

        static Window m_Window;
        string m_Status = "Waiting for inputs";
        string m_Log = "";
        int m_CurrentSceneId = 0;
        bool m_IsProcessing = false;
#if UNITY_2019_2_OR_NEWER
        bool m_IsCallbackRegistered = false;
#endif

        private bool m_ComputeLighting = true;
        private bool m_ClearLighting = false;
        private bool m_ComputeOcclusion = true;
        private bool m_ClearOcclusion = false;
        private bool m_ComputeNavMesh = true;
        private bool m_ClearNavMesh = false;
        [SerializeField] SceneAsset[] m_ScenesList = new SceneAsset[0];

        [MenuItem("Tools/Super Scene Baker", false)]
        static void OpenWindow()
        {
            m_Window = EditorWindow.GetWindow<Window>();
            if (m_Window)
            {
                m_Window.titleContent = new GUIContent("Super Scene Baker");
                m_Window.autoRepaintOnSceneChange = true;
                m_Window.Show();
                EditorPrefs.SetFloat(kKeyPrefix + "Version", kVersion);
            }
        }

        void OnValidate()
        {
            Utils.CleanDuplicates(ref m_ScenesList);
        }


        void LoadProperties()
        {
            //m_ComputeLighting = EditorPrefs.GetBool(kKeyPrefix + MemberInfo.GetMemberName(() => m_ComputeLighting), m_ComputeLighting);
            m_ComputeOcclusion = EditorPrefs.GetBool(kKeyPrefix + Utils.GetMemberName(() => m_ComputeOcclusion), m_ComputeOcclusion);
            m_ClearOcclusion = EditorPrefs.GetBool(kKeyPrefix + Utils.GetMemberName(() => m_ClearOcclusion), m_ClearOcclusion);
            m_ComputeNavMesh = EditorPrefs.GetBool(kKeyPrefix + Utils.GetMemberName(() => m_ComputeNavMesh), m_ComputeNavMesh);
            m_ComputeNavMesh = EditorPrefs.GetBool(kKeyPrefix + Utils.GetMemberName(() => m_ClearNavMesh), m_ClearNavMesh);
            m_ScenesList = Utils.GetAssetArray<SceneAsset>(kKeyPrefix + Utils.GetMemberName(() => m_ScenesList));
        }

        void SaveProperties()
        {
            //EditorPrefs.SetBool(kKeyPrefix + MemberInfo.GetMemberName(() => m_ComputeLighting), m_ComputeLighting);
            EditorPrefs.SetBool(kKeyPrefix + Utils.GetMemberName(() => m_ComputeOcclusion), m_ComputeOcclusion);
            EditorPrefs.SetBool(kKeyPrefix + Utils.GetMemberName(() => m_ClearOcclusion), m_ClearOcclusion);
            EditorPrefs.SetBool(kKeyPrefix + Utils.GetMemberName(() => m_ComputeNavMesh), m_ComputeNavMesh);
            EditorPrefs.SetBool(kKeyPrefix + Utils.GetMemberName(() => m_ClearNavMesh), m_ClearNavMesh);
            Utils.SetAssetArray(kKeyPrefix + Utils.GetMemberName(() => m_ScenesList), m_ScenesList);
        }

        void OnEnable()
        {
            LoadProperties();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        void Log(string str) { m_Log += str + "\n"; }
        void LogReset() { m_Log = ""; }

        void OnGUI()
        {
            EditorGUILayout.BeginScrollView(scrollPos);
            if (m_Window == null)
                OpenWindow();

            var serializedObject = new SerializedObject(this);
            EditorGUILayout.LabelField("Scenes list", EditorStyles.boldLabel);
            var serializedProp = serializedObject.FindProperty("m_ScenesList");
            Debug.Assert(serializedProp != null);
            serializedProp.isExpanded = true;
            EditorGUILayout.PropertyField(serializedProp, true);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear list", EditorStyles.miniButton) && !m_IsProcessing)
            {
                m_ScenesList = new SceneAsset[0];
                SaveProperties();
            }

            if (GUILayout.Button("Add current scene", EditorStyles.miniButton) && !m_IsProcessing)
            {
                var activeScene = SceneManager.GetActiveScene();
                if (activeScene.IsValid())
                {
                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(activeScene.path);
                    if (sceneAsset)
                    {
                        Utils.Add(ref m_ScenesList, sceneAsset);
                        Utils.CleanDuplicates(ref m_ScenesList);
                        SaveProperties();
                    }
                }
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
#if UNITY_5_5_OR_NEWER
            if (GUILayout.Button("Override with scenes in build", EditorStyles.miniButton) && !m_IsProcessing)
            {
                var newList = new List<SceneAsset>();
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                    if(sceneAsset)
                        newList.Add(sceneAsset);
                }

                if (newList.Count > 0)
                {
                    m_ScenesList = newList.ToArray();
                    SaveProperties();
                }
            }
#endif

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Operations", EditorStyles.boldLabel);
            //m_ComputeLighting = EditorGUILayout.Toggle("Compute Lighting", m_ComputeLighting);
            m_ComputeOcclusion = EditorGUILayout.Toggle("Compute Static Occlusion", m_ComputeOcclusion);
            m_ClearOcclusion = EditorGUILayout.Toggle("Clear Static Occlusion", m_ClearOcclusion);
            m_ComputeNavMesh = EditorGUILayout.Toggle("Build NavMesh", m_ComputeNavMesh);
            m_ClearNavMesh = EditorGUILayout.Toggle("Clear NavMesh", m_ClearNavMesh);
            EditorGUILayout.LabelField("Lighting is automatically baked.");

            EditorGUILayout.Separator();
            var buttonString = m_IsProcessing ? "Cancel" : string.Format("Bake {0} scene(s)", m_ScenesList == null ? 0 : m_ScenesList.Length);
            if (GUILayout.Button(buttonString))
                Go();

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(m_Status);
            EditorGUILayout.TextArea(m_Log);

            serializedObject.ApplyModifiedProperties();
        }

        void OnDestroy()
        {
            SaveProperties();
        }

        void Go()
        {
            SaveProperties();

            if (!m_IsProcessing)
            {
                LogReset();

                if (m_ScenesList == null || m_ScenesList.Length <= 0)
                {
                    m_Status = "No scenes";
                    return;
                }

                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                ProcessBegin();
            }
            else
            {
                ProcessCancel();
            }
        }


        System.DateTime m_ProcessStartTime;
        void ProcessBegin()
        {
            Debug.Assert(m_ScenesList != null && m_ScenesList.Length > 0);
            m_CurrentSceneId = 0;
            m_ProcessStartTime = System.DateTime.Now;
            m_IsProcessing = true;
            CurrentSceneBegin();
        }

        void ProcessDone()
        {
            UnregisterBakeCompletedCallback();
            m_IsProcessing = false;
            m_Status = "Done!";
            Log(string.Format("*** Done baking {0} scene(s) in {1} ***", m_ScenesList.Length, GetElapsedTime(m_ProcessStartTime)));
        }

        void ProcessCancel()
        {
            UnregisterBakeCompletedCallback();
            Lightmapping.Cancel(); 
            StaticOcclusionCulling.Cancel();
            NavMeshBuilderClass.Cancel();
            m_IsProcessing = false;
            m_Status = "Cancelled";
            Log(string.Format("Cancelled by user after having baked {0} scene(s) in {1}", m_CurrentSceneId, GetElapsedTime(m_ProcessStartTime)));
        }

        void ProcessNextScene()
        {
            if (m_CurrentSceneId < m_ScenesList.Length - 1)
            {
                m_CurrentSceneId++;
                CurrentSceneBegin();
            }
            else
            {
                ProcessDone();
            }
        }

        void RegisterBakeCompletedCallback()
        {
#if UNITY_2019_2_OR_NEWER
            if (!m_IsCallbackRegistered)
            {
                Lightmapping.bakeCompleted += CurrentSceneDone;
                m_IsCallbackRegistered = true;
            }
#else
            Lightmapping.completed = CurrentSceneDone;
#endif
        }

        void UnregisterBakeCompletedCallback()
        {
#if UNITY_2019_2_OR_NEWER
            if (m_IsCallbackRegistered)
            {
                Lightmapping.bakeCompleted -= CurrentSceneDone;
                m_IsCallbackRegistered = false;
            }
#else
            Lightmapping.completed = null;
#endif
        }

        System.DateTime m_CurrentSceneStartTime;
        void CurrentSceneBegin()
        {
            Debug.Assert(m_CurrentSceneId < m_ScenesList.Length);
            SceneAsset asset = m_ScenesList[m_CurrentSceneId];
            string name = asset.name;
            string path = AssetDatabase.GetAssetPath(asset);

            m_Status = string.Format("Currently baking '{0}' {1}/{2} (please wait)", name, m_CurrentSceneId + 1, m_ScenesList.Length);
            Log(string.Format("Opening scene '{0}'", name));

            EditorSceneManager.OpenScene(path);
            if (EditorSceneManager.GetActiveScene().path != path)
            {
                Log("Failed to open scene " + path);
                CurrentSceneDone();
            }

            if (m_ComputeOcclusion)
            {
                Log("Computing static occlusion...");
                StaticOcclusionCulling.Compute();
            }

            if (m_ClearOcclusion)
            {
                Log("Clearing static occlusion...");
                StaticOcclusionCulling.Clear();
            }

            if (m_ComputeNavMesh)
            {
                Log("Building navmesh...");
                NavMeshBuilderClass.BuildNavMesh();
            }

            if (m_ClearNavMesh)
            {
                Log("Clearing navmesh...");
                NavMeshBuilderClass.ClearAllNavMeshes();
            }

            if (m_ComputeLighting)
            {
                Log("Baking lighting...");
                RegisterBakeCompletedCallback();
                Lightmapping.BakeAsync();
            }
            else
            {
                CurrentSceneDone();
            }
        }

        void CurrentSceneDone()
        {
            var scene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.SaveScene(scene);

            Log(string.Format("Done with scene '{0}', baked {1} lightmaps in {2}", scene.name, LightmapSettings.lightmaps.Length, GetElapsedTime(m_CurrentSceneStartTime)));
            ProcessNextScene();
        }

        static string GetElapsedTime(System.DateTime startTime)
        {
            var elapsedTime = System.DateTime.Now - startTime;
            return string.Format("{0:00}:{1:00}:{2:00}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }
    }
}
