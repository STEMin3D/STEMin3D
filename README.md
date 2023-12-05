# STEMin3D GitHub Repository
This repository has three main subdirectories each of which holds specific files for the [STEMin3D Project](https://STEMin3D.net).

  * [Documentation Folder](Documentations): contains project notes
  * [Storyboard Folder](Storyboard): contains storyboard files
  * [Script Folder](Scripts): contains 3-D model scripts

In addition to these files, you can create any relevant files to hold information such as (1) a description of a specific task, (2) new ideas, (3) todos, (4) in-document discussion, (5) to-do items, etc.

> [!IMPORTANT]
> Your project note should contain *enough details* so that a 3rd person can reproduce your result just by following steps in your report.

## 👀 A short tutorial on how to use git and this repository
The fact that you are reading this README file indicates that you already created your GitHub account. You will need to use your GitHub account information in step #3 below.

There are mainly two ways to use the git/repository: (1) the command-line approach in a terminal and (2) by using the [GitHub Desktop application](https://desktop.github.com/). This tutorial shows how to use our repository using the terminal approach. For using an app, you can find relevant tutorials by searching the internet.

`Step 1` First step is installing *Git*. Read [this tutorial](https://github.com/git-guides/install-git) to install it on your machine.

`Step 2` Open a *terminal* in your computer, and follow the rest of the steps below.

`Step 3` Because our repository is a __private__ one, one has to authenticate to access the content.
```shell
git config --global user.name "Put your name here"
git config --global user.email "Put your email here"
# after this step, git commands that try to access the remote repository may prompt you for a password.
```
`Step 4` Create a directory where you will work for this project. In this example, ***STEM3D_repo*** is used. 
Then, the first thing to do is to clone the content of the remote repository to your local directory. 
To clone, you need to know the git address which you find by clicking `<> Code` in the menu bar on the GitHub homepage.
```shell
mkdir STEM3D_repo      # this command assumes you are using Linux
cd STEM3D_repo
git clone https://github.com/STEMin3D/Documentation.git
```
`Step 5` Start your work in the local directory. You may create a subdirectory(ies), create new files, 
delete some old no-longer-useful files, modify existing files, etc. After you finish your work, you need to 
synchronize the changes in your local directory to the remote repository. This step consists of two commands: "git commit .." + "git push".
```shell
git fetch               # download any changes from the repository. Always start your work with this command.
git add <new file>      # add a new file you created
git add -A              # add ALL changes
git rm <filename>       # remove a file
git status              # check the current status of the local directory
...
```

`Step 6` After finishing your work, copy over the changes to the online repository,
```shell
git commit -a     # this may bring up an editor window where you can add a commit log.
# Provide a short info on what changes are made.
git push          # Synchronize the remote repository with changes from your local directory.
```

`Step 7` Your daily work on the project should start with the command "git fetch" which will guarantee that your 
local directory is sync with the remote repository. The remote repository might have been changed since your 
previous work due to changes made by someone else or by yourself on another machine.

> [!TIP]
> Any git commands (or most commands) can have an option flag "-n" which means a dry-run. 
> When you want to check if a command runs OK but do not want to make actual changes, add this option. 
> ```shell
> git add -n <new_file>   # see if this command creates any error or warning (due to a possible conflict)
> git rm -n <a file>      # likewise..
> ```
>
> Here is a 1-page [git cheatsheet](https://rogerdudler.github.io/git-guide/files/git_cheat_sheet.pdf) that can be handy. 
