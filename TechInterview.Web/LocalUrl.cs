using System.Diagnostics.CodeAnalysis;

namespace TechInterview.Web;

/// <summary>
/// Controllo dei returnUrl passati tra le pagine: solo percorsi locali ("/search?query=mxp"),
/// altrimenti il pulsante Back diventerebbe un redirect verso un sito esterno.
/// </summary>
public static class LocalUrl
{
    public static bool IsLocal([NotNullWhen(true)] string? url) =>
        !string.IsNullOrEmpty(url)
        && url[0] == '/'
        && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'));
}
