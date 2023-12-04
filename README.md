# Documentation
This repository holds various documentation files. 
"Documentation files" are your lab-note that summarize information such as 
  * comment on a specific task
  * ideas
  * todos

**A general guideline in creating 'documentation' for your task is the report should contain __enough details__ such that a 3rd person can reproduce your result just by following your report.**

## A short tutorial on how to use git and this repository
The fact that you are reading this README file indicates that you already created your GitHub account. 

There are mainly two ways to use the git/repository: (1) terminal and (2) application. This tutorial shows how to do so using the terminal approach. For using an app, you can find relevant tutorials by searching the internet.

1. First step is installing `Git`. Following [this tutorial](https://github.com/git-guides/install-git)

2. Open a `terminal` in your computer, and follow the rest of the steps below.
3. Because our respository is __private__, one has to authenticate to access the content.
```shell
git config --global user.name "Put your name here"
git config --global user.email "Put your email here"
```
4. Create a directory where you will work for this project. In this example, **STEM3D_repo** is used. Then, the first thing to do is clone the content of the remote repository to your local directory. You can get the address of this repository by clicking `<> Code` in the menu bar.
```shell
mkdir STEM3D_repo      # this command assumes you are using Linux
cd STEM3D_repo
git clone https://github.com/STEMin3D/Documentation.git
```
5. Start your work in the local directory such as (1) creating a subdirectory, (2) create a new file, (3) modify existing files, (4) deleting some files, etc.
```shell
```
6. Once you are done for your task and ready to copy over the changes to the online repository,
```shell
git commit -a     # this will bring up an editor where you can add a commit log. Provide a short info on what changes are made.
git push
```

7. You can check the current status of your local directory against the online repository by
```shell
git status   # or
git show
```

8. When you resume work later on, **pull** any changes made on the online repository (by someone elses or by you on other machine)
```shell
git pull
```
---
Here is a 1-page `git cheatsheet`: [git cheatsheet](https://rogerdudler.github.io/git-guide/files/git_cheat_sheet.pdf) can be useful.
