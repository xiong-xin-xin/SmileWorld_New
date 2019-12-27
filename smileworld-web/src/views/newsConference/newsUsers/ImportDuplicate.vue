<template>
  <div id="imp-dup">
    <Modal v-model="modal.visible" :title="modal.title" @on-ok="handleSubmit" :loading="loading.form">
      <div id="i-form">
        <Form ref="form" :label-width="150" @keyup.enter.native="handleSubmit">
          <FormItem label="发布会数据库" v-show="!Boolean(data.isNew)">
            <Select placeholder="请选择" filterable v-model="data.seldupid" :label-in-value="true" @on-change="handleChange" :transfer="true">
              <Option v-for="(opt, index) in dups" :key="index" :value="opt.value">{{ opt.label }}</Option>
            </Select>
          </FormItem>
          <FormItem label="是否创建子数据库">
            <RadioGroup v-model="data.isNew">
              <Radio :label="1">是</Radio>
              <Radio :label="0">否</Radio>
            </RadioGroup>
          </FormItem>
          <FormItem label="子数据库名称" v-show="Boolean(data.isNew)">
            <Input v-model="data.dupname" placeholder="dupname"></Input>
          </FormItem>
        </Form>
      </div>
    </Modal>
  </div>
</template>
<script>
import { importDuplicate, getDuplicateList } from "@/services/conference/conference";
export default {
  name: "ImportDuplicate",
  props: {
    ids: Array
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
        form: true,
        btn: false
      },
      data: {
        seldupid: "",
        seldupname: "",
        dupname: "",
        isNew: 0,
        impCols: []
      },
      // 补丁数据(发布会列表)
      dups: [
        {
          label: "X23发布会",
          value: "110100"
        },
        {
          label: "X21发布会",
          value: "310100"
        },
        {
          label: "X10发布会",
          value: "440100"
        },
        {
          label: "X12发布会",
          value: "330100"
        },
        {
          label: "NingBo",
          value: "330200"
        }
      ]
    };
  },
  created() {
    this.data.isNew = 0;
  },
  methods: {
    handleChange(val) {
      this.data.seldupid = val.value;
      this.data.seldupname = val.label;
    },
    handleModal(key) {
      this.modal = {
        title: '导入子数据库',
        visible: true
      };
    },
    // 获取下拉框数据
    handlePatch() {
      getDuplicateList().then(res => {
        this.dups = res.data;
      })
    },
    // 表单提交
    handleSubmit() {
      this.loading.form = false;
      this.$Loading.start();
      let req = {
        isNew: Boolean(this.data.isNew),
        ids: this.ids.join(","),
        dupname: this.data.dupname,
        seldupid: this.data.seldupid,
        seldupname: this.data.seldupname
      };
      if (req.isNew) {
        if (req.dupname == '') {
          this.$Notice.warning({
            title: '请输入名称'
          });
          this.$nextTick(() => { this.loading.form = true; })
          return false;
        }
      } else if (req.seldupid == '') {
        this.$Notice.warning({
          title: '请选择发布会'
        });
        this.$nextTick(() => { this.loading.form = true; })
        return false;
      }
      importDuplicate(req)
        .then(res => {
          let { code, msg } = res;
          if (code === 0) {
            this.$Message.success('ok');
            this.$Loading.finish();
            this.modal.visible = false;
          } else {
            this.$Message.error(msg);
            this.$Loading.error();
          }
        })
    }
  }
};
</script>
<style lang="postcss" scoped>
</style>
