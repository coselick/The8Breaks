using System;
using System.Reflection;
using Q17pD;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if GOLD_PLAYER_CINEMACHINE
#if GOLD_PLAYER_CINEMACHINE_3
using Unity.Cinemachine;
#else
using Cinemachine;
#endif
#endif

#if UNITY_2018_3_OR_NEWER
using UnityEditor.SceneManagement;
#endif

namespace Hertzole.GoldPlayer.Editor
{
    public static class GoldPlayerMenuItems
    {
        [MenuItem("GameObject/3D Object/Gold Player Controller")]
        public static void CreateGoldPlayer(MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;

            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(
                parent != null ? parent.transform : null,
                "Gold Player Controller"
            );

            Undo.IncrementCurrentGroup();

            GameObject root = new GameObject(uniqueName)
            {
                tag = "Player",
                layer = LayerMask.NameToLayer("TransparentFX")
            };

            Undo.RegisterCreatedObjectUndo(root, "Create Gold Player");

            PlaceInScene(root, parent);

            // Graphics
            GameObject graphicsHolder = CreateChild("Graphics", root);

            GameObject graphic = GameObject.CreatePrimitive(
                PrimitiveType.Capsule
            );

            Undo.RegisterCreatedObjectUndo(
                graphic,
                "Create graphic"
            );

            Undo.RecordObject(
                graphic,
                "Set graphic properties"
            );

            graphic.name = "Capsule";
            graphic.layer = root.layer;

            Undo.SetTransformParent(
                graphic.transform,
                graphicsHolder.transform,
                false,
                "Set graphics parent"
            );

            SetObjectTransform(
                graphic.transform,
                Vector3.up,
                null,
                new Vector3(0.8f, 1f, 0.8f)
            );

            Collider graphicsCollider =
                graphic.GetComponent<Collider>();

            if (graphicsCollider != null)
            {
                Undo.DestroyObjectImmediate(graphicsCollider);
            }

            // Camera Head
            GameObject cameraHead = CreateChild(
                "Camera Head",
                root
            );

            SetObjectTransform(
                cameraHead.transform,
                new Vector3(0f, 1.6f, 0f),
                null,
                null
            );

            // Bob Target
            GameObject bobTarget = CreateChild(
                "Bob Target",
                cameraHead
            );

            // Camera
#if GOLD_PLAYER_CINEMACHINE_3

            GameObject playerCameraGo = CreateChild(
                "Player Camera",
                bobTarget,
                typeof(CinemachineCamera)
            );

            CinemachineCamera playerCamera =
                playerCameraGo.GetComponent<CinemachineCamera>();

            playerCamera.Lens.FieldOfView = 80;
            playerCamera.Lens.NearClipPlane = 0.01f;
            playerCamera.Lens.FarClipPlane = 1000f;

            CreateCinemachineBrainIfNeeded();

#elif GOLD_PLAYER_CINEMACHINE

            GameObject playerCameraGo = CreateChild(
                "Player Camera",
                bobTarget,
                typeof(CinemachineVirtualCamera)
            );

            CinemachineVirtualCamera playerCamera =
                playerCameraGo.GetComponent<CinemachineVirtualCamera>();

            CinemachineComposer composer =
                Undo.AddComponent<CinemachineComposer>(
                    playerCamera.GetComponentOwner().gameObject
                );

            Undo.DestroyObjectImmediate(composer);

            playerCamera.InvalidateComponentPipeline();

            Undo.RecordObject(
                playerCamera,
                "Set camera properties"
            );

            playerCamera.m_Lens.FieldOfView = 80;
            playerCamera.m_Lens.NearClipPlane = 0.01f;
            playerCamera.m_Lens.FarClipPlane = 1000f;

            CreateCinemachineBrainIfNeeded();

#else

            GameObject playerCameraGo = CreateChild(
                "Player Camera",
                bobTarget,
                typeof(Camera)
            );

            Camera playerCamera =
                playerCameraGo.GetComponent<Camera>();

            Undo.RecordObject(
                playerCamera,
                "Set camera properties"
            );

            playerCamera.clearFlags = CameraClearFlags.Skybox;
            playerCamera.fieldOfView = 80;
            playerCamera.nearClipPlane = 0.01f;
            playerCamera.farClipPlane = 1000f;
            playerCamera.tag = "MainCamera";

#if !UNITY_6000_5_OR_NEWER
            Undo.AddComponent<FlareLayer>(playerCameraGo);
#endif

            Undo.AddComponent<AudioListener>(playerCameraGo);

#endif

            // Character Controller
            CharacterController characterController =
                Undo.AddComponent<CharacterController>(root);

            characterController.radius = 0.4f;
            characterController.height = 2f;
            characterController.center = new Vector3(0f, 1f, 0f);

            // Gold Player Controller
            GoldPlayerController goldController =
                Undo.AddComponent<GoldPlayerController>(root);

            Undo.RecordObject(
                goldController,
                "Set Gold Player Controller properties"
            );

            // Camera Settings
            goldController.Camera.CameraHead =
                cameraHead.transform;

#if GOLD_PLAYER_CINEMACHINE

            goldController.Camera.UseCinemachine = true;

            goldController.Camera.TargetVirtualCamera =
                playerCamera;

#else

            goldController.Camera.TargetCamera =
                playerCamera;

#endif

            // Movement Settings
            goldController.Movement.GroundLayer = 1;

            // Head Bob Settings
            goldController.HeadBob.BobTarget =
                bobTarget.transform;

            // Audio Handler
            AudioHandler audioHandler = FindAudioHandler();

            if (audioHandler != null)
            {
                goldController.Audio.AudioHandler = audioHandler;

                Debug.Log(
                    "GoldPlayer: AudioHandler assigned successfully.",
                    root
                );
            }
            else
            {
                Debug.LogWarning(
                    "GoldPlayer: AudioHandler not found in scene. " +
                    "Assign it manually in PlayerAudio.",
                    root
                );
            }

            // Input
#if ENABLE_INPUT_SYSTEM && GOLD_PLAYER_NEW_INPUT

            Undo.AddComponent<GoldPlayerInputSystem>(root);

#else

            Undo.AddComponent<GoldPlayerInput>(root);

#endif

            EditorUtility.SetDirty(goldController);

            Undo.SetCurrentGroupName(
                "Created Gold Player Controller"
            );
        }

