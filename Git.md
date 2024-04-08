# [git commit之后，想撤销commit](https://www.cnblogs.com/lfxiao/p/9378763.html)

**git reset --soft HEAD^**

 

这样就成功的撤销了你的commit

注意，仅仅是撤回commit操作，您写的代码仍然保留。

如果你进行了2次commit，想都撤回，可以使用HEAD~2

## 至于这几个参数：

## --mixed 

意思是：不删除工作空间改动代码，撤销commit，并且撤销git add . 操作

这个为默认参数,git reset --mixed HEAD^ 和 git reset HEAD^ 效果是一样的。

 

## --soft  

不删除工作空间改动代码，撤销commit，不撤销git add . 

 

## --hard

删除工作空间改动代码，撤销commit，撤销git add . 

注意完成这个操作后，就恢复到了上一次的commit状态。



# Rebase

http://jartto.wang/2018/12/11/git-rebase/

# Amend

https://blog.csdn.net/qq_52855744/article/details/136046146?spm=1001.2014.3001.5502



修改分支名

先将本地分支重命名

```
git branch -m oldBranch newBranch
```

删除远程分支（远端无此分支则跳过该步骤）

```
git push --delete origin oldBranch
```

将重命名后的分支推到远端

```
git push origin newBranch
```

把修改后的本地分支与远程分支关联

```
git branch --set-upstream-to origin/newBranch
```


## git只忽略本地

https://luisdalmolin.dev/blog/ignoring-files-in-git-without-gitignore/

https://mengqi92.github.io/2020/07/17/hide-files-from-git/
# fork如何更新

![image-20240319115917690](assets/image-20240319115917690.png)

# fetch与pull区别

`git fetch` 和 `git pull` 是 Git 中用于获取远程仓库更新的两个命令，它们有一些区别：

1. **git fetch：**
   - `git fetch` 命令会将远程仓库的所有分支、标签等信息都下载到本地，但不会自动合并任何东西到您当前的工作分支。
   - 它会将获取的更新保存到本地仓库的远程跟踪分支（remote tracking branch）中，例如 `origin/main`。
   - 使用 `git fetch` 可以查看远程仓库的最新状态，但不会直接影响您当前的工作区。
2. **git pull：**
   - `git pull` 命令实际上包含了两个操作：`git fetch` 和 `git merge`（或 `git rebase`）。
   - 它会从远程仓库获取最新的提交并自动合并到您当前的工作分支。
   - 如果使用 `git pull` 时存在冲突，Git 会尝试自动解决冲突并进行合并操作。

因此，主要区别在于 `git fetch` 只是获取远程仓库的更新但不会直接合并到当前分支，而 `git pull` 则会获取并合并远程更新到当前分支。选择使用哪个命令取决于您的工作流程和需求：如果您想查看更新并手动决定如何合并，可以使用 `git fetch`；如果您希望自动合并更新到当前分支，可以使用 `git pull`。

