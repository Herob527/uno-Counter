# ✅ **Uno + MVUX Persistence & Best Practices Guide**

This document outlines how to persist application state, settings, and user data across sessions in **Uno Platform** using **MVUX**, along with recommended patterns and tooling integration.

---

## 📦 1. **Persistence Across Sessions**

### A. **App State Management**
Use `IAppState` (provided by Uno) to manage cross-platform persistence:

```csharp
// Save state
IAppState.Save("userPreferences", new { Theme = "Dark", Language = "en-US" });

// Restore state
var preferences = IAppState.Load<Preferences>("userPreferences");
```

### B. **Local Storage**
Use **IsolatedStorage** or **SQLite** for platform-specific persistence:

```csharp
// Example with IsolatedStorage (Uno)
using Microsoft.Toolkit.Uwp.Helpers;

var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("settings.json", CreationCollisionOption.ReplaceExisting);
await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(userSettings));
```

### C. **Cloud Sync (Optional)**
For cross-device persistence, integrate with:
- Azure App Configuration
- Firebase Remote Config
- Uno's `IAppService` abstractions

---

## 🔄 2. **MVUX Best Practices**

### A. **State Management Patterns**
1. **Use `@Uno.State` Attributes**
Annotate properties in your ViewModel to auto-save/load:

```csharp
[State("userPreferences")]
public class UserSettings {
    public string Theme { get; set; }
}
```

2. **Avoid Over-Reliance on Global State**
Use scoped state managers for components instead of a monolithic `IAppState`.

---

### B. **C# Markup + MVUX**
1. **Declarative UI in C# Markup**
Replace XAML with C#-based markup for better tooling integration:

```csharp
<Page>
    <TextBlock Text="{Binding UserSettings.Theme}" />
</Page>
```

2. **Hot Reload Compatibility**
Ensure state restoration works seamlessly with **Uno Platform Studio's Hot Reload**.

---

## 🛠 3. **Tooling & Configuration**

### A. **Persistent Tooling Settings**
- **VS/VS Code Config**: Save tooling preferences in `.unoconfig`:
  ```json
  {
    "theme": "dark",
    "hotreload": true,
    "mvux": { "autoSaveState": true }
  }
  ```

### B. **Build-Time Persistence**
Use `Directory.Build.targets` to define persistent build-time settings:

```xml
<Project>
  <PropertyGroup>
    <UnoAppPersistentSettings>Theme=Dark;Language=en-US</UnoAppPersistentSettings>
  </PropertyGroup>
</Project>
```

---

## 🧪 4. **Testing Persistence**

### A. **Unit Tests for State**
```csharp
[TestClass]
public class AppPersistenceTests {
    [TestMethod]
    public void TestStateRestore() {
        IAppState.Save("testKey", new { Value = "Hello" });
        var result = IAppState.Load<TestObject>("testKey");
        Assert.AreEqual("Hello", result.Value);
    }
}
```

### B. **UI Tests with Uno.UITest**
```csharp
[TestClass]
public class UITests {
    [TestMethod]
    public void TestThemePersistence() {
        // Simulate app launch and check persisted theme
        App.Current.Settings.Theme.Should().Be("Dark");
    }
}
```

---

## 📌 5. **Key Takeaways**

| Feature                | MVUX Recommendation                          | Uno Platform Tip                          |
|------------------------|---------------------------------------------|--------------------------------------------|
| State Persistence      | Use `IAppState` with scoped state managers  | Avoid platform-specific storage unless needed |
| UI Configuration         | Save in `.unoconfig`                       | Use C# Markup for tooling compatibility |
| Cross-Session Sync     | Combine local + cloud storage              | Leverage Uno's built-in sync abstractions |

---

## 📦 6. **Example Project Structure**

```
/MyApp
├── App.cs                    // Entry point with IAppState integration
├── Services/
│   └── AppStateService.cs  // Centralized state management
├── Views/
│   └── SettingsView.cs     // Uses C# Markup + MVUX bindings
├── Models/
│   └── UserSettings.cs     // Annotated with [State]
└── CLAUDE.md               // This file!
```

---

## 📚 7. **References**
- [Uno Platform State Management Docs](https://platform.uno/docs/articles/state-management)
- [MVUX GitHub Examples](https://github.com/unoplatform/uno/tree/master/src/Samples/MVUX)
- [Uno.UITest Documentation](https://github.com/unopl平台/Uno.UITest)