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
      </div>
      <!-- .toolbar -->
      <ITable ref="list" stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" @on-selection-change="handleSelectionChange" />
      <!-- ITable -->
      <NewsUsersEdit ref="edit" :model="edit" />
      <!-- NewsUsersEdit -->
      <IUpload ref="upload" :upurl="upurl" />
    </Content>

  </div>
</template>
<script>
import { batchDelNewsConferenceUser, getNewsConferenceUserPageList, getDuplicateList, exportNewsConferenceUser, getMediaTypeList } from '@/services/conference/conference';
import NewsUsersEdit from './NewsConferenceUsersEdit';
import IUpload from '@/views/components/Upload'
import config from '@/config'
export default {  name: 'NewsUsers', components: { NewsUsersEdit, IUpload },
  data() {
    return {
      // 工具条(批量删除)
      toolbar: {
        ids: [], // ID数组
        number: 0, // 选择数量
        selectRow: {},
      },
      // 表单数据(用户)
      edit: {
        gender: 1,
        mediaCity: '东莞',
      },
      upurl: config.baseURL + '/conference/uploadNewsConferenceUser?duplicateid=',
      // 初始表单数据(用户)
      init: '',
      dulist: [],
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
        { prop: 'duplicateid', placeholder: '活动列表', element: 'select', option: [], width: 120 },
        { prop: 'mediaType', placeholder: '媒体类别', element: 'select', option: [], width: 120, clearable: true },
        { prop: 'mediaName', placeholder: '媒体名称', width: 100 },
        { prop: 'mediaHandover', placeholder: '媒体交接人', width: 100 },
        { prop: 'mobile', placeholder: '手机号', width: 110 },
        { prop: 'address', placeholder: '地址' },
        { prop: 'email', placeholder: '邮箱' },
        { prop: 'testBarcode', placeholder: '评测机串码' },
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
        { title: '备注', key: 'remarks', width: 150 },
        { title: '评测机串码', key: 'testBarcode', width: 100 },
      ]
    };
  },
  mounted() {
    this.handleDuplicateList();
    this.init = Object.assign({}, this.edit); // 复制初始表单数据
    this.handleDataList(); // 获取列表数据
    this.handleOtherList()
  },
  methods: {
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
    handleOtherList() {
      getMediaTypeList().then(res => {
        this.searchElem.filter(n => n.prop === 'mediaType')[0].option = res.data;
      })
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
          batchDelNewsConferenceUser({
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
    handleDuplicateList() {
      getDuplicateList().then(res => {
        this.searchElem.filter(n => n.prop === 'duplicateid')[0].option = res.data;
        if (res.data.length > 0) {
          this.search.duplicateid = res.data[0].value;
        }
        this.dulist = res.data;
      })
    },
    /**
     * 获取用户列表
     * @param  {string} type 是否使用搜索, 默认值 undefined
     */
    handleDataList(type) {
      if (type === 'reset') {
        this.handleDuplicateList()
        this.handleOtherList()
      }
      this.$refs['list'].selectAll(false); // 取消全选
      this.$Loading.start();
      this.list.loading = true; // 列表加载状态
      const page = this.$refs['list'].handlePage(type); // 获取分页信息
      // (搜索)
      setTimeout(() => {
        getNewsConferenceUserPageList({
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
    // 新增界面
    handleCreate() {
      this.$refs['edit'].handleModal(); // 显示模态框
      const duplicateid = this.search.duplicateid;
      const duplicatename = this.dulist.filter(n => n.value === duplicateid)[0].label;
      this.edit = Object.assign({ duplicateid, duplicatename }, this.init);
    },
    handleImport() {
      this.upurl += this.search.duplicateid + "&duplicatename=" + this.dulist.filter(n => n.value === this.search.duplicateid)[0].label;
      this.$refs['upload'].handleModal(); // 显示模态框
    },
    handleExport() {
      const page = this.$refs['list'].handlePage(); // 获取分页信息
      const duplicateid = this.search.duplicateid;
      exportNewsConferenceUser({
        ...this.search,
        duplicateid,
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
