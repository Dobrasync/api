#!/bin/bash

# Input vars
VERSION=$1
if [[ -z "$VERSION" ]]; then
    echo "No version set."
    exit 1;
fi

# Common vars
source common-vars.sh

#region Push
docker push ghcr.io/$GHCR_NAMESPACE/$DOCKER_IMAGE_NAME:$VERSION
#endregion

