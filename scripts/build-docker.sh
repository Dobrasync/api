#!/bin/bash

# Input vars
VERSION=$1
if [[ -z "$VERSION" ]]; then
    echo "No version set."
    exit 1;
fi

# Common vars
source common-vars.sh

#region CD
cd "$SOLUTION_DIR" || { echo "Failed to change directory to SOLUTION_DIR!"; exit 1; }
#endregion

#region Restore
dotnet restore $SOLUTION_FILE || { echo "Failed to restore packages!"; exit 1; }
#endregion

#region Build
dotnet build $SOLUTION_FILE --configuration Release || { echo "Failed to build!"; exit 1; }
#endregion

#region Publish
mkdir temp
dotnet publish $SOLUTION_FILE -c Release -o ./temp/publish || { echo "Failed to publish!"; exit 1; }
#endregion

#region Build docker image
echo "Building the Docker image..."
docker build -t $DOCKER_IMAGE_NAME:$VERSION -f Dockerfile . || { echo "Failed to build Docker image!"; exit 1; }
#endregion

#region Tag docker image
echo "Tagging Docker image..."
docker tag $DOCKER_IMAGE_NAME:$VERSION ghcr.io/$GHCR_NAMESPACE/$DOCKER_IMAGE_NAME:$VERSION || { echo "Failed to tag Docker image!"; exit 1; }
#endregion

echo "Docker image build succeeded."