        private static AudioHandler FindAudioHandler()
        {
#if UNITY_2023_1_OR_NEWER

            return Object.FindFirstObjectByType<AudioHandler>();

#else

            return Object.FindObjectOfType<AudioHandler>();

#endif
        }

#if GOLD_PLAYER_CINEMACHINE

        private static void CreateCinemachineBrainIfNeeded()
        {
            CinemachineBrain[] brains =
                FindObjectsByType<CinemachineBrain>();

            CinemachineBrain brain =
                brains != null && brains.Length > 0
                    ? brains[0]
                    : null;

            if (brain != null)
                return;

            Camera cam = Camera.main;

            if (cam == null)
            {
                Camera[] cams =
                    FindObjectsByType<Camera>();

                if (cams != null && cams.Length > 0)
                {
                    cam = cams[0];
                }
            }

            if (cam != null)
            {
                Undo.AddComponent<CinemachineBrain>(
                    cam.gameObject
                );
            }
        }

#endif

        private static T[] FindObjectsByType<T>()
            where T : Object
        {
#if UNITY_6000_4_OR_NEWER

            return Object.FindObjectsByType<T>(
                FindObjectsInactive.Include
            );

#elif UNITY_2023_1_OR_NEWER

            return Object.FindObjectsByType<T>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

#else

            return Object.FindObjectsOfType<T>();

#endif
        }

        private static void SetObjectTransform(
            Transform transform,
            Vector3? position,
            Vector3? rotation,
            Vector3? scale)
        {
            Undo.RegisterFullObjectHierarchyUndo(
                transform,
                "Set object transform"
            );

            if (position != null)
            {
                transform.localPosition =
                    position.Value;
            }

            if (rotation != null)
            {
                transform.localEulerAngles =
                    rotation.Value;
            }

            if (scale != null)
            {
                transform.localScale =
                    scale.Value;
            }
        }

        private static GameObject CreateChild(
            string name,
            GameObject parent,
            params Type[] components)
        {
            GameObject go =
                ObjectFactory.CreateGameObject(
                    name,
                    components
                );

            Undo.RegisterCreatedObjectUndo(
                go,
                "Create " + go.name
            );

            Undo.RecordObject(
                go,
                "Modify " + go.name
            );

            go.layer = parent.layer;

            Undo.SetTransformParent(
                go.transform,
                parent.transform,
                false,
                "Create " + go.name
            );

            return go;
        }

        private static void PlaceInScene(
            GameObject go,
            GameObject parent)
        {
            if (parent != null)
            {
                Transform transform = go.transform;

                Undo.SetTransformParent(
                    transform,
                    parent.transform,
                    "Reparenting"
                );

                Undo.RegisterFullObjectHierarchyUndo(
                    transform,
                    "Resetting position"
                );

                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            }
            else
            {
                if (ShouldPlaceAtOrigin())
                {
                    go.transform.position = Vector3.zero;
                }
                else
                {
                    PlaceGameObjectInFrontOfSceneView(go);
                }

#if UNITY_2018_3_OR_NEWER

                StageUtility.PlaceGameObjectInCurrentStage(go);

#endif
            }

            Selection.activeGameObject = go;
        }

        private static void PlaceGameObjectInFrontOfSceneView(
            GameObject go)
        {
            SceneView view =
                SceneView.currentDrawingSceneView;

            if (!view && SceneView.sceneViews.Count > 0)
            {
                view = SceneView.sceneViews[0] as SceneView;
            }

            if (view)
            {
                view.MoveToView(go.transform);
            }
        }

        private static bool ShouldPlaceAtOrigin()
        {
            try
            {
                Type goCreationCommandsType =
                    typeof(EditorApplication)
                        .Assembly
                        .GetType(
                            "UnityEditor.GOCreationCommands"
                        );

                PropertyInfo placeObjectsAtWorldOrigin =
                    goCreationCommandsType.GetProperty(
                        "placeObjectsAtWorldOrigin",
                        BindingFlags.Static |
                        BindingFlags.NonPublic
                    );

                return (bool)
                    placeObjectsAtWorldOrigin.GetValue(null);
            }
            catch (Exception e)
            {
                Debug.LogWarning(
                    "Failed to place object in scene. " +
                    "Placing at world origin.\n" + e
                );

                return true;
            }
        }
    }
}