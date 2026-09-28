# Global Search – Possible evolution

**Not implemented.** The in-memory index fits the current data (about 1,100 documents). This note describes
how the service could evolve if the data grew to millions of documents.

## Persistent search store

The in-memory index would be replaced by a PostgreSQL database owned by the search service, with trigram indexes
(`pg_trgm`) for "contains" matches and ordinary indexes for exact and prefix matches. The new implementation keeps
the same `ISearchIndex` interface, so the API, the sources and the refresh do not change. This is why the
interface is asynchronous even though the current implementation is in memory.

Compared to today:

- **Ranking**: the same rules, expressed in SQL. Since the match level always weighs more than the field, the
  search can proceed by levels (exact, then prefix, then contains) and stop once it has enough rows for the
  requested page, instead of ranking every match.
- **Paging**: a maximum depth for `offset + limit` and a total count that stops at a threshold (for example
  "1000+"), as search engines usually do. Counting and sorting millions of matches on every request would cost too much.
- **Refresh**: instead of replacing everything, each load writes only the documents that were added, changed or
  removed, in a single transaction, so concurrent searches keep seeing a consistent state. A safeguard skips
  mass deletions when a load looks suspicious.

## Feeding the store

With millions of rows, a full read through the paged APIs becomes too slow and is rarely consistent.
The alternatives:

| Option | Freshness | Impact on the source services |
|---|---|---|
| Periodic full read (current approach) | one refresh interval | none |
| Change data capture on the source databases (e.g. Debezium) | seconds | no code change, but coupled to their schema |
| Domain events published by the services (outbox + message broker) | seconds | the services must publish events |

With the last two options, the full read stays as a periodic reconciliation (for example nightly) that repairs
missed events.
