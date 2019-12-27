<template>
  <div id="user-edit">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide>
      <IForm :key="modal.visible" :model="model" :elem="userElem" :rules="userRule" :loading="loading.form" :btn-loading="loading.btn" :width="600" :label-width="80" @on-submit="handleSubmit" @on-click="modal.visible = false" button button-text="取消" />
      <!-- IForm -->
    </Modal>
  </div>
</template>
<script>
import { editRole } from "@/services/manage/role";
export default {
  name: "RoleEdit",
  props: {
    model: Object
  },
  data() {
    // 自定义验证(年龄)
    const validAge = (age, value, callback) => {
      if (!age) {
        callback(new Error("Age cannot be empty"));
      }
      // 模拟异步验证
      setTimeout(() => {
        if (!Number.isInteger(value)) {
          callback(new Error("Please enter a numeric value"));
        } else {
          if (value < 18) {
            callback(new Error("Must be over 18 years of age"));
          } else {
            callback();
          }
        }
      }, 500);
    };
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
      userElem: [
        { label: "角色名", prop: "name", placeholder: "Enter your name" },
        { label: "排序", prop: "orderSort", placeholder: "Enter your orderSort", number: true },
        { label: "描述", prop: "description", placeholder: "Enter your description", type: "textarea" },
        { label: "状态", prop: "enabled", element: "radio", option: [{ label: "正常", value: 1 }, { label: "锁定", value: 2 }] }
      ],
      // 表单验证(用户)
      userRule: {
        name: [
          {
            required: true,
            message: "The name cannot be empty",
            trigger: "blur"
          }
        ],
        orderSort: [
          {
            required: true,
            message: "orderSort cannot be empty",
            trigger: "blur",
            type: "number"
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
      editRole(payload)
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
<style lang="postcss" scoped>
</style>
