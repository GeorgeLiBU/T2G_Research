#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;

namespace T2G.UnityAdapter
{
    [InitializeOnLoad]
    public class LauchEditorStartUp
    {
        static readonly string k_Started = "SessionStartedKey";
        static LauchEditorStartUp()
        {
            if (!SessionState.GetBool(k_Started, false))
            {
                EditorApplication.quitting += EditorApplication_quitting;

                CommunicatorServerEditor.Dashboard();
                CommunicatorServerEditor.InitializeOnLoad();
                SessionState.SetBool(k_Started, true);
            }
        }

        private static void EditorApplication_quitting()
        {
            CommunicatorServerEditor.Uninitialize();
        }
    }

    public class CommunicatorServerEditor : EditorWindow
    {
        static CommunicatorServer _server;
        static Vector2 _scroll = Vector2.zero;
        static string _text = string.Empty;

        [MenuItem("T2G/Communicator", false)]
        public static void Dashboard()
        {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
            EditorWindow.GetWindow<CommunicatorServerEditor>("Communicator Server", new Type[] { inspectorType });
        }

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            CommunicatorServer communicatorServer = CommunicatorServer.Instance;

            communicatorServer.OnServerStarted += () =>
            {
                _text += "\n System> Server started.";
            };

            communicatorServer.AfterShutdownServer += () =>
            {
                _text += "\n System> Server was shut down.";
            };

            communicatorServer.OnFailedToStartServer += () => 
            {
                _text += "\n System> Failed to start litsening server!";
            };

            communicatorServer.OnClientConnected += () =>
            {
                _text += "\n System> Client was connected!";
            };

            communicatorServer.OnClientDisconnected += () =>
            {
                _text += "\n System> Client was disconnected!";
            };

            communicatorServer.OnReceivedMessage += (message) =>
            {
                _text += "\n Received> " + message;
            };

            communicatorServer.OnSentMessage += (message) =>
            {
                _text += "\n Sent> " + message;
            };

            communicatorServer.OnLogMessage += (message) =>
            {
                _text += "\n Received> " + message;
            };

            if (_server == null)
            {
                _server = CommunicatorServer.Instance;
                AssemblyReloadEvents.beforeAssemblyReload += () =>
                {
                    _server.StopServer();
                };
            }

            if (EditorPrefs.GetInt(Defs.k_InitStartListener, 1) != 0)
            {
                _text = string.Empty;
                _server.StartServer();
            }
        }

        public static void Uninitialize()
        {
            if(_server != null)
            {
                _server.StopServer();
            }
        }

        public void OnGUI()
        {
            if(_server == null)
            {
                _server = CommunicatorServer.Instance;
                AssemblyReloadEvents.beforeAssemblyReload += () => {
                    _server.StopServer();
                };
            }

            bool isActive = EditorGUILayout.Toggle("Server is on: ", _server.IsActive);
            if(isActive != _server.IsActive)
            {
                if (isActive)
                {
                    _server.StartServer();
                }
                else
                {
                    _server.StopServer();
                }
                EditorPrefs.SetInt(Defs.k_InitStartListener, isActive ? 1 : 0);
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Client is connected: ", _server.IsConnected);
            EditorGUI.EndDisabledGroup();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            _text = EditorGUILayout.TextArea(_text, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

        }
    }
}

#endif