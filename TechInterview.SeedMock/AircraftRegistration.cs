namespace TechInterview.SeedMock
{
    /// <summary>
    /// Genera le immatricolazioni aeromobile dai template del catalogo compagnie:
    /// sta qui, accanto ai template, perché ne definisce il formato.
    /// </summary>
    public static class AircraftRegistration
    {
        /// <summary>
        /// Espande il template della compagnia ('#' = cifra, '@' = lettera,
        /// gli altri caratteri restano invariati). Es. "I-@@@@" -> "I-BIKA".
        /// </summary>
        public static string Build(AirlineSeed airline, Random random)
        {
            var template = airline.RegistrationTemplate;
            var registration = new char[template.Length];

            for (int i = 0; i < template.Length; i++)
            {
                registration[i] = template[i] switch
                {
                    '#' => (char)('0' + random.Next(10)),
                    '@' => (char)('A' + random.Next(26)),
                    _ => template[i]
                };
            }

            return new string(registration);
        }
    }
}
