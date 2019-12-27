<template>
  <div id="mobileSale">
    <Content>
      <div slot="search">
        <Search :model="search" :elem="searchElem" :btn-loading="list.loading" @on-search="handleDataList" />
      </div>
      <!-- Search -->
      <div slot="extra" class="toolbar">
        <span class="number" v-show="toolbar.number!=0">已选择 {{ toolbar.number }} 数据 </span>
        <Button @click="handleImport" icon="md-document" v-show=false>导入</Button>
        <Button @click="handleExport" icon="md-document" v-show=false>导出</Button>
        <Button type="error" @click="handleBatchDelete()" v-show=false :disabled="!toolbar.number" icon="md-trash">删除</Button>
        <Button @click="handleCreate" v-show=false icon="md-document">新增</Button>
        <Button @click="handleAlter" :disabled="toolbar.number!==1" v-show=false icon="md-document">修改</Button>
      </div>
      <!-- .toolbar -->
      <ITable ref="list" border stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" @on-selection-change="handleSelectionChange" />
      <!-- ITable -->
      <MobileSaleEdit ref="edit" @on-update="handleDataList" />
      <MobileSaleAlter ref="alter" @on-update="handleDataList" />
      <IUpload ref="upload" :upurl="upurl" />
    </Content>
  </div>
</template>
<script>
import { getMobileSalePageList, delMobileSale, editMobileSale, exportMobileSale } from "@/services/distributor/mobileSale";
import IUpload from '@/views/components/Upload'
import config from '@/config'
import MobileSaleEdit from "./mobileSaleEdit"
import MobileSaleAlter from "./mobileSaleAlter"
export default {
  name: "MobileSale",
  components: { IUpload, MobileSaleEdit, MobileSaleAlter },
  data() {
    return {
      upurl: config.baseURL + '/mobileSale/uploadMobileSale',
      // 工具条(批量删除)
      toolbar: {
        ids: [], // ID数组
        number: 0, // 选择数量
        selectRow: {}
      },
      // 表单数据(搜索)
      search: {
        dates: [],
        channelId: ""
      },
      // 列表属性
      list: {
        data: [], // 结构化数据
        total: 0, // 数据总数
        loading: false // 加载状态
      },
      // 表单元素(搜索)
      searchElem: [
        { prop: 'channelName', placeholder: '渠道名称', width: 120 },
        { prop: "mobileTypeName", placeholder: "机型名称", minWidth: 120 },
        { prop: "dates", element: 'date', format: "yyyy-MM-dd", type: "daterange", placeholder: "Select date", elemWidth: "width:200px" }
      ],
      // 表格列的配置描述(用户)
      constcolumns: [
        { type: "selection", width: 60, align: "center", fixed: 'left' },
        { type: 'index', width: 70, align: 'center', fixed: 'left' },
        { title: '时间', key: 'uploadtime', width: 110, align: 'center', fixed: 'left' },
        { title: '渠道', key: 'channelname', width: 100, align: 'center', fixed: 'left' }
      ],
      columns: []
    };
  },
  mounted() {
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
    handleBatchDelete() {
      let that = this;
      this.$Modal.confirm({
        title: "提示",
        content: '此操作为不可逆操作，是否确认删除？',
        width: 400,
        loading: true,
        onOk() {
          that.$Loading.start();
          delMobileSale({
            'ids': that.toolbar.ids.join(',')
          }).then(res => {
            let { data, msg, code } = res;
            if (code === 0) {
              that.$Message.success('OK');
              that.handleDataList();
            } else {
              that.$Loading.error();
            }
          })
          this.remove();
        }
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

      let start_time = this.search.dates[0];
      let end_time = this.search.dates[1];

      getMobileSalePageList({
        ...this.search,
        start_time,
        end_time,
        pagination: {
          pageIndex: page.current,
          pageSize: page.pageSize
        }
      }).then(res => {
        this.$Loading.finish();
        const { data, total, columns } = res.data;
        this.columns = Object.assign([], this.constcolumns);
        columns.forEach(v => {
          this.columns.push({
            title: v, key: v, minWidth: 100, align: "center",
            render: (h, params) => {
              return h('div', {
                style: { 'line-height': '30px', 'min-height': '30px', 'min-width': '80px' },
                on: {
                  'dblclick': () => {
                    if (this.$store.state.app.userInfo.auths['MobileSale'].indexOf("修改") != -1) {
                      this.handleEdit(params, v);
                    }
                  }
                }
              }, params.row[v.toLowerCase()])
            }
          });
        });
        this.list = { data: data, total: total, loading: false };
      }).catch(() => {
        this.$Loading.error();
        this.list.loading = false;
      });
    },
    /**
     * 修改销量
     */
    handleEdit(params, colname) {
      let sales = Number(params.row[colname.toLowerCase()]);
      this.$Modal.confirm({
        title: "修改销量",
        closable: true,
        render: (h) => {
          return h('Input', {
            props: {
              value: sales,
              type: 'number',
              autofocus: true,
              placeholder: '请输入销量'
            },
            on: {
              input: (val) => { sales = val; }
            }
          })
        }, onOk: () => {
          this.$Loading.start();
          editMobileSale({
            channelId: params.row.channelid,
            mobiletypename: colname,
            ReportDate: params.row.reportdate,
            sales: sales
          }).then(res => {
            let { code, msg } = res;
            if (code == -1) {
              this.$Message.error(msg);
            } else {
              this.$set(this.list.data[params.row._index], colname.toLowerCase(), sales);
              this.$Message.success("修改成功");
            }
          })
          this.$Loading.finish();
        }
      })
    },
    handleExport() {
      let start_time = this.search.dates[0];
      let end_time = this.search.dates[1];

      exportMobileSale({
        ...this.search, start_time, end_time
      }
      ).then(res => {
        let blob = new Blob([res], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        let objUrl = URL.createObjectURL(blob);
        window.location.href = objUrl;
      })
    },
    // 新增界面
    handleCreate() {
      this.$refs['edit'].handleModal('Create') // 显示创建模态框
    },
    handleImport() {
      this.$refs['upload'].handleModal(); // 显示模态框
    },
    //修改
    handleAlter() {
      this.$refs['alter'].handleModal('Alter', this.toolbar.selectRow[0])
    }
  }
};
</script>