# Muffin Dev for Unity - Multiple Editors Manager - `NativeObjectMultipleEditorsHandler`

The base class of any custom multiple editors handler that targets a native object (like `Transform` for example).

**This class is only meant to create custom editor extensions for native object types. For regular multiple editors handlers, see [`MultipleEditorsHandler` class documentation](./multiple-editors-handler.md).**

## Purpose

The only difference with regular [`MultipleEditorsHandler`](./multiple-editors-handler.md) is to correctly enable/disable and destroy bult-in objects editors. So it will create and cache an instance of the built-in editor until it's close.

There is a few built-in objects that need this, and some of them already have a handler like `GameObject` and `Transform`. Other objects can use the regular [`MultipleEditorsHandler`](./multiple-editors-handler.md).

## Usage

This class can be used the same as the regular [`MultipleEditorsHandler`](./multiple-editors-handler.md), and by overriding the needed methods.

See the existing implementations for [`GameObject`](../../MultipleEditors/Editor/NativeObjectMultipleEditorsHandlers/GameObjectMultipleEditors.cs) and `Transform` (../../MultipleEditors/Editor/NativeObjectMultipleEditorsHandlers/TransformMultipleEditors.cs) to check how the native editors lifecycle is managed.

## Methods

### `OnEnable()`

```cs
protected virtual void OnEnable()
```

This function is called when the object is loaded. Creates an instance of the built-in editor, then loads the custom editors extensions, and enable them.

### `OnDisable()`

```cs
protected virtual void OnDisable()
```

This function is called when the scriptable object goes out of scope. Disables the loaded custom editor extensions and destroy the cached built-in editor.

### `OnHeaderGUI()`

```cs
protected override void OnHeaderGUI()
```

Called when the header of the object being inspected is drawn. By default, calls `OnBeforeHeaderGUI()` on loaded custom editor extensions, draws the cached editor original header, then call `OnHeaderGUI()` on loaded custom editor extensions.

### `OnInspectorGUI()`

```cs
public override void OnInspectorGUI()
```

Called when the inspector of the object being inspected is drawn. By default, calls `OnBeforeInspectorGUI()` on loaded custom editor extensions, draws the cached editor original header, then call `OnInspectorGUI()` on loaded custom editor extensions.

### `CreateInspectorGUI()`

```cs
public override VisualElement CreateInspectorGUI()
```

Called when the inspector of the object being inspected is drawn.

This is an alternative to `OnInspectorGUI()` that allow you to use ***UIElements*** instead of only *IMGUI* controls. Note that these methods are not exclusive, so you can use both. In this case, `CreateInspectorGUI()` is called before `OnInspectorGUI()`.

Returns the root `VisualElement` to draw for the inspected object.

### `OnSceneGUI()`

```cs
protected void OnSceneGUI()
```

Handles scene events. Note that this message is sent by Unity only when inspecting scene objects.

### `RequiresConstantRepaint()`

```cs
public override bool RequiresConstantRepaint()
```

Defines if this Editor should be repainted constantly, (similar to an Update() for an Editor class).

### `CreateNativeEditor()`

```cs
protected virtual void CreateNativeEditor(string _CreateEditorType = null)
```

Called when this editor is enabled. Creates an instance of the native editor of the inspected Object.

- `string _CreateEditorType = null`: The type name of the native editor to get. If this value is set to null, use `"UnityEditor.[TTarget]Inspector, UnityEditor"`.

### `DestroyNativeEditor()`

```cs
protected virtual void DestroyNativeEditor(Editor _NativeEditor)
```

Called when this editor is disabled. Destroys the created native editor properly in order to avoid memory leaks.

By default, executes the `OnDisable()` method on the native editor, and destroys it using `Object.DestroyImmediate()`.

- `Editor _NativeEditor`: The created built-in `Editor` instance.

## Accessors

### `NativeEditor`

```cs
protected Editor NativeEditor { get; set; }
```

Gets the native editor, created using `CreateNativeEditor()`.

When a new value is set, destroys the current cached native editor if there's one using `DestroyNativeEditor()` method, then assign the new native editor. The setter shouldn't be used out of the `CreateNativeEditor()` method.