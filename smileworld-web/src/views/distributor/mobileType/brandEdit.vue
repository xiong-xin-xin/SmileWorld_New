<template>
  <div id="brand-edit">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide :styles="{top: '150px'}">
      <IForm :key="modal.visible" :model="model" :elem="formElem" :rules="formRule" :loading="loading.form" :btn-loading="loading.btn" :width="600" :label-width="80" @on-submit="handleSubmit" @on-click="modal.visible = false" button button-text="取消" />
      <!-- IForm -->
    </Modal>
  </div>
</template>
<script>
import { editBrand, getChannelList } from "@/services/distributor/mobileType";
export default {
  name: "BrandEdit",
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
        { label: "渠道", prop: "channelId", element: "select", option: [], multiple: true },
        { label: "品牌名", prop: "name", placeholder: "Enter your name" }
      ],
      // 表单验证(用户)
      formRule: {
        name: [
          {
            required: true,
            message: "请输入品牌名称",
            trigger: "blur"
          }
        ],
        channelId: [
          {
            type: 'array',
            required: true,
            message: "请选择渠道",
            trigger: "change"
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
    }, // 获取补丁数据
    handlePatch() {
      const _false = false;
      const data = this.formElem.filter(n => n.option && n.option.length === 0);
      let total = data.length;
      if (total === 0) {
        this.loading.form = _false; // 表单加载状态
        return _false;
      }
      for (const item of data) {
        if (item["prop"] === "channelId") {
          getChannelList().then(res => {
            item["option"] = res.data;
            total--;
            this.loading.form = total === 0 ? _false : true; // 补丁完成状态
          })
        }
      }
    },
    // 表单提交
    handleSubmit() {
      this.$Loading.start();
      this.loading.btn = true;
      const payload = this.model;
      payload['channelIds'] = payload['channelId'];
      editBrand(payload)
        .then(res => {
          this.$Message.success(res.msg);
          this.$emit("on-update", false);
          this.$Loading.finish();
          this.loading.btn = false;
          this.modal.visible = false;
        }).catch(() => {
          this.$Loading.error();
          this.loading.btn = false;
          this.modal.visible = false;
        });
    }
  }
};
</script>
