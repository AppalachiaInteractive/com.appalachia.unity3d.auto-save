version=$npm_package_version

tag=$(git describe --abbrev=0)
previous_tag=$(git describe --abbrev=0 $tag^)
previous_tag_hash=$(git rev-list -n 1 $previous_tag)
commit_after_previous=$(git log --reverse --ancestry-path $previous_tag_hash..main | head -n 1 | cut -d \  -f 2)
message=$(git log --pretty=format:"%h %s" --graph $commit_after_previous..HEAD)
clean_message=$(echo "$message" | sed -z 's/\n/\\n/g')

find="## Unreleased"
replace="## Unreleased\n\n## $version\n$clean_message"

echo '--------------'
#cat CHANGELOG.md
mv CHANGELOG.md CHANGELOG.md.bk
sed "s|$find|$replace|g" CHANGELOG.md.bk > CHANGELOG.md.bk2
echo '--------------'
#cat CHANGELOG.md
sed "s|\n\n\n|\n\n|g" CHANGELOG.md.bk2 > CHANGELOG.md
echo '--------------'
cat CHANGELOG.md
echo '--------------'