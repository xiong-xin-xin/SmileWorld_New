<template>
  <div id="header">
    <Row>
      <Col :xs="0" :sm="0" :md="12">
      <div class="logo-info">
        <img class="logo" src="@/assets/logo.png" alt="logo">
        <p class="name">smile world
          <br>
          <span class="sub">青春时光转眼即逝</span>
        </p>
      </div>
      <!-- .logo-info -->
      </Col>
      <Col :xs="6" :sm="12" :md="0">
      <Poptip class="navigation" placement="right-start" v-model="visible">
        <Icon class="nav-icon" type="md-menu" size="32" />
        <Sidebar ref="navigation" slot="content" @on-click="handleClose" />
      </Poptip>
      <!-- .navigation -->
      </Col>
      <Col :xs="18" :sm="12" :md="12">
      <div class="login-info">
        <Dropdown placement="bottom-end" @on-click="handleDropdown">
          <strong class="user">
            <Avatar style="background-color: #1890ff" icon="md-person" size="small" />
            Hi, {{ userName }}
          </strong>
          <DropdownMenu class="list" slot="list">
            <DropdownItem v-for="item in dropdown" :key="item.name" :name="item.name">
              <Icon :type="item.icon" size="16" style="margin-top: -2px;" />
              {{ item.label }}
            </DropdownItem>
          </DropdownMenu>
        </Dropdown>
      </div>
      <!-- .login-info -->
      </Col>
    </Row>
    <EditPassword ref="editPassword" />
    <!-- EditPassword -->
  </div>
</template>
<script>
import EditPassword from "@/layouts/pages/EditPassword";
import Sidebar from "@/layouts/partials/Sidebar";
export default {
  name: "IHeader",
  components: {
    EditPassword,
    Sidebar
  },
  data: () => ({
    // 导航可视状态
    visible: false,
    // 用户名
    userName: "",
    // 下拉菜单元素数组
    dropdown: [
      {
        label: "修改密码",
        name: "editPassword",
        icon: "md-lock"
      },
      {
        label: "Sign out",
        name: "signout",
        icon: "md-log-out"
      }
    ]
  }),
  watch: {
    // 侦听路由变化
    $route() {
      // 手动更新展开的子目录
      this.$nextTick(function() {
        this.$refs["navigation"].updateOpened();
      });
    }
  },
  mounted() {
    // 获取用户信息
    const userInfo = this.$store.state.app.userInfo;
    this.userName = userInfo.real_name || "Null";
  },
  methods: {
    // 关闭导航
    handleClose(state) {
      this.visible = state;
    },
    // 下拉菜单项
    handleDropdown(name) {
      switch (name) {
        case "editPassword":
          // 修改密码
          this.$refs[name].showModal();
          break;
        default:
          // 退出系统
          this.$Modal.confirm({
            title: "提示",
            content: "是否确认退出系统?",
            okText: "确认",
            cancelText: "取消",
            onOk: () => {
              this.$router.push("/login");
            }
          });
      }
    }
  }
};
</script>
<style lang="postcss">
#header {
  background-color: #1f2d3d;
  color: #fff;
  & #sidebar {
    height: auto;
    width: 240px;
  }
  & .ivu-poptip-popper[x-placement^="right"] .ivu-poptip-arrow:after,
  & .ivu-poptip-popper[x-placement^="right"] .ivu-poptip-arrow {
    border-right-color: #304156;
  }
  & .ivu-poptip-inner {
    background-color: #304156;
  }
  & .ivu-poptip-body {
    padding: 8px 0;
  }
  & .logo {
    float: left;
    height: 40px;
    margin: 10px 16px;
    width: 90px;
  }
  & .name {
    font-size: 18px;
    line-height: 20px;
    padding: 12px 0 6px;
  }
  & .sub {
    font-size: 12px;
    line-height: 1;
  }
  & .navigation {
    cursor: pointer;
    margin-left: 16px;
    margin-top: 14px;
  }
  & .login-info {
    margin: 18px 16px;
    text-align: right;
    & .user {
      color: #fff;
      cursor: pointer;
      display: block;
    }
    & .list {
      text-align: left;
    }
  }
}
</style>
