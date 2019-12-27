<template>
  <div id="mobileType">
    <Content>
      <div slot="search">
        <Search :model="search" :elem="searchElem" :btn-loading="list.loading" @on-search="handleDataList" />
      </div>
      <!-- Search -->
      <div slot="extra" class="toolbar">
        <span class="number" v-show="toolbar.number!=0">已选择 {{ toolbar.number }} 数据 </span>
        <Button @click="handleChannel" icon="md-document">渠道管理</Button>
        <Button @click="handleBrand" icon="md-document">品牌管理</Button>
        <Button type="error" @click="handleBatchDelete" :disabled="!toolbar.number" icon="md-trash">删除</Button>
        <Button @click="handleEdit" :disabled="toolbar.number!==1" icon="md-document">修改</Button>
        <Button @click="handleCreate" icon="md-document">新增</Button>
      </div>
      <!-- .toolbar -->
      <ITable ref="list" border stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" @on-selection-change="handleSelectionChange" />
      <!-- ITable -->
      <MobileTypeEdit ref="edit" @on-update="handleDataList" />
      <Brand ref="brand" />
      <Channel ref="channel" />
    </Content>
  </div>
</template>
<script>
import { getMobileTypePageList, delMobileType, editMobileTypeEnable } from "@/services/distributor/mobileType";
import MobileTypeEdit from "./mobileTypeEdit";
import Brand from "./brand";
import Channel from "./channel";
export default {
  name: "MobileType",
  components: { MobileTypeEdit, Brand, Channel },
  data() {
    return {
      // 工具条(批量删除)
      toolbar: {
        ids: [], // ID数组
        number: 0, // 选择数量
        selectRow: {}
      },
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
        { type: "selection", width: 60, align: "center", fixed: 'left' },
        { type: 'index', width: 70, align: 'center' },
        { title: "渠道名称", key: "channelName", minWidth: 120 },
        { title: "品牌名称", key: "brandName", minWidth: 120 },
        { title: "机型名称", key: "mobileTypeName", minWidth: 120 },
        {          title: "是否启用", key: "operation", align: "center", fixed: "right", minWidth: 80,
          render: (h, params) =>
            h("div", [
              h("i-switch", {
                props: {
                  type: "primary",
                  value: params.row.enable === 1
                },
                on: {
                  'on-change': (value) => {
                    params.row.enable = value ? 1 : 0;
                    this.EditEnable(params.row.id, params.row.enable);
                  }
                }
              })
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
        number: selection.length,
        selectRow: selection
      };
    },
    // 批量删除
    handleBatchDelete(n) {
      let that = this;
      this.$Modal.confirm({
        title: "提示",
        content: '此操作为不可逆操作，是否确认删除？',
        width: 400,
        loading: true,
        onOk() {
          that.$Loading.start();
          that.list.loading = true;
          delMobileType({
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
          that.$Loading.finish();
          this.remove();
        }
      })
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
      getMobileTypePageList({
        ...this.search,
        pagination: {
          pageIndex: page.current,
          pageSize: page.pageSize
        }
      }).then(res => {
        this.$Loading.finish();
        const { data, total } = res.data;
        this.list = {
          data: data,
          total: total,
          loading: false
        };
      }).catch(() => {
        this.$Loading.error();
        this.list.loading = false;
      });
    },
    /**
     * 编辑界面
     */
    handleEdit() {
      this.$refs['edit'].handleModal('Edit', this.toolbar.selectRow[0])
    },
    /**
    * 设置启用状态
    */
    EditEnable(id, enable) {
      this.$Loading.start();
      editMobileTypeEnable({
        id, enable
      }).then(res => {
        let { data, msg, code } = res;
        if (code === 0) {
          this.$Message.success('OK');
        } else {
          this.$Loading.error();
        }
      })
      this.$Loading.finish();
    },
    // 新增界面
    handleCreate() {
      this.$refs['edit'].handleModal('Create') // 显示创建模态框
    },
    handleChannel() {
      this.$refs["channel"].modal.visible = true;
    },
    handleBrand() {
      this.$refs["brand"].modal.visible = true;
    }
  }
};
</script>