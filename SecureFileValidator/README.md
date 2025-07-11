# SecureFileValidator

跨平台檔案簽名驗證共用函式庫（支援 .NET 8 + .NET Framework 4.7）

## 功能特色

- ✅ 驗證檔案內容是否與副檔名相符（Magic Number / File Signature）
- ✅ 支援常見格式：JPG、PNG、PDF、DOCX、XLSX、MP4、ZIP、RAR 等
- ✅ 提供 ASP.NET Core 與 ASP.NET MVC 專用的 `ActionFilterAttribute`
- ✅ 可用於 API 上傳檔案驗證、資安保護、病毒掃描前置防禦

## 安裝方式

### 1. 加入參考

若您從原始碼引入：

```bash
dotnet add reference ../SecureFileValidator/SecureFileValidator.csproj
```

或將它打包為 NuGet 後使用：

```bash
dotnet add package SecureFileValidator
```

### 2. 在 Controller 中使用

#### ASP.NET Core (.NET 8 以上)

```csharp
[HttpPost]
[ValidateFileSignature("file")]
public IActionResult Upload(IFormFile file)
{
    return Ok("驗證通過");
}
```

#### ASP.NET MVC (.NET Framework)

```csharp
[HttpPost]
[ValidateFileSignature("file")]
public ActionResult Upload(HttpPostedFileBase file)
{
    return Content("驗證通過");
}
```

## 支援格式對照表（部分）

| 副檔名 | 格式        | Magic Number |
|--------|-------------|--------------|
| `.jpg` | JPEG        | `FF D8 FF`   |
| `.png` | PNG         | `89 50 4E 47`|
| `.pdf` | PDF         | `25 50 44 46`|
| `.docx`| Word (ZIP)  | `50 4B 03 04`|
| `.xlsx`| Excel (ZIP) | `50 4B 03 04`|
| `.mp4` | MP4         | `ftyp`@byte4 |
| `.exe` | PE EXE      | `4D 5A`      |
| `.rar` | RAR4/RAR5   | `52 61 72...`|

## 授權

MIT License
