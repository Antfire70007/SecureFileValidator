using System.Collections.Generic;

namespace SecureFileValidator
{
    public static class FileSignatureMap
    {
        public static readonly Dictionary<string, string[]> SignatureTable = new Dictionary<string, string[]>
        {
          { "FFD8FF", new[] { ".jpg", ".jpeg" } },
          { "89504E47", new[] { ".png" } },
          { "47494638", new[] { ".gif" } },
          { "25504446", new[] { ".pdf" } },
          { "504B0304", new[] { ".zip", ".docx", ".xlsx" } },
          { "D0CF11E0A1B11AE1", new[] { ".doc", ".xls", ".ppt", ".msi", ".msg" } },
          { "4D5A", new[] { ".exe" } },
          { "494433", new[] { ".mp3" } },
          { "FFFB", new[] { ".mp3" } },
          { "526172211A0700", new[] { ".rar" } },
          { "526172211A070100", new[] { ".rar" } },
          { "377ABCAF271C", new[] { ".7z" } },
          { "424D", new[] { ".bmp" } },
          { "49492A00", new[] { ".tif", ".tiff" } },
          { "4D4D002A", new[] { ".tif", ".tiff" } },
          { "4D4D002B", new[] { ".tif", ".tiff" } },
          { "492049", new[] { ".tif", ".tiff" } },
          { "1A45DFA3", new[] { ".mkv", ".mka", ".mks", ".mk3d", ".webm" } },
          { "52494646", new[] { ".webp", ".avi" } }
        };
    }
}
