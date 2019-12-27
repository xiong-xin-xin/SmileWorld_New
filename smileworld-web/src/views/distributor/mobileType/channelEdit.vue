<template>
  <div id="channel-edit">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide :styles="{top: '150px'}">
      <IForm :key="modal.visible" :model="model" :elem="formElem" :rules="formRule" :loading="loading.form" :btn-loading="loading.btn" :width="600" :label-width="80" @on-submit="handleSubmit" @on-click="modal.visible = false" button button-text="取消" />
      <!-- IForm -->
    </Modal>
  </div>
</template>
<script>
import { editChannel } from "@/services/distributor/mobileType";
export default {
  name: "ChannelEdit",
  props: {
    model: Object
  },
  data() {
    return {
      // 模态框属性
      modal: {
        visible: false,
        title: ""
      },
      // 加载状态
      loading: {
        form: false,
        btn: false
      },
      // 表单元素(用户)
      formElem: [
        { label: "渠道名称", prop: "name", placeholder: "Enter your name" }
      ],
      // 表单验证(用户)
      formRule: {
        name: [
          {
            required: true,
            message: "The name cannot be empty",
            trigger: "blur"
          }
        ]
      },
    };
  },
  methods: {
    /**
     * 显示模态框
     * @param  {string} key 模态框标题名称
     */
    handleModal(key) {
      key = key || 0;
      const title = {
        0: "Create",
        1: "Edit"
      };
      this.modal = {
        title: title[key],
        visible: true
      };
    },
    // 表单提交
    handleSubmit() {
      this.$Loading.start();
      this.loading.btn = true;
      const payload = this.model;
      editChannel(payload)
        .then(res => {
          this.$Message.success(res.msg);
          this.$emit("on-update", false);
          this.$Loading.finish();
          this.loading.btn = false;
          this.modal.visible = false;
        })
        .catch(() => {
          this.$Loading.error();
          this.loading.btn = false;
          this.modal.visible = false;
        });
    }
  }
};
</script>
