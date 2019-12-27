<template>
  <div id="module-edit">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide>
      <Form ref="form" :rules="rules" :loading="loading.form" :btn-loading="loading.btn" :width="600" :model="model" :label-width="80" @keyup.enter.native="handleSubmit('form')">
        <FormItem hidden label="ID">
          <Input :value="model.id"></Input>
        </FormItem>
        <FormItem v-if="model.isMenu==2 && model.parentName!=null" label="父级名称">
          <Input disabled :value="model.parentName"></Input>
        </FormItem>
        <FormItem label="名称" prop="title">
          <Input v-model.trim="model.title"></Input>
        </FormItem>
        <FormItem label="类型">
          <Select v-model.trim="model.isMenu" style="width:100%">
            <Option v-for="item in [{label:'菜单',value:1},{label:'按钮',value:2}]" :value="item.value" :key="item.value">{{ item.label }}</Option>
          </Select>
        </FormItem>
        <FormItem label="Url" prop="path">
          <Input v-model.trim="model.path"></Input>
        </FormItem>
        <FormItem label="Name" prop="Name">
          <Input v-model.trim="model.name"></Input>
        </FormItem>
        <FormItem label="API">
          <Input v-model.trim="model.api"></Input>
        </FormItem>
        <FormItem label="是否启用">
          <RadioGroup v-model="model.enabled">
            <Radio :label="1">是</Radio>
            <Radio :label="2">否</Radio>
          </RadioGroup>
        </FormItem>
        <FormItem label="图标">
          <Input v-model.trim="model.icon"></Input>
        </FormItem>
        <FormItem label="排序">
          <InputNumber :min="1" :step="1" v-model.trim="model.orderSort" style="width:100%" />
        </FormItem>
        <FormItem>
          <Button type="primary" @click="handleSubmit('form')">提交</Button>
          <Button style="margin-left: 8px" @click="modal.visible = false">取消</Button>
        </FormItem>
      </Form>
    </Modal>
  </div>
</template>
<script>
import { editModule } from "@/services/manage/module";
export default {
  name: "ModuleEdit",
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
      // 表单验证(用户)
      rules: {
        path: [
          {
            required: true,
            message: "The path cannot be empty",
            trigger: "blur"
          }
        ],
        title: [
          {
            required: true,
            message: "title cannot be empty",
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
        1: "Create",
        2: "Edit",
        3: "Create"
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
      this.$refs['form'].validate(valid => {
        if (valid) {
          editModule(payload)
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
      })

    }
  }
};
</script>
<style lang="postcss" scoped>
</style>
