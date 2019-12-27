<template>
  <div id="channel">
    <Modal v-model="modal.visible" :title="modal.title" :width="800" footer-hide>
      <div class="search-bar">
        <div slot="search">
          <Search :model="search" :elem="searchElem" :btn-loading="list.loading" @on-search="handleDataList" />
        </div>
        <!-- Search -->
        <div slot="extra" class="toolbar">
          <span class="number" v-show="toolbar.number!=0">已选择 {{ toolbar.number }} 数据 </span>
          <Button type="error" @click="handleBatchDelete" :disabled="!toolbar.number" icon="md-trash">删除</Button>
          <Button @click="handleEdit" :disabled="toolbar.number!==1" icon="md-document">修改</Button>
          <Button @click="handleCreate" icon="md-document">新增</Button>
        </div>
      </div>
      <!-- .toolbar -->
      <ITable ref="list" stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" @on-selection-change="handleSelectionChange" :theight="100" />
      <!-- ITable -->
      <ChannelEdit ref="edit" :model="edit" @on-update="handleDataList" />
    </Modal>
  </div>
</template>
<script>
import { getChannelPageList, delChannel } from "@/services/distributor/mobileType";
import ChannelEdit from "./channelEdit";
export default {
  name: "Channel",
  components: { ChannelEdit },
  data() {
    return {
      // 模态框属性
      modal: {
        visible: false,
        title: "渠道管理"
      },
      // 工具条(批量删除)
      toolbar: {
        ids: [], // ID数组
        number: 0, // 选择数量
        selectRow: {}
      },
      // 表单数据(用户)
      edit: {},
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
      // 表格列的配置描述
      columns: [
        { type: "selection", width: 60, align: "center" },
        { type: 'index', width: 70, align: 'center', fixed: 'left' },
        { title: "渠道名称", key: "name", minWidth: 120 }
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
        ids.push(item['id']);
      }
      this.toolbar = {
        ids: ids,
        number: selection.length,
        selectRow: selection
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
          delChannel({
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
     * 获取列表
     * @param  {string} type 是否使用搜索, 默认值 undefined
     */
    handleDataList(type) {
      this.$refs["list"].selectAll(false); // 取消全选
      this.$Loading.start();
      this.list.loading = true; // 列表加载状态
      const page = this.$refs["list"].handlePage(type); // 获取分页信息
      getChannelPageList({
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
     */
    handleEdit() {
      this.$refs['edit'].handleModal(1); // 显示模态框
      const edit = Object.assign({}, this.toolbar.selectRow[0]);
      this.edit = {
        ...edit
      };
    },
    // 新增界面
    handleCreate() {
      this.$refs["edit"].handleModal(); // 显示模态框
      this.edit = Object.assign({}, this.init);
    }
  }
};
</script>
