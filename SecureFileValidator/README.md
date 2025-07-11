# SecureFileValidator

📦 強型別檔案簽章驗證器，支援 .NET 8 / .NET Framework 4.7  
支援 DataAnnotation、Action Filter 驗證方式，可套用於 ASP.NET Core / MVC 控制器與表單檢查。

---

## ✅ 支援格式（依 magic number 檢查）

- `.jpg`, `.jpeg`, `.png`, `.gif`, `.bmp`, `.tif`
- `.pdf`, `.docx`, `.xlsx`, `.zip`, `.rar`, `.7z`
- `.mp3`, `.mp4`, `.avi`, `.webp`, `.exe`

---

## 🔐 驗證方式

### 1. DataAnnotation 驗證（ASP.NET Core）

```csharp
public class UploadModel
{
    [ValidateFileSignature(AllowedExtensions = new[] { ".docx", ".xlsx" })]
    public IFormFile File { get; set; }
}
```

> 驗證失敗會寫入 ModelState 錯誤，可搭配 `TryValidateModel()` 或 `TryUpdateModelAsync()` 使用。

---

### 2. Action Filter 驗證（ASP.NET Core）

```csharp
[ValidateFileSignature(FileParameterName = "file")]
public IActionResult Upload(IFormFile file) { ... }
```

或驗證所有表單中檔案：

```csharp
[ValidateFileSignature]
public IActionResult Upload() { ... }
```

---

### 3. MVC (.NET Framework)

```csharp
[ValidateFileSignature(FileParameterName = "file", AllowedExtensions = new[] { ".docx" })]
public ActionResult Upload(HttpPostedFileBase file)
```

---

## 🧪 單元測試

請見 `SecureFileValidator.Tests` 中範例，包括：

- `.png` 冒充 `.docx` 驗證失敗
- 合法 `.docx` 驗證成功
- 驗證失敗時 `ModelState` 錯誤訊息檢查

---

## 🛠 安裝套件

若手動引用專案，請加入：

```xml
<ItemGroup Condition="'$(TargetFramework)' == 'net8.0'">
  <PackageReference Include="Microsoft.AspNetCore.App" />
</ItemGroup>

<ItemGroup Condition="'$(TargetFramework)' == 'net47'">
  <Reference Include="System.Web" />
</ItemGroup>
```

---

## 📄 授權

MIT License
