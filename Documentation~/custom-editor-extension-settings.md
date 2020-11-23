# Muffin Dev for Unity - Multiple Editors Manager - `CustomEditorExtensionSettings`

Represents the settings stored in the `MultipleEditorsManager` instance for one custom editor extension.

## Constructor

```cs
public CustomEditorExtensionSettings(Type _TargetType, Type _CustomEditorType, MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, CustomEditorExtensionOptions _DefaultOptions)
```

Creates an instance of `CustomEditorExtensionSettings`.

- `Type _TargetType`: The target type of the custom editor.
- `Type _CustomEditorType`: The custom editor type.
- `MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod`: The method to use to create an instance of the custom editor.
- `CustomEditorExtensionOptions _DefaultOptions`: The default options to use for the custom editor.

## Methods

### `Update()`

```cs
public void Update(Type _TargetType, MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod, CustomEditorExtensionOptions _DefaultOptions)
```

Updates the settings of a custom editor.

- `Type _TargetType`: The target type of the custom editor.
- `MultipleEditorsManager.CreateEditorDelegate _CreateEditorMethod`: The method to use to create an instance of the custom editor.
- `CustomEditorExtensionOptions _DefaultOptions`: The default options to use for the custom editor.

### `Reset()`

```cs
public void Reset()
```

Resets all settings of the custom editor to their default values.

### `CreateEditor()`

```cs
public ICustomEditorExtension CreateEditor()
```

Calls this CustomEditorExtension's creation method.

Returns the created custom editor instance.

## Accessors

### `TargetType`

```cs
public Type TargetType { get; set; }
```

Gets the target type of the custom editor.

### `CustomEditorType`

```cs
public Type CustomEditorType { get; set; }
```

Gets the custom editor type.

### `Enabled`

```cs
public bool Enabled { get; set; }
```

Enables/Disables the custom editor.

### `DisplayName`

```cs
public string DisplayName { get; set; }
```

Gets the display name of the custom editor. If it has not been defined, returns the type name of the custom editor.

### `Description`

```cs
public string Description { get; }
```

Gets the description of the custom editor.

### `Order`

```cs
public int Order { get; set; }
```

Gets/sets the current order of the custom editor.

### `RequiresConstantRepaint`

```cs
public bool RequiresConstantRepaint { get; set; }
```

Checks if the main Editor should use Editor.RequiresConstantRepaint() when using the custom editor.

### `TargetTypeName`

```cs
public string TargetTypeName { get; }
```

Gets the target type name of the custom editor.