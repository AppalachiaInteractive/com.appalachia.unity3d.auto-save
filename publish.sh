

if [[ "$1" == "patch" || "$1" == "minor" || "$1" == "major" || "$1" == "prepatch" || "$1" == "preminor" || "$1" == "premajor" || "$1" == "prerelease" ]]; then
    npm version $1
    npm publish --registry "http://localhost:4873"
else
    echo "Choose [patch, minor, major, prepatch, preminor, premajor, prerelease]"
fi
