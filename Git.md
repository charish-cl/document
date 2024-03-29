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