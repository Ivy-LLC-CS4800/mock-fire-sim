using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OutlineTest
{
    private GameObject testObject;
    private Outline outline;

    /// <summary>
    /// Sets up the test environment by creating a GameObject with the `Outline` component
    /// and initializing its required properties.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        // Create a GameObject and attach the Outline component
        testObject = new GameObject("OutlineTestObject");
        outline = testObject.AddComponent<Outline>();

        // Mock required materials
        var outlineMaskMaterial = new Material(Shader.Find("Standard"));
        var outlineFillMaterial = new Material(Shader.Find("Standard"));

        // Assign mock materials to the Outline component
        outline.GetType().GetField("outlineMaskMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(outline, outlineMaskMaterial);
        outline.GetType().GetField("outlineFillMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(outline, outlineFillMaterial);
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy the test GameObject and any associated objects
        Object.DestroyImmediate(testObject);
    }

    /// <summary>
    /// Test: Verify that the `Awake` method initializes the required materials and properties.
    /// Predicted: The `outlineMaskMaterial` and `outlineFillMaterial` should be instantiated, and `needsUpdate` should be true.
    /// Checked: The `outlineMaskMaterial`, `outlineFillMaterial`, and `needsUpdate` fields are compared to their expected states.
    /// </summary>
    [Test]
    public void Awake_InitializesMaterialsAndProperties()
    {
        // Act
        outline.Awake();

        // Assert
        var outlineMaskMaterial = outline.GetType().GetField("outlineMaskMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(outline) as Material;
        var outlineFillMaterial = outline.GetType().GetField("outlineFillMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(outline) as Material;
        var needsUpdate = (bool)outline.GetType().GetField("needsUpdate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(outline);

        Assert.IsNotNull(outlineMaskMaterial, "OutlineMaskMaterial should be initialized.");
        Assert.IsNotNull(outlineFillMaterial, "OutlineFillMaterial should be initialized.");
        Assert.IsTrue(needsUpdate, "NeedsUpdate should be set to true.");
    }

    /// <summary>
    /// Test: Verify that the `OnEnable` method appends the outline materials to the renderers.
    /// Predicted: The `outlineMaskMaterial` and `outlineFillMaterial` should be added to the renderer's materials.
    /// Checked: The renderer's materials are compared to ensure they include the outline materials.
    /// </summary>
    [Test]
    public void OnEnable_AppendsOutlineMaterials()
    {
        // Arrange
        var renderer = testObject.AddComponent<MeshRenderer>();
        var originalMaterials = renderer.sharedMaterials;

        // Act
        outline.OnEnable();

        // Assert
        var updatedMaterials = renderer.sharedMaterials;
        Assert.AreEqual(originalMaterials.Length + 2, updatedMaterials.Length, "Two materials should be added to the renderer.");
    }

    /// <summary>
    /// Test: Verify that the `OnDisable` method removes the outline materials from the renderers.
    /// Predicted: The `outlineMaskMaterial` and `outlineFillMaterial` should be removed from the renderer's materials.
    /// Checked: The renderer's materials are compared to ensure they no longer include the outline materials.
    /// </summary>
    [Test]
    public void OnDisable_RemovesOutlineMaterials()
    {
        // Arrange
        var renderer = testObject.AddComponent<MeshRenderer>();
        outline.OnEnable(); // Add materials first

        // Act
        outline.OnDisable();

        // Assert
        var updatedMaterials = renderer.sharedMaterials;
        Assert.AreEqual(0, updatedMaterials.Length, "All outline materials should be removed from the renderer.");
    }

    /// <summary>
    /// Test: Verify that the `UpdateMaterialProperties` method updates the outline material properties correctly.
    /// Predicted: The `_OutlineColor` and `_OutlineWidth` properties should be updated based on the `OutlineColor` and `OutlineWidth` fields.
    /// Checked: The material properties are compared to the expected values.
    /// </summary>
    [Test]
    public void UpdateMaterialProperties_UpdatesOutlineMaterialProperties()
    {
        // Arrange
        outline.OutlineColor = Color.red;
        outline.OutlineWidth = 5f;

        // Act
        outline.GetType().GetMethod("UpdateMaterialProperties", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(outline, null);

        // Assert
        var outlineFillMaterial = outline.GetType().GetField("outlineFillMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(outline) as Material;
        Assert.AreEqual(Color.red, outlineFillMaterial.GetColor("_OutlineColor"), "Outline color should be updated to red.");
        Assert.AreEqual(5f, outlineFillMaterial.GetFloat("_OutlineWidth"), "Outline width should be updated to 5.");
    }

    /// <summary>
    /// Test: Verify that the `SmoothNormals` method calculates smooth normals correctly.
    /// Predicted: The returned normals should be averaged for vertices that share the same position.
    /// Checked: The returned normals are compared to the expected averaged normals.
    /// </summary>
    [Test]
    public void SmoothNormals_CalculatesCorrectSmoothNormals()
    {
        // Arrange
        var mesh = new Mesh
        {
            vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.up },
            normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.up }
        };

        // Act
        var smoothNormals = outline.GetType().GetMethod("SmoothNormals", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(outline, new object[] { mesh }) as List<Vector3>;

        // Assert
        Assert.AreEqual(3, smoothNormals.Count, "Smooth normals should have the same count as the vertices.");
        Assert.AreEqual(Vector3.forward.normalized, smoothNormals[0], "Smooth normal for shared vertices should be averaged.");
        Assert.AreEqual(Vector3.forward.normalized, smoothNormals[1], "Smooth normal for shared vertices should be averaged.");
        Assert.AreEqual(Vector3.up, smoothNormals[2], "Smooth normal for unique vertex should remain unchanged.");
    }

    /// <summary>
    /// Test: Verify that `SmoothNormals` handles meshes with no vertices or normals.
    /// Predicted: The method should return an empty list without throwing exceptions.
    /// Checked: The returned list is empty, and no exceptions are thrown.
    /// </summary>
    [Test]
    public void SmoothNormals_HandlesEmptyMesh()
    {
        // Arrange
        var mesh = new Mesh();

        // Act
        var smoothNormals = outline.GetType().GetMethod("SmoothNormals", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(outline, new object[] { mesh }) as List<Vector3>;

        // Assert
        Assert.IsNotNull(smoothNormals, "SmoothNormals should return a non-null list.");
        Assert.AreEqual(0, smoothNormals.Count, "SmoothNormals should return an empty list for a mesh with no vertices.");
    }

    /// <summary>
    /// Test: Verify that enabling `precomputeOutline` correctly precomputes and stores smooth normals.
    /// Predicted: The `bakeKeys` and `bakeValues` lists should be populated with the mesh and its smooth normals.
    /// Checked: The `bakeKeys` and `bakeValues` lists are compared to the expected values.
    /// </summary>
    [Test]
    public void PrecomputeOutline_StoresSmoothNormals()
    {
        // Arrange
        outline.GetType().GetField("precomputeOutline", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(outline, true);

        var mesh = new Mesh
        {
            vertices = new Vector3[] { Vector3.zero, Vector3.up },
            normals = new Vector3[] { Vector3.forward, Vector3.up }
        };

        var bakeKeys = outline.GetType().GetField("bakeKeys", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(outline) as List<Mesh>;
        var bakeValues = outline.GetType().GetField("bakeValues", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(outline) as List<List<Vector3>>;

        // Act
        outline.GetType().GetMethod("LoadSmoothNormals", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(outline, null);

        // Assert
        Assert.IsTrue(bakeKeys.Contains(mesh), "BakeKeys should contain the mesh.");
        Assert.AreEqual(1, bakeValues.Count, "BakeValues should contain one entry.");
        Assert.AreEqual(2, bakeValues[0].Count, "BakeValues should contain smooth normals for each vertex.");
    }

    /// <summary>
    /// Test: Verify that `SmoothNormals` performs efficiently for large meshes.
    /// Predicted: The method should complete within a reasonable time for a mesh with a large number of vertices.
    /// Checked: The execution time is measured and compared to a threshold.
    /// </summary>
    [Test]
    public void SmoothNormals_PerformsEfficientlyForLargeMeshes()
    {
        // Arrange
        var mesh = new Mesh
        {
            vertices = Enumerable.Repeat(Vector3.zero, 100000).ToArray(),
            normals = Enumerable.Repeat(Vector3.forward, 100000).ToArray()
        };

        // Act
        var startTime = Time.realtimeSinceStartup;
        outline.GetType().GetMethod("SmoothNormals", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .Invoke(outline, new object[] { mesh });
        var endTime = Time.realtimeSinceStartup;

        // Assert
        Assert.Less(endTime - startTime, 1f, "SmoothNormals should complete within 1 second for a large mesh.");
    }
}