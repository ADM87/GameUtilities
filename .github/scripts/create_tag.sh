#!/bin/bash
set -euo pipefail

PREFIX=$1

# Validate input parameter
if [[ -z "$PREFIX" ]]; then
    echo "Branch prefix is required." >&2
    exit 1
fi

# TODO - This can probably be better.
# Fetch tags from remote repository
git fetch --tags

# Get the last tag that matches the versioning pattern major.minor.patch
last_tag=$(git tag -l --sort=-v:refname 'v[0-9]*.[0-9]*.[0-9]*' | head -n 1)

# Default to v0.0.0 if no previous tags exist
last_tag=${last_tag:-"v0.0.0"}

# Parse major, minor, and patch versions from last_tag
IFS='.' read -r major_version minor_version patch_version <<< "${last_tag#v}"

# Check for major/ prefix
if [[ "$PREFIX" == major* ]]; then
    major_version=$((major_version + 1))
    minor_version=0
    patch_version=0
elif [[ "$PREFIX" == minor* ]]; then
    minor_version=$((minor_version + 1))
    patch_version=0
elif [[ "$PREFIX" == patch* ]]; then
    patch_version=$((patch_version + 1))
fi

# Create a new version tag
new_tag="v${major_version}.${minor_version}.${patch_version}"
echo "$new_tag"
