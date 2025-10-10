#! /bin/sh

REPOSITORY=repos

if [ -e "$REPOSITORY" ]; then
    rm -rf "$REPOSITORY"
fi

git init $REPOSITORY
cd $REPOSITORY

echo "First line" > file.txt
git add file.txt
git commit -m "Initial commit"

git checkout -b branch1
echo "Changes from branch1" > file1.txt
git add file1.txt
git commit -m "Commit from branch1"
git log --oneline --graph --all
cat file.txt
echo "-----"

git checkout -b branch2 main
echo "Changes from branch2" > file2.txt
git add file2.txt
git commit -am "Commit from branch2"

git checkout branch1

# This changes the COMMIT_MSG that will be shown in the editor when merging.
git merge --no-ff --into-name good-name-to-merge branch2

git log --oneline --graph --all
cat file.txt
