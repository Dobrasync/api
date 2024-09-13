# Sync pipeline

Keep similar to how Syncthing does it

Client to server pipeline:

1. Scanning
    - Waiting for OS filewatcher events within library directory
    - Running "full scans" every VAR seconds.
2. Blocking
    - Divide every file into "blocks" (chunks) of 128 KiB up to 16 MiB (research why this range was chosen by Syncthing devs)
    - Compute SHA256 hash for each block
    - Store blocklist in local DB
    - Send list to remote
