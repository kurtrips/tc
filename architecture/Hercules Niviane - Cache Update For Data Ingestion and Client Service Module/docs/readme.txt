Both updated architectures are included.
New changes are denoted in red. 
Please read Hercules Niviane Client Service Module ADS section 1.3.10 first.

Hercules Niviane Client Service Module:

1. The Cache Component is new. See the Cache Component Interface Diagram.
2. The Persistence Component is updated to use the Cache component instead of memcached.
3. The Assembly spec is updated to require setup of Ehcache replication and bootstrapping configuration.
4. The ADS is updated with the caching based design decisions (see 1.3.10)
5. Component Diagram is updated
6. Persistence Request SD and Clear Cache SD are updated.

Hercules Niviane Data Ingestion Module:

1. The DataIngestionPersistenceService is updated to use cache.
2. Persistence Operation SD is updated.