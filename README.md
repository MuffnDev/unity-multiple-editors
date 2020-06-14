# Muffin Dev for Unity - Multiple Editors

Allows you to create multiple editor extensions for the same target type through code. In the editor, you can use the manager's window to set these custom editor options : enable/disable them, change their order, etc.

![Preview of the Multiple Editors Manager window](./_Documentation/Images/multiple-editors-manager-window.png)

## Demo

You can enable the Multiple Editors system demo by uncommenting the second line of the script at [`/MuffinDev/Core/Editor/MultipleEditors/Demos/DemoCustomObjectEditor.cs`](./Editor/Demos/DemoCustomObjectEditor.cs).

This will add custom editor extensions to `GameObject` native inspector, `Transform` and `Rigidbody` components.

You can also change their order and settings from the Multiple Editors Manager window in `Muffin Dev > Multiple Editors Manager`.

## Documentation

[=> See module documentation](./_Documentation/README.md)

## Known issues

The first time you add this plugin and you want to open the *Multiple Editors* window, the window can be empty and you get this error in the Unity Console:

`Instance of MultipleEditorsManagerEditor couldn't be created. The script class needs to derive from ScriptableObject and be placed in the Assets/Editor folder.`

This bug is internal to Unity. You can fix it by just restarting the Editor.
