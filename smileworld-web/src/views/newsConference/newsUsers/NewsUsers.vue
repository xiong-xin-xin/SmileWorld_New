<template>
  <div id="users">
    <Content>
      <div slot="search">
        <Search :model="search" :elem="searchElem" :btn-loading="list.loading" @on-search="handleDataList" />
      </div>
      <!-- Search -->
      <div slot="extra" class="toolbar">
        <span class="number" v-show="toolbar.number!=0">已选择 {{ toolbar.number }} 数据 </span>
        <Button @click="handleCreate" icon="md-document">新 增</Button>
        <Button @click="handleEdit" :disabled="toolbar.number!==1" icon="md-document">修改</Button>
        <Button type="error" @click="handleBatchDelete" :disabled="!toolbar.number" icon="md-trash">删除</Button>
        <Button @click="handleImport" icon="md-document">导 入</Button>
        <Button @click="handleExport" icon="md-document">导 出</Button>
        <Button @click="handleImpDuplicate" :disabled="!toolbar.number" icon="md-document">导入子数据库</Button>
      </div>
      <!-- .toolbar -->
      <ITable ref="list" stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" @on-selection-change="handleSelectionChange" @on-row-dblclick="handleRowDblclick" />
      <!-- ITable -->
      <NewsUsersEdit ref="edit" :model="edit" @on-update="handleDataList" />
      <ImportDuplicate ref="impdup" :ids="toolbar.ids" />
      <!-- NewsUsersEdit -->
      <IUpload ref="upload" :upurl="upurl" />
      <NewsUsersLog ref="userlog" />
    </Content>
  </div>
</template>
<script>
import { batchDelNewsUser, delNewsUser, getNewsUserPageList, exportNewsUser, getMediaTypeList, getMediaLevelList } from '@/services/conference/conference';
import NewsUsersEdit from './NewsUsersEdit';
import IUpload from '@/views/components/Upload'
import ImportDuplicate from './ImportDuplicate';
import NewsUsersLog from './NewsUsersLog';
import config from '@/config'
export default {  name: 'NewsUsers', components: { NewsUsersEdit, ImportDuplicate, IUpload, NewsUsersLog },
  data() {
    return {
      // 工具条(批量删除)
      toolbar: {
        ids: [], // ID数组
        number: 0, // 选择数量
        selectRow: {}
      },
      // 表单数据(用户)
      edit: {
        gender: 1,
        mediaCity: '东莞',
      },
      upurl: config.baseURL + '/conference/uploadNewsUser',
      // 初始表单数据(用户)
      init: '',
      // 表单数据(搜索)
      search: {},
      // 列表属性
      list: {
        data: [], // 结构化数据
        total: 0, // 数据总数
        loading: false // 加载状态
      },
      // 表单元素(搜索)
      searchElem: [
        { prop: 'mediaType', placeholder: '媒体类别', element: 'select', option: [], width: 120, clearable: true },
        { prop: 'mediaName', placeholder: '媒体名称' },
        { prop: 'mediaLevel', placeholder: '媒体级别', element: 'select', option: [], width: 120, clearable: true, filterable: true },
        { prop: 'mediaHandover', placeholder: '媒体交接人' },
        { prop: 'mobile', placeholder: '手机号' },
        { prop: 'address', placeholder: '地址' },
        { prop: 'email', placeholder: '邮箱' },
      ],
      // 表格列的配置描述(用户)
      columns: [
        { type: 'selection', width: 60, align: 'center', fixed: 'left' },
        { type: 'index', width: 70, align: 'center', fixed: 'left' },
        { title: '媒体类别', key: 'mediaType', width: 100 },
        { title: '媒体级别', key: 'mediaLevel', width: 100 },
        { title: '媒体地区', key: 'mediaCity', width: 100 },
        { title: 'vivo交接人', key: 'vivoHandover', width: 100 },
        { title: '媒体名称', key: 'mediaName', width: 150 },
        { title: '媒体交接人', key: 'mediaHandover', width: 100 },
        { title: '性别', key: 'gender', render: (h, params) => h('span', params.row.gender === 1 ? '男' : '女'), width: 60 },
        { title: '生日', key: 'birth', sortable: true, width: 120 },
        { title: '电话', key: 'mobile', width: 120 },
        { title: '微信', key: 'weChat', width: 100 },
        { title: '邮箱', key: 'email', width: 200 },
        { title: '地址', key: 'address', width: 200 },
        { title: '备注', key: 'remarks', width: 180 }
      ]
    };
  },
  mounted() {
    this.init = Object.assign({}, this.edit); // 复制初始表单数据
    this.handleDataList(); // 获取列表数据
    this.handleOtherList();
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
    handleRowDblclick(row, index) {
      this.$refs['userlog'].handleModal(row.id);
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
          batchDelNewsUser({
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
    handleOtherList() {
      getMediaTypeList().then(res => {
        this.searchElem.filter(n => n.prop === 'mediaType')[0].option = res.data;
      })
      getMediaLevelList().then(res => {
        this.searchElem.filter(n => n.prop === 'mediaLevel')[0].option = res.data;
      })
    },
    /**
     * 获取列表
     * @param  {string} type 是否使用搜索, 默认值 undefined
     */
    handleDataList(type) {
      if (type === 'reset') {
        this.handleOtherList()
      }
      this.$refs['list'].selectAll(false); // 取消全选
      this.$Loading.start();
      this.list.loading = true; // 列表加载状态
      const page = this.$refs['list'].handlePage(type); // 获取分页信息
      // 模拟异步请求(搜索)
      setTimeout(() => {
        getNewsUserPageList({
          ...this.search,
          pagination: {
            pageIndex: page.current,
            pageSize: page.pageSize
          }
        }).then(res => {
          this.$Loading.finish();
          //this.columns = res.columns;
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
      }, 500);
    },
    /**
     * 编辑界面
     * @param  {object} 
     */
    handleEdit() {
      // 模拟异步请求(获取详情)
      if (this.toolbar.number > 1 || this.toolbar.number == 0) {
        this.$Message.info('请选择一条数据');
        return;
      }
      this.$refs['edit'].handleModal(1); // 显示模态框
      const edit = Object.assign({}, this.toolbar.selectRow[0]);
      const birth = edit.birth
        ? this.$Utils.formatDate.parse(edit.birth, 'yyyy-MM-dd')
        : '';
      this.edit = {
        ...edit,
        birth
      };
    },
    /* 导入子数据库 */
    handleImpDuplicate() {
      this.$refs['impdup'].handlePatch();
      this.$refs['impdup'].handleModal();
    },
    // 新增界面
    handleCreate() {
      this.$refs['edit'].handleModal(); // 显示模态框
      this.edit = Object.assign({}, this.init);
    },
    handleImport() {
      this.$refs['upload'].handleModal(); // 显示模态框
    },
    handleExport() {
      const page = this.$refs['list'].handlePage(); // 获取分页信息
      exportNewsUser({
        ...this.search,
        pagination: {
          pageIndex: page.current,
          pageSize: page.pageSize
        }
      }).then(res => {
        let blob = new Blob([res], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        let objUrl = URL.createObjectURL(blob);
        window.location.href = objUrl;
      })
    }
  }
};
</script>
