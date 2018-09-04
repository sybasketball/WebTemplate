var SelectComboTree = {
    Tree: null,
    ForInputText: null,
    ForInputId: null,
    IdField: "Id",
    TextField: "Text",
    PidField: "Pid",
    TreePanel: null,
    GetTreeSetting: function () {
        var setting = {
            view: {
                dblClickExpand: false,
                showLine: true,
                selectedMulti: false
            },
            data: {
                key: {
                    name: SelectComboTree.TextField,
                    title: SelectComboTree.TextField
                },
                simpleData: {
                    enable: true,
                    idKey: SelectComboTree.IdField,
                    pIdKey: SelectComboTree.PidField,
                    rootPId: 0
                }
            },
            callback: {
                onClick: function (e, treeId, treeNode) {                    
                    var currentNode = SelectComboTree.Tree.getNodeByParam("Id", $("#" + SelectComboTree.IdField).val(), false);
                    if (currentNode) {
                        if (currentNode == treeNode) {
                            SelectComboTree.HideSelectTree();
                            BasePage.ErrorMsg("不允许将该节点自身再次设置为该节点的父节点");
                        }
                        else {
                            pnode = SelectComboTree.Tree.getNodeByParam("Id", treeNode.Id, currentNode);
                            if (pnode) {
                                SelectComboTree.HideSelectTree();
                                BasePage.ErrorMsg("不允许将该节点的子节点再次设置为该节点的父节点");
                            }
                            else {
                                SelectComboTree.SelectTreeNode(treeNode);
                            }
                        }
                    }
                    else
                        SelectComboTree.SelectTreeNode(treeNode);
                }
            }
        }
        return setting;
    },
    SelectTreeNode: function (treeNode) {
        
        var textInput = $(SelectComboTree.ForInputText);
        var idInput = $(SelectComboTree.ForInputId);
        if (textInput) {
            textInput.val(treeNode.Title);
            textInput.blur();
        }
        if (idInput) idInput.val(treeNode.Id);
        SelectComboTree.HideSelectTree();
    },
    ShowSelectTree: function () {
        if ($(SelectComboTree.TreePanel).css("display") == "none") {
            var inputObj = $(SelectComboTree.ForInputText);
            if (inputObj) {
                var inputOffset = inputObj.offset();
                $(SelectComboTree.TreePanel).css({
                    left: inputOffset.left + "px",
                    top: inputOffset.top + inputObj.outerHeight() + 1 + "px",
                    width: inputObj.innerWidth()
                });
                if ($(SelectComboTree.TreePanel).height() > 300) {
                    $(SelectComboTree.TreePanel).height(300);
                }
                $(SelectComboTree.TreePanel).slideDown("fast");
                $("body").bind("mousedown", SelectComboTree.OnBodyDown);
            }
        }
    },
    HideSelectTree: function () {
        $(SelectComboTree.TreePanel).fadeOut("fast");
        $("body").unbind("mousedown", SelectComboTree.OnBodyDown);
    },
    OnBodyDown: function (event) {
        if (!(event.target.id == $(SelectComboTree.ForInputText).attr("id") || event.target.id == $(SelectComboTree.TreePanel).attr("id") || $(event.target).parents(SelectComboTree.TreePanel.selector).length > 0)) {
            SelectComboTree.HideSelectTree();
        }
    },
    LoadTreeData: function (tree, treePanel, forInputText, forInputId, loadDataUrl, idField, textField, pidField, defaultVal) {
        this.ForInputText = forInputText;
        this.ForInputId = forInputId;
        this.IdField = idField;
        this.TextField = textField;
        this.TreePanel = treePanel;
        this.PidField = pidField;
        BasePage.Ajax({
            type: "GET",
            url: loadDataUrl,
            isLoading: false,
            success: function (data, message) {
                SelectComboTree.Tree = $.fn.zTree.init($(tree), SelectComboTree.GetTreeSetting(), data);
                SelectComboTree.Tree.expandAll(true);
                $(SelectComboTree.ForInputText).bind("click", SelectComboTree.ShowSelectTree);
                if (defaultVal) {
                    var treeNode = SelectComboTree.Tree.getNodeByParam("Id", defaultVal, false);
                    SelectComboTree.Tree.selectNode(treeNode);
                    SelectComboTree.SelectTreeNode(treeNode);
                }
                //var newNode = { Title: "全部", isParent:true };
                //SelectComboTree.Tree.addNodes(null,0,newNode);
            }
        });
    }
}