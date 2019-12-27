<template>
  <div id="user-edit">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide>
      <IForm :key="modal.visible" :model="model" :elem="userElem" :rules="userRule" :loading="loading.form" :btn-loading="loading.btn" :width="600" :label-width="80" @on-submit="handleSubmit" @on-click="modal.visible = false" button button-text="取消" />
      <!-- IForm -->
    </Modal>
  </div>
</template>
<script>
import { editUser, getRoleList } from "@/services/manage/users";
export default {
  name: "UserEdit",
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
        { label: "登录名", prop: "loginName", placeholder: "Enter your name" },
        { label: "用户名", prop: "realName", placeholder: "Enter your realName" },
        { label: "年龄", prop: "age", placeholder: "Enter your age", number: true },
        { label: "性别", prop: "gender", element: "radio", option: [{ label: "男", value: 1 }, { label: "女", value: 2 }] },
        { label: "状态", prop: "status", element: "radio", option: [{ label: "正常", value: 1 }, { label: "锁定", value: 2 }] },
        { label: "角色", prop: "roles", element: "checkbox", option: [] },
        { label: "备注", prop: "remark", placeholder: "Enter something...", type: "textarea" }
      ],
      // 表单验证(用户)
      userRule: {
        loginName: [
          {
            required: true,
            message: "The name cannot be empty",
            trigger: "blur"
          }
        ],
        age: [
          {
            required: true,
            validator: validAge,
            trigger: "blur"
          }
        ],
        realName: [
          {
            required: true,
            message: "Mailbox cannot be empty",
            trigger: "blur"
          }
        ],
        gender: [
          {
            required: true,
            message: "Please select gender",
            trigger: "change",
            type: "number"
          }
        ],
        roles: [
          {
            required: true,
            message: "Choose at least one role",
            trigger: "change",
            type: "array",
            min: 1
          }
        ],
        remark: [
          {
            required: true,
            message: "Please enter a personal introduction",
            trigger: "blur"
          },
          {
            message: "Introduce no less than 3 words",
            trigger: "blur",
            type: "string",
            min: 3
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
      this.loading.form = true; // 表单加载状态
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
    // 获取补丁数据
    handlePatch() {
      const _false = false;
      const data = this.userElem.filter(n => n.option && n.option.length === 0);
      let total = data.length;
      if (total === 0) {
        this.loading.form = _false; // 表单加载状态
        return _false;
      }
      for (const item of data) {
        if (item["prop"] === "roles") {
          getRoleList().then(res => {
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
      editUser(payload)
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
