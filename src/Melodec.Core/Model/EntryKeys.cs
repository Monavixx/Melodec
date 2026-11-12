using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Melodec.Core.Model;

public static class EntryKeys
{
    /// <summary>NFKC, trimmed, lower-cased, whitespace collapsed. Keys must never change between versions.</summary>
    public static string Normalize(string s)
    {
        var n = s.Normalize(NormalizationForm.FormKC).Trim().ToLowerInvariant();
        var sb = new StringBuilder(n.Length);
        var space = false;
        foreach (var c in n)
        {
            if (char.IsWhiteSpace(c)) { space = true; continue; }
            if (space && sb.Length > 0) sb.Append(' ');
            space = false;
            sb.Append(c);
        }
        return sb.ToString();
    }

    public static string HashUrl(Uri url)
    {
        var canonical = url.GetComponents(
            UriComponents.SchemeAndServer | UriComponents.Path | UriComponents.Query,
            UriFormat.UriEscaped).TrimEnd('/').ToLowerInvariant();
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(canonical));
        return Convert.ToHexString(hash, 0, 6).ToLower(CultureInfo.InvariantCulture);
    }
}
