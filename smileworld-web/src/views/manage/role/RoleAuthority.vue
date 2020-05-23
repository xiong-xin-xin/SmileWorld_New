<template>
  <div>
    <Modal v-model="modal.visible" title="权限设置" footer-hide>
      <Form :loading="loading.form" :btn-loading="loading.btn" :width="600" :label-width="70">
        <FormItem label="权限集：">
          <Tree ref="tree" :data="formItem.permissions" show-checkbox check-strictly check-directly multiple></Tree>

          <Button type="default" :disabled="loading.btn" @click="modal.visible = false">取消</Button>
          <Button type="primary" :loading="loading.btn" @click="handleSubmit" style="margin-left: 8px">确定</Button>
        </FormItem>
      </Form>
    </Modal>
  </div>
</template>
<script>
import { getRoleAuthList, saveRoleAuth } from "@/services/manage/role";
export default {
  data() {
    return {
      modal: {
        visible: false,
        title: ""
      },
      // 加载状态
      loading: {
        form: false,
        btn: false
      },
      formItem: {
        name: "",
        permissions: []
      }
    };
  },
  props: {
    id: String
  },
  methods: {
    handleModal() {
      this.modal = {
        title: "Auth",
        visible: true
      };
    },
    handleData() {
      getRoleAuthList({ roleId: this.id }).then(res => {
        this.formItem.permissions = res.data;
      });
    },
    handleSubmit() {
      this.$Loading.start();
      this.loading.btn = true;
      let authList = this.$refs.tree.getCheckedNodes();
      var data = { roleId: this.id, authLists: [] };
      authList.forEach(item => {
        data.authLists.push({ ItemId: item.id, itemType: item.isMenu });
      });
      saveRoleAuth(data)
        .then(res => {
          this.$Message.success(res.msg);
          this.$Loading.finish();
        })
        .catch(() => {
          this.$Loading.error();
        });
      this.loading.btn = false;
      this.modal.visible = false;
    }
  }
};
</script>

