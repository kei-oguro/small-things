#! /bin/sh

# 50000 small files make the git repository 80KB in size.

if [ -e ".git" ]; then
    rm -rf .git
fi
rm -f "*.dat"

git init .

git add try.sh .gitignore
git commit -m "first commit"

if [ -e "dat" ]; then
    rm -rf dat
fi

for i in {1..50000}
do
    echo "${i}" > "dat/${i}.dat"
done

git add -f "dat/*.dat"
git commit -m "add dat"

du -sh .git
