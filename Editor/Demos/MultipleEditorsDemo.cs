// Uncomment the following line to enable demo custom editors, working with the multiple editors system.
//#define MULTIPLE_EDITORS_DEMO

#if MULTIPLE_EDITORS_DEMO

using System;
using System.Globalization;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

using Object = UnityEngine.Object;

namespace MuffinDev.EditorUtils.MultipleEditors.Demos
{

    /// <summary>
    /// This example adds an extension to original GameObject editor that displays a simple text box that says
    /// "Hi [inspected object name]!".
    /// </summary>
    [InitializeOnLoad]
    public class GameObjectGreeter : CustomEditorExtension<GameObject>
    {

        // You must use static constructors to register your custom editors.
        static GameObjectGreeter()
        {
            RegisterCustomEditor(() => { return new GameObjectGreeter(); }, 0, "Greeter", "Says hello to the inspected GameObject.");
        }

        // Called after the GameObject's original header is drawn.
        public override void OnHeaderGUI()
        {
            EditorGUILayout.HelpBox($"Hi {target.name}!", MessageType.Info);
            EditorGUILayout.Space();
        }

    }

    /// <summary>
    /// This example adds an extension to the original GameObject editor that draws above the built-in header a text box that shows the
    /// current time. Note that it also enables the "require constant repaint" option, so the seconds are updated constantly.
    /// </summary>
    [InitializeOnLoad]
    public class WhatTimeIsIt : CustomEditorExtension<GameObject>
    {

        // You must use static constructors to register your custom editors.
        static WhatTimeIsIt()
        {
            RegisterCustomEditor(() => { return new WhatTimeIsIt(); }, 0, "What Time Is It?", "Displays the current time, and use \"Requires Constant Repaint\" to update each seconds.", true);
        }

