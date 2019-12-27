<template>
  <div id="user-edit">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide :width="550">
      <IForm :key="modal.visible" :model="model" :elem="userElem" :rules="userRule" :loading="loading.form" :btn-loading="loading.btn" :width="600" :inline="true" :label-width="80" @on-submit="handleSubmit" @on-click="modal.visible = false" button button-text="取消" />
      <!-- IForm -->
    </Modal>
  </div>
</template>
<script>
import { editNewsConferenceUser } from "@/services/conference/conference";
export default {
  name: "UserEdit",
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
      userElem: [
        {
          label: "媒体类别",
          prop: "mediaType",
          placeholder: "mediaType",
          elemWidth: 150
        },
        {
          label: "媒体级别",
          prop: "mediaLevel",
          placeholder: "mediaLevel",
          number: true,
          elemWidth: 150
        },
        {
          label: "媒体名称",
          prop: "mediaName",
          placeholder: "mediaName",
          elemWidth: 150
        },
        {
          label: "媒体地区",
          prop: "mediaCity",
          placeholder: "Select your city",
          elemWidth: 150
        },
        {
          label: "媒体交接人",
          prop: "mediaHandover",
          placeholder: "mediaHandover",
          elemWidth: 150
        },
        {
          label: "vivo交接人",
          prop: "vivoHandover",
          placeholder: "Enter your vivoHandover",
          elemWidth: 150
        },
        {
          label: "性别",
          prop: "gender",
          element: "radio",
          option: [
            {
              label: "男",
              value: 1
            },
            {
              label: "女",
              value: 2
            }
          ],
          width: 227
        },
        {
          label: "生日",
          prop: "birth",
          placeholder: "Select your birth",
          element: "date",
          startdate: new Date(1991, 1, 1),
          elemWidth: 150
        },
        {
          label: "电话",
          prop: "mobile",
          placeholder: "mobile",
          elemWidth: 150
        },
        {
          label: "微信",
          prop: "weChat",
          placeholder: "Enter your weChat",
          elemWidth: 150
        },
        {
          label: '邮箱',
          prop: 'email',
          placeholder: "Enter your e-mail",
          elemWidth: 150
        },
        {
          label: '评测机串码',
          prop: 'testBarcode',
          placeholder: "Enter your testBarcode",
          elemWidth: 150
        },
        {
          label: '地址',
          prop: 'address',
          placeholder: "Enter your address",
          elemWidth: 385
        },
        {
          label: "备注",
          prop: "remarks",
          placeholder: "Enter something...",
          type: "textarea",
          elemWidth: 385
        }
      ],
      // 表单验证
      userRule: {
        mediaName: [
          {
            required: true,
            message: "请输入媒体名称",
            trigger: "blur"
          }
        ],
        mediaHandover: [
          {
            required: true,
            message: "请输入媒体交接人",
            trigger: "blur"
          }
        ]
      }
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
        0: "新增",
        1: "修改"
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
      payload.birth = payload.birth
        ? this.$Utils.formatDate.format(new Date(payload.birth), "yyyy-MM-dd")
        : "";
      // (编辑 Or 新增)
      editNewsConferenceUser(payload)
        .then(res => {
          let { data, msg, code } = res;
          if (code === 0) {
            this.$Message.success("ok");
            this.$emit("on-update", false);
            this.$Loading.finish();
            this.modal.visible = false;
          }
          else {
            this.$Message.error(msg);
            this.$Loading.error();
          }
        })
      this.loading.btn = false;
    }
  }
};
</script>
<style lang="postcss" scoped>
</style>
