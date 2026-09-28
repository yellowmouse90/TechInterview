using System.Globalization;
using System.Text;

namespace TechInterview.GlobalSearch.Indexing;

/// <summary>
/// Normalizzazione unica per query e dati: senza spazi ai bordi, senza diacritici
/// ("München" → "MUNCHEN"), maiuscola invariant e con gli spazi consecutivi ridotti a uno.
/// Dopo la normalizzazione tutti i confronti sono ordinal: niente CurrentCulture,
/// che con la cultura turca trasformerebbe "i" in "İ" e romperebbe codici come "IST".
/// </summary>
public static class TextNormalizer
{
    public static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        // FormD separa le lettere dai segni diacritici, che vengono scartati.
        var decomposed = value.Trim().Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(decomposed.Length);
        var previousWasSpace = false;

        foreach (var c in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                continue;

            if (char.IsWhiteSpace(c))
            {
                if (!previousWasSpace)
                    builder.Append(' ');

                previousWasSpace = true;
                continue;
            }

            builder.Append(char.ToUpperInvariant(c));
            previousWasSpace = false;
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
