# SecureFileValidator

跨 .NET Framework 4.7 與 .NET 8 的檔案安全驗證工具，支援常見格式（Word、Excel、MP4 等）的檔案簽名（magic number）比對，防止副檔名偽裝攻擊。

## 功能特色

- ✅ 支援 Office 格式 (DOCX, XLSX)
- ✅ 支援影音檔 (MP4, AVI, PNG, JPG 等)
- ✅ 檢查 ZIP 結構是否合法（如 Office OpenXML）
- ✅ 提供 ActionFilter 與 DataAnnotation 屬性，可於 ASP.NET MVC / Core 中驗證上傳檔案
- ✅ 可驗證單一檔案或整批上傳檔案
- ✅ 完整單元測試與多框架支援

---

## 安裝方式

### 套件參考（NuGet）

若使用 `.NET 8`：

```xml
<PackageReference Include="Microsoft.AspNetCore.Mvc.Core" Version="2.2.5" />
<PackageReference Include="Microsoft.AspNetCore.Http.Abstractions" Version="2.2.0" />
```

若使用 `.NET Framework 4.7`：

```xml
<PackageReference Include="Microsoft.AspNet.Mvc" Version="5.2.9" />
<PackageReference Include="System.ComponentModel.Annotations" Version="4.7.0" />
```

---

## 使用方式

### 1️⃣ 驗證副檔名與檔案內容是否符合

```csharp
using (var stream = file.OpenReadStream())
{
    bool valid = FileSignatureValidator.Validate(stream, file.FileName);
}
```

也可加入副檔名白名單：

```csharp
FileSignatureValidator.Validate(stream, file.FileName, new[] { ".docx", ".xlsx" });
```

---

### 2️⃣ 套用 ActionFilterAttribute 進行 API 驗證

#### ✅ 指定檔案欄位名稱：

```csharp
[ValidateFileSignature("file")]
public IActionResult Upload(IFormFile file)
```

#### ✅ 自動檢查所有上傳檔案（不指定參數名）：

```csharp
[ValidateFileSignature]
public IActionResult UploadAll()
```

---

### 3️⃣ 使用 DataAnnotation 屬性驗證模型檔案欄位

```csharp
public class UploadModel
{
    [ValidFileSignature(AllowedExtensions = new[] { ".docx", ".xlsx" }, ErrorMessage = "請上傳正確的檔案類型")]
    public IFormFile File { get; set; }
}
```

---

## 單元測試

範例測試已涵蓋：

- ✅ 正常的副檔名與簽名比對
- ✅ 限制副檔名過濾
- ✅ 多檔案同時上傳時，自動驗證所有檔案

執行方式：

```bash
dotnet test
```

---

## 授權 License

此專案採用 MIT 授權，請自由使用並保留原始出處。
