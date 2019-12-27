<template>
  <div id="mobileSaleAlter">
    <Modal v-model="modal.visible" :title="modal.title" @on-ok="handleSubmit" :loading="loading.form" :width="700">
      <div id="i-form">
        <Form ref="form" :model="edit" inline :rules="formRule" @keyup.enter.native="handleSubmit" :label-width="100">
          <FormItem label="上报渠道" prop="channelId" :style="{width: '300px'}">
            <Select v-model="edit['channelId']" filterable :transfer="true" disabled>
              <Option v-for=" (opt, index) in formData.channels" :key="index" :value="opt.value">{{ opt.label }}</Option>
            </Select>
          </FormItem>
          <FormItem label="上报时间" prop="reportdate" :style="{width: '300px'}">
            <DatePicker type="date" :transfer="true" placeholder="Select date" v-model="edit['reportdate']" disabled></DatePicker>
          </FormItem>
          <FormItem v-for="(item, index) in formData.mobileTypes" :key="index" :prop="item.id" :label="item.mobiletypename">
            <Input type="number" v-model="edit[item.id]" placeholder="销量" :style="{width: '200px'}"></Input>
          </FormItem>
        </Form>
      </div>
    </Modal>
  </div>
</template>
<script>
import { getChannelList } from "@/services/distributor/mobileType";
import { alterMobileSale, getMobileSale } from "@/services/distributor/mobileSale";
export default {
  name: "MobileSaleAlter",
  data() {
    return {
      // 模态框属性
      modal: { visible: false, title: "" },
      // 加载状态
      loading: { form: true },
      formData: { channels: [], mobileTypes: [] },
      // 表单验证(用户)
      formRule: {},
      edit: {},
      reportdate: ''
    };
  },
  methods: {
    handleModal(title, row) {
      this.modal = {
        title: title,
        visible: true
      }
      if (title === 'Alter') {
        this.handleEdit(row) // 修改
      }
    },
    // 创建
    handleEdit(row) {
      const edit = Object.assign({}, row)
      this.edit = { reportdate: edit.reportdate, channelId: edit.channelid }
      this.handlePatch() // 获取补丁数据
    },
    // 获取补丁数据
    handlePatch(n) {
      getChannelList().then(res => {
        this.formData.channels = res.data;
      })
      getMobileSale({ channelId: this.edit['channelId'], reportdate: this.edit['reportdate'] }).then(res => {
        this.formData.mobileTypes = res.data;
        this.formData.mobileTypes.forEach(t => this.edit[t.id] = t.sales);
      })
    },
    // 表单提交
    handleSubmit() {
      this.loading.form = false;
      this.$refs['form'].validate(valid => {
        if (valid) {
          this.$Loading.start();
          const payload = { ...this.edit };
          alterMobileSale({ data: JSON.stringify(payload) })
            .then(res => {
              this.$Message.success(res.msg);
              this.$emit("on-update", false);
              this.$Loading.finish();
              this.modal.visible = false;
            }).catch(() => {
              this.$Loading.error();
              this.modal.visible = false;
            });
        }
        this.$nextTick(() => { this.loading.form = true; })
      })
    }
  }
};
</script>
