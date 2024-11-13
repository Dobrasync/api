## Setting up with Docker


### Docker Compose

#### Get compose file

You can download a sample `docker-compose.yml` [here](https://github.com/Dobrasync/api/blob/main/docs/docker/docker-compose.yml).


## Backing up

To persist your data you need to back up the following container dirs and files:

### app service

- `/app/appsettings.json`
- `/app/libraries`
- `/app/tempblocks`

### db service

- `/var/lib/mysql/data`
