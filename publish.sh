

if [[ "$1" == "patch" || "$1" == "minor" || "$1" == "major" || "$1" == "prepatch" || "$1" == "preminor" || "$1" == "premajor" || "$1" == "prerelease" ]]; then
    npm version $1
    
    if [ $? -eq 0 ]; then
        npm publish --registry "http://localhost:4873"
        exit 1
    fi

else
    echo "Choose [patch, minor, major, prepatch, preminor, premajor, prerelease]"
    exit 1
fi
