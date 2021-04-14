

if [[ "$1" == "patch" || "$1" == "minor" || "$1" == "major" || "$1" == "prepatch" || "$1" == "preminor" || "$1" == "premajor" || "$1" == "prerelease" ]]; then
    npm version $1
    
    if [ $? -ne 0 ]; then
        exit $?        
    fi

    package_version=$(cat package.json \
    | grep version \
    | head -1 \
    | awk -F: '{ print $2 }' \
    | sed 's/[",]//g'
    | tr -d '[[:space:]]')

    rm -f .dist
    
    if [ $? -ne 0 ]; then
        exit $?        
    fi

    mkdir .dist
    
    if [ $? -ne 0 ]; then
        exit $?        
    fi

    cd .dist

    npm pack ..

    if [ $? -ne 0 ]; then
        exit $?        
    fi

    cd ..

    package=`ls .dist | head -n 1`

    echo "Publishing"

    npm publish $package --registry "http://localhost:4873"
    
    if [ $? -ne 0 ]; then
        exit $?        
    fi 
    
    #use release notes from a file
    echo "Sending to github as release"

    gh release create $package_version .dist/*.tgz -F CHANGELOG.md
    if [ $? -ne 0 ]; then
        exit $?        
    fi 

    echo "Destroying tarballs"
    
    rm -f .dist


else
    echo "Choose [patch, minor, major, prepatch, preminor, premajor, prerelease]"
    exit 1
fi
