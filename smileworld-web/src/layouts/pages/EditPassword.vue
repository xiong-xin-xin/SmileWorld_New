<template>
  <div id="edit-password">
    <Modal v-model="modal.visible" :title="modal.title" footer-hide>
      <IForm :key="modal.visible" ref="editPassword" label-position="top" :elem="editPasswordElem" :model="editPassword" :rules="editPasswordRule" :btn-loading="loading" :width="420" @on-submit="handleSubmit" @on-click="modal.visible = false" submit-text="保存" button button-text="取消" />
      <!-- IForm -->
    </Modal>
  </div>
</template>
<script>
import {
  editPwd
} from '@/services/app'
export default {
  name: 'EditPassword',
  data() {
    const validPwd = (rule, value, callback) => {
      if (!value) {
        callback(new Error('Please enter your new password'))
      } else {
        if (this.editPassword.passwordConfirmation) {
          // 对第二个密码框单独验证
          this.$refs['editPassword'].validateField('passwordConfirmation')
        }
        callback()
      }
    }
    const validPwdCheck = (rule, value, callback) => {
      if (!value) {
        callback(new Error('Please enter your password confirmation'))
      } else {
        if (value !== this.editPassword.newPassword) {
          callback(new Error('The two input password do not match!'))
        } else {
          callback()
        }
      }
    }
    return {
      // 加载状态
      loading: false,
      // 模态框属性
      modal: {
        title: '',
        visible: false
      },
      // 表单元素数组
      editPasswordElem: [{
        label: '当前密码',
        prop: 'currentPassword',
        placeholder: 'Please enter your current password',
        type: 'password'
      }, {
        label: '新密码',
        prop: 'newPassword',
        placeholder: 'Please enter your new password',
        type: 'password'
      }, {
        label: '确认密码',
        prop: 'passwordConfirmation',
        placeholder: 'Please enter your password confirmation',
        type: 'password'
      }],
      // 表单数据对象
      editPassword: {
        currentPassword: '',
        newPassword: '',
        passwordConfirmation: ''
      },
      // 表单验证规则
      editPasswordRule: {
        currentPassword: [{
          required: true,
          message: 'Please enter your current password',
          trigger: 'blur'
        }],
        newPassword: [{
          required: true,
          validator: validPwd,
          trigger: 'blur'
        }],
        passwordConfirmation: [{
          required: true,
          validator: validPwdCheck,
          trigger: 'blur'
        }]
      }
    }
  },
  methods: {
    showModal() {
      this.modal = {
        title: '修改密码',
        visible: true
      }
      this.editPassword = {
        currentPassword: '',
        newPassword: '',
        passwordConfirmation: ''
      }
    },
    handleSubmit(name) {
      this.$Loading.start()
      this.loading = true
      editPwd(this.editPassword).then(res => {
        this.$Loading.finish()
        this.loading = false
        if (res.code === -1) {
          this.$Message.error(res.msg)
        }
        else {
          this.$Message.success("ok");
          this.modal.visible = false
        }
      }).catch(() => {
        this.$Loading.error()
        this.loading = false
      })
    }
  }
}
</script>
