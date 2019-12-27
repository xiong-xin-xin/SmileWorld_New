<template>
  <div id="user-edit">
    <Modal v-model="modal.visible" :title="modal.title" @on-ok="handleSubmit" :loading="loading.form">
      <div id="i-form">
        <Form ref="form" :model="edit" :rules="formRule" @keyup.enter.native="handleSubmit">

          <FormItem label="所属渠道" prop="channelId">
            <Select v-model="edit['channelId']" filterable :transfer="true" @on-change="selectChange" multiple>
              <Option v-for=" (opt, index) in formData.channels" :key="index" :value="opt.value">{{ opt.label }}</Option>
            </Select>
          </FormItem>
          <FormItem label="所属品牌" prop="brandId">
            <Select v-model="edit['brandId']" filterable :transfer="true">
              <Option v-for=" (opt, index) in formData.brands" :key="index" :value="opt.value">{{ opt.label }}</Option>
            </Select>
          </FormItem>
          <FormItem label="机 型" prop="name">
            <Input type="textarea" :rows="2" v-model="edit['name']" placeholder="机 型(批量请使用逗号隔开)"></Input>
          </FormItem>
        </Form>
      </div>
    </Modal>
  </div>
</template>
<script>
import { editMobileType, getBrandList, getChannelList } from "@/services/distributor/mobileType";
export default {
  name: "MobileTypeEdit",
  data() {
    return {
      // 模态框属性
      modal: { visible: false, title: "" },
      // 加载状态
      loading: { form: true },
      formData: { channels: [], brands: [] },
      // 表单验证(用户)
      formRule: {
        channelId: [{ type: 'array', required: true, message: "请选择渠道" }],
        brandId: [{ required: true, message: "请选择品牌" }],
        name: [{ required: true, message: "请输入机型名称", trigger: "blur" }]
      },
      edit: {},
      init: ''
    };
  },
  mounted() {
    this.init = Object.assign({}, this.edit) // 复制初始表单数据
  },
  methods: {
    handleModal(title, row) {
      this.modal = {
        title: title,
        visible: true
      }
      //this.loading.form = true // 表单加载状态
      if (title === 'Create') {
        this.handleCreate() // 创建
      } else {
        row['name'] = row.mobileTypeName;
        this.handleEdit(row) // 编辑
      }
    },
    // 编辑
    handleEdit(row) {
      var self = this;
      const edit = Object.assign({}, row)
      this.edit = {
        ...edit
      }
      this.handlePatch(this.edit.channelId) // 获取补丁数据
      setTimeout(function () {
        self.edit['brandId'] = row['brandId'];
      }, 400);
    },
    // 创建
    handleCreate() {
      this.edit = Object.assign({}, this.init)
      this.handlePatch() // 获取补丁数据
    },
    // 获取补丁数据
    handlePatch(n) {
      getChannelList().then(res => {
        this.formData.channels = res.data;
        if (this.modal.title == 'Create') {
          this.edit['channelId'] = res.data.map(t => t.value);
        }
      })
    },
    selectChange(n) {
      getBrandList({ channelIds: n }).then(res => {
        this.formData.brands = res.data;
      })
    },
    // 表单提交
    handleSubmit() {
      this.loading.form = false;
      this.$refs['form'].validate(valid => {
        if (valid) {
          this.$Loading.start();
          const payload = this.edit;
          editMobileType(payload)
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
