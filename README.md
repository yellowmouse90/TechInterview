# README.md
# Tech Interview – Global Search API Challenge

## Contesto
Questa solution contiene due microservizi .NET 10 (Aspire):

1. **Aeroporti Service**  
   - Espone API **REST**.  
   - Gestisce operazioni **CRUD** sugli aeroporti.

2. **Voli Service**  
   - Espone API **gRPC**.  
   - Gestisce operazioni **CRUD** sui voli.

Questi due sistemi rappresentano due fonti dati separate. I rispettivi contratti e modelli sono disponibili all’interno della solution.

I dati mock di entrambi i domini sono costruiti sopra il catalogo condiviso **TechInterview.SeedMock** (aeroporti e compagnie): il codice IATA dell'aeroporto è la chiave con cui i voli referenziano gli aeroporti.

---

## Prerequisiti ed esecuzione
La solution è cross-platform: gira su macOS (Intel e Apple Silicon), Windows e Linux con il solo **SDK .NET 10**.

```bash
dotnet dev-certs https --trust     # una volta sola, su macOS e Windows
dotnet restore
dotnet run --project TechInterview.AppHost
```

L'AppHost Aspire stampa in console l'URL di login del dashboard, da cui si raggiungono i due servizi.

> La code generation gRPC richiede `Grpc.Tools >= 2.84.0`, la prima versione che include un `protoc`
> universal per macOS (arm64 + x64). Con le versioni precedenti il build su Apple Silicon fallisce con
> `Bad CPU type in executable`, a meno di installare Rosetta.

---

## Obiettivo del task
Il tuo compito è progettare e implementare una **funzionalità di ricerca globale** che, dato un termine di ricerca testuale, consenta di ottenere risultati provenienti sia dal dominio *Aeroporti* sia dal dominio *Voli*.

L’API di ricerca globale deve:

- accettare una **stringa di ricerca** di almeno **3 caratteri**;
- accettare parametri di paginazione: `offset`, `limit`;
- restituire una lista di risultati eterogenei, ciascuno con:
  - `id`
  - `resourceType`
  - `description`
- rispettare il limite imposto da `limit`.

### Campi da includere nella ricerca
**Aeroporti**: codice, nome, città, nazione.  
**Voli**: codice volo, numero aeromobile, città di partenza/arrivo, codice aeroporto di partenza/arrivo.

---

## Libertà architetturali
La progettazione è completamente **a tua discrezione**.
Puoi costruire la Global Search API come:

- un nuovo microservizio dedicato;
- un'estensione di un progetto esistente;
- un orchestratore;
- un servizio con proprio database o cache;
- qualunque architettura tu ritenga appropriata.

Non ci sono vincoli: valuta integrazione, modellazione, performance e manutenibilità.

---

## Output atteso
Endpoint HTTP previsto:
```
GET /api/global-search?query={string}&offset={n}&limit={n}
```

### Esempio risposta
```json
{
  "items": [
    {
      "id": "MXP",
      "resourceType": "airport",
      "description": "MXP – Malpensa (Italy)"
    },
    {
      "id": "AZ178",
      "resourceType": "flight",
      "description": "AZ178 – MXP → JFK"
    }
  ],
  "offset": 0,
  "limit": 10,
  "count": 2
}
```

---

## Requisiti minimi
- Query con almeno 3 caratteri.  
- Rispetto del limite `limit`.  
- `description` leggibile.  
- Solution compilabile ed eseguibile.

---

## Consegna
Fornire:
- codice dell’implementazione
- eventuali note architetturali
- istruzioni di esecuzione
