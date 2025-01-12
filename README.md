# Unity Advanced Serialization Utilities

This Unity package provides a set of highly flexible and powerful tools for advanced serialization and inspector customization. Whether you need dynamic data structures or enhanced inspector features, this package has you covered. Below is an overview of the included components:

---

## Features

### 1. **Array2D<T>**
A fully serializable 2D array for the Unity Inspector that supports:
- Any generic type (`T`) (e.g., `int`, `float`, `Vector3`, custom classes, etc.).
- Arbitrary dimensions, allowing for dynamic grid-based data storage.
- Easy integration into your projects with robust, user-friendly APIs.

### 2. **SerializedDictionary<TKey, TValue>**
A dictionary implementation serialized and editable directly in the Unity Inspector:
- Supports any key (`TKey`) and value (`TValue`) types, including custom objects.
- Fully compatible with Unity serialization, providing flexibility for complex data mappings.

### 3. **STuple (2 to 8 Parameters)**
A serializable tuple structure supporting between 2 and 8 parameters:
- Ideal for storing lightweight data groups without creating custom classes.
- Fully compatible with Unity's serialization system, making it easy to use in the inspector.

### 4. **Inspector Attributes**
Enhance your Unity Inspector experience with the following attributes:
- **ExpandRef**  
  Allows editing the fields of referenced objects inline in the inspector.  
  Example:
  ```csharp
  [ExpandRef] public Transform transformReference;
  ```
  Edit the fields of the `Transform` directly in the inspector without needing to expand or navigate separately.

- **FolderPath**  
  Adds a button to string fields for selecting a folder path. Perfect for referencing directories in your project.  
  Example:
  ```csharp
  [FolderPath] public string saveFolderPath;
  ```

- **ResizableRichTextArea**  
  Transforms a `string` field into a resizable text area that supports Unity's Rich Text formatting.  
  Example:
  ```csharp
  [ResizableRichTextArea] public string richTextContent;
  ```

---

## Getting Started

### Installation via Unity Package Manager (UPM)

1. Open Unity and navigate to **Edit > Project Settings > Package Manager**.
2. Add the following Git URL to your project:
   ```
   https://github.com/malanek/SerializedCollections.git
   ```
3. Unity will automatically fetch and add the package to your project.

### Manual Installation

1. Clone or download the repository.
2. Copy the package into your Unity project under the `Assets` folder.
3. Add the components or attributes to your scripts as needed.

---

## Examples

### Array2D Example
```csharp
[Serializable]
public class Example
{
    public Array2D<int> grid;
}
```

### SerializedDictionary Example
```csharp
[Serializable]
public class Example
{
    public SerializedDictionary<string, GameObject> objectMapping;
}
```

### STuple Example
```csharp
[Serializable]
public class Example
{
    public STuple<int, string, float> customTuple;
}
```

### Using Attributes
```csharp
public class Example : MonoBehaviour
{
    [ExpandRef] public Transform targetTransform;
    [FolderPath] public string outputFolder;
    [ResizableRichTextArea] public string description;
}
```

---

## Why Use This Package?

- Simplifies working with complex data structures in Unity.
- Adds powerful customization options for the Unity Inspector.
- Fully compatible with Unity's serialization system and works out of the box.
- Highly flexible and adaptable to any type of project.

---

## License

This package is licensed under the MIT License. Feel free to use and modify it in your projects.

---

Happy developing! 🎮
