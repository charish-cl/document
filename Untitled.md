

[[Unity\]无限循环列表(根据内容自适应Item大小、可自动滑动)](https://www.kadastudio.cn/archives/248)

[unity UGUI无限滚动 unity循环滚动列表](https://blog.51cto.com/u_16099215/9874661)



 

    public  class NormalBtnItem
    {   
        
        public Button NormalBtn { get;  set; }
        public TextMeshProUGUI BtnText { get;  set; }
        public Transform BgParent { get;  set; }
    
        public NormalBtnItem (Transform transform)
        {
            
            NormalBtn = transform.Find("").GetComponent<Button>();
            NormalBtn.onClick.AddListener(() => OnClick_NormalBtn());
            BtnText = transform.Find("BtnText").GetComponent<TextMeshProUGUI>();
            BgParent = transform.Find("BgParent").GetComponent<Transform>();
    
        }
        
        private void OnClick_NormalBtn()
        {
        
        }


    }