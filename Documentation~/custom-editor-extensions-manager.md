# Muffin Dev for Unity - Multiple Editors Manager - `CustomEditorExtensionsManager`

Utility class for loading custom editor extensions and use them.

## Methods

```cs
public void LoadExtensions<TTarget>(Editor _Editor)
```

Creates all the `CustomEditorExtension` that have the same target type as this `Editor`.

- `TTarget`: The target type of the custom editor extensions you want to load from `MultipleEditorsManager`
- `Editor _Editor`: The calling Editor instance

---

```cs
public void EnableCustomEditors()
```

Calls `OnEnable()` on each loaded custom editor extensions.

---

```cs
public void DisableCustomEditors()
```

Calls `OnDisable()` on each loaded custom editor extensions.

---

```cs
public void DrawCustomEditorsBeforeHeaderGUI()
```

Calls `OnBeforeHeaderGUI()` on each loaded custom editor extensions.

---

```cs
public void DrawCustomEditorsHeaderGUI()
```

Calls `OnHeaderGUI()` on each loaded custom editor extensions.

---

```cs
public void DrawCustomEditorsBeforeInspectorGUI()
```

Calls `OnBeforeInspectorGUI()` on each loaded custom editor extensions.

---

```cs
public void DrawCustomEditorsInspectorGUI()
```

Calls `OnInspectorGUI()` on each loaded custom editor extensions.

---

```cs
public void DrawCustomEditorsSceneGUI()
```

Calls `OnSceneGUI()` on each loaded custom editor extensions.

---

```cs
public ICustomEditorExtension[] LoadedExtensions { get; }
```

Gets all the loaded custom editor extensions. You must use `LoadExtensions()` method to load custom editor extensions.

---

```cs
public bool RequiresConstantRepaint { get; }
```

Returns `true` if at least one of the loaded custom editor extensions has its "require constant repaint" option enabled. This value should be returned in an override of `Editor.RequireConstantRepaint()` of the calling `Editor` instance.