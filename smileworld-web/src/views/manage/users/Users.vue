<template>
  <div id="users">
    <Content>
      <div slot="search">
        <Search :model="search" :elem="searchElem" :btn-loading="list.loading" @on-search="handleDataList" />
      </div>
      <!-- Search -->
      <div slot="extra" class="toolbar">
        <span class="number" v-show="toolbar.number!=0">已选择 {{ toolbar.number }} 数据 </span>
        <Button @click="handleCreate" icon="md-document">新增</Button>
        <Button type="error" @click="handleBatchDelete" :disabled="!toolbar.number" icon="md-trash">删除</Button>
      </div>
      <!-- .toolbar -->
      <ITable ref="list" stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" @on-selection-change="handleSelectionChange" />
      <!-- ITable -->
      <UserEdit ref="edit" :model="edit" @on-update="handleDataList" />
      <!-- UserEdit -->
    </Content>
  </div>
</template>
<script>
import { delUser, getUserList } from "@/services/manage/users";
import UserEdit from "./UserEdit";
export default {
  name: "Users",
  components: {
    UserEdit
  },
  data() {
    return {
      // 工具条(批量删除)
      toolbar: {
        ids: [], // ID数组
        number: 0 // 选择数量
      },
      // 表单数据(用户)
      edit: {
        loginName: "",
        realName: "",
        age: "",
        gender: 1,
        roles: [],
        status: 1,
        remark: "",
      },
      // 初始表单数据(用户)
      init: "",
      // 表单数据(搜索)
      search: {
        name: ""
      },
      // 列表属性
      list: {
        data: [], // 结构化数据
        total: 0, // 数据总数
        loading: false // 加载状态
      },
      // 表单元素(搜索)
      searchElem: [
        {
          // label: 'Name',
          prop: "name",
          placeholder: "Search Name",
          icon: "md-search"
        }
      ],
      // 表格列的配置描述(用户)
      columns: [
        { type: "selection", width: 60, align: "center" },
        { title: "登陆名", key: "loginName", minWidth: 120 },
        { title: "用户名", key: "realName", minWidth: 120 },
        { title: "年龄", key: "age", minWidth: 80 },
        {          title: "性别", key: "gender", minWidth: 80,
          render: (h, params) =>
            h("span", params.row.gender === 1 ? "男" : "女")
        },
        {          title: '状态', minWidth: 100, key: 'status',
          render: (h, params) => {
            return h('span',
              {
                style: { color: params.row.status == 1 ? 'green' : 'red' }
              }, params.row.status == 1 ? '正常' : '锁定中')
          },
          sortable: true
        },
        { title: "备注", key: "remark", minWidth: 100 },
        { title: "最后登陆时间", key: "lastLoginTime", minWidth: 120 },
        {          title: "Operation", key: "operation", align: "center", fixed: "right", minWidth: 150,
          render: (h, params) =>
            h("div", [
              h(
                "a",
                {
                  style: {
                    marginRight: "16px"
                  },
                  on: {
                    click: () => this.handleEdit(params.row)
                  }
                },
                [
                  h("Icon", {
                    props: {
                      type: "md-create",
                      size: 16
                    },
                    style: {
                      marginTop: "-2px",
                      marginRight: "4px"
                    }
                  }),
                  "编辑"
                ]
              ),
              h(
                "Poptip",
                {
                  props: {
                    confirm: true,
                    transfer: true,
                    title: "确实要删除此数据吗?",
                    "ok-text": "yes",
                    "cancel-text": "no"
                  },
                  on: {
                    "on-ok": () => this.handleDelete(params.row)
                  }
                },
                [
                  h("a", [
                    h("Icon", {
                      props: {
                        type: "md-trash",
                        size: 16
                      },
                      style: {
                        marginTop: "-2px",
                        marginRight: "4px"
                      }
                    }),
                    "删除"
                  ])
                ]
              )
            ])
        }
      ]
    };
  },
  mounted() {
    this.init = Object.assign({}, this.edit); // 复制初始表单数据
    this.handleDataList(); // 获取列表数据
  },
  methods: {
    /**
     * 在多选模式下有效，只要选中项发生变化时就会触发
     * @param  {array} selection 已选项数据
     */
    handleSelectionChange(selection) {
      const ids = [];
      for (const item of selection) {
        ids.push(item["id"]);
      }
      this.toolbar = {
        ids: ids,
        number: selection.length
      };
    },
    // 批量删除用户
    handleBatchDelete() {
      let that = this;
      this.$Modal.confirm({
        title: "提示",
        content: '此操作为不可逆操作，是否确认删除？',
        width: 400,
        loading: true,
        onOk() {
          that.$Loading.start();
          that.list.loading = true;
          delUser({
            'ids': that.toolbar.ids.join(',')
          }).then(res => {
            let { data, msg, code } = res;
            if (code === 0) {
              that.$Message.success('OK');
              that.handleDataList();
            } else {
              that.$Loading.error();
              that.list.loading = false;
            }
          })
          this.remove();
        }
      })
    },
    /**
     * 删除用户
     * @param  {object} row 当前行数据
     */
    handleDelete(row) {
      this.$Loading.start();
      this.list.loading = true;
      // (删除)
      delUser({
        ids: [row.id]
      })
        .then(res => {
          this.$Message.success(res.data.msg);
          this.handleDataList();
        })
        .catch(() => {
          this.$Loading.error();
          this.list.loading = false;
        });
    },
    /**
     * 获取用户列表
     * @param  {string} type 是否使用搜索, 默认值 undefined
     */
    handleDataList(type) {
      this.$refs["list"].selectAll(false); // 取消全选
      this.$Loading.start();
      this.list.loading = true; // 列表加载状态
      const page = this.$refs["list"].handlePage(type); // 获取分页信息
      getUserList({
        ...this.search,
        pagination: {
          pageIndex: page.current,
          pageSize: page.pageSize
        }
      })
        .then(res => {
          this.$Loading.finish();
          const { data, total } = res.data;
          this.list = {
            data: data,
            total: total,
            loading: false
          };
        })
        .catch(() => {
          this.$Loading.error();
          this.list.loading = false;
        });
    },
    /**
     * 编辑界面
     * @param  {object} row 当前行数据
     */
    handleEdit(row) {
      this.$refs["edit"].handleModal(1); // 显示模态框
      // (获取详情)
      const edit = Object.assign({}, row);
      const roles = [];
      for (const item of row.userRoles) {
        roles.push(item.roleId);
      }
      this.edit = {
        ...edit,
        roles
      };
      this.$refs["edit"].handlePatch(); // 获取补丁数据
    },
    // 新增界面
    handleCreate() {
      this.$refs["edit"].handleModal(); // 显示模态框
      this.edit = Object.assign({}, this.init);
      this.$refs["edit"].handlePatch(); // 获取补丁数据
    }
  }
};
</script>
