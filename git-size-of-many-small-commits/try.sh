#! /bin/sh

# 55733 small commits make the git repository 62MB in size.

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

for i in {1..1000000}
do
    echo "${i}" > "dat/${i}.dat"
    git add -f "dat/${i}.dat"
    git commit -m "add ${i}.dat"
done

du -sh .git
