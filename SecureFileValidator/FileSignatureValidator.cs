using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace SecureFileValidator
{
    public static class FileSignatureValidator
    {
        public static bool Validate(Stream stream, string fileName, string[]? allowedExtensions)
        {
            var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
            if (allowedExtensions != null && !allowedExtensions.Contains(ext))
                return false;

            return Validate(stream, fileName);
        }

        public static bool Validate(Stream stream, string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLowerInvariant();
            byte[] buffer = new byte[12];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(buffer, 0, buffer.Length);
            string hex = BitConverter.ToString(buffer).Replace("-", "");

            foreach (var kv in FileSignatureMap.SignatureTable)
            {
                if (hex.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
                {
                    // WEBP / AVI 特例（檢查 RIFF container 的格式）
                    if (kv.Key == "52494646")
                    {
                        string sub = Encoding.ASCII.GetString(buffer, 8, 4);
                        if ((sub == "WEBP" && ext == ".webp") || (sub.Trim() == "AVI" && ext == ".avi"))
                            return true;
                        return false;
                    }

                    // ✅ 檢查是否該 magic number 對應的副檔名包含此副檔名
                    if (!kv.Value.Contains(ext))
                        return false;

                    // DOCX / XLSX 檢查內部結構
                    if (ext == ".docx" || ext == ".xlsx")
                    {
                        try
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                            using (var zip = new ZipArchive(stream, ZipArchiveMode.Read, true))
                            {
                                if (ext == ".docx" && zip.Entries.Any(e => e.FullName.StartsWith("word/")))
                                    return true;
                                if (ext == ".xlsx" && zip.Entries.Any(e => e.FullName.StartsWith("xl/")))
                                    return true;
                            }
                        }
                        catch
                        {
                            return false;
                        }

                        return false;
                    }
                    return false;
                }
            }

            // MP4 特例：檢查 ftyp 並確認副檔名
            if (buffer.Length >= 12 && ext == ".mp4")
            {
                string ftyp = Encoding.ASCII.GetString(buffer, 4, 4);
                if (ftyp == "ftyp")
                    return true;
            }

            return false;
        }
    }
}
