 <template>
  <div id="del-msg">
    <Modal v-model="modal.visible" width="360">
      <p slot="header" style="color:#f60;text-align:center">
        <Icon type="information-circled"></Icon>
        <span>提示</span>
      </p>
      <div style="text-align:center">
        <p>此操作为不可逆操作，是否确认删除？</p>
      </div>
      <div slot="footer">
        <Button type="error" size="large" long @click="handleSubmit">确认删除</Button>
      </div>
    </Modal>
  </div>
</template>

<script>
export default {
  data() {
    return {
      // 模态框属性
      modal: {
        visible: false
      }
    }
  },
  methods: {
    handleModal() {
      this.modal = {
        visible: true
      };
    },
    handleSubmit() {
      this.$emit("on-submit");
      this.modal.visible = false;
    }
  }
}
</script>
  
