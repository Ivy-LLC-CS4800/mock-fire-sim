using NUnit.Framework;
using System.IO;
using UnityEngine;

public class DatabaseManagerTests
{
    private GameObject dbGO;
    private DatabaseManager dbManager;

    private string testDBPath = "URI=file:test_users.db";

    [SetUp]
    public void SetUp()
    {
        dbGO = new GameObject("DBManager");
        dbManager = dbGO.AddComponent<DatabaseManager>();

        // Override the path with a test database
        var pathField = typeof(DatabaseManager).GetField("dbPath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        pathField.SetValue(dbManager, testDBPath);

        dbManager.CreateDB(); // Ensure table is ready
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(dbGO);

        // Delete test DB file to reset environment
        var realPath = Path.Combine(Application.dataPath, "../test_users.db");
        if (File.Exists(realPath))
        {
            File.Delete(realPath);
        }
    }

    [Test]
    public void RegisterUsername_Succeeds_WhenNewUser()
    {
        bool result = dbManager.RegisterUsername("testUser", "testPass");
        Assert.IsTrue(result);
    }

    [Test]
    public void RegisterUsername_Fails_WhenDuplicateUser()
    {
        dbManager.RegisterUsername("duplicateUser", "pass1");
        bool result = dbManager.RegisterUsername("duplicateUser", "pass2");
        Assert.IsFalse(result);
    }

    [Test]
    public void CheckUsernameAndPassword_ReturnsTrue_WithCorrectCredentials()
    {
        dbManager.RegisterUsername("validUser", "secret");
        bool result = dbManager.CheckUsernameAndPassword("validUser", "secret");
        Assert.IsTrue(result);
    }

    [Test]
    public void CheckUsernameAndPassword_ReturnsFalse_WithWrongPassword()
    {
        dbManager.RegisterUsername("validUser2", "correctPass");
        bool result = dbManager.CheckUsernameAndPassword("validUser2", "wrongPass");
        Assert.IsFalse(result);
    }

    [Test]
    public void CheckUsernameAndPassword_ReturnsFalse_WithNonExistentUser()
    {
        bool result = dbManager.CheckUsernameAndPassword("ghostUser", "nope");
        Assert.IsFalse(result);
    }
}

