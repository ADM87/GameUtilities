#!/bin/bash
set -euo pipefail

BRANCH_NAME=$1

# Validate input parameter
if [[ -z "$BRANCH_NAME" ]]; then
    echo "Branch name is required." >&2
    exit 1
fi

# Validate branch name pattern
if [[ ! "$BRANCH_NAME" =~ ^(major|minor|patch)/.*$ ]]; then
    echo "Invalid branch name format. Expected format: major/, minor/, or patch/ prefix." >&2
    exit 1
fi

# Fetch tags from remote repository
if ! git fetch --tags; then
    echo "Failed to fetch tags from remote repository." >&2
    exit 1
fi

# Get the last tag that matches the versioning pattern major.minor.patch
last_tag=$(git tag -l --sort=-v:refname 'v[0-9]*.[0-9]*.[0-9]*' | head -n 1)

# Default to v0.0.0 if no previous tags exist
last_tag=${last_tag:-"v0.0.0"}

echo "Current version: $last_tag"

# Parse major, minor, and patch versions from last_tag
IFS='.' read -r major_version minor_version patch_version <<< "${last_tag#v}"

echo "Branch: $BRANCH_NAME"

# Check for major/ prefix
if [[ "$BRANCH_NAME" == major/* ]]; then
    echo "Incrementing major version."
    major_version=$((major_version + 1))
    minor_version=0
    patch_version=0
elif [[ "$BRANCH_NAME" == minor/* ]]; then
    echo "Incrementing minor version."
    minor_version=$((minor_version + 1))
    patch_version=0
elif [[ "$BRANCH_NAME" == patch/* ]]; then
    echo "Incrementing patch version."
    patch_version=$((patch_version + 1))
else
    echo "Current branch does not have major/, minor/, or patch/ prefix" >&2
    echo "Version will not be updated, and no tag will be created." >&2
    exit 1
fi

# Create a new version tag
new_tag="v${major_version}.${minor_version}.${patch_version}"
echo "New version: $new_tag"

if ! git tag -a "$new_tag" -m "Version $new_tag" >&2; then
    echo "Failed to create tag." >&2
    exit 1
fi

if ! git push origin "$new_tag" >&2; then
    echo "Failed to push tag." >&2
    exit 1
fi

echo "Tag created and pushed successfully."
