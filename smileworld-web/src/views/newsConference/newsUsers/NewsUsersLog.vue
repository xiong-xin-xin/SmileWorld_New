<template>
  <div id="newsUsersLog">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide width="1000">
      <ITable ref="list" stripe :columns="columns" :loading="list.loading" :data="list.data" :total="list.total" @on-page-change="handleDataList" />
    </Modal>
  </div>
</template>

<script>
import { getNewsUserLogList } from "@/services/conference/conference";
export default {
  name: "NewsUsersLog",
  data() {
    return {
      // 模态框属性
      modal: {
        visible: false,
        title: ""
      },
      list: {
        data: [], // 结构化数据
        total: 0, // 数据总数
        loading: false // 加载状态
      },
      columns: [
        { type: 'index', width: 40, align: 'center', fixed: 'left' },
        { title: '媒体类别', key: 'mediaType', width: 100 },
        { title: '媒体级别', key: 'mediaLevel', width: 100 },
        { title: '媒体地区', key: 'mediaCity', width: 100 },
        { title: 'vivo交接人', key: 'vivoHandover', width: 100 },
        { title: '媒体名称', key: 'mediaName', width: 150 },
        { title: '媒体交接人', key: 'mediaHandover', width: 100 },
        { title: '性别', key: 'gender', render: (h, params) => h("span", params.row.gender === 1 ? "男" : "女"), width: 60 },
        { title: '生日', key: 'birth', width: 120 },
        { title: '电话', key: 'mobile', width: 120 },
        { title: '微信', key: 'weChat', width: 100 },
        { title: '邮箱', key: 'email', width: 170 },
        { title: '地址', key: 'address', width: 200 },
        { title: '备注', key: 'remarks', width: 150 },
        { title: '时间', key: 'updatedDate', width: 160, sortable: true, fixed: 'right' },
      ]
    }
  },
  methods: {
    handleModal(val) {
      this.modal = {
        title: '日志',
        visible: true
      };
      this.dbclickid = val;
      this.handleDataList();
    },
    handleDataList(type) {
      this.$Loading.start();
      this.list.loading = true; // 列表加载状态
      const page = this.$refs["list"].handlePage(type); // 获取分页信息
      getNewsUserLogList({
        id: this.dbclickid,
        pagination: {
          pageIndex: page.current,
          pageSize: page.pageSize
        }
      })
        .then(res => {
          this.$Loading.finish();
          const { data } = res;
          this.list = {
            data: data,
            total: data.length,
            loading: false
          };
        })
        .catch(() => {
          this.$Loading.error();
          this.list.loading = false;
        });

    }
  }
}
</script>
  


