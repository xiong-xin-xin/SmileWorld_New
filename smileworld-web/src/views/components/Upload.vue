<template>
  <div id="upload">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide>
      <Upload :action="upurl" style="text-align:center" ref="uploadButton" :headers="headers" :format="['xlsx']" :on-format-error="handleFormatError" :on-success="handleUpSuccess">
        <Button icon=" ios-cloud-upload-outline">上传</Button>
      </Upload>
    </Modal>
  </div>
</template>

<script>
export default {
  name: "IUpload",
  props: {
    upurl: String
  },
  data() {
    return {
      // 模态框属性
      modal: {
        visible: false,
        title: ""
      },
      headers: {
        'Access-Control-Allow-Origin': '*',
        'Authorization': 'Bearer ' + JSON.parse(sessionStorage.getItem('userInfo')).auth_token.token
      },
    }
  },
  methods: {
    handleFormatError(file) {
      this.$Message.warning({ content: '请上传2007-2010版的EXCEL文件（后辍名为xlsx）' });
    },
    handleModal() {
      this.modal = {
        title: '上传',
        visible: true
      };
      this.$nextTick(() => { this.$refs.uploadButton.clearFiles() });
    },
    handleUpSuccess(res) {
      if (res.code == -1) {
        this.$Modal.warning({
          title: '提示',
          width: '400',
          content: res.data.join('<br/>')
        });
      }
    }
  }
}
</script>
  


