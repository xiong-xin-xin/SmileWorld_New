<template>
  <div id="i-table">
    <Table ref="table" :border="border" :stripe="stripe" :row-class-name="rowClassName" :loading="loading" :columns="columns" :data="data" @on-selection-change="handleSelectionChange" @on-row-dblclick="handleRowDblclick" :height="tableHeight" highlight-row @on-row-click="handlerClicked" size="small" />
    <!-- Table -->
    <slot> </slot>
    <Page v-if="total > 10" id="page" show-elevator show-total show-sizer placement="top" :total="total" :current="current" :page-size="pageSize" @on-change="handlePageChange" @on-page-size-change="handlePageSizeChange" :page-size-opts="[10, 30, 50,100]" />
    <!-- Page -->
  </div>
</template>
<script>
export default {
  name: 'ITable',
  props: {
    border: Boolean,
    stripe: Boolean,
    rowClassName: Function,
    loading: Boolean,
    columns: Array,
    data: Array,
    total: Number,
    theight: {
      type: Number,
      default: 0
    }
  },
  data: () => ({
    tableHeight: 450,
    current: 1,
    pageSize: 10
  }),
  methods: {
    // 将数据导出为 .csv 文件
    exportCsv(obj) {
      this.$refs['table'].exportCsv(obj)
    },
    // 在多选模式下有效, 点击全选时触发
    selectAll(status) {
      this.$refs['table'].selectAll(status)
    },
    // 在多选模式下有效, 只要选中项发生变化时就会触发
    handleSelectionChange(selection) {
      this.$emit('on-selection-change', selection)
    },
    handleRowDblclick(row, index) {
      this.$emit('on-row-dblclick', row, index)
    },
    // 获取当前分页
    handlePage(type) {
      this.current = type ? 1 : this.current
      return {
        current: this.current,
        pageSize: this.pageSize
      }
    },
    // 改变页码
    handlePageChange(page) {
      this.current = page
      this.$emit('on-page-change')
    },
    // 切换每页条数
    handlePageSizeChange(page) {
      this.pageSize = page
      if (this.current === 1) {
        this.$emit('on-page-change')
      }
    },
    handlerClicked(data, index) {
      this.$refs.table.selectAll(false);
      this.$refs.table.toggleSelect(index);
    }
  },
  mounted() {
    // 设置表格高度
    this.tableHeight = window.innerHeight - this.$refs.table.$el.offsetTop - 218 - this.theight
  },
  created() {
    window.addEventListener('resize', () => { this.tableHeight = window.innerHeight - this.$refs.table.$el.offsetTop - 250 - this.theight });
  }
}
</script>
<style lang="postcss" scoped>
#page {
  margin-top: 15px;
}
</style>