        // Called before the GameObject's built-in header is drawn.
        public override void OnBeforeHeaderGUI()
        {
            GUIContent label = EditorGUIUtility.IconContent("d_UnityEditor.AnimationWindow");
            label.text = "What time is it?";

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                // Use CultureInfo.CurrentCulture to display the current time using your location's date format.
                EditorGUILayout.LabelField(DateTime.Now.ToString(CultureInfo.CurrentCulture));
            }
            EditorGUILayout.EndVertical();
        }

    }

    /// <summary>
    /// A dummy GameObject extension that uses order value.
    /// Use the Multiple Editors Manager window from "Muffin Dev > Multiples Editors Manager" to place the 3 dummy extensions in the
    /// correct order.
    /// </summary>
    [InitializeOnLoad]
    public class GameObjectExtension1 : CustomEditorExtension<GameObject>
    {

        // You must use static constructors to register your custom editors.
        static GameObjectExtension1()
        {
            RegisterCustomEditor(() => { return new GameObjectExtension1(); }, 10, "Demo GameObject Extension 1");
        }

        // Called after the GameObject's built-in header is drawn.
        public override void OnHeaderGUI()
        {
            EditorGUILayout.HelpBox("[1] Should be placed before the others", MessageType.None);
            EditorGUILayout.Space();
        }

    }

    /// <summary>
    /// A dummy GameObject extension that uses order value.
    /// Use the Multiple Editors Manager window from "Muffin Dev > Multiples Editors Manager" to place the 3 dummy extensions in the
    /// correct order.
    /// </summary>
    [InitializeOnLoad]
    public class GameObjectExtension2 : CustomEditorExtension<GameObject>
    {

        // You must use static constructors to register your custom editors.
        static GameObjectExtension2()
        {
            RegisterCustomEditor(() => { return new GameObjectExtension2(); }, 100, "Demo GameObject Extension 2");
        }

        // Called after the GameObject's built-in header is drawn.
        public override void OnHeaderGUI()
        {
            EditorGUILayout.HelpBox("[2] Should be placed in second position", MessageType.None);
            EditorGUILayout.Space();
        }

    }

    /// <summary>
    /// A dummy GameObject extension that uses order value.
    /// Use the Multiple Editors Manager window from "Muffin Dev > Multiples Editors Manager" to place the 3 dummy extensions in the
    /// correct order.
    /// </summary>
    [InitializeOnLoad]
    public class GameObjectExtension3 : CustomEditorExtension<GameObject>
    {

        // You must use static constructors to register your custom editors.
        static GameObjectExtension3()
        {
            RegisterCustomEditor(() => { return new GameObjectExtension3(); }, 50, "Demo GameObject Extension 3");
        }

        // Called after the GameObject's built-in header is drawn.
        public override void OnHeaderGUI()
        {
            EditorGUILayout.HelpBox("[3] Should be placed after the others", MessageType.None);
            EditorGUILayout.Space();
        }

    }

    /// <summary>
    /// This extension displays a quick description of the object using Unity's UIElements library.
    /// </summary>
    [InitializeOnLoad]
    public class GameObjectQuickView : CustomEditorExtension<GameObject>
    {

        private const string VISUAL_ASSETS_NAME = "MultipleEditorsDemo_QuickView";

        // You must use static constructors to register your custom editors.
        static GameObjectQuickView()
        {
            RegisterCustomEditor(() => { return new GameObjectQuickView(); }, "Quick View", "Adds common data about the selected object in the GameObject header.");
        }

        // Called when the inspector of the target is created.
        public override VisualElement CreateInspectorGUI()
        {
            // Create new tree
            VisualElement root = new VisualElement();

            // Load the visual tree (the elements to draw in the inspector) from a *.uxml file
            VisualTreeAsset visualTree = FindAssetByName<VisualTreeAsset>(VISUAL_ASSETS_NAME);
            if (visualTree != null)
                visualTree.CloneTree(root);

            // Load the stylesheet from a *.uss file
            StyleSheet style = FindAssetByName<StyleSheet>(VISUAL_ASSETS_NAME);
            if (style != null)
                root.styleSheets.Add(style);

            // Replace template content
            root.Q<Image>("preview").image = AssetPreview.GetAssetPreview(target);
            root.Q<Label>("title").text = target.name;
            root.Q<Label>("isDynamic").text = $"Dynamic: {(IsDynamic ? "Yes" : "No")}";
            root.Q<Label>("state").text = $"State: {(EditorUtility.IsPersistent(target) ? "saved on disk" : "scene object")}";
            root.Q<Label>("componentsCount").text = $"Nb. components: {target.GetComponents<Component>().Length}";

            return root;
        }

        // Checks if this GameObject is dynamic (and so has a Rigidbody or Rigidbody2D component).
        private bool IsDynamic
        {
            get { return target.GetComponent<Rigidbody>() ? true : target.GetComponent<Rigidbody2D>(); }
        }

        // Finds an asset of the given type by its name in the Asset Database.
        private TAssetType FindAssetByName<TAssetType>(string _AssetName)
            where TAssetType : Object
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(TAssetType).Name}");
            foreach (string guid in guids)
            {
                TAssetType asset = AssetDatabase.LoadAssetAtPath<TAssetType>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset.name == _AssetName)
                    return asset;
            }
            return null;
        }

    }

    /// <summary>
    /// This Transform extension example adds a "Parent Position" field after the built-in Transform inspector that allows you to set the
    /// position of the parent Transform if there's one.
    /// </summary>
    [InitializeOnLoad]
    public class ParentPosition : CustomEditorExtension<Transform>
    {

        // You must use static constructors to register your custom editors.
        static ParentPosition()
        {
            RegisterCustomEditor(() => { return new ParentPosition(); }, 0, "Parent Position", "Allows you to set the position of the parent Transform if there's one.");
        }

        // Called after the Transform's built-in inspector is drawn.
        public override void OnInspectorGUI()
        {
            // If the inspected Transform has a parent in its hierarchy, draw the "Parent Position" field.
            if(target.parent)
            {
                EditorGUILayout.Space();
                target.parent.position = EditorGUILayout.Vector3Field("Parent Position", target.parent.position);
            }
        }

    }

    /// <summary>
    /// This Rigidbody extension example adds a "Velocity" field after the built-in Rigidbody inspector. It allows you to manually set the
    /// velocity of an object at runtime, from the editor.
    /// </summary>
    [InitializeOnLoad]
    public class RigidbodyVelocity : CustomEditorExtension<Rigidbody>
    {

        // You must use static constructors to register your custom editors.
        static RigidbodyVelocity()
        {
            RegisterCustomEditor(() => { return new RigidbodyVelocity(); }, 0, "Velocity", "In play mode, you can see and set the velocity using the \"Velocity\" field.");
        }

        // Called after the Rigidbody's built-in inspector is drawn.
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            // Enable the Velocity field only in play mode (the velocity is reset when the game starts anyway).
            GUI.enabled = EditorApplication.isPlaying;
            Vector3 currentVelocity = EditorGUILayout.Vector3Field("Velocity", target.velocity);
            target.velocity = currentVelocity;
            GUI.enabled = true;
        }

    }

}

#endif