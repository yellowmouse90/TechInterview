using System.Buffers;
using TechInterview.GlobalSearch.Domain;

namespace TechInterview.GlobalSearch.Indexing;

/// <summary>
/// Punteggio di rilevanza di un documento per una query normalizzata: per ogni campo conta il
/// match più forte (esatto, prefisso, contenuto) più il bonus del tipo di campo; il documento
/// prende il massimo sui suoi campi. La distanza tra due livelli di match (100) supera il bonus
/// massimo (30), quindi il tipo di match domina sempre il tipo di campo.
/// </summary>
public static class SearchRanker
{
    private const int ExactMatch = 300;
    private const int PrefixMatch = 200;
    private const int ContainsMatch = 100;

    // Inizio di parola: "Milano Malpensa", "Saint-Exupery", "Cote d'Azur", "I-BIKA".
    private static readonly SearchValues<char> WordSeparators = SearchValues.Create(" -'.");

    /// <returns>Il punteggio del documento, 0 se nessun campo contiene la query.</returns>
    public static int Score(SearchDocument document, string normalizedQuery)
    {
        if (normalizedQuery.Length == 0)
            return 0;

        var best = 0;

        foreach (var field in document.Fields)
        {
            var match = MatchScore(field.NormalizedValue, normalizedQuery);
            if (match > 0)
                best = Math.Max(best, match + FieldBonus(field.Kind));
        }

        return best;
    }

    private static int MatchScore(string value, string query)
    {
        var index = value.IndexOf(query, StringComparison.Ordinal);
        if (index < 0)
            return 0;

        if (index == 0)
            return value.Length == query.Length ? ExactMatch : PrefixMatch;

        // Prefisso di una parola: basta un'occorrenza preceduta da un separatore.
        while (index > 0)
        {
            if (WordSeparators.Contains(value[index - 1]))
                return PrefixMatch;

            index = value.IndexOf(query, index + 1, StringComparison.Ordinal);
        }

        return ContainsMatch;
    }

    private static int FieldBonus(FieldKind kind) => kind switch
    {
        FieldKind.PrimaryCode => 30,
        FieldKind.Name or FieldKind.City => 20,
        FieldKind.RelatedCode => 10,
        FieldKind.Country => 0,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
    };
}
