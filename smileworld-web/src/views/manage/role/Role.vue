<template>
  <div id="role">
    <Content>
      <div slot="search">
        <Search :model="search" :elem="searchElem" :btn-loading="list.loading" @on-search="handleDataList" />
      </div>
      <!-- Search -->
      <div slot="extra" class="toolbar" ref="toolbar">
        <span class="number" v-show="toolbar.number!=0">已选择 {{ toolbar.number }} 数据 </span>
        <Button @click="handleAuth" :disabled="!toolbar.number" icon="md-document">权限</Button>
        <Button type="error" @click="handleBatchDelete" :disabled="!toolbar.number" icon="md-trash">删除</Button>
        <Button @click="handleCreate" icon="md-document">新增</Button>
      </div>
      <!-- .toolbar -->
      <ITable ref="list" stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" @on-selection-change="handleSelectionChange" />
      <!-- ITable -->
      <RoleEdit ref="edit" :model="edit" @on-update="handleDataList" />
      <!-- RoleEdit -->
      <RoleAuthority ref="auth" :id="toolbar.ids[0]" />
    </Content>
  </div>
</template>
<script>
import { delRole, getRolePageList } from "@/services/manage/role";
import RoleEdit from "./RoleEdit";
import RoleAuthority from "./RoleAuthority";
export default {
  name: "Role",
  components: { RoleEdit, RoleAuthority },
  data() {
    return {
      // 工具条(批量删除)
      toolbar: {
        ids: [], // ID数组
        number: 0 // 选择数量
      },
      // 表单数据(用户)
      edit: {
        enabled: 1,
        description: "",
        orderSort: "",
        name: "",
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
          prop: "name",
          placeholder: "Search Name",
          icon: "md-search"
        }
      ],
      // 表格列的配置描述(用户)
      columns: [
        { type: "selection", width: 60, align: "center" },
        { title: "角色名", key: "name", minWidth: 120 },
        { title: "排序", key: "orderSort", minWidth: 100 },
        { title: "描述", key: "description", minWidth: 120 },
        {          title: '状态', minWidth: 100, key: 'enabled',
          render: (h, params) => {
            return h('span',
              {
                style: { color: params.row.enabled == 1 ? 'green' : 'red' }
              }, params.row.enabled == 1 ? '正常' : '锁定中')
          },
          sortable: true
        },

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
    // 批量删除
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
          delRole({
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
     * 删除
     * @param  {object} row 当前行数据
     */
    handleDelete(row) {
      this.$Loading.start();
      this.list.loading = true;
      // (删除)
      delRole({
        ids: [row.id]
      }).then(res => {
        this.$Message.success(res.data.msg);
        this.handleDataList();
      }).catch(() => {
        this.$Loading.error();
        this.list.loading = false;
      });
    },
    /**
     * 获取列表
     * @param  {string} type 是否使用搜索, 默认值 undefined
     */
    handleDataList(type) {
      this.$refs["list"].selectAll(false); // 取消全选
      this.$Loading.start();
      this.list.loading = true; // 列表加载状态
      const page = this.$refs["list"].handlePage(type); // 获取分页信息
      getRolePageList({
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
      this.edit = {
        ...edit
      };
    },
    // 新增界面
    handleCreate() {
      this.$refs["edit"].handleModal(); // 显示模态框
      this.edit = Object.assign({}, this.init);
    },
    handleAuth() {
      if (this.toolbar.ids.length != 1) {
        this.$Message.info('请选择一条数据');
        return;
      }
      this.$refs["auth"].handleData();
      this.$refs["auth"].handleModal();
    }
  }
};
</script>