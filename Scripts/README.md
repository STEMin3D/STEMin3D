# This directory holds various 3-D model scripts from Blender, Unity, or Unreal Engine

## Large-File Storage
3-D script files are larger than the file size limit (100 MB) in the standard GitHub repository. Therefore, we need to use a different/separate functionality for handling these large files. 

GitHub provides ''lfs'', Large-File Storage, which has a 5GB file size limit. To use this in conjunction with the standard GitHub repo, you need to install git-lfs and initialize it. Follow the instructions below.

### Installation

You need to install git-lfs on your local machine. Follow the instruction in [installing-lfs](https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage).

### Configure lfs

After installing git-lfs, follow instruction in [configure-lfs](https://docs.github.com/en/repositories/working-with-files/managing-large-files/configuring-git-large-file-storage).

### How to Use

In the repository, you can specify the type of files that you want to store as LFS files by ''tracking''. For example, if you want to add "MyExample.blend" in the repository as a LFS file, then issue the command below.
```
git lfs track "MyExample.blend"

# or more generally, you can track all files with .blend file extension.
git lfs track "*.blend"
```

After adding information for a new track, you need to commit/push before you add new LFS file types. In this example, a new LFS file type ''*.unity'' is added.
```
git lfs track "*.unity"
git commit .gitattributes
git push
```
Then, you can add a large script file as shown below.
```
# Add/copy/create the large script file in your local repo directory.
git add MyLargeModel.blend
git commit -m "Adding a large script file (to LFS)"
git push origin main
```

You can check all types of tracked files with
```
git lfs track
```